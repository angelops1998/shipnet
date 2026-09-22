USE shipnet;

-- 1. Insertar Materias
INSERT INTO materias (Nombre, Codigo) VALUES
('Redes I', 'RED-101'),
('Bases de Datos', 'BD-201');

-- 2. Insertar Equipos (PC-SERVIDOR es clave para pruebas locales)
INSERT INTO equipos (Codigo, MacAddress, Ubicacion) VALUES
('PC-SERVIDOR', '00:00:00:00:00:00', 'PC donde corre la app (pruebas en localhost)'),
('PC-01', 'AA:BB:CC:DD:EE:01', 'Lab 2, fila 1'),
('PC-02', 'AA:BB:CC:DD:EE:02', 'Lab 2, fila 1');

-- 3. Insertar Grupos
INSERT INTO grupos (Nombre, MateriaId, DocenteId) VALUES
('Grupo A', (SELECT Id FROM materias WHERE Codigo = 'RED-101'), (SELECT Id FROM docentes LIMIT 1)),
('Grupo B', (SELECT Id FROM materias WHERE Codigo = 'BD-201'), (SELECT Id FROM docentes LIMIT 1));