USE juego_db;

DELIMITER //

DROP PROCEDURE IF EXISTS ObtenerResumenBatallasPorTipo//
CREATE PROCEDURE ObtenerResumenBatallasPorTipo(
    IN p_FechaInicio DATETIME,
    IN p_FechaFin DATETIME
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    SELECT
        p.Tipo,
        COUNT(DISTINCT b.Id) AS BatallasTotales,
        SUM(CASE WHEN b.GanadorId = p.Id THEN 1 ELSE 0 END) AS Victorias,
        SUM(CASE WHEN b.GanadorId IS NOT NULL AND b.GanadorId <> p.Id THEN 1 ELSE 0 END) AS Derrotas
    FROM Personaje p
    LEFT JOIN Batalla b
        ON (b.Personaje1Id = p.Id OR b.Personaje2Id = p.Id)
        AND b.Fecha BETWEEN p_FechaInicio AND p_FechaFin
    GROUP BY p.Tipo;

    COMMIT;
END//

DROP PROCEDURE IF EXISTS ObtenerVictoriasDerrotasPorTipo//
CREATE PROCEDURE ObtenerVictoriasDerrotasPorTipo(
    IN p_FechaInicio DATETIME,
    IN p_FechaFin DATETIME
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    SELECT
        p.Tipo,
        COUNT(DISTINCT b.Id) AS BatallasTotales,
        SUM(CASE WHEN b.GanadorId = p.Id THEN 1 ELSE 0 END) AS Victorias,
        SUM(CASE WHEN b.GanadorId IS NOT NULL AND b.GanadorId <> p.Id THEN 1 ELSE 0 END) AS Derrotas
    FROM Personaje p
    LEFT JOIN Batalla b
        ON (b.Personaje1Id = p.Id OR b.Personaje2Id = p.Id)
        AND b.Fecha BETWEEN p_FechaInicio AND p_FechaFin
        AND b.Estado = 'Finalizada'
    GROUP BY p.Tipo;

    COMMIT;
END//

DROP PROCEDURE IF EXISTS ObtenerRankingPersonajes//
CREATE PROCEDURE ObtenerRankingPersonajes(
    IN p_FechaInicio DATETIME,
    IN p_FechaFin DATETIME
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    SELECT
        p.Id,
        p.Nombre,
        p.Tipo,
        COUNT(b.Id) AS PartidasJugadas,
        SUM(CASE WHEN b.GanadorId = p.Id THEN 1 ELSE 0 END) AS Victorias,
        ROUND(
            (
                SUM(CASE WHEN b.GanadorId = p.Id THEN 1 ELSE 0 END)
                / NULLIF(COUNT(b.Id), 0)
            ) * 100,
            2
        ) AS PorcentajeEfectividad
    FROM Personaje p
    INNER JOIN Batalla b
        ON (b.Personaje1Id = p.Id OR b.Personaje2Id = p.Id)
    WHERE b.Fecha BETWEEN p_FechaInicio AND p_FechaFin
      AND b.Estado = 'Finalizada'
    GROUP BY p.Id, p.Nombre, p.Tipo
    ORDER BY Victorias DESC, PorcentajeEfectividad DESC;

    COMMIT;
END//

DROP PROCEDURE IF EXISTS ObtenerDanioPromedio//
CREATE PROCEDURE ObtenerDanioPromedio(
    IN p_FechaInicio DATETIME,
    IN p_FechaFin DATETIME
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    SELECT
        p.Id,
        p.Nombre,
        p.Tipo,
        t.TipoAccion,
        ROUND(AVG(t.Danio), 2) AS DanioPromedio,
        0 AS DanioRealizado,
        0 AS DanioRecibido
    FROM Turno t
    INNER JOIN Personaje p ON p.Id = t.AtacanteId
    WHERE t.Fecha BETWEEN p_FechaInicio AND p_FechaFin
    GROUP BY p.Id, p.Nombre, p.Tipo, t.TipoAccion
    ORDER BY p.Tipo, t.TipoAccion;

    COMMIT;
END//

DROP PROCEDURE IF EXISTS ObtenerDanioTotalPorPersonaje//
CREATE PROCEDURE ObtenerDanioTotalPorPersonaje(
    IN p_FechaInicio DATETIME,
    IN p_FechaFin DATETIME
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    SELECT
        p.Id,
        p.Nombre,
        p.Tipo,
        '' AS TipoAccion,
        0 AS DanioPromedio,
        COALESCE(
            (
                SELECT SUM(t1.Danio)
                FROM Turno t1
                WHERE t1.AtacanteId = p.Id
                  AND t1.Fecha BETWEEN p_FechaInicio AND p_FechaFin
            ), 0
        ) AS DanioRealizado,
        COALESCE(
            (
                SELECT SUM(t2.Danio)
                FROM Turno t2
                WHERE t2.DefensorId = p.Id
                  AND t2.Fecha BETWEEN p_FechaInicio AND p_FechaFin
            ), 0
        ) AS DanioRecibido
    FROM Personaje p
    ORDER BY p.Tipo, p.Nombre;

    COMMIT;
END//

DROP PROCEDURE IF EXISTS ObtenerBatallasPorPeriodo//
CREATE PROCEDURE ObtenerBatallasPorPeriodo(
    IN p_FechaInicio DATETIME,
    IN p_FechaFin DATETIME
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    SELECT
        DATE(Fecha) AS Fecha,
        COUNT(*) AS TotalBatallas
    FROM Batalla
    WHERE Fecha BETWEEN p_FechaInicio AND p_FechaFin
    GROUP BY DATE(Fecha)
    ORDER BY DATE(Fecha);

    COMMIT;
END//

DELIMITER ;
