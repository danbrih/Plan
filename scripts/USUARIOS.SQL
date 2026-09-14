CREATE USER IF NOT EXISTS 'administrador'@'localhost' IDENTIFIED BY 'admin123';
GRANT ALL PRIVILEGES ON *.* TO 'administrador'@'localhost' WITH GRANT OPTION;

CREATE USER IF NOT EXISTS 'desarrollo'@'localhost' IDENTIFIED BY 'dev123';
GRANT SELECT, INSERT, UPDATE, DELETE ON juego_db.* TO 'desarrollo'@'localhost';
GRANT EXECUTE ON PROCEDURE juego_db.* TO 'desarrollo'@'localhost';

FLUSH PRIVILEGES;
