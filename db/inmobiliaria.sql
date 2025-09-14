CREATE DATABASE IF NOT EXISTS inmobiliaria;
USE inmobiliaria;

CREATE TABLE usuario (
    id INT AUTO_INCREMENT PRIMARY KEY,
    email VARCHAR(100) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    nombre VARCHAR(100),
    apellido VARCHAR(100),
    avatar VARCHAR(255),
    rol ENUM('Administrador', 'Empleado') NOT NULL DEFAULT 'Empleado'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


CREATE TABLE propietario (
  id INT AUTO_INCREMENT PRIMARY KEY,
  dni VARCHAR(20) NOT NULL UNIQUE,
  nombre VARCHAR(100) NOT NULL,
  apellido VARCHAR(100) NOT NULL,
  telefono VARCHAR(20),
  email VARCHAR(100)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


CREATE TABLE inquilino (
  id INT AUTO_INCREMENT PRIMARY KEY,
  dni VARCHAR(20) NOT NULL UNIQUE,
  nombre VARCHAR(100) NOT NULL,
  apellido VARCHAR(100) NOT NULL,
  telefono VARCHAR(20),
  email VARCHAR(100)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


CREATE TABLE inmueble (
    id INT AUTO_INCREMENT PRIMARY KEY,
    direccion VARCHAR(200) NOT NULL,
    uso ENUM('Residencial', 'Comercial') NOT NULL,
    tipo VARCHAR(50) NOT NULL,
    ambientes INT NOT NULL,
    longitud DECIMAL(10,7) NOT NULL,
    latitud DECIMAL(10,7) NOT NULL,
    precio DECIMAL(10,2) NOT NULL,
    estado BOOLEAN NOT NULL DEFAULT TRUE,
    propietario_id INT NOT NULL,
    INDEX (propietario_id),
    CONSTRAINT fk_inmueble_propietario FOREIGN KEY (propietario_id)
      REFERENCES propietario(id)
      ON UPDATE CASCADE
      ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


CREATE TABLE contrato (
    id INT AUTO_INCREMENT PRIMARY KEY,
    inquilino_id INT NOT NULL,
    inmueble_id INT NOT NULL,
    fecha_inicio DATE NOT NULL,
    fecha_fin DATE NOT NULL,
    fecha_finalizacion_anticipada DATE,
    monto_mensual DECIMAL(10,2) NOT NULL,
    usuario_creador_id INT,
    usuario_finalizador_id INT,
    INDEX (inquilino_id),
    INDEX (inmueble_id),
    INDEX (usuario_creador_id),
    INDEX (usuario_finalizador_id),
    CONSTRAINT fk_contrato_inquilino FOREIGN KEY (inquilino_id) REFERENCES inquilino(id) ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT fk_contrato_inmueble FOREIGN KEY (inmueble_id) REFERENCES inmueble(id) ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT fk_contrato_usuario_creador FOREIGN KEY (usuario_creador_id) REFERENCES usuario(id) ON UPDATE CASCADE ON DELETE SET NULL,
    CONSTRAINT fk_contrato_usuario_finalizador FOREIGN KEY (usuario_finalizador_id) REFERENCES usuario(id) ON UPDATE CASCADE ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


CREATE TABLE pago (
    id INT AUTO_INCREMENT PRIMARY KEY,
    contrato_id INT NOT NULL,
    numero INT NOT NULL,
    fecha_pago DATE NOT NULL,
    detalle VARCHAR(200),
    importe DECIMAL(10,2) NOT NULL,
    anulado BOOLEAN NOT NULL DEFAULT FALSE,
    usuario_creador_id INT,
    usuario_anulador_id INT,
    INDEX (contrato_id),
    INDEX (usuario_creador_id),
    INDEX (usuario_anulador_id),
    CONSTRAINT fk_pago_contrato FOREIGN KEY (contrato_id) REFERENCES contrato(id) ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT fk_pago_usuario_creador FOREIGN KEY (usuario_creador_id) REFERENCES usuario(id) ON UPDATE CASCADE ON DELETE SET NULL,
    CONSTRAINT fk_pago_usuario_anulador FOREIGN KEY (usuario_anulador_id) REFERENCES usuario(id) ON UPDATE CASCADE ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
