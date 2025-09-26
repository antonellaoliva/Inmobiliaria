SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


--
-- Base de datos: `inmobiliaria`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

CREATE TABLE `contrato` (
  `id` int(11) NOT NULL,
  `fechaInicio` date NOT NULL,
  `fechaFin` date NOT NULL,
  `monto` decimal(10,0) NOT NULL,
  `InquilinoId` int(11) NOT NULL,
  `InmuebleId` int(11) NOT NULL,
  `creado_por` int(11) DEFAULT NULL,
  `creado_en` datetime DEFAULT NULL,
  `terminado_por` int(11) DEFAULT NULL,
  `terminado_en` datetime DEFAULT NULL,
  `estado` enum('activo','finalizado','cancelado') NOT NULL DEFAULT 'activo'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`id`, `fechaInicio`, `fechaFin`, `monto`, `InquilinoId`, `InmuebleId`, `creado_por`, `creado_en`, `terminado_por`, `terminado_en`, `estado`) VALUES
(1, '2025-09-12', '2025-09-30', 515000, 1, 1, NULL, NULL, NULL, '2025-09-26 15:53:24', 'finalizado'),
(4, '2025-10-01', '2026-02-28', 350000, 3, 6, NULL, '2025-09-20 22:18:18', NULL, NULL, 'activo'),
(5, '2025-09-21', '2025-10-22', 250000, 3, 11, NULL, '2025-09-21 10:03:26', 10, '2025-09-26 16:09:51', 'finalizado'),
(6, '2025-09-21', '2025-12-21', 250000, 2, 7, NULL, '2025-09-21 19:36:10', NULL, NULL, 'activo'),
(7, '2025-09-21', '2025-09-30', 100000, 3, 10, NULL, '2025-09-21 19:38:02', NULL, NULL, 'activo'),
(8, '2025-09-21', '2025-10-21', 150000, 1, 8, NULL, '2025-09-21 19:44:56', NULL, NULL, 'activo'),
(9, '2025-09-25', '2025-09-30', 10000, 2, 4, 9, '2025-09-25 19:29:07', NULL, NULL, 'activo'),
(10, '2025-10-01', '2025-11-30', 150000, 3, 10, 10, '2025-09-25 19:31:24', 10, '2025-09-25 19:32:42', 'cancelado'),
(11, '2025-10-01', '2025-12-31', 260000, 3, 9, 10, '2025-09-25 20:50:11', 10, '2025-09-25 20:50:43', 'cancelado'),
(12, '2025-10-01', '2027-09-30', 515000, 1, 1, 10, '2025-09-26 15:52:22', NULL, NULL, 'activo'),
(13, '2025-10-01', '2025-12-31', 350000, 3, 12, 9, '2025-09-26 16:51:53', NULL, NULL, 'activo');

--
-- Disparadores `contrato`
--
DELIMITER $$
CREATE TRIGGER `trg_contrato_before_insert` BEFORE INSERT ON `contrato` FOR EACH ROW BEGIN
  IF NEW.creado_en IS NULL THEN
    SET NEW.creado_en = NOW();
  END IF;
  -- asegurarse que estado no sea NULL
  IF NEW.estado IS NULL THEN
    SET NEW.estado = 'activo';
  END IF;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trg_contrato_before_update` BEFORE UPDATE ON `contrato` FOR EACH ROW BEGIN
  -- si se está poniendo terminado_en desde la app, dejarlo; si estado cambia a finalizado y no hay terminado_en, fijarlo
  IF NEW.estado = 'finalizado' AND NEW.terminado_en IS NULL THEN
    SET NEW.terminado_en = NOW();
  END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

CREATE TABLE `inmueble` (
  `id` int(11) NOT NULL,
  `direccion` varchar(200) NOT NULL,
  `uso` enum('Residencial','Comercial') NOT NULL,
  `ambientes` int(11) NOT NULL,
  `longitud` decimal(10,7) NOT NULL,
  `latitud` decimal(10,7) NOT NULL,
  `precio` decimal(10,2) NOT NULL,
  `estado` tinyint(1) NOT NULL DEFAULT 1,
  `propietario_id` int(11) NOT NULL,
  `tipoInmuebleId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`id`, `direccion`, `uso`, `ambientes`, `longitud`, `latitud`, `precio`, `estado`, `propietario_id`, `tipoInmuebleId`) VALUES
(1, 'Balcarce 322', 'Residencial', 1, 145.0000000, 53.0000000, 200000.00, 1, 1, 1),
(2, '25 de mayo', 'Residencial', 3, 2.0000000, 1.0000000, 350000.00, 1, 1, 1),
(3, 'Juan B. Justo 122', 'Comercial', 5, 4.0000000, 2.0000000, 500000.00, 0, 1, 1),
(4, 'Suipacha 178', 'Residencial', 2, 999.9999999, 534.0000000, 685.00, 1, 2, 3),
(5, 'Las Heras 199', 'Residencial', 3, 123.0000000, 56.0000000, 250000.00, 1, 5, 4),
(6, 'Av. Mitre 525', 'Comercial', 1, 156.0000000, 69.0000000, 425000.00, 1, 4, 1),
(7, 'Lavalle 380', 'Comercial', 1, 169.0000000, 78.0000000, 400000.00, 1, 5, 1),
(8, 'Chacabuco 123', 'Residencial', 5, 165.0000000, 73.0000000, 560000.00, 1, 5, 3),
(9, 'Marconi 219', 'Residencial', 2, 89.0000000, 52.0000000, 150000.00, 1, 4, 4),
(10, 'Mulleady 263', 'Residencial', 3, 145.0000000, 56.0000000, 200000.00, 1, 2, 4),
(11, 'Teodoro Fels 129', 'Comercial', 2, 146.0000000, 82.0000000, 275000.00, 1, 5, 1),
(12, 'Pablo Lucero 269', 'Residencial', 2, 175.0000000, 59.0000000, 250000.00, 1, 12, 3);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

CREATE TABLE `inquilino` (
  `id` int(11) NOT NULL,
  `dni` varchar(20) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL,
  `telefono` varchar(20) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`id`, `dni`, `nombre`, `apellido`, `telefono`, `email`) VALUES
(1, '35698214', 'Pedro', 'Gomez', '2657423592', 'pgomez@gmail.com'),
(2, '37621589', 'Jazmin', 'Pereyra', '2657413285', 'jazpereyra@gmail.com'),
(3, '35698326', 'Luciano', 'Pedernera', '2657253612', 'Lu_pedernera@gmail.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `id` int(11) NOT NULL,
  `numeroPago` int(11) NOT NULL,
  `fechaPago` date NOT NULL,
  `monto` decimal(10,0) NOT NULL,
  `contratoId` int(11) NOT NULL,
  `creado_por` int(11) DEFAULT NULL,
  `creado_en` datetime DEFAULT NULL,
  `anulado_por` int(11) DEFAULT NULL,
  `anulado_en` datetime DEFAULT NULL,
  `fechaUpdate` date NOT NULL,
  `estado` enum('activo','anulado') NOT NULL DEFAULT 'activo'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`id`, `numeroPago`, `fechaPago`, `monto`, `contratoId`, `creado_por`, `creado_en`, `anulado_por`, `anulado_en`, `fechaUpdate`, `estado`) VALUES
(1, 1, '2025-09-18', 355000, 1, 1, '2025-09-18 19:50:27', 1, '2025-09-18 22:14:44', '2025-09-26', 'activo'),
(2, 2, '2025-09-01', 200000, 1, 1, '2025-09-18 22:21:26', NULL, NULL, '2025-09-20', 'activo'),
(3, 3, '2025-09-25', 250000, 5, 10, '2025-09-25 20:22:31', NULL, NULL, '2025-09-25', 'activo'),
(4, 4, '2025-09-24', 515000, 5, 10, '2025-09-25 20:28:20', 10, '2025-09-25 20:28:42', '2025-09-25', 'anulado');

--
-- Disparadores `pago`
--
DELIMITER $$
CREATE TRIGGER `trg_pago_before_insert` BEFORE INSERT ON `pago` FOR EACH ROW BEGIN
  IF NEW.creado_en IS NULL THEN
    SET NEW.creado_en = NOW();
  END IF;
  IF NEW.estado IS NULL THEN
    SET NEW.estado = 'activo';
  END IF;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trg_pago_before_update` BEFORE UPDATE ON `pago` FOR EACH ROW BEGIN
  IF NEW.estado = 'anulado' AND NEW.anulado_en IS NULL THEN
    SET NEW.anulado_en = NOW();
  END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

CREATE TABLE `propietario` (
  `id` int(11) NOT NULL,
  `dni` varchar(20) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL,
  `telefono` varchar(20) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`id`, `dni`, `nombre`, `apellido`, `telefono`, `email`) VALUES
(1, '25693841', 'Juan Jose', 'Ramirez', '2664164231', 'juanjor@gmail.com'),
(2, '26537894', 'Lucas ', 'Gonzalez', '2657259841', 'lgonza@gmail.com'),
(3, '14965231', 'Luciana', 'Mendez', '2657412385', 'lmendez@gmail.com'),
(4, '25693842', 'Karina', 'Rodriguez', '2657325186', 'kari@gmail.com'),
(5, '25693726', 'Rodrigo', 'Mora', '2664751230', 'rodri_mo@gmail.com'),
(6, '10253641', 'Juan', 'Perez', '2657265321', 'jperez@gmail.com'),
(7, '15369214', 'Diego', 'Lopez', '2657412385', 'diego@gmail.com'),
(8, '25698744', 'Luciana', 'Gomez', '2657412351', 'lu_gomez@gmail.com'),
(9, '37521564', 'Lorenzo', 'Gonzalez', '2657423168', 'lorenzo@gmail.com'),
(10, '26412365', 'Marina', 'Gil', '2664752148', 'mari@gmail.com'),
(11, '25639854', 'Mariano', 'Guzman', '2657412389', 'mariano@gmail.com'),
(12, '32698741', 'Marcos', 'Diaz', '2657412385', 'marcos@gmail.com'),
(13, '25631478', 'Marcela', 'Perez', '2657412398', 'marcep@gmail.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipoinmueble`
--

CREATE TABLE `tipoinmueble` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tipoinmueble`
--

INSERT INTO `tipoinmueble` (`Id`, `Nombre`) VALUES
(1, 'Local'),
(2, 'Residencial'),
(3, 'Casa'),
(4, 'Departamento');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `id` int(11) NOT NULL,
  `email` varchar(100) NOT NULL,
  `password` varchar(255) NOT NULL,
  `nombre` varchar(100) DEFAULT NULL,
  `apellido` varchar(100) DEFAULT NULL,
  `avatar` varchar(255) DEFAULT NULL,
  `rol` enum('Administrador','Empleado') NOT NULL DEFAULT 'Empleado'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`id`, `email`, `password`, `nombre`, `apellido`, `avatar`, `rol`) VALUES
(1, 'admin1@ejemplo.local', '3eb3fe66b31e3b4d10fa70b5cad49c7112294af6ae4e476a1c405155d45aa121', 'Admin', 'Grupo', NULL, 'Administrador'),
(2, 'emple1@ejemplo.local', 'a5eb10313b9116ce94dc36afd5b653bf03fee85101278b1a0f044ebc21a98a93', 'Empleado', 'Grupo', NULL, 'Empleado'),
(6, 'mica@empleado.com', 'UrpMWVPfkecv6Fb/W/ssxRrJfgylaALQG18TUjcfpOc=', 'Micaela', 'Lucero', '/Uploads/avatar_6.png', 'Empleado'),
(8, 'admin@prueba.com', 'rJ17Efx9h/2bhAWttPlQVLkll0D2zJkufFnQrjVtuFo=', 'Pepe', 'Gomez', '', 'Administrador'),
(9, 'admin2@prueba.com', 'rJ17Efx9h/2bhAWttPlQVLkll0D2zJkufFnQrjVtuFo=', 'Pepe', 'Gomez', '/Uploads/avatar_9.jpeg', 'Administrador'),
(10, 'lola@empleado.com', 'HgH/BqpQS2AWsCuxMWWP5GK/+gEbvDlSY9lCvzth0S4=', 'Lola', 'Pereyra', '/Uploads/avatar_10.png', 'Empleado');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`id`),
  ADD KEY `inquilino_id` (`InquilinoId`),
  ADD KEY `inmueble_id` (`InmuebleId`),
  ADD KEY `idx_contrato_creado_por` (`creado_por`),
  ADD KEY `idx_contrato_terminado_por` (`terminado_por`);

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`id`),
  ADD KEY `propietario_id` (`propietario_id`),
  ADD KEY `fk_inmueble_tipo` (`tipoInmuebleId`);

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `dni` (`dni`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`id`),
  ADD KEY `contrato_id` (`contratoId`),
  ADD KEY `idx_pago_creado_por` (`creado_por`),
  ADD KEY `idx_pago_anulado_por` (`anulado_por`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `dni` (`dni`);

--
-- Indices de la tabla `tipoinmueble`
--
ALTER TABLE `tipoinmueble`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email` (`email`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT de la tabla `tipoinmueble`
--
ALTER TABLE `tipoinmueble`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `fk_contrato_creado_por_usuario` FOREIGN KEY (`creado_por`) REFERENCES `usuario` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_contrato_inmueble` FOREIGN KEY (`InmuebleId`) REFERENCES `inmueble` (`id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_contrato_inquilino` FOREIGN KEY (`InquilinoId`) REFERENCES `inquilino` (`id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_contrato_terminado_por_usuario` FOREIGN KEY (`terminado_por`) REFERENCES `usuario` (`id`) ON DELETE SET NULL ON UPDATE CASCADE;

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `fk_inmueble_propietario` FOREIGN KEY (`propietario_id`) REFERENCES `propietario` (`id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_inmueble_tipo` FOREIGN KEY (`tipoInmuebleId`) REFERENCES `tipoinmueble` (`Id`);

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `fk_pago_anulado_por_usuario` FOREIGN KEY (`anulado_por`) REFERENCES `usuario` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_pago_contrato` FOREIGN KEY (`contratoId`) REFERENCES `contrato` (`id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_pago_creado_por_usuario` FOREIGN KEY (`creado_por`) REFERENCES `usuario` (`id`) ON DELETE SET NULL ON UPDATE CASCADE;
COMMIT;


