<?php
mysqli_report(MYSQLI_REPORT_ERROR | MYSQLI_REPORT_STRICT);
try {
    $conexion = mysqli_connect("localhost", "root", "", "registro");
    mysqli_set_charset($conexion, "utf8mb4");
} catch (Throwable $e) {
    http_response_code(500);
    header('Content-Type: application/json; charset=utf-8');
    die(json_encode(['error' => 'Conexión: ' . $e->getMessage()]));
}
?>