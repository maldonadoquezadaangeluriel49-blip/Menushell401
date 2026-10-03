<?php
header("Content-Type: application/json; charset=utf-8");
header("Access-Control-Allow-Origin: *");

if ($_SERVER['REQUEST_METHOD'] !== 'POST') {
    http_response_code(405);
    echo json_encode(["ok" => false, "mensaje" => "Método no permitido"]);
    exit;
}

$datos = json_decode(file_get_contents("php://input"), true);

$nombre      = trim($datos['nombre'] ?? '');
$matricula   = trim($datos['matricula'] ?? '');
$tipo        = trim($datos['tipo'] ?? '');
$descripcion = trim($datos['descripcion'] ?? '');

if ($nombre === '' || $matricula === '' || $tipo === '' || $descripcion === '') {
    http_response_code(400);
    echo json_encode(["ok" => false, "mensaje" => "Faltan datos"]);
    exit;
}

$conn = new mysqli("localhost", "root", "", "diario_upvm");
$conn->set_charset("utf8mb4");

if ($conn->connect_error) {
    http_response_code(500);
    echo json_encode(["ok" => false, "mensaje" => "Error de conexión"]);
    exit;
}

$stmt = $conn->prepare("INSERT INTO reportes (nombre, matricula, tipo, descripcion) VALUES (?, ?, ?, ?)");
$stmt->bind_param("ssss", $nombre, $matricula, $tipo, $descripcion);

if ($stmt->execute()) {
    echo json_encode(["ok" => true, "mensaje" => "Reporte guardado", "id" => $stmt->insert_id]);
} else {
    http_response_code(500);
    echo json_encode(["ok" => false, "mensaje" => "No se pudo guardar"]);
}

$stmt->close();
$conn->close();