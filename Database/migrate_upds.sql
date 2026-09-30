USE shipnet;

CREATE TABLE IF NOT EXISTS carreras (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(200) NOT NULL,
    Codigo VARCHAR(20) NOT NULL UNIQUE,
    Facultad VARCHAR(200) NOT NULL,
    Duracion INT NOT NULL DEFAULT 10,
    Activa TINYINT(1) NOT NULL DEFAULT 1
) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

ALTER TABLE materias ADD COLUMN IF NOT EXISTS CarreraId INT NULL;
ALTER TABLE materias ADD COLUMN IF NOT EXISTS Semestre INT NOT NULL DEFAULT 1;
ALTER TABLE materias MODIFY Nombre VARCHAR(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
ALTER TABLE materias MODIFY Codigo VARCHAR(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

ALTER TABLE docentes ADD COLUMN IF NOT EXISTS Email VARCHAR(256) NULL;
ALTER TABLE docentes ADD COLUMN IF NOT EXISTS Telefono VARCHAR(20) NULL;
ALTER TABLE docentes ADD COLUMN IF NOT EXISTS Especialidad VARCHAR(200) NULL;

ALTER TABLE estudiantes ADD COLUMN IF NOT EXISTS MacAddress VARCHAR(17) NULL;
ALTER TABLE estudiantes ADD COLUMN IF NOT EXISTS Carrera VARCHAR(200) NULL;
ALTER TABLE estudiantes ADD COLUMN IF NOT EXISTS Semestre INT NOT NULL DEFAULT 1;

INSERT IGNORE INTO carreras (Nombre, Codigo, Facultad, Duracion, Activa) VALUES
('Ingeniería de Sistemas', 'ING-SIS', 'Facultad de Ingeniería y Tecnología', 10, 1),
('Ingeniería Civil', 'ING-CIV', 'Facultad de Ingeniería y Tecnología', 10, 1),
('Ingeniería en Redes y Telecomunicaciones', 'ING-RED', 'Facultad de Ingeniería y Tecnología', 10, 1),
('Ingeniería Industrial', 'ING-IND', 'Facultad de Ingeniería y Tecnología', 10, 1),
('Ingeniería Comercial', 'ING-COM', 'Facultad de Ciencias Empresariales', 10, 1),
('Ingeniería en Gestión Ambiental', 'ING-AMB', 'Facultad de Ingeniería y Tecnología', 10, 1),
('Ingeniería en Gestión Petrolera', 'ING-PET', 'Facultad de Ingeniería y Tecnología', 10, 1),
('Administración de Empresas', 'ADM-EMP', 'Facultad de Ciencias Empresariales', 10, 1),
('Contaduría Pública', 'CON-PUB', 'Facultad de Ciencias Empresariales', 10, 1),
('Derecho', 'DER-BOL', 'Facultad de Ciencias Jurídicas y Políticas', 10, 1),
('Psicología', 'PSI-CLN', 'Facultad de Humanidades y Ciencias de la Educación', 10, 1),
('Ciencias de la Comunicación Social', 'COM-SOC', 'Facultad de Humanidades y Ciencias de la Educación', 10, 1),
('Medicina', 'MED-BOL', 'Facultad de Ciencias de la Salud', 12, 1),
('Bioquímica y Farmacia', 'BIO-FAR', 'Facultad de Ciencias de la Salud', 10, 1),
('Marketing y Publicidad', 'MKT-PUB', 'Facultad de Ciencias Empresariales', 10, 1),
('Nutrición y Dietética', 'NUT-DIE', 'Facultad de Ciencias de la Salud', 10, 1),
('Fisioterapia y Kinesiología', 'FIS-KIN', 'Facultad de Ciencias de la Salud', 10, 1),
('Relaciones Internacionales', 'REL-INT', 'Facultad de Ciencias Jurídicas y Políticas', 10, 1);

UPDATE materias SET CarreraId = 1, Semestre = 3 WHERE Nombre = 'Matemáticas';
UPDATE materias SET CarreraId = 1, Semestre = 2 WHERE Nombre = 'Física';
UPDATE materias SET CarreraId = 1, Semestre = 2 WHERE Nombre = 'Química';
UPDATE materias SET CarreraId = 1, Semestre = 4 WHERE Nombre = 'Programación I';
UPDATE materias SET CarreraId = 1, Semestre = 5 WHERE Nombre = 'Bases de Datos';
UPDATE materias SET CarreraId = 3, Semestre = 5 WHERE Nombre = 'Redes I';
UPDATE materias SET CarreraId = 1, Semestre = 6 WHERE Nombre = 'Sistemas Operativos';
UPDATE materias SET CarreraId = 1, Semestre = 3 WHERE Nombre = 'Estadística';
UPDATE materias SET CarreraId = 1, Semestre = 1 WHERE Nombre = 'Inglés I';
UPDATE materias SET CarreraId = 1, Semestre = 9 WHERE Nombre = 'Inteligencia Artificial';

UPDATE docentes d
JOIN aspnetusers u ON d.ApplicationUserId = u.Id
SET d.Email = u.Email
WHERE d.Email IS NULL;
