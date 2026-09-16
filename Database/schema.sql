CREATE DATABASE IF NOT EXISTS shipnet
CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE shipnet;

CREATE TABLE AspNetRoles (
    Id VARCHAR(255) NOT NULL PRIMARY KEY,
    Name VARCHAR(256) NULL,
    NormalizedName VARCHAR(256) NULL UNIQUE,
    ConcurrencyStamp LONGTEXT NULL
);

CREATE TABLE AspNetUsers (
    Id VARCHAR(255) NOT NULL PRIMARY KEY,
    NombreCompleto VARCHAR(255) NULL, 
    UserName VARCHAR(256) NULL,
    NormalizedUserName VARCHAR(256) NULL UNIQUE,
    Email VARCHAR(256) NULL,
    NormalizedEmail VARCHAR(256) NULL,
    EmailConfirmed TINYINT(1) NOT NULL,
    PasswordHash LONGTEXT NULL,
    SecurityStamp LONGTEXT NULL,
    ConcurrencyStamp LONGTEXT NULL,
    PhoneNumber LONGTEXT NULL,
    PhoneNumberConfirmed TINYINT(1) NOT NULL,
    TwoFactorEnabled TINYINT(1) NOT NULL,
    LockoutEnd DATETIME(6) NULL,
    LockoutEnabled TINYINT(1) NOT NULL,
    AccessFailedCount INT NOT NULL
);

CREATE TABLE AspNetRoleClaims (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    RoleId VARCHAR(255) NOT NULL,
    ClaimType LONGTEXT NULL,
    ClaimValue LONGTEXT NULL,
    CONSTRAINT FK_AspNetRoleClaims_AspNetRoles_RoleId FOREIGN KEY (RoleId) REFERENCES AspNetRoles (Id) ON DELETE CASCADE
);

CREATE TABLE AspNetUserClaims (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    UserId VARCHAR(255) NOT NULL,
    ClaimType LONGTEXT NULL,
    ClaimValue LONGTEXT NULL,
    CONSTRAINT FK_AspNetUserClaims_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE
);

CREATE TABLE AspNetUserLogins (
    LoginProvider VARCHAR(255) NOT NULL,
    ProviderKey VARCHAR(255) NOT NULL,
    ProviderDisplayName LONGTEXT NULL,
    UserId VARCHAR(255) NOT NULL,
    PRIMARY KEY (LoginProvider, ProviderKey),
    CONSTRAINT FK_AspNetUserLogins_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE
);

CREATE TABLE AspNetUserRoles (
    UserId VARCHAR(255) NOT NULL,
    RoleId VARCHAR(255) NOT NULL,
    PRIMARY KEY (UserId, RoleId),
    CONSTRAINT FK_AspNetUserRoles_AspNetRoles_RoleId FOREIGN KEY (RoleId) REFERENCES AspNetRoles (Id) ON DELETE CASCADE,
    CONSTRAINT FK_AspNetUserRoles_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE
);

CREATE TABLE AspNetUserTokens (
    UserId VARCHAR(255) NOT NULL,
    LoginProvider VARCHAR(255) NOT NULL,
    Name VARCHAR(255) NOT NULL,
    Value LONGTEXT NULL,
    PRIMARY KEY (UserId, LoginProvider, Name),
    CONSTRAINT FK_AspNetUserTokens_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE
);

CREATE TABLE Materias (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(255) NOT NULL,
    Codigo VARCHAR(50) NOT NULL
);

CREATE TABLE Equipos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Codigo VARCHAR(50) NOT NULL,
    MacAddress VARCHAR(17) NOT NULL UNIQUE,
    Ubicacion VARCHAR(255) NULL
);

CREATE TABLE Estudiantes (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    CI VARCHAR(20) NOT NULL,
    ApplicationUserId VARCHAR(255) NOT NULL,
    CONSTRAINT FK_Estudiantes_AspNetUsers FOREIGN KEY (ApplicationUserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE
);

CREATE TABLE Docentes (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    ApplicationUserId VARCHAR(255) NOT NULL,
    CONSTRAINT FK_Docentes_AspNetUsers FOREIGN KEY (ApplicationUserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE
);

CREATE TABLE Grupos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    MateriaId INT NOT NULL,
    DocenteId INT NOT NULL,
    CONSTRAINT FK_Grupos_Materias FOREIGN KEY (MateriaId) REFERENCES Materias (Id) ON DELETE CASCADE,
    CONSTRAINT FK_Grupos_Docentes FOREIGN KEY (DocenteId) REFERENCES Docentes (Id) ON DELETE CASCADE
);

CREATE TABLE SesionesAsistencia (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    GrupoId INT NOT NULL,
    Fecha DATE NOT NULL,
    HoraInicio TIME NULL,
    HoraFin TIME NULL,
    Abierta TINYINT(1) NOT NULL DEFAULT 1,
    CONSTRAINT FK_Sesiones_Grupos FOREIGN KEY (GrupoId) REFERENCES Grupos (Id) ON DELETE CASCADE
);

CREATE TABLE Evaluaciones (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    GrupoId INT NOT NULL,
    Titulo VARCHAR(255) NOT NULL,
    FechaInicio DATETIME NOT NULL,
    FechaFin DATETIME NOT NULL,
    DuracionMinutos INT NOT NULL,
    CONSTRAINT FK_Evaluaciones_Grupos FOREIGN KEY (GrupoId) REFERENCES Grupos (Id) ON DELETE CASCADE
);

CREATE TABLE RegistrosAsistencia (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    SesionAsistenciaId INT NOT NULL,
    EstudianteId INT NOT NULL,
    EquipoId INT NOT NULL,
    HoraRegistro TIME NOT NULL,
    Estado VARCHAR(50) NOT NULL,
    CONSTRAINT FK_Registros_Sesiones FOREIGN KEY (SesionAsistenciaId) REFERENCES SesionesAsistencia (Id) ON DELETE CASCADE,
    CONSTRAINT FK_Registros_Estudiantes FOREIGN KEY (EstudianteId) REFERENCES Estudiantes (Id) ON DELETE CASCADE,
    CONSTRAINT FK_Registros_Equipos FOREIGN KEY (EquipoId) REFERENCES Equipos (Id) ON DELETE CASCADE
);

CREATE TABLE IntentosEvaluacion (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    EvaluacionId INT NOT NULL,
    EstudianteId INT NOT NULL,
    EquipoId INT NOT NULL,
    FechaInicio DATETIME NOT NULL,
    FechaFin DATETIME NULL,
    Puntaje DECIMAL(5,2) NULL,
    CONSTRAINT FK_Intentos_Evaluaciones FOREIGN KEY (EvaluacionId) REFERENCES Evaluaciones (Id) ON DELETE CASCADE,
    CONSTRAINT FK_Intentos_Estudiantes FOREIGN KEY (EstudianteId) REFERENCES Estudiantes (Id) ON DELETE CASCADE,
    CONSTRAINT FK_Intentos_Equipos FOREIGN KEY (EquipoId) REFERENCES Equipos (Id) ON DELETE CASCADE
);