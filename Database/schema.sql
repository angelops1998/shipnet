-- =============================================================================
-- SHIPNET - BASE DE DATOS INSTITUCIONAL UPDS 2026
-- Sistema de Control Modular de Asistencia y Evaluaciones por Validación MAC
-- Motor: MySQL / MariaDB (utf8mb4)
-- =============================================================================

CREATE DATABASE IF NOT EXISTS `shipnet`
  CHARACTER SET utf8mb4 
  COLLATE utf8mb4_unicode_ci;

USE `shipnet`;

SET FOREIGN_KEY_CHECKS = 0;

-- -----------------------------------------------------------------------------
-- 1. TABLAS DE SEGURIDAD Y AUTENTICACIÓN (ASP.NET CORE IDENTITY)
-- -----------------------------------------------------------------------------

DROP TABLE IF EXISTS `aspnetroleclaims`;
DROP TABLE IF EXISTS `aspnetuserclaims`;
DROP TABLE IF EXISTS `aspnetuserlogins`;
DROP TABLE IF EXISTS `aspnetuserroles`;
DROP TABLE IF EXISTS `aspnetusertokens`;
DROP TABLE IF EXISTS `aspnetroles`;
DROP TABLE IF EXISTS `aspnetusers`;

CREATE TABLE `aspnetroles` (
    `Id` VARCHAR(255) NOT NULL PRIMARY KEY,
    `Name` VARCHAR(256) NULL,
    `NormalizedName` VARCHAR(256) NULL UNIQUE,
    `ConcurrencyStamp` LONGTEXT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `aspnetusers` (
    `Id` VARCHAR(255) NOT NULL PRIMARY KEY,
    `NombreCompleto` VARCHAR(255) NULL,
    `UserName` VARCHAR(256) NULL,
    `NormalizedUserName` VARCHAR(256) NULL UNIQUE,
    `Email` VARCHAR(256) NULL,
    `NormalizedEmail` VARCHAR(256) NULL,
    `EmailConfirmed` TINYINT(1) NOT NULL DEFAULT 1,
    `PasswordHash` LONGTEXT NULL,
    `SecurityStamp` LONGTEXT NULL,
    `ConcurrencyStamp` LONGTEXT NULL,
    `PhoneNumber` VARCHAR(50) NULL,
    `PhoneNumberConfirmed` TINYINT(1) NOT NULL DEFAULT 0,
    `TwoFactorEnabled` TINYINT(1) NOT NULL DEFAULT 0,
    `LockoutEnd` DATETIME(6) NULL,
    `LockoutEnabled` TINYINT(1) NOT NULL DEFAULT 0,
    `AccessFailedCount` INT NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `aspnetuserroles` (
    `UserId` VARCHAR(255) NOT NULL,
    `RoleId` VARCHAR(255) NOT NULL,
    PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_AspNetUserRoles_AspNetRoles` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AspNetUserRoles_AspNetUsers` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `aspnetroleclaims` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `RoleId` VARCHAR(255) NOT NULL,
    `ClaimType` LONGTEXT NULL,
    `ClaimValue` LONGTEXT NULL,
    CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `aspnetuserclaims` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `UserId` VARCHAR(255) NOT NULL,
    `ClaimType` LONGTEXT NULL,
    `ClaimValue` LONGTEXT NULL,
    CONSTRAINT `FK_AspNetUserClaims_AspNetUsers` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `aspnetuserlogins` (
    `LoginProvider` VARCHAR(255) NOT NULL,
    `ProviderKey` VARCHAR(255) NOT NULL,
    `ProviderDisplayName` LONGTEXT NULL,
    `UserId` VARCHAR(255) NOT NULL,
    PRIMARY KEY (`LoginProvider`, `ProviderKey`),
    CONSTRAINT `FK_AspNetUserLogins_AspNetUsers` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `aspnetusertokens` (
    `UserId` VARCHAR(255) NOT NULL,
    `LoginProvider` VARCHAR(255) NOT NULL,
    `Name` VARCHAR(255) NOT NULL,
    `Value` LONGTEXT NULL,
    PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
    CONSTRAINT `FK_AspNetUserTokens_AspNetUsers` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- -----------------------------------------------------------------------------
-- 2. TABLAS ACADÉMICAS Y DE GESTIÓN MODULAR
-- -----------------------------------------------------------------------------

DROP TABLE IF EXISTS `intentosevaluacion`;
DROP TABLE IF EXISTS `evaluaciones`;
DROP TABLE IF EXISTS `registrosasistencia`;
DROP TABLE IF EXISTS `sesionesasistencia`;
DROP TABLE IF EXISTS `inscripciones`;
DROP TABLE IF EXISTS `estudiantes`;
DROP TABLE IF EXISTS `grupos`;
DROP TABLE IF EXISTS `materias`;
DROP TABLE IF EXISTS `carreras`;
DROP TABLE IF EXISTS `docentes`;
DROP TABLE IF EXISTS `equipos`;

-- Carreras Oficiales UPDS
CREATE TABLE `carreras` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `Nombre` VARCHAR(200) NOT NULL,
    `Codigo` VARCHAR(20) NOT NULL UNIQUE,
    `Facultad` VARCHAR(150) NOT NULL,
    `Duracion` INT NOT NULL DEFAULT 10,
    `Activa` TINYINT(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Materias por Carrera y Semestre
CREATE TABLE `materias` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `Nombre` VARCHAR(200) NOT NULL,
    `Codigo` VARCHAR(50) NOT NULL UNIQUE,
    `CarreraId` INT NULL,
    `Semestre` INT NOT NULL DEFAULT 1,
    CONSTRAINT `FK_Materias_Carreras` FOREIGN KEY (`CarreraId`) REFERENCES `carreras` (`Id`) ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Docentes
CREATE TABLE `docentes` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `Nombre` VARCHAR(100) NOT NULL,
    `Apellido` VARCHAR(100) NOT NULL,
    `ApplicationUserId` VARCHAR(255) NOT NULL,
    `Email` VARCHAR(256) NULL,
    `Telefono` VARCHAR(50) NULL,
    `Especialidad` VARCHAR(150) NULL,
    CONSTRAINT `FK_Docentes_AspNetUsers` FOREIGN KEY (`ApplicationUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Equipos y Terminales Físicas de Laboratorio
CREATE TABLE `equipos` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `Codigo` VARCHAR(50) NOT NULL,
    `MacAddress` VARCHAR(17) NOT NULL UNIQUE,
    `Ubicacion` VARCHAR(255) NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Grupos Modulares (Asignación Docente - Materia - Módulo - Turno)
CREATE TABLE `grupos` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `Nombre` VARCHAR(100) NOT NULL,
    `MateriaId` INT NOT NULL,
    `DocenteId` INT NOT NULL,
    `Modulo` VARCHAR(50) NOT NULL DEFAULT 'Módulo 9 (Septiembre)',
    `Turno` VARCHAR(50) NOT NULL DEFAULT 'Mañana (07:30 - 10:00)',
    `Activo` TINYINT(1) NOT NULL DEFAULT 1,
    CONSTRAINT `FK_Grupos_Materias` FOREIGN KEY (`MateriaId`) REFERENCES `materias` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Grupos_Docentes` FOREIGN KEY (`DocenteId`) REFERENCES `docentes` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Estudiantes
CREATE TABLE `estudiantes` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `Nombre` VARCHAR(100) NOT NULL,
    `Apellido` VARCHAR(100) NOT NULL,
    `CI` VARCHAR(20) NOT NULL,
    `ApplicationUserId` VARCHAR(255) NOT NULL,
    `GrupoId` INT NULL,
    `MacAddress` VARCHAR(17) NULL,
    `Carrera` VARCHAR(200) NULL,
    `Semestre` INT NOT NULL DEFAULT 1,
    CONSTRAINT `FK_Estudiantes_AspNetUsers` FOREIGN KEY (`ApplicationUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Estudiantes_Grupos` FOREIGN KEY (`GrupoId`) REFERENCES `grupos` (`Id`) ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Inscripciones Modulares (Reglas: 1 por turno, máx. 7 por semestre)
CREATE TABLE `inscripciones` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `EstudianteId` INT NOT NULL,
    `GrupoId` INT NOT NULL,
    `FechaInscripcion` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT `FK_Inscripciones_Estudiantes` FOREIGN KEY (`EstudianteId`) REFERENCES `estudiantes` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Inscripciones_Grupos` FOREIGN KEY (`GrupoId`) REFERENCES `grupos` (`Id`) ON DELETE CASCADE,
    UNIQUE KEY `UQ_Estudiante_Grupo` (`EstudianteId`, `GrupoId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Sesiones de Asistencia de Docente
CREATE TABLE `sesionesasistencia` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `GrupoId` INT NOT NULL,
    `Fecha` DATE NOT NULL,
    `HoraInicio` TIME NOT NULL,
    `HoraFin` TIME NULL,
    `Abierta` TINYINT(1) NOT NULL DEFAULT 1,
    CONSTRAINT `FK_SesionesAsistencia_Grupos` FOREIGN KEY (`GrupoId`) REFERENCES `grupos` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Registros de Asistencia Validados por MAC
CREATE TABLE `registrosasistencia` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `SesionAsistenciaId` INT NOT NULL,
    `EstudianteId` INT NOT NULL,
    `EquipoId` INT NOT NULL,
    `Hora` TIME NOT NULL,
    `Observacion` VARCHAR(255) NULL,
    CONSTRAINT `FK_Registros_Sesion` FOREIGN KEY (`SesionAsistenciaId`) REFERENCES `sesionesasistencia` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Registros_Estudiante` FOREIGN KEY (`EstudianteId`) REFERENCES `estudiantes` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Registros_Equipo` FOREIGN KEY (`EquipoId`) REFERENCES `equipos` (`Id`) ON DELETE CASCADE,
    UNIQUE KEY `UQ_Sesion_Estudiante` (`SesionAsistenciaId`, `EstudianteId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Evaluaciones y Exámenes Parciales Multimedia
CREATE TABLE `evaluaciones` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `Titulo` VARCHAR(200) NOT NULL,
    `GrupoId` INT NOT NULL,
    `FechaInicio` DATETIME NOT NULL,
    `FechaFin` DATETIME NOT NULL,
    `DuracionMinutos` INT NOT NULL DEFAULT 45,
    `PreguntasJson` LONGTEXT NULL,
    CONSTRAINT `FK_Evaluaciones_Grupos` FOREIGN KEY (`GrupoId`) REFERENCES `grupos` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Intentos y Calificaciones de Evaluaciones
CREATE TABLE `intentosevaluacion` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `EvaluacionId` INT NOT NULL,
    `EstudianteId` INT NOT NULL,
    `EquipoId` INT NOT NULL,
    `FechaInicio` DATETIME NOT NULL,
    `FechaFin` DATETIME NULL,
    `Puntaje` DECIMAL(5,2) NULL,
    `RespuestasJson` LONGTEXT NULL,
    CONSTRAINT `FK_Intentos_Evaluacion` FOREIGN KEY (`EvaluacionId`) REFERENCES `evaluaciones` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Intentos_Estudiante` FOREIGN KEY (`EstudianteId`) REFERENCES `estudiantes` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Intentos_Equipo` FOREIGN KEY (`EquipoId`) REFERENCES `equipos` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

SET FOREIGN_KEY_CHECKS = 1;

-- =============================================================================
-- 3. DATOS INICIALES SEMILLA (SEED DATA)
-- =============================================================================

-- Roles
INSERT INTO `aspnetroles` (`Id`, `Name`, `NormalizedName`) VALUES
('rol-admin', 'Admin', 'ADMIN'),
('rol-docente', 'Docente', 'DOCENTE'),
('rol-estudiante', 'Estudiante', 'ESTUDIANTE');

-- Usuarios Base (Contraseñas: admin123, docente123, est123)
-- Hash PBKDF2 de ASP.NET Identity:
-- admin123   -> AQAAAAIAAYagAAAAEFPyZbjsrGVXRDxhG+YUlrLHZgyydSKKsx2B/FFRoWRFN2o1hsqsmgB3llcllILRmA==
-- docente123 -> AQAAAAIAAYagAAAAEJEBAzr6Lc/zBCp6qgTpGzXZunOjOczURwOzRZMSdtQg1IlkAlcQ9pUKOMmZS+jx4g==
-- est123     -> AQAAAAIAAYagAAAAEDVdjsJwoby3N3/XxXp+Ho+RIVeQKu9b8OGTKmwJ1SnNuTQ5oZoUo73fDgRZYhXgMg==

INSERT INTO `aspnetusers` (`Id`, `NombreCompleto`, `UserName`, `NormalizedUserName`, `Email`, `NormalizedEmail`, `PasswordHash`, `SecurityStamp`, `ConcurrencyStamp`) VALUES
('usr-admin', 'Administrador General', 'admin@shipnet.com', 'ADMIN@SHIPNET.COM', 'admin@shipnet.com', 'ADMIN@SHIPNET.COM', 'AQAAAAIAAYagAAAAEFPyZbjsrGVXRDxhG+YUlrLHZgyydSKKsx2B/FFRoWRFN2o1hsqsmgB3llcllILRmA==', UUID(), UUID()),
('usr-docente', 'Juan Perez', 'docente@shipnet.com', 'DOCENTE@SHIPNET.COM', 'docente@shipnet.com', 'DOCENTE@SHIPNET.COM', 'AQAAAAIAAYagAAAAEJEBAzr6Lc/zBCp6qgTpGzXZunOjOczURwOzRZMSdtQg1IlkAlcQ9pUKOMmZS+jx4g==', UUID(), UUID()),
('usr-estudiante', 'Ana Lopez', 'estudiante@shipnet.com', 'ESTUDIANTE@SHIPNET.COM', 'estudiante@shipnet.com', 'ESTUDIANTE@SHIPNET.COM', 'AQAAAAIAAYagAAAAEDVdjsJwoby3N3/XxXp+Ho+RIVeQKu9b8OGTKmwJ1SnNuTQ5oZoUo73fDgRZYhXgMg==', UUID(), UUID());

INSERT INTO `aspnetuserroles` (`UserId`, `RoleId`) VALUES
('usr-admin', 'rol-admin'),
('usr-docente', 'rol-docente'),
('usr-estudiante', 'rol-estudiante');

-- Carreras Oficiales UPDS
INSERT INTO `carreras` (`Id`, `Nombre`, `Codigo`, `Facultad`, `Duracion`, `Activa`) VALUES
(1, 'Ingeniería de Sistemas', 'SIS', 'Facultad de Ingeniería', 10, 1),
(2, 'Ingeniería en Redes y Telecomunicaciones', 'RED', 'Facultad de Ingeniería', 10, 1),
(3, 'Ingeniería Industrial', 'IND', 'Facultad de Ingeniería', 10, 1),
(4, 'Ingeniería Civil', 'CIV', 'Facultad de Ingeniería', 10, 1),
(5, 'Derecho', 'DER', 'Facultad de Ciencias Jurídicas', 10, 1),
(6, 'Administración de Empresas', 'ADM', 'Facultad de Ciencias Empresariales', 10, 1),
(7, 'Contaduría Pública', 'CPA', 'Facultad de Ciencias Empresariales', 10, 1),
(8, 'Ingeniería Comercial', 'ICO', 'Facultad de Ciencias Empresariales', 10, 1),
(9, 'Marketing y Publicidad', 'MKT', 'Facultad de Ciencias Empresariales', 10, 1),
(10, 'Psicología', 'PSI', 'Facultad de Ciencias Sociales y Humanísticas', 10, 1),
(11, 'Ciencias de la Comunicación', 'COM', 'Facultad de Ciencias Sociales y Humanísticas', 10, 1),
(12, 'Medicina', 'MED', 'Facultad de Ciencias de la Salud', 12, 1),
(13, 'Enfermería', 'ENF', 'Facultad de Ciencias de la Salud', 10, 1),
(14, 'Fisioterapia y Kinesiología', 'FIS', 'Facultad de Ciencias de la Salud', 10, 1);

-- Materias Semilla
INSERT INTO `materias` (`Id`, `Nombre`, `Codigo`, `CarreraId`, `Semestre`) VALUES
(1, 'Programación I', 'SIS-101', 1, 1),
(2, 'Redes de Computadoras I', 'RED-201', 2, 3),
(3, 'Bases de Datos I', 'SIS-202', 1, 3),
(4, 'Sistemas Operativos', 'SIS-301', 1, 4),
(5, 'Seguridad Informática', 'RED-401', 2, 7),
(6, 'Derecho Constitucional', 'DER-101', 5, 1),
(7, 'Administración General', 'ADM-101', 6, 1);

-- Docente Demo
INSERT INTO `docentes` (`Id`, `Nombre`, `Apellido`, `ApplicationUserId`, `Email`, `Telefono`, `Especialidad`) VALUES
(1, 'Juan', 'Perez', 'usr-docente', 'docente@shipnet.com', '70712345', 'Redes y Telecomunicaciones');

-- Estudiante Demo
INSERT INTO `estudiantes` (`Id`, `Nombre`, `Apellido`, `CI`, `ApplicationUserId`, `MacAddress`, `Carrera`, `Semestre`) VALUES
(1, 'Ana', 'Lopez', '1234567', 'usr-estudiante', '14:D4:24:1B:EE:27', 'Ingeniería de Sistemas', 1);

-- Equipos de Laboratorio
INSERT INTO `equipos` (`Id`, `Codigo`, `MacAddress`, `Ubicacion`) VALUES
(1, 'LAB1-PC01', '00:1A:2B:3C:4D:5E', 'Laboratorio 1 - Fila 1'),
(2, 'LAB1-PC02', '00:1A:2B:3C:4D:5F', 'Laboratorio 1 - Fila 1'),
(3, 'LAB1-PC03', '00:1A:2B:3C:4D:60', 'Laboratorio 1 - Fila 2'),
(4, 'LAB1-PC04', '00:1A:2B:3C:4D:61', 'Laboratorio 1 - Fila 2'),
(5, 'LAB1-PC05', '14:D4:24:1B:EE:27', 'Laboratorio Central - Terminal Demo');

-- Grupos Modulares
INSERT INTO `grupos` (`Id`, `Nombre`, `MateriaId`, `DocenteId`, `Modulo`, `Turno`, `Activo`) VALUES
(1, 'Grupo A - Mañana', 1, 1, 'Módulo 9 (Septiembre)', 'Mañana (07:30 - 10:00)', 1),
(2, 'Grupo B - Mediodía', 1, 1, 'Módulo 9 (Septiembre)', 'Mediodía (11:00 - 13:30)', 1),
(3, 'Grupo C - Tarde', 2, 1, 'Módulo 9 (Septiembre)', 'Tarde (16:00 - 18:30)', 1),
(4, 'Grupo D - Noche', 3, 1, 'Módulo 9 (Septiembre)', 'Noche (19:00 - 21:30)', 1);

-- Inscripción Demo
INSERT INTO `inscripciones` (`EstudianteId`, `GrupoId`) VALUES
(1, 2);