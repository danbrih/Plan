USE juego_db;

DELIMITER //
CREATE PROCEDURE ObtenerRanking()
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
    END;

    START TRANSACTION;
        SELECT p.Nombre, COUNT(b.Id) AS Victorias
        FROM Personaje p
        JOIN Batalla b ON p.Id = b.GanadorId
        GROUP BY p.Id, p.Nombre
        ORDER BY Victorias DESC;
    COMMIT;
END //
DELIMITER ;
