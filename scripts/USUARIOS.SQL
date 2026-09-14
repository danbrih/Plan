CREATE USER IF NOT EXISTS 'administrador'@'localhost' IDENTIFIED BY 'admin123';
GRANT ALL PRIVILEGES ON *.* TO 'administrador'@'localhost';

CREATE USER IF NOT EXISTS 'desarrollo'@'localhost' IDENTIFIED BY 'dev123';
GRANT ALL PRIVILEGES ON juego_db.* TO 'desarrollo'@'localhost';
FLUSH PRIVILEGES;
