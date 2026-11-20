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
