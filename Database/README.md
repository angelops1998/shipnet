# Base de datos ShipNet

No usamos migraciones de EF Core. La base se crea a mano con estos pasos:

1. Borrar la base si ya existe: `DROP DATABASE IF EXISTS shipnet;`
2. Abrir y ejecutar completo `schema.sql` (crea la base y todas las tablas).
3. Correr la app una vez (`dotnet run`) y cerrarla: crea los roles, los usuarios de prueba y el docente.
4. Abrir y ejecutar completo `seed.sql` (materias, equipos y grupos de prueba).

## Usuarios de prueba (los crea la app):

| Correo | Contraseña | Rol |
|---|---|---|
| admin@shipnet.com | admin123 | Admin |
| docente@shipnet.com | docente123 | Docente |
| estudiante@shipnet.com | est123 | Estudiante |

## Verificación rápida:
Verifica que los datos se hayan cargado correctamente ejecutando:
SELECT * FROM materias; 
SELECT * FROM equipos; 
SELECT * FROM grupos;