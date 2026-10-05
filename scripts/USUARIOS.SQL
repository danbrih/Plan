CREATE DATABASE IF NOT EXISTS juego_db;

CREATE USER IF NOT EXISTS 'administrador'@'localhost'
IDENTIFIED BY 'admin123';

CREATE USER IF NOT EXISTS 'desarrollo'@'localhost'
IDENTIFIED BY 'dev123';

GRANT ALL PRIVILEGES ON *.* TO 'administrador'@'localhost'
WITH GRANT OPTION;

GRANT SELECT, INSERT, UPDATE, DELETE, EXECUTE
ON juego_db.* TO 'desarrollo'@'localhost';

FLUSH PRIVILEGES;
