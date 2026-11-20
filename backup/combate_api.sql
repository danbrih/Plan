-- Backup inicial de estructura y datos de muestra de combate_api.
-- Para capturar datos reales posteriores, regenerar con mysqldump (README.md).
CREATE DATABASE IF NOT EXISTS combate_api
  CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE combate_api;

CREATE TABLE IF NOT EXISTS Personaje (
  Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  Nombre VARCHAR(50) NOT NULL,
  Tipo VARCHAR(20) NOT NULL,
  Vida INT NOT NULL,
  VidaMaxima INT NOT NULL,
  Ataque INT NOT NULL,
  Defensa INT NOT NULL,
  CONSTRAINT CK_Personaje_Vida CHECK (Vida >= 0),
  CONSTRAINT CK_Personaje_VidaMaxima CHECK (VidaMaxima > 0 AND Vida <= VidaMaxima),
  CONSTRAINT CK_Personaje_Ataque CHECK (Ataque > 0),
  CONSTRAINT CK_Personaje_Defensa CHECK (Defensa >= 0),
  CONSTRAINT CK_Personaje_Tipo CHECK (Tipo IN ('Guerrero','Mago','Arquero','Asesino'))
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS Batalla (
  Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  Fecha DATETIME NOT NULL,
  Personaje1Id INT NOT NULL,
  Personaje2Id INT NOT NULL,
  GanadorId INT NULL,
  Estado VARCHAR(20) NOT NULL DEFAULT 'EnProgreso',
  CONSTRAINT CK_Batalla_Participantes CHECK (Personaje1Id <> Personaje2Id),
  CONSTRAINT CK_Batalla_Estado CHECK (Estado IN ('EnProgreso','Finalizada')),
  CONSTRAINT FK_Batalla_Personaje1 FOREIGN KEY (Personaje1Id) REFERENCES Personaje(Id),
  CONSTRAINT FK_Batalla_Personaje2 FOREIGN KEY (Personaje2Id) REFERENCES Personaje(Id),
  CONSTRAINT FK_Batalla_Ganador FOREIGN KEY (GanadorId) REFERENCES Personaje(Id),
  INDEX IX_Batalla_Personaje1 (Personaje1Id),
  INDEX IX_Batalla_Personaje2 (Personaje2Id),
  INDEX IX_Batalla_Fecha (Fecha)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS Turno (
  Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  BatallaId INT NOT NULL,
  AtacanteId INT NOT NULL,
  DefensorId INT NOT NULL,
  Danio INT NOT NULL,
  TipoAccion VARCHAR(20) NOT NULL,
  Numero INT NOT NULL,
  Fecha DATETIME NOT NULL,
  Nota VARCHAR(255) NOT NULL DEFAULT '',
  VidaPersonaje1Antes INT NOT NULL,
  VidaPersonaje2Antes INT NOT NULL,
  EstadoAntes VARCHAR(20) NOT NULL,
  GanadorAntes INT NULL,
  CONSTRAINT CK_Turno_Danio CHECK (Danio >= 0),
  CONSTRAINT CK_Turno_Numero CHECK (Numero > 0),
  CONSTRAINT CK_Turno_Accion CHECK (TipoAccion IN ('atacar','habilidad')),
  CONSTRAINT FK_Turno_Batalla FOREIGN KEY (BatallaId) REFERENCES Batalla(Id),
  CONSTRAINT FK_Turno_Atacante FOREIGN KEY (AtacanteId) REFERENCES Personaje(Id),
  CONSTRAINT FK_Turno_Defensor FOREIGN KEY (DefensorId) REFERENCES Personaje(Id),
  CONSTRAINT FK_Turno_GanadorAntes FOREIGN KEY (GanadorAntes) REFERENCES Personaje(Id),
  CONSTRAINT UQ_Turno_Batalla_Numero UNIQUE (BatallaId, Numero),
  INDEX IX_Turno_Batalla (BatallaId)
) ENGINE=InnoDB;
USE combate_api;

INSERT INTO Personaje (Nombre, Tipo, Vida, VidaMaxima, Ataque, Defensa)
SELECT 'Conan', 'Guerrero', 100, 100, 10, 2
WHERE NOT EXISTS (SELECT 1 FROM Personaje WHERE Nombre = 'Conan' AND Tipo = 'Guerrero');

INSERT INTO Personaje (Nombre, Tipo, Vida, VidaMaxima, Ataque, Defensa)
SELECT 'Merlin', 'Mago', 100, 100, 10, 2
WHERE NOT EXISTS (SELECT 1 FROM Personaje WHERE Nombre = 'Merlin' AND Tipo = 'Mago');

INSERT INTO Personaje (Nombre, Tipo, Vida, VidaMaxima, Ataque, Defensa)
SELECT 'Robin', 'Arquero', 100, 100, 10, 2
WHERE NOT EXISTS (SELECT 1 FROM Personaje WHERE Nombre = 'Robin' AND Tipo = 'Arquero');

INSERT INTO Personaje (Nombre, Tipo, Vida, VidaMaxima, Ataque, Defensa)
SELECT 'Shade', 'Asesino', 100, 100, 10, 2
WHERE NOT EXISTS (SELECT 1 FROM Personaje WHERE Nombre = 'Shade' AND Tipo = 'Asesino');
