# Indagación — Minimal API, DTO, MySQL y Swagger

## Minimal API y códigos HTTP

Una ruta se registra con `MapGet`, `MapPost`, `MapPut` o `MapDelete`. La ruta identifica el recurso (por ejemplo, `/api/personajes/3`) y el cuerpo JSON contiene los datos de una creación o modificación. Los parámetros de ruta identifican el recurso; los parámetros DTO del handler reciben el cuerpo.

- `200 OK`: operación completada y devuelve representación.
- `201 Created`: se creó un recurso; la respuesta identifica su nueva ruta.
- `400 Bad Request`: datos inválidos o acción fuera de las reglas.
- `404 Not Found`: no existe el recurso identificado.
- `409 Conflict`: el recurso existe, pero su estado impide la operación.

## DTO y entidades

Un DTO define el contrato HTTP y evita exponer campos internos de dominio o persistencia. La entrada de creación de personaje contiene sólo nombre y tipo; el servicio construye la clase concreta. La respuesta devuelve ID, nombre, tipo, vida y estadísticas de combate, no la implementación de `IHabilidad`.

Ejemplo de entrada:

```json
{ "nombre": "Conan", "tipo": "Guerrero" }
```

Ejemplo de salida:

```json
{ "id": 1, "nombre": "Conan" }
```

Los DTO definitivos están en `src/Aplicacion/DTOs/CombateDtos.cs`; el ejercicio en memoria está en `exploracion/dto/`.

## Inyección de dependencias

La API recibe servicios por parámetros de los handlers. `Program.cs` registra los repositorios, la fábrica de conexiones, los servicios de aplicación y `CombateService`. Así las rutas no construyen repositorios ni abren conexiones por su cuenta. Los servicios y repositorios se registran como `Scoped`, apropiado para compartir sus dependencias durante una solicitud HTTP.

## Swagger

`AddEndpointsApiExplorer()` descubre las rutas Minimal API y `AddSwaggerGen()` genera el documento OpenAPI. `UseSwagger()` publica el JSON y `UseSwaggerUI()` ofrece la interfaz en `/swagger`.

La prueba mínima de ruta está en `exploracion/Program.cs`; la prueba de repositorio en memoria e inyección de dependencias está en `exploracion/configuracion/`.

## Persistencia y permisos

El invitado necesita únicamente `SELECT` sobre `combate_api.*`; ese permiso a nivel de base cubre las tablas presentes y las que se creen después. El administrador recibe todos los privilegios sobre `combate_api.*`. Las lecturas del proyecto usan `GUEST_DB_CONNECTION`; las escrituras usan `ADMIN_DB_CONNECTION`.

El backup debe incluir estructura y datos de muestra, pero nunca las credenciales. Los comandos de instalación, backup y restauración se encuentran en `README.md`.
