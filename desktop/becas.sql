USE registro;

CREATE TABLE IF NOT EXISTS becas (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(150) NOT NULL,
    tipo VARCHAR(40) NOT NULL,
    descripcion VARCHAR(400),
    requisitos VARCHAR(400),
    monto VARCHAR(60),
    fecha_limite DATE NOT NULL
);

INSERT INTO becas (nombre, tipo, descripcion, requisitos, monto, fecha_limite) VALUES
('Beca de excelencia académica', 'Académica', 'Apoyo para estudiantes con alto promedio.', 'Promedio mínimo de 9.0 y no tener materias reprobadas.', '$1,500 mensuales', DATE_ADD(CURDATE(), INTERVAL 2 DAY)),
('Beca deportiva', 'Deportiva', 'Para integrantes de selecciones representativas.', 'Constancia del entrenador y examen médico.', '$800 mensuales', DATE_ADD(CURDATE(), INTERVAL 9 DAY)),
('Beca cultural', 'Cultural', 'Apoyo a talleres de música, danza y teatro.', 'Carta de motivos y participación en un taller.', '$600 mensuales', DATE_ADD(CURDATE(), INTERVAL 20 DAY)),
('Beca de transporte', 'Económica', 'Apoyo para gastos de traslado.', 'Comprobante de domicilio y estudio socioeconómico.', '$500 mensuales', DATE_ADD(CURDATE(), INTERVAL 35 DAY)),
('Beca de inscripción (cerrada)', 'Económica', 'Descuento en inscripción del ciclo anterior.', 'Estudio socioeconómico.', '50% de descuento', DATE_SUB(CURDATE(), INTERVAL 5 DAY));