# Plan de Refactorización - Claridad del Dominio

## 1. Renombrar SignatureInfo a SignaturePlacement

### Archivos a modificar:
1. `LapDrive.SignatureAdapter.Models/DTOs/Request/SignatureInfo.cs` -> `SignaturePlacement.cs`
   - Renombrar clase manteniendo toda la funcionalidad
   - Actualizar comentarios para reflejar el nuevo nombre

2. `LapDrive.SignatureAdapter.Models/Entities/SignatureInfo.cs` -> `SignaturePlacement.cs`
   - Mismo cambio en la capa de entidades
   - Mantener la consistencia con los DTOs

### Referencias a actualizar:
1. En SignerInfo.cs:
```csharp
public SignaturePlacement Signature { get; set; } = new();
```

2. En Signer.cs:
```csharp
public SignaturePlacement SignatureInfo { get; set; } = new();
```

3. En WatanaSignatureProviderClient.cs:
- Actualizar todas las referencias a SignatureInfo por SignaturePlacement

## 2. Renombrar ProcessStatus a SignatureStatus

1. Mover todos los estados al nuevo enum SignatureStatus
2. Actualizar referencias en:
   - SignatureProcess
   - SignatureProcessResponse
   - SignatureProcessDetailResponse

## 3. Renombrar DocumentInfo a SharePointDocumentInfo

1. Renombrar clase en DTOs
2. Actualizar referencias en:
   - SignatureProcessRequest
   - DocumentDetail
   - SignatureProcessService

## Pasos de Implementación

1. Crear nuevas clases con los nombres actualizados
2. Migrar propiedades y lógica
3. Actualizar referencias en todo el proyecto
4. Validar que los cambios no afecten la integración con Watana
5. Ejecutar pruebas unitarias y de integración
6. Eliminar clases antiguas una vez validados los cambios

## Próximos Pasos

1. Revisar este plan de refactorización
2. Aprobar los cambios propuestos
3. Cambiar a modo Code para implementar los cambios
4. Ejecutar pruebas para validar los cambios

## Impacto

- No hay cambios en la funcionalidad
- Mejora la claridad del código
- Facilita el mantenimiento
- Nombres más descriptivos del dominio