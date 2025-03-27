# LapDrive.SignatureAdapter

LapDrive.SignatureAdapter es una API que facilita la integración con proveedores de firma digital, permitiendo crear procesos de firma para documentos almacenados en SharePoint.

## Estructura del Proyecto

El proyecto está organizado en una arquitectura de 3 capas:

- **LapDrive.SignatureAdapter.API**: Capa de presentación que expone los endpoints RESTful.
- **LapDrive.SignatureAdapter.Business**: Capa de lógica de negocio que implementa los servicios y validaciones.
- **LapDrive.SignatureAdapter.Data**: Capa de acceso a datos que se comunica con SharePoint y el proveedor de firmas.
- **LapDrive.SignatureAdapter.Models**: Modelos compartidos entre todas las capas.

## Requisitos

- .NET 8.0 SDK
- Acceso a SharePoint Online
- Credenciales de aplicación para SharePoint (ClientId, ClientSecret)
- Credenciales para el proveedor de firmas digitales (Watana)

## Configuración

La aplicación se configura mediante el archivo `appsettings.json`. Deben configurarse las siguientes secciones:

### SharePoint

```json
"SharePoint": {
  "ClientId": "00000000-0000-0000-0000-000000000000",
  "ClientSecret": "your-client-secret",
  "TenantId": "your-tenant-id.onmicrosoft.com",
  "SiteUrl": "https://yourtenant.sharepoint.com/sites/yoursite"
}
```

### Proveedor de Firmas

```json
"SignatureProvider": {
  "ApiUrl": "https://api.watana.pe/api/v1/xxxxxxxx",
  "Token": "your-token-here",
  "Timeout": "00:01:00",
  "MaxRetries": 3
}
```

## API Endpoints

### POST /api/v1/signature-processes

Crea un nuevo proceso de firma digital para un documento o carpeta almacenada en SharePoint.

#### Request

```json
{
  "requestId": "string",
  "metadata": {
    "subject": "string",
    "message": "string",
    "createdAt": "2025-03-27T12:00:00Z"
  },
  "document": {
    "id": "string",
    "name": "string",
    "libraryName": "string",
    "webUrl": "string",
    "type": "file"
  },
  "signers": [
    {
      "displayName": "string",
      "email": "string",
      "signature": {
        "x": 0,
        "y": 0,
        "pageNumber": 1,
        "position": "string"
      }
    }
  ],
  "recipients": [
    {
      "displayName": "string",
      "email": "string"
    }
  ]
}
```

#### Response

```json
{
  "processId": "string",
  "status": "string",
  "message": "string",
  "signingUrl": "string"
}
```

## Desarrollo

### Compilación

```bash
dotnet build
```

### Ejecución

```bash
dotnet run --project LapDrive.SignatureAdapter.API
```

### Pruebas

```bash
dotnet test
```

## Extensibilidad

El sistema está diseñado para permitir el cambio de proveedor de firmas digitales en el futuro. Esto se logra mediante:

1. Interfaces de abstracción en la capa de datos
2. DTOs agnósticos al proveedor
3. Modelo de dominio independiente de los detalles de implementación

Para cambiar el proveedor de firmas, se debe implementar la interfaz `ISignatureProviderClient` y actualizar el registro en la inyección de dependencias.

## Licencia

Derechos Reservados © 2025