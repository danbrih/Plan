USE juego_db;

DELIMITER //

CREATE PROCEDURE ObtenerRankingEfectividad()
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
    END;

    START TRANSACTION;
        SELECT 
            p.Id, 
            p.Nombre, 
            p.Tipo,
            COUNT(b.Id) AS Victorias,
            (COUNT(b.Id) * 100) / (SELECT COUNT(*) FROM Batalla WHERE Personaje1Id = p.Id OR Personaje2Id = p.Id) AS Efectividad
        FROM Personaje p
        LEFT JOIN Batalla b ON p.Id = b.GanadorId
        GROUP BY p.Id, p.Nombre, p.Tipo
        ORDER BY Victorias DESC;
    COMMIT;
END //

CREATE PROCEDURE ObtenerResumenBatallas(IN p_FechaInicio DATETIME, IN p_FechaFin DATETIME)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
    END;

    START TRANSACTION;
        SELECT COUNT(*) AS TotalBatallas, Fecha
        FROM Batalla
        WHERE Fecha BETWEEN p_FechaInicio AND p_FechaFin
        GROUP BY DATE(Fecha);
    COMMIT;
END //

DELIMITER ;
