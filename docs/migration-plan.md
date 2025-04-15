# Plan de Migración: Implementación de Azure SQL Database para Tracking

## 1. Estructura de Base de Datos

El esquema seguirá exactamente el modelo definido en el diagrama tracking-database-diagram.mermaid:

```sql
CREATE TABLE SignatureRequest (
    SignatureRequestId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProcessId NVARCHAR(100) UNIQUE NOT NULL,
    Subject NVARCHAR(255) NOT NULL,
    Message NVARCHAR(MAX),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    Status NVARCHAR(50) NOT NULL,
    RequesterEmail NVARCHAR(255) NOT NULL,
    RequesterName NVARCHAR(255) NOT NULL
);

CREATE TABLE Document (
    DocumentId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SignatureRequestId UNIQUEIDENTIFIER NOT NULL,
    Name NVARCHAR(255) NOT NULL,
    LibraryName NVARCHAR(255) NOT NULL,
    WebUrl NVARCHAR(1024) NOT NULL,
    ItemId NVARCHAR(20) NOT NULL,
    FileSize INT NOT NULL,
    FOREIGN KEY (SignatureRequestId) REFERENCES SignatureRequest(SignatureRequestId)
);

CREATE TABLE Signature (
    SignatureId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SignatureRequestId UNIQUEIDENTIFIER NOT NULL,
    SignerEmail NVARCHAR(255) NOT NULL,
    SignerName NVARCHAR(255) NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    SignatureDate DATETIME2,
    SignerDocumentUrl NVARCHAR(1024) NOT NULL,
    PlacementData NVARCHAR(MAX) NOT NULL,
    FOREIGN KEY (SignatureRequestId) REFERENCES SignatureRequest(SignatureRequestId)
);

CREATE TABLE RequestHistory (
    RequestHistoryId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SignatureRequestId UNIQUEIDENTIFIER NOT NULL,
    PreviousStatus NVARCHAR(50) NOT NULL,
    NewStatus NVARCHAR(50) NOT NULL,
    ChangeDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (SignatureRequestId) REFERENCES SignatureRequest(SignatureRequestId)
);

CREATE TABLE SignatureHistory (
    SignatureHistoryId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SignatureId UNIQUEIDENTIFIER NOT NULL,
    PreviousStatus NVARCHAR(50) NOT NULL,
    NewStatus NVARCHAR(50) NOT NULL,
    ChangeDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (SignatureId) REFERENCES Signature(SignatureId)
);

-- Índices
CREATE INDEX IX_SignatureRequest_Status ON SignatureRequest(Status);
CREATE INDEX IX_SignatureRequest_ProcessId ON SignatureRequest(ProcessId);
CREATE INDEX IX_Document_SignatureRequestId ON Document(SignatureRequestId);
CREATE INDEX IX_Signature_SignatureRequestId ON Signature(SignatureRequestId);
CREATE INDEX IX_Signature_Status ON Signature(Status);
CREATE INDEX IX_RequestHistory_SignatureRequestId ON RequestHistory(SignatureRequestId);
CREATE INDEX IX_SignatureHistory_SignatureId ON SignatureHistory(SignatureId);
```

## 2. Fases de Implementación

### Fase 1: Preparación de Infraestructura (1 día)

1. **Crear Azure SQL Database**
   - Configurar DTUs o vCores según necesidad
   - Configurar alta disponibilidad
   - Configurar backups automáticos
   - Configurar seguridad y firewall

2. **Configuración del Proyecto**
   - Agregar paquetes NuGet:
     ```xml
     <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="7.x.x" />
     <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="7.x.x" />
     ```
   - Configurar cadena de conexión:
     ```json
     {
       "ConnectionStrings": {
         "SignatureTracking": "Server=tcp:your-server.database.windows.net;Database=SignatureDB;..."
       }
     }
     ```

### Fase 2: Implementación del Código (2 días)

1. **Modelos de Entity Framework**
```csharp
public class SignatureDbContext : DbContext
{
    public DbSet<SignatureRequest> SignatureRequests { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Signature> Signatures { get; set; }
    public DbSet<RequestHistory> RequestHistory { get; set; }
    public DbSet<SignatureHistory> SignatureHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuraciones de entidades
        modelBuilder.Entity<SignatureRequest>()
            .HasIndex(sr => sr.ProcessId)
            .IsUnique();

        modelBuilder.Entity<SignatureRequest>()
            .HasMany(sr => sr.Documents)
            .WithOne()
            .HasForeignKey(d => d.SignatureRequestId);

        // ... otras configuraciones
    }
}
```

2. **Implementación del Repositorio**
```csharp
public class SqlSignatureTrackingRepository : ISignatureTrackingRepository
{
    private readonly SignatureDbContext _context;
    private readonly ILogger<SqlSignatureTrackingRepository> _logger;

    public SqlSignatureTrackingRepository(
        SignatureDbContext context,
        ILogger<SqlSignatureTrackingRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task RegisterTrackingAsync(string processId, /* otros parámetros */)
    {
        var request = new SignatureRequest
        {
            ProcessId = processId,
            // ... otras propiedades
        };

        _context.SignatureRequests.Add(request);
        await _context.SaveChangesAsync();
    }

    public async Task<SignatureProcessTracking?> GetTrackingAsync(string processId)
    {
        return await _context.SignatureRequests
            .Include(sr => sr.Documents)
            .Include(sr => sr.Signatures)
            .FirstOrDefaultAsync(sr => sr.ProcessId == processId);
    }

    public async Task UpdateTrackingStatusAsync(string processId, string status)
    {
        var request = await _context.SignatureRequests
            .FirstOrDefaultAsync(sr => sr.ProcessId == processId);

        if (request != null)
        {
            var oldStatus = request.Status;
            request.Status = status;

            _context.RequestHistory.Add(new RequestHistory
            {
                SignatureRequestId = request.SignatureRequestId,
                PreviousStatus = oldStatus,
                NewStatus = status
            });

            await _context.SaveChangesAsync();
        }
    }
}
```

3. **Configuración de Servicios**
```csharp
services.AddDbContext<SignatureDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("SignatureTracking")));

services.AddScoped<ISignatureTrackingRepository, SqlSignatureTrackingRepository>();
```

### Fase 3: Migraciones y Scripts (1 día)

1. **Crear Migraciones**
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

2. **Scripts de Rollback**
```sql
-- Guardar para posible rollback
DROP TABLE SignatureHistory;
DROP TABLE RequestHistory;
DROP TABLE Signature;
DROP TABLE Document;
DROP TABLE SignatureRequest;
```

### Fase 4: Pruebas (1 día)

1. **Pruebas Unitarias**
```csharp
public class SqlSignatureTrackingRepositoryTests
{
    [Fact]
    public async Task RegisterTrackingAsync_Should_CreateAllRelatedEntities()
    {
        // Arrange
        var repository = CreateRepository();
        
        // Act
        await repository.RegisterTrackingAsync("test-id", ...);
        
        // Assert
        var tracking = await repository.GetTrackingAsync("test-id");
        Assert.NotNull(tracking);
        Assert.NotEmpty(tracking.Documents);
    }
}
```

2. **Pruebas de Integración**
   - Verificar operaciones CRUD
   - Probar relaciones y cascadas
   - Validar triggers y constraints

## 3. Plan de Rollout

### Fase 1: Desarrollo (1 día)
1. Implementar y probar en ambiente local
2. Validar todas las operaciones CRUD
3. Verificar manejo de errores

### Fase 2: QA (1 día)
1. Desplegar a ambiente de pruebas
2. Ejecutar pruebas automatizadas
3. Realizar pruebas manuales
4. Validar performance

### Fase 3: Producción (1 día)
1. Crear backup de la configuración actual
2. Desplegar cambios
3. Verificar funcionamiento
4. Monitorear performance

## 4. Monitoreo

1. **Métricas a Monitorear**
   - DTU/CPU Usage
   - Tiempos de respuesta
   - Errores de conexión
   - Deadlocks
   - Tamaño de la base de datos

2. **Alertas**
   - Alto uso de DTUs
   - Errores de conexión
   - Latencia alta
   - Espacio en disco bajo

## Timeline Total: 7 días laborables

- Día 1: Infraestructura
- Días 2-3: Implementación
- Día 4: Migraciones
- Día 5: Pruebas
- Día 6: QA
- Día 7: Producción

## Ventajas de Esta Solución

1. **Mantiene el Modelo Relacional**
   - Sin cambios en la estructura de datos
   - Relaciones y constraints garantizados
   - Transacciones ACID

2. **Facilidad de Mantenimiento**
   - Herramientas familiares
   - Mejor soporte para consultas complejas
   - Backups y recuperación robustos

3. **Escalabilidad**
   - Escalado vertical automático
   - Réplicas de lectura si se necesitan
   - Sharding disponible para futuro crecimiento

4. **Seguridad**
   - Integración con Azure AD
   - Encriptación en reposo y tránsito
   - Auditoría detallada