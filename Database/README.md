# Base de datos ShipNet

No usamos migraciones de EF Core. La base se crea a mano con estos pasos:

1. Borrar la base si ya existe: `DROP DATABASE IF EXISTS shipnet;`
2. Abrir y ejecutar completo `schema.sql` (crea la base y todas las tablas).
3. Correr la API una vez (`cd ShipNetApi && dotnet run`) y cerrarla: crea los roles, los usuarios de prueba y el docente.
4. Abrir y ejecutar completo `seed.sql` (materias, equipos y grupos de prueba).

## Cómo levantar el proyecto

Son dos apps separadas; hay que levantar las dos, cada una en su propia terminal:

| Proyecto | Rol | URL | ¿Toca MySQL? |
|---|---|---|---|
| `ShipNetApi` | API REST (EF Core + Identity + JWT) | http://localhost:5220 (Swagger en `/swagger`) | Sí |
| `ShipNetMvc` | Interfaz web, consume la API con HttpClient | http://localhost:5176 | No, nunca |

```
cd ShipNetApi && dotnet run
cd ShipNetMvc && dotnet run
```

La cadena de conexión va solo en `ShipNetApi/appsettings.Development.json`. El MVC solo conoce la URL de la API (`ApiSettings:BaseUrl` en `ShipNetMvc/appsettings.json`).

## Usuarios de prueba (los crea la API):

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