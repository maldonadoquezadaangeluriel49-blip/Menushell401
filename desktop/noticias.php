<?php
header("Content-Type: application/json; charset=utf-8");
header("Access-Control-Allow-Origin: *");

$conn = new mysqli("localhost", "root", "", "diario_upvm");
$conn->set_charset("utf8mb4");

if ($conn->connect_error) {
    http_response_code(500);
    echo json_encode(["error" => "Error de conexión"]);
    exit;
}

// Usa el mismo host con el que la app llamó (localhost, 10.0.2.2 o tu IP)
$base = "http://" . $_SERVER['HTTP_HOST'] . "/diario/imagenes/";

$result = $conn->query("SELECT id, titulo, descripcion, imagen FROM noticias ORDER BY id DESC");
$noticias = [];
while ($row = $result->fetch_assoc()) {
    $row['id'] = (int)$row['id'];
    $row['imagen'] = $row['imagen'] ? $base . rawurlencode($row['imagen']) : null;
    $noticias[] = $row;
}

echo json_encode($noticias, JSON_UNESCAPED_UNICODE | JSON_UNESCAPED_SLASHES);
$conn->close();