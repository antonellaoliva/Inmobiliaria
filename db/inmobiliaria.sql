CREATE DATABASE IF NOT EXISTS inmobiliaria
USE inmobiliaria;

DROP TABLE IF EXISTS propietario;
CREATE TABLE propietario (
  id INT AUTO_INCREMENT PRIMARY KEY,
  DNI      VARCHAR(20)  NOT NULL UNIQUE,
  nombre   VARCHAR(100) NOT NULL,
  apellido VARCHAR(100) NOT NULL,
  telefono VARCHAR(20),
  email    VARCHAR(100)
);

DROP TABLE IF EXISTS inquilino;
CREATE TABLE inquilino (
  id INT AUTO_INCREMENT PRIMARY KEY,
  DNI      VARCHAR(20)  NOT NULL UNIQUE,
  nombre   VARCHAR(100) NOT NULL,
  apellido VARCHAR(100) NOT NULL,
  telefono VARCHAR(20),
  email    VARCHAR(100)
);

INSERT INTO Propietario (DNI, Nombre, Apellido, Telefono, Email) VALUES
('37528654','Lucas','Gomez','2657853698','lugomez@gmail.com'),
('11532823','Romina','Gonzalez','2664853694','rgonza@gmail.com'),
('23865147','Emanuel','Lopez','2657869321','ema@gmail.com');

INSERT INTO Inquilino (DNI, Nombre, Apellido, Telefono, Email) VALUES
('37523698','Amancio','Rivadera','2657253698','amanriv@gmail.com'),
('14852321','Analia','Garcia','2657523695','anag@gmail.com'),
('25896321','Lorenzo','Perez','2657529651','lperez@gmail.com');