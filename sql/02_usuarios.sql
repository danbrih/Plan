
-- Crear usuario administrador
CREATE USER IF NOT EXISTS 'administrador'@'localhost'
  IDENTIFIED BY 'catire';

-- Crear usuario invitado
CREATE USER IF NOT EXISTS 'invitado'@'localhost'
  IDENTIFIED BY 'catire';

-- Establecer contraseñas
ALTER USER 'administrador'@'localhost' IDENTIFIED BY 'catire';
ALTER USER 'invitado'@'localhost' IDENTIFIED BY 'catire';

-- Limpiar permisos anteriores
REVOKE ALL PRIVILEGES, GRANT OPTION FROM 'administrador'@'localhost';
REVOKE ALL PRIVILEGES, GRANT OPTION FROM 'invitado'@'localhost';

-- Permisos del administrador
GRANT ALL PRIVILEGES ON combate_api.* TO 'administrador'@'localhost';

-- Permisos de solo lectura del invitado
GRANT SELECT ON combate_api.* TO 'invitado'@'localhost';

-- Aplicar cambios
FLUSH PRIVILEGES;

