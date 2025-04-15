# Diccionario de Datos - Sistema de Seguimiento de Firmas

## Tabla: SignatureRequest
Almacena las solicitudes de firma de documentos y gestiona el flujo completo del proceso de firmas digitales en SharePoint.

| Campo | Tipo de Dato | Descripción | Restricciones |
|-------|-------------|-------------|---------------|
| SignatureRequestId | UUID | Identificador único de la solicitud de firma | PK, Requerido, No nulo |
| ProcessId | VARCHAR(100) | Identificador único del proceso en el sistema de firmas digitales | Requerido, Único |
| Subject | VARCHAR(255) | Asunto o título del proceso de firma | Requerido, No nulo |
| Message | TEXT | Mensaje o instrucciones para los firmantes y destinatarios | Opcional |
| CreatedAt | DATETIME | Fecha y hora de creación de la solicitud (UTC) | Requerido, Default: CURRENT_TIMESTAMP |
| Status | VARCHAR(50) | Estado actual del proceso de firma | Requerido, Ver tabla de estados |
| RequesterEmail | VARCHAR(255) | Correo electrónico del solicitante del proceso | Requerido, Formato email válido |
| RequesterName | VARCHAR(255) | Nombre completo del solicitante del proceso | Requerido, No nulo |

## Tabla: Document
Almacena la información de los documentos en SharePoint que requieren firma digital.

| Campo | Tipo de Dato | Descripción | Restricciones |
|-------|-------------|-------------|---------------|
| DocumentId | UUID | Identificador único del documento | PK, Requerido, No nulo |
| SignatureRequestId | UUID | Referencia a la solicitud de firma | FK, Requerido, No nulo |
| Name | VARCHAR(255) | Nombre del documento en SharePoint | Requerido, No nulo |
| LibraryName | VARCHAR(255) | Nombre de la biblioteca de SharePoint donde reside el documento | Requerido, No nulo |
| WebUrl | VARCHAR(1024) | URL web completa del documento en SharePoint | Requerido, URL válida |
| ItemId | VARCHAR(20) | ID del elemento en la biblioteca de SharePoint | Requerido, No nulo |
| FileSize | INTEGER | Tamaño del archivo en bytes | Requerido, > 0 |

## Tabla: Signature
Registra las firmas requeridas y su configuración para cada documento.

| Campo | Tipo de Dato | Descripción | Restricciones |
|-------|-------------|-------------|---------------|
| SignatureId | UUID | Identificador único de la firma | PK, Requerido, No nulo |
| SignatureRequestId | UUID | Referencia a la solicitud de firma | FK, Requerido, No nulo |
| SignerEmail | VARCHAR(255) | Correo electrónico del firmante | Requerido, Formato email válido |
| SignerName | VARCHAR(255) | Nombre completo del firmante | Requerido, No nulo |
| Status | VARCHAR(50) | Estado actual de la firma | Requerido, Ver tabla de estados |
| SignatureDate | DATETIME | Fecha y hora en que se realizó la firma (UTC) | NULL hasta que se firme |
| SignerDocumentUrl | VARCHAR(1024) | URL personalizada para que el firmante acceda a su documento | Requerido |
| PlacementData | JSON | Configuración de ubicación de la firma en el documento. Formato: <br>{<br>&nbsp;&nbsp;"x": [número], // Coordenada X opcional<br>&nbsp;&nbsp;"y": [número], // Coordenada Y opcional<br>&nbsp;&nbsp;"pageNumber": número, // Número de página, default: 1<br>&nbsp;&nbsp;"position": "string" // Posición predefinida opcional<br>} | Requerido |

## Tabla: RequestHistory
Mantiene un registro auditable de los cambios de estado en el proceso de firma.

| Campo | Tipo de Dato | Descripción | Restricciones |
|-------|-------------|-------------|---------------|
| RequestHistoryId | UUID | Identificador único del registro histórico | PK, Requerido, No nulo |
| SignatureRequestId | UUID | Referencia a la solicitud de firma | FK, Requerido, No nulo |
| PreviousStatus | VARCHAR(50) | Estado anterior del proceso | Requerido, Estado válido |
| NewStatus | VARCHAR(50) | Nuevo estado del proceso | Requerido, Estado válido |
| ChangeDate | DATETIME | Fecha y hora del cambio de estado (UTC) | Requerido, Default: CURRENT_TIMESTAMP |

## Tabla: SignatureHistory
Mantiene un registro auditable de los cambios de estado en las firmas individuales.

| Campo | Tipo de Dato | Descripción | Restricciones |
|-------|-------------|-------------|---------------|
| SignatureHistoryId | UUID | Identificador único del registro histórico | PK, Requerido, No nulo |
| SignatureId | UUID | Referencia a la firma | FK, Requerido, No nulo |
| PreviousStatus | VARCHAR(50) | Estado anterior de la firma | Requerido, Estado válido |
| NewStatus | VARCHAR(50) | Nuevo estado de la firma | Requerido, Estado válido |
| ChangeDate | DATETIME | Fecha y hora del cambio de estado (UTC) | Requerido, Default: CURRENT_TIMESTAMP |

## Estados del Proceso de Firma (SignatureRequest.Status)

| Valor | Descripción |
|-------|-------------|
| Pending | Proceso iniciado, pendiente de envío a firmantes |
| WaitingForSignatures | Documentos enviados, esperando firmas de los participantes |
| InProgress | Al menos un participante ha firmado |
| Completed | Todos los participantes han firmado exitosamente |
| Rejected | Proceso rechazado por al menos un firmante |
| Expired | Proceso expirado por tiempo límite de firma |
| Canceled | Proceso cancelado por el solicitante |

## Estados de la Firma (Signature.Status)

| Valor | Descripción |
|-------|-------------|
| Pending | Esperando acción del firmante |
| InProgress | Proceso de firma iniciado por el firmante |
| Signed | Documento firmado exitosamente |
| Rejected | Firma rechazada por el firmante |
| Cancelled | Firma cancelada durante el proceso |

## Índices Recomendados

| Tabla | Campos | Tipo | Descripción |
|-------|--------|------|-------------|
| SignatureRequest | Status, CreatedAt | Índice compuesto | Optimiza filtrado de procesos por estado y fecha |
| SignatureRequest | ProcessId | Índice único | Garantiza unicidad del ProcessId en el sistema |
| SignatureRequest | RequesterEmail | Índice | Optimiza búsquedas por solicitante |
| Document | SignatureRequestId | Índice | Optimiza recuperación de documentos por proceso |
| Document | ItemId, LibraryName | Índice compuesto | Optimiza búsquedas por documento en SharePoint |
| Signature | SignatureRequestId, Status | Índice compuesto | Optimiza filtrado de firmas por proceso y estado |
| Signature | SignerEmail, Status | Índice compuesto | Optimiza búsqueda de firmas pendientes por usuario |
| RequestHistory | SignatureRequestId, ChangeDate | Índice compuesto | Optimiza consultas de historial por proceso |
| SignatureHistory | SignatureId, ChangeDate | Índice compuesto | Optimiza consultas de historial por firma |

## Consideraciones Técnicas

1. Integración con SharePoint:
   - Los documentos se almacenan en SharePoint y se referencian mediante ItemId y LibraryName
   - Los campos WebUrl proporcionan acceso directo al documento en SharePoint
   - Se mantiene consistencia con los metadatos de SharePoint

2. Gestión de Firmas:
   - SignerDocumentUrl contiene la URL personalizada para cada firmante
   - PlacementData define la ubicación exacta de la firma en coordenadas o posición predefinida
   - Los estados de firma reflejan el flujo real del proceso de firma

3. Auditoría y Seguimiento:
   - Se mantiene historial completo de cambios de estado
   - Todas las fechas se almacenan en UTC
   - Se registran todos los cambios de estado tanto del proceso como de firmas individuales

4. Seguridad:
   - Control de acceso basado en correo electrónico
   - Validación de firmantes autorizados
   - Trazabilidad completa de acciones
   - URLs personalizadas por firmante
