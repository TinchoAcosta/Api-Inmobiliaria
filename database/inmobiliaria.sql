-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 28-08-2025 a las 22:32:54
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliaria`
--
CREATE DATABASE IF NOT EXISTS `inmobiliaria` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `inmobiliaria`;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

CREATE TABLE `inmueble` (
  `id_inmueble` int(11) NOT NULL,
  `direccion_inmueble` varchar(100) DEFAULT NULL,
  `ambientes_inmueble` int(11) NOT NULL,
  `superficie_inmueble` int(11) NOT NULL,
  `lat_inmueble` decimal(10,8) NOT NULL,
  `long_inmueble` decimal(11,8) NOT NULL,
  `PropietarioId` int(11) NOT NULL,
  `portada_inmueble` varchar(100) DEFAULT NULL,
  `tipo_inmueble` int(11) NOT NULL,
  `uso_inmueble` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

CREATE TABLE `inquilino` (
  `id_inquilino` int(11) NOT NULL,
  `nombre_inquilino` varchar(50) NOT NULL,
  `apellido_inquilino` varchar(50) NOT NULL,
  `dni_inquilino` int(11) NOT NULL,
  `telefono_inquilino` varchar(20) NOT NULL,
  `email_inquilino` varchar(50) NOT NULL,
  `borrado_inquilino` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`id_inquilino`, `nombre_inquilino`, `apellido_inquilino`, `dni_inquilino`, `telefono_inquilino`, `email_inquilino`, `borrado_inquilino`) VALUES
(1, 'Lucía', 'Fernández', 40123456, '2664001122', 'lucia.fernandez@example.com', 1),
(2, 'Gonzalo', 'Juarez', 40234567, '2664003344', 'carlos.ramirez@example.com', 1),
(3, 'inquilino', 'inquilinoapellido', 1112223, '65465464656', 'inquilino@inquilino.inquilino', 1),
(4, 'asa', 'sas', 123123421, '123213', 'asdasdadsadsa@asas.asa', 0),
(5, 'borrado_inquilino', 'sas', 134213, '123213', 'asdasdadsadsa@asas.asa', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

CREATE TABLE `propietario` (
  `id_propietario` int(11) NOT NULL,
  `dni_propietario` int(11) DEFAULT NULL,
  `contrasena_propietario` varchar(50) NOT NULL,
  `nombre_propietario` varchar(50) NOT NULL,
  `apellido_propietario` varchar(50) NOT NULL,
  `email_propietario` varchar(50) NOT NULL,
  `telefono_propietario` varchar(20) NOT NULL,
  `borrado_propietario` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`id_propietario`, `dni_propietario`, `contrasena_propietario`, `nombre_propietario`, `apellido_propietario`, `email_propietario`, `telefono_propietario`, `borrado_propietario`) VALUES
(1, 30123456, 'clave123', 'María', 'Gómez', 'maria.gomez@example.com', '2664123456', 1),
(2, 30234567, 'segura456', 'Juan', 'Pérez', 'juan.perez@example.com', '2664987654', 1),
(3, 12312, 'adsasd', 'J', 'Perez', 'propietario@p.prop', '123213', 1),
(4, 213412, 'fdsfds', 'prop2', 'asdaddsa', 'asdads@asd.s', '123213', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipo_inmueble`
--

CREATE TABLE `tipo_inmueble` (
  `id` int(11) NOT NULL,
  `descripcion` varchar(40) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tipo_inmueble`
--

INSERT INTO `tipo_inmueble` (`id`, `descripcion`) VALUES
(1, 'Deposito'),
(2, 'Local'),
(3, 'Casa'),
(4, 'Departamento');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`id_inmueble`),
  ADD KEY `PropietarioId` (`PropietarioId`),
  ADD KEY `tipo_inmueble` (`tipo_inmueble`);

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`id_inquilino`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`id_propietario`);

--
-- Indices de la tabla `tipo_inmueble`
--
ALTER TABLE `tipo_inmueble`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `id_inmueble` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `id_inquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `id_propietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `tipo_inmueble`
--
ALTER TABLE `tipo_inmueble`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `inmueble_ibfk_1` FOREIGN KEY (`PropietarioId`) REFERENCES `propietario` (`id_propietario`),
  ADD CONSTRAINT `inmueble_ibfk_2` FOREIGN KEY (`tipo_inmueble`) REFERENCES `tipo_inmueble` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
