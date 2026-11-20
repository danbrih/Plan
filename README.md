# Combate por API

Web API de combate construida con .NET 10, Minimal API, DTO, Swagger y MySQL. La solución conserva el modelo polimórfico de `Personaje` y separa API, aplicación, persistencia y pruebas.

## Requisitos

- .NET 10 SDK.
- MySQL 8.x o compatible.
- `mysql` y `mysqldump` para crear/restaurar la base y el backup.
- Bash y Python 3 para ejecutar el script de preparación.

## Estructura relevante

- `src/API`: Minimal API, configuración y Swagger.
- `src/Aplicacion`: servicios, interfaces y DTO.
- `src/Persistencia`: entidades, fábrica de conexiones y repositorios MySQL.
- `src/Tests`: pruebas unitarias de dominio y servicios; no requieren MySQL.
- `sql/01_estructura.sql`: base y tablas.
- `sql/02_usuarios.sql`: administrador e invitado.
- `sql/03_datos_muestra.sql`: cuatro personajes de muestra.
- `backup/combate_api.sql`: backup lógico inicial con estructura y datos de muestra, sin usuarios ni contraseñas.
- `docs/`: punto de partida, indagación y reflexión final.
- `exploracion/`: pruebas de concepto pedidas en la etapa de indagación.

## Configurar MySQL

Los scripts nuevos de `sql/` son los que usa la API y crean `combate_api`; no eliminan ni modifican `juego_db`. Los archivos antiguos de `scripts/` pertenecen al esquema previo `juego_db` y no deben ejecutarse para preparar esta API.

Ejecutá los scripts por separado y en este orden, con un usuario administrador de MySQL:

```bash
mysql -u root -p < sql/01_estructura.sql
```

Antes de ejecutar `sql/02_usuarios.sql`, reemplazá `CAMBIAR_ADMIN_PASSWORD` y `CAMBIAR_INVITADO_PASSWORD` por contraseñas de prueba locales. No subas credenciales al repositorio.

```bash
mysql -u root -p < sql/02_usuarios.sql
mysql -u root -p < sql/03_datos_muestra.sql
```

El administrador recibe todos los permisos sobre `combate_api`. El invitado recibe únicamente `SELECT` sobre `combate_api.*`, también aplicable a las tablas futuras de esa base.

### Verificar los permisos del invitado

La consulta debe funcionar:

```bash
mysql -u invitado -p combate_api -e 'SELECT * FROM Personaje;'
```

Estas escrituras deben fallar con error de permisos:

```bash
mysql -u invitado -p combate_api -e "INSERT INTO Personaje (Nombre,Tipo,Vida,VidaMaxima,Ataque,Defensa) VALUES ('NoDebeEntrar','Guerrero',100,100,10,2);"
mysql -u invitado -p combate_api -e "UPDATE Personaje SET Nombre='NoDebeModificar' WHERE Id=1;"
```

La API no recibe contraseñas desde `appsettings.json`. Configurá las dos cadenas de conexión en el entorno donde inicies el proceso:

```bash
export ADMIN_DB_CONNECTION='Server=localhost;Database=combate_api;User ID=administrador;Password=TU_PASSWORD_ADMIN;'
export GUEST_DB_CONNECTION='Server=localhost;Database=combate_api;User ID=invitado;Password=TU_PASSWORD_INVITADO;'
```

Todas las rutas GET consultan con `GUEST_DB_CONNECTION`. Las rutas POST, PUT y DELETE escriben con `ADMIN_DB_CONNECTION`.

## Compilar y probar

Desde la raíz del repositorio:

```bash
dotnet --list-sdks
dotnet restore src/Proyecto.sln
dotnet build src/Proyecto.sln
dotnet test src/Proyecto.sln
```

Las pruebas unitarias de dominio usan objetos en memoria y no necesitan el servidor MySQL.

## Iniciar la API y abrir Swagger

Luego de configurar las dos variables de entorno:

```bash
ASPNETCORE_ENVIRONMENT=Development dotnet run --project src/API/API.csproj --urls http://localhost:5000
```

Abrí `http://localhost:5000/swagger`.

## Contratos de las quince rutas

Los nombres de propiedades de los cuerpos JSON son camelCase.

### Personajes

1. `GET /api/personajes` — `200`, lista de personajes.
   ```bash
   curl http://localhost:5000/api/personajes
   ```
2. `GET /api/personajes/1` — `200`, personaje; `404`, no existe.
   ```bash
   curl http://localhost:5000/api/personajes/1
   ```
3. `GET /api/personajes/1/combates` — `200`, lista de combates del personaje; `404`, personaje inexistente.
   ```bash
   curl http://localhost:5000/api/personajes/1/combates
   ```
4. `POST /api/personajes` — `201`, creado con ID; `400`, nombre/tipo inválido.
   ```bash
   curl -X POST http://localhost:5000/api/personajes -H 'Content-Type: application/json' -d '{"nombre":"Conan","tipo":"Guerrero"}'
   ```
5. `PUT /api/personajes/1` — `200`, nombre actualizado; `400`, nombre inválido; `404`, no existe.
   ```bash
   curl -X PUT http://localhost:5000/api/personajes/1 -H 'Content-Type: application/json' -d '{"nombre":"Conan el Grande"}'
   ```
6. `DELETE /api/personajes/1` — `204`, eliminado; `404`, no existe; `409`, participó en un combate.
   ```bash
   curl -i -X DELETE http://localhost:5000/api/personajes/1
   ```

### Combates

7. `GET /api/combates` — `200`, lista con participantes y estado.
   ```bash
   curl http://localhost:5000/api/combates
   ```
8. `POST /api/combates` — `201`, combate creado; `400`, IDs inválidos/iguales o personajes derrotados; `404`, personaje inexistente.
   ```bash
   curl -X POST http://localhost:5000/api/combates -H 'Content-Type: application/json' -d '{"personaje1Id":1,"personaje2Id":2}'
   ```
9. `GET /api/combates/1` — `200`, participantes, historial y resultado; `404`, no existe.
   ```bash
   curl http://localhost:5000/api/combates/1
   ```
10. `PUT /api/combates/1` — `200`, participantes actualizados; `400`, IDs inválidos; `404`, combate/personaje inexistente; `409`, ya tiene turnos.
    ```bash
    curl -X PUT http://localhost:5000/api/combates/1 -H 'Content-Type: application/json' -d '{"personaje1Id":1,"personaje2Id":3}'
    ```
11. `DELETE /api/combates/1` — `204`, eliminado; `404`, no existe; `409`, ya tiene turnos.
    ```bash
    curl -i -X DELETE http://localhost:5000/api/combates/1
    ```

### Turnos

12. `GET /api/combates/1/turnos` — `200`, historial ordenado; `404`, combate inexistente.
    ```bash
    curl http://localhost:5000/api/combates/1/turnos
    ```
13. `POST /api/combates/1/turnos` — `200`, turno y estado actualizados; `400`, acción inválida/fuera de turno/combate finalizado; `404`, combate o atacante inexistente.
    ```bash
    curl -X POST http://localhost:5000/api/combates/1/turnos -H 'Content-Type: application/json' -d '{"atacanteId":1,"accion":"atacar"}'
    ```
    Para habilidad, enviá `"accion":"habilidad"`. El primer turno corresponde al participante 1; el segundo, al participante 2.
14. `PUT /api/combates/1/turnos/1` — `200`, nota actualizada; `400`, nota inválida; `404`, combate/turno inexistente.
    ```bash
    curl -X PUT http://localhost:5000/api/combates/1/turnos/1 -H 'Content-Type: application/json' -d '{"nota":"Turno de prueba"}'
    ```
15. `DELETE /api/combates/1/turnos/1` — `204`, último turno borrado y vida restaurada; `404`, combate/turno inexistente; `409`, no es el último turno.
    ```bash
    curl -i -X DELETE http://localhost:5000/api/combates/1/turnos/1
    ```

## Comprobar las reglas 409 y la reversión

- Creá un combate y ejecutá un turno. Intentá `PUT /api/combates/{id}` o `DELETE /api/combates/{id}`: ambas operaciones deben responder `409`.
- Con dos turnos registrados, intentá borrar el primero: debe responder `409`. Borrá el segundo (el último): debe responder `204`.
- Consultá `GET /api/personajes/{id}` antes del turno, después del turno y después del DELETE. La vida debe volver a los valores previos al turno eliminado.
- Cambiá la nota de un turno y consultá el combate: sólo debe cambiar `nota`; daño y vida permanecen iguales.
- Intentá enviar dos turnos seguidos con el ID del primer participante: el segundo debe responder `400` porque está fuera de turno.

## Backup y restauración

El archivo `backup/combate_api.sql` incluido contiene estructura y datos de muestra, nunca usuarios ni contraseñas. Restauración inicial:

```bash
mysql -u root -p < backup/combate_api.sql
```

Para exportar un backup actualizado con los datos reales del servidor:

```bash
mysqldump -u root -p --single-transaction --routines --triggers --no-tablespaces combate_api > backup/combate_api.sql
```

Para restaurarlo:

```bash
mysql -u root -p < backup/combate_api.sql
```

El backup no configura los usuarios: ejecutá `sql/02_usuarios.sql` por separado en una instalación nueva.

## Entrega y defensa

- `docs/punto-de-partida.md`: modelo inicial, recorrido, respuestas, hipótesis y DER.
- `docs/indagacion.md`: Minimal API, DTO, DI, Swagger y permisos.
- `docs/reflexion-final.md`: borrador técnico. Completá la dificultad que realmente encontraste, qué aprendiste y tu comparación personal antes de la defensa.
- Probá manualmente las quince rutas desde Swagger y guardá evidencia para la demostración.
