# Limitaciones de Azure Table Storage y Soluciones Propuestas

## Limitaciones Principales

1. **No Soporte para Relaciones**
   - Azure Table Storage no soporta relaciones como las bases de datos relacionales
   - No hay concepto de foreign keys
   - No hay joins entre tablas

2. **Estructura de Partición**
   - Cada entidad requiere PartitionKey y RowKey
   - No hay índices secundarios nativos
   - Consultas más eficientes son por PartitionKey

## Análisis del Modelo Actual

### Relaciones en el Diagrama
1. SignatureRequest → Document (1:N)
2. SignatureRequest → Signature (1:N)
3. SignatureRequest → RequestHistory (1:N)
4. Signature → SignatureHistory (1:N)

## Propuestas de Solución

### 1. Modelo Desnormalizado (Recomendado)
```csharp
// Tabla principal de tracking
public class SignatureTrackingEntity : TableEntity
{
    // Datos de SignatureRequest
    public string ProcessId { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }
    public string RequesterEmail { get; set; }
    public string RequesterName { get; set; }

    // Documento embebido
    public string DocumentId { get; set; }
    public string DocumentName { get; set; }
    public string LibraryName { get; set; }
    public string WebUrl { get; set; }
    public string ItemId { get; set; }
    public int FileSize { get; set; }

    public SignatureTrackingEntity(string processId)
    {
        PartitionKey = processId;
        RowKey = "main"; // Registro principal
    }
}

// Tabla de firmas
public class SignatureEntity : TableEntity
{
    public string SignerEmail { get; set; }
    public string SignerName { get; set; }
    public string Status { get; set; }
    public DateTime? SignatureDate { get; set; }
    public string SignerDocumentUrl { get; set; }
    public string PlacementData { get; set; }

    public SignatureEntity(string processId, string signerEmail)
    {
        PartitionKey = processId;
        RowKey = $"sig_{signerEmail}"; // Identificador único por firmante
    }
}

// Tabla de historial
public class HistoryEntity : TableEntity
{
    public string EntityType { get; set; } // "Request" o "Signature"
    public string PreviousStatus { get; set; }
    public string NewStatus { get; set; }
    public DateTime ChangeDate { get; set; }
    public string SignerEmail { get; set; } // Solo para firmas

    public HistoryEntity(string processId, DateTime changeDate)
    {
        PartitionKey = processId;
        RowKey = $"hist_{changeDate.Ticks}";
    }
}
```

### 2. Múltiples Tablas con Referencias

```csharp
services.Configure<AzureStorageOptions>(options =>
{
    options.Tables = new Dictionary<string, string>
    {
        { "Tracking", "signaturetracking" },
        { "Signatures", "signaturesdata" },
        { "History", "trackinghistory" }
    };
});
```

## Ventajas y Desventajas

### Modelo Desnormalizado
✅ **Ventajas**
- Consultas más rápidas
- Menos operaciones de lectura
- Mejor rendimiento general
- Más simple de implementar

❌ **Desventajas**
- Duplicación de datos
- Mayor uso de almacenamiento
- Actualizaciones más complejas

### Múltiples Tablas
✅ **Ventajas**
- Mejor organización de datos
- Menos duplicación
- Más cercano al modelo relacional

❌ **Desventajas**
- Requiere múltiples queries
- Más complejo de mantener
- Peor rendimiento en consultas

## Recomendación

Se recomienda usar el **Modelo Desnormalizado** por las siguientes razones:

1. **Rendimiento**
   - Menos operaciones de lectura
   - Mejor latencia en consultas
   - Menos complejidad en el código

2. **Caso de Uso**
   - El tracking no requiere actualizaciones frecuentes
   - La duplicación de datos no es crítica
   - El volumen de datos es manejable

3. **Implementación**
   - Más simple de implementar
   - Menos propenso a errores
   - Más fácil de mantener

## Consultas Comunes

```csharp
// Obtener estado actual de un proceso
var tracking = await _tableClient.GetEntityAsync<SignatureTrackingEntity>(
    processId, "main");

// Obtener todas las firmas de un proceso
var signatures = _tableClient.QueryAsync<SignatureEntity>(
    filter: $"PartitionKey eq '{processId}'");

// Obtener historial de un proceso
var history = _tableClient.QueryAsync<HistoryEntity>(
    filter: $"PartitionKey eq '{processId}'",
    select: new string[] { "ChangeDate", "PreviousStatus", "NewStatus" });
```

## Plan de Implementación Actualizado

1. **Día 1: Estructura Base**
   - Crear las entidades principales
   - Implementar operaciones CRUD básicas
   - Configurar la cuenta de Azure Storage

2. **Día 2: Lógica de Negocio**
   - Implementar manejo de firmas
   - Implementar tracking de historial
   - Pruebas unitarias

3. **Día 3: Integración y Pruebas**
   - Integrar con el sistema existente
   - Pruebas de integración
   - Documentación