-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 15-10-2026 a las 03:35:23
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
-- Base de datos: `diario_upvm`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `acerca_de`
--

CREATE TABLE `acerca_de` (
  `id` int(11) NOT NULL,
  `titulo` varchar(100) NOT NULL,
  `contenido` text NOT NULL,
  `icono` varchar(10) NOT NULL DEFAULT 'ℹ️',
  `orden` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `acerca_de`
--

INSERT INTO `acerca_de` (`id`, `titulo`, `contenido`, `icono`, `orden`) VALUES
(1, '¿Qué es Diario UPVM?', 'Es la app oficial de noticias y avisos de la comunidad universitaria, donde puedes consultar novedades y registrar reportes.', '📰', 1),
(2, 'Nuestra misión', 'Mantener informada a la comunidad con noticias claras, oportunas y confiables.', '🎯', 2),
(3, 'Nuestra visión', 'Ser el principal canal de comunicación entre alumnos, docentes y control escolar.', '🔭', 3),
(4, 'Equipo de desarrollo', 'Proyecto realizado por estudiantes de la UPVM con .NET MAUI, PHP y MySQL.', '👥', 4),
(5, 'Contacto', 'Para dudas o aclaraciones, acude a control escolar o envía un reporte desde la app.', '📞', 5);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `calendario`
--

CREATE TABLE `calendario` (
  `id` int(11) NOT NULL,
  `titulo` varchar(200) NOT NULL,
  `fecha` date NOT NULL,
  `contenido` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `calendario`
--

INSERT INTO `calendario` (`id`, `titulo`, `fecha`, `contenido`) VALUES
(1, 'Inicio de clases', '2026-10-19', 'Inician las actividades del cuatrimestre.'),
(2, 'Entrega de reportes', '2026-10-23', 'Fecha límite para entregar reportes en control escolar.'),
(3, 'Aviso de hoy', '2026-10-14', 'Evento de prueba con la fecha de hoy.'),
(4, 'Inicio de clases', '2026-10-19', 'Inician las actividades del cuatrimestre.'),
(5, 'Entrega de reportes', '2026-10-23', 'Fecha límite para entregar reportes en control escolar.'),
(6, 'Aviso de hoy', '2026-10-14', 'Evento de prueba con la fecha de hoy.');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `noticias`
--

CREATE TABLE `noticias` (
  `id` int(11) NOT NULL,
  `titulo` varchar(200) NOT NULL,
  `descripcion` text NOT NULL,
  `imagen` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `noticias`
--

INSERT INTO `noticias` (`id`, `titulo`, `descripcion`, `imagen`) VALUES
(1, 'Bienvenida al ciclo escolar', 'Iniciamos un nuevo ciclo con muchas actividades.', 'noticia1.jpg'),
(2, 'Inscripciones abiertas', 'Ya puedes consultar fechas y requisitos.', 'noticia2.jpg');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `reportes`
--

CREATE TABLE `reportes` (
  `id` int(11) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `matricula` varchar(30) NOT NULL,
  `tipo` varchar(30) NOT NULL,
  `descripcion` text NOT NULL,
  `fecha` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `reportes`
--

INSERT INTO `reportes` (`id`, `nombre`, `matricula`, `tipo`, `descripcion`, `fecha`) VALUES
(1, 'PANDASEXUAL', '1225221345', 'Incidente', 'rompi la laptop de santiago', '2026-10-03 02:42:01'),
(2, 'angeles', '626336', 'Queja', 'panda es. onito', '2026-10-05 02:16:27');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `acerca_de`
--
ALTER TABLE `acerca_de`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `calendario`
--
ALTER TABLE `calendario`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `noticias`
--
ALTER TABLE `noticias`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `reportes`
--
ALTER TABLE `reportes`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `acerca_de`
--
ALTER TABLE `acerca_de`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `calendario`
--
ALTER TABLE `calendario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `noticias`
--
ALTER TABLE `noticias`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `reportes`
--
ALTER TABLE `reportes`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
