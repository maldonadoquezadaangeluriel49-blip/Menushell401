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

$result = $conn->query("SELECT id, titulo, descripcion FROM noticias ORDER BY id DESC");
$noticias = [];
while ($row = $result->fetch_assoc()) {
    $row['id'] = (int)$row['id'];
    $noticias[] = $row;
}
echo json_encode($noticias, JSON_UNESCAPED_UNICODE);
$conn->close();