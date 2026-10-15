-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 15-10-2026 a las 03:34:31
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
-- Base de datos: `registro`
--

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
(3, 'Aviso de hoy', '2026-10-14', 'Evento de prueba con la fecha de hoy.');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `categorias`
--

CREATE TABLE `categorias` (
  `id_categoria` int(11) NOT NULL,
  `nombre` varchar(80) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `categorias`
--

INSERT INTO `categorias` (`id_categoria`, `nombre`) VALUES
(1, 'Ciberseguridad'),
(2, 'Software'),
(3, 'Hardware');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `directorio`
--

CREATE TABLE `directorio` (
  `id` int(11) NOT NULL,
  `nombre` varchar(120) NOT NULL,
  `cargo` varchar(120) DEFAULT NULL,
  `departamento` varchar(120) DEFAULT NULL,
  `correo` varchar(120) DEFAULT NULL,
  `extension` varchar(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `directorio`
--

INSERT INTO `directorio` (`id`, `nombre`, `cargo`, `departamento`, `correo`, `extension`) VALUES
(1, 'Dra. Ana Torres', 'Directora de carrera', 'Ingeniería en Sistemas', 'ana.torres@ejemplo.edu.mx', '101'),
(2, 'Lic. Carlos Mena', 'Jefe de control escolar', 'Servicios escolares', 'carlos.mena@ejemplo.edu.mx', '120'),
(3, 'Mtra. Laura Díaz', 'Bibliotecaria', 'Biblioteca', 'laura.diaz@ejemplo.edu.mx', '135');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `edificios`
--

CREATE TABLE `edificios` (
  `id` int(11) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `descripcion` varchar(300) DEFAULT NULL,
  `latitud` decimal(10,7) DEFAULT NULL,
  `longitud` decimal(10,7) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `edificios`
--

INSERT INTO `edificios` (`id`, `nombre`, `descripcion`, `latitud`, `longitud`) VALUES
(1, 'Edificio A', 'Aulas de docencia', 19.6900000, -99.2100000),
(2, 'Laboratorios de cómputo', 'LC-1 a LC-4', 19.6903000, -99.2104000),
(3, 'Biblioteca', 'Acervo y salas de estudio', 19.6897000, -99.2098000);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `guardados`
--

CREATE TABLE `guardados` (
  `id` int(11) NOT NULL,
  `usuario` varchar(50) NOT NULL,
  `tipo` enum('noticia','evento') NOT NULL,
  `id_referencia` int(11) NOT NULL,
  `fecha_guardado` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `horarios`
--

CREATE TABLE `horarios` (
  `id` int(11) NOT NULL,
  `grupo` varchar(20) NOT NULL,
  `dia` tinyint(4) NOT NULL,
  `hora_inicio` time NOT NULL,
  `hora_fin` time NOT NULL,
  `materia` varchar(120) NOT NULL,
  `profesor` varchar(120) DEFAULT NULL,
  `salon` varchar(40) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `horarios`
--

INSERT INTO `horarios` (`id`, `grupo`, `dia`, `hora_inicio`, `hora_fin`, `materia`, `profesor`, `salon`) VALUES
(1, 'ISC401', 1, '07:00:00', '09:00:00', 'Programación móvil', 'Ing. Pérez', 'LC-2'),
(2, 'ISC401', 1, '09:00:00', '11:00:00', 'Bases de datos', 'Mtra. López', 'A-105'),
(3, 'ISC401', 2, '07:00:00', '09:00:00', 'Redes', 'Ing. Ramírez', 'LC-1'),
(4, 'ISC401', 3, '09:00:00', '11:00:00', 'Ingeniería de software', 'Mtro. Soto', 'A-201');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `noticias`
--

CREATE TABLE `noticias` (
  `id_noticia` int(11) NOT NULL,
  `id_categoria` int(11) NOT NULL,
  `titulo` varchar(200) NOT NULL,
  `resumen` varchar(500) DEFAULT NULL,
  `contenido` text DEFAULT NULL,
  `bibliografia` varchar(500) DEFAULT NULL,
  `plataforma_afectada` varchar(100) DEFAULT NULL,
  `total_likes` int(11) DEFAULT 0,
  `total_dislikes` int(11) DEFAULT 0,
  `vistas` int(11) DEFAULT 0,
  `fecha_creacion` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `noticias`
--

INSERT INTO `noticias` (`id_noticia`, `id_categoria`, `titulo`, `resumen`, `contenido`, `bibliografia`, `plataforma_afectada`, `total_likes`, `total_dislikes`, `vistas`, `fecha_creacion`) VALUES
(1, 1, 'Nueva campaña de phishing detectada', 'Correos falsos imitan avisos bancarios.', 'Se recomienda no abrir enlaces de remitentes desconocidos y activar la verificación en dos pasos.', 'Ejemplo de bibliografía 1', 'Windows, Android', 12, 1, 150, '2026-10-12 18:36:19'),
(2, 1, 'Actualización de seguridad disponible', 'Se corrigen fallas de severidad alta.', 'Mantén tu sistema actualizado para reducir riesgos.', 'Ejemplo de bibliografía 2', 'Windows', 8, 0, 90, '2026-10-10 18:36:19'),
(3, 2, 'Nueva versión de framework móvil', 'Mejoras de rendimiento y nuevas herramientas.', 'La nueva versión incluye mejoras en compilación y depuración.', 'Ejemplo de bibliografía 3', 'Android, iOS', 20, 2, 300, '2026-10-11 18:36:19'),
(4, 3, 'Nuevos procesadores de bajo consumo', 'Más eficiencia para laptops delgadas.', 'Los fabricantes presentan chips con mejor autonomía de batería.', 'Ejemplo de bibliografía 4', 'Laptops', 5, 1, 60, '2026-10-07 18:36:19');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `calendario`
--
ALTER TABLE `calendario`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `categorias`
--
ALTER TABLE `categorias`
  ADD PRIMARY KEY (`id_categoria`);

--
-- Indices de la tabla `directorio`
--
ALTER TABLE `directorio`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `edificios`
--
ALTER TABLE `edificios`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `guardados`
--
ALTER TABLE `guardados`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `uq_guardado` (`usuario`,`tipo`,`id_referencia`);

--
-- Indices de la tabla `horarios`
--
ALTER TABLE `horarios`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `noticias`
--
ALTER TABLE `noticias`
  ADD PRIMARY KEY (`id_noticia`),
  ADD KEY `id_categoria` (`id_categoria`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `calendario`
--
ALTER TABLE `calendario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `categorias`
--
ALTER TABLE `categorias`
  MODIFY `id_categoria` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `directorio`
--
ALTER TABLE `directorio`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `edificios`
--
ALTER TABLE `edificios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `guardados`
--
ALTER TABLE `guardados`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `horarios`
--
ALTER TABLE `horarios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `noticias`
--
ALTER TABLE `noticias`
  MODIFY `id_noticia` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `noticias`
--
ALTER TABLE `noticias`
  ADD CONSTRAINT `noticias_ibfk_1` FOREIGN KEY (`id_categoria`) REFERENCES `categorias` (`id_categoria`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
