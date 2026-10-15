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

$result = $conn->query("SELECT id, titulo, contenido, icono FROM acerca_de ORDER BY orden ASC, id ASC");
$items = [];
while ($row = $result->fetch_assoc()) {
    $row['id'] = (int)$row['id'];
    $items[] = $row;
}

echo json_encode($items, JSON_UNESCAPED_UNICODE);
$conn->close();