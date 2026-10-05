CREATE DATABASE IF NOT EXISTS juego_db;
USE juego_db;

DROP TABLE IF EXISTS Turno;
DROP TABLE IF EXISTS Batalla;
DROP TABLE IF EXISTS Personaje;

CREATE TABLE Personaje
(
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Tipo VARCHAR(20) NOT NULL,
    Vida INT NOT NULL CHECK (Vida >= 0),
    Ataque INT NOT NULL CHECK (Ataque > 0),
    Defensa INT NOT NULL CHECK (Defensa >= 0)
);

CREATE TABLE Batalla
(
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Fecha DATETIME NOT NULL,
    Personaje1Id INT NOT NULL,
    Personaje2Id INT NOT NULL,
    GanadorId INT NULL,
    Estado VARCHAR(20) NOT NULL,
    FOREIGN KEY (Personaje1Id) REFERENCES Personaje(Id),
    FOREIGN KEY (Personaje2Id) REFERENCES Personaje(Id),
    FOREIGN KEY (GanadorId) REFERENCES Personaje(Id)
);

CREATE TABLE Turno
(
    Id INT AUTO_INCREMENT PRIMARY KEY,
    BatallaId INT NOT NULL,
    AtacanteId INT NOT NULL,
    DefensorId INT NOT NULL,
    Danio INT NOT NULL CHECK (Danio >= 0),
    TipoAccion VARCHAR(20) NOT NULL,
    Numero INT NOT NULL,
    Fecha DATETIME NOT NULL,
    FOREIGN KEY (BatallaId) REFERENCES Batalla(Id),
    FOREIGN KEY (AtacanteId) REFERENCES Personaje(Id),
    FOREIGN KEY (DefensorId) REFERENCES Personaje(Id)
);
