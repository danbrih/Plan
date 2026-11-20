# Reflexión final — Combate por API

> Borrador técnico basado en la versión inicial del repositorio y en los cambios solicitados por SOFTWARE 2.4. Antes de entregar, completá las partes marcadas con tu experiencia real y compará con tu archivo `docs/punto-de-partida.md`.

## Decisiones que cambiaron

1. **De una solución sin API a Minimal API.** La solución inicial contenía Aplicacion, Persistencia y Tests, pero no tenía un proyecto API. La versión final agrega `src/API`, registra servicios y repositorios mediante inyección de dependencias y define las quince rutas solicitadas.
2. **De .NET 8 a .NET 10.** Los proyectos iniciales apuntaban a `net8.0`; la consigna de producción requiere `net10.0`.
3. **De una única conexión de desarrollo a permisos separados.** La configuración anterior usaba `administrador` y `desarrollo`, y desarrollo podía escribir. La consigna exige un invitado de sólo lectura y que todas las escrituras usen administrador.
4. **De turnos sin datos de reversión a turnos deshacibles.** La tabla original no guardaba la nota ni los puntos de vida previos de ambos participantes. Ahora cada turno conserva esos datos y el borrado del último turno restaura el estado dentro de una transacción.
5. **De entidades expuestas a contratos DTO.** Los endpoints reciben y devuelven DTO en lugar de publicar directamente las entidades del dominio.

## Aprendizajes

Completá con tus palabras qué aprendiste al aplicar polimorfismo, DTO, Minimal API, transacciones, permisos y Swagger. Incluí al menos un ejemplo concreto que puedas explicar durante la defensa oral.

## Dificultad real encontrada y resolución

Completá este apartado con una dificultad que efectivamente hayas encontrado al compilar, conectar MySQL, ejecutar las pruebas o probar una ruta, y describí el error y cómo lo resolviste. No inventes una dificultad que no hayas vivido.

## Mejora futura

Una mejora posible es ampliar la persistencia del estado específico de cada personaje (por ejemplo, recursos propios de sus habilidades) para que también pueda restaurarse exactamente al deshacer un turno, además de los puntos de vida requeridos por la consigna.
