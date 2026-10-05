USE juego_db;

DELIMITER //

DROP PROCEDURE IF EXISTS RegistrarBatalla//
CREATE PROCEDURE RegistrarBatalla(
    IN p_Personaje1Id INT,
    IN p_Personaje2Id INT
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    IF p_Personaje1Id = p_Personaje2Id THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Los personajes deben ser diferentes';
    END IF;

    IF NOT EXISTS (SELECT 1 FROM Personaje WHERE Id = p_Personaje1Id) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El personaje 1 no existe';
    END IF;

    IF NOT EXISTS (SELECT 1 FROM Personaje WHERE Id = p_Personaje2Id) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El personaje 2 no existe';
    END IF;

    INSERT INTO Batalla
    (
        Fecha,
        Personaje1Id,
        Personaje2Id,
        GanadorId,
        Estado
    )
    VALUES
    (
        NOW(),
        p_Personaje1Id,
        p_Personaje2Id,
        NULL,
        'EnProgreso'
    );

    SELECT LAST_INSERT_ID() AS Id;

    COMMIT;
END//

DROP PROCEDURE IF EXISTS RegistrarTurno//
CREATE PROCEDURE RegistrarTurno(
    IN p_BatallaId INT,
    IN p_AtacanteId INT,
    IN p_DefensorId INT,
    IN p_Danio INT,
    IN p_TipoAccion VARCHAR(20),
    IN p_Numero INT
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    INSERT INTO Turno
    (
        BatallaId,
        AtacanteId,
        DefensorId,
        Danio,
        TipoAccion,
        Numero,
        Fecha
    )
    VALUES
    (
        p_BatallaId,
        p_AtacanteId,
        p_DefensorId,
        p_Danio,
        p_TipoAccion,
        p_Numero,
        NOW()
    );

    COMMIT;
END//

DROP PROCEDURE IF EXISTS FinalizarBatalla//
CREATE PROCEDURE FinalizarBatalla(
    IN p_BatallaId INT,
    IN p_GanadorId INT
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    IF NOT EXISTS (SELECT 1 FROM Batalla WHERE Id = p_BatallaId) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'La batalla no existe';
    END IF;

    UPDATE Batalla
    SET GanadorId = p_GanadorId,
        Estado = 'Finalizada'
    WHERE Id = p_BatallaId;

    COMMIT;
END//

DELIMITER ;
