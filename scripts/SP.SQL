USE juego_db;

DELIMITER //
CREATE PROCEDURE RegistrarBatalla(IN p_GanadorId INT)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
    END;

    START TRANSACTION;
        INSERT INTO Batalla (Fecha, GanadorId) VALUES (NOW(), p_GanadorId);
    COMMIT;
END //
DELIMITER ;
