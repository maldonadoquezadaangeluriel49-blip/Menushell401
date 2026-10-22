<?php
header('Content-Type: application/json; charset=utf-8');

$conexion = mysqli_connect("localhost", "root", "", "registro");
if (!$conexion) {
    http_response_code(500);
    echo json_encode(["error" => "Sin conexión a la base de datos"]);
    exit;
}
mysqli_set_charset($conexion, "utf8mb4");

// Entrada: { "tipo": "Todas", "soloVigentes": true }
$e = json_decode(file_get_contents("php://input"));
$tipo = $e->tipo ?? 'Todas';
$soloVigentes = $e->soloVigentes ?? true;

$sql = "SELECT id, nombre, tipo, descripcion, requisitos, monto,
               DATE_FORMAT(fecha_limite, '%d/%m/%Y') AS fechaLimite,
               DATEDIFF(fecha_limite, CURDATE()) AS diasRestantes
        FROM becas WHERE 1=1";

if ($tipo !== 'Todas')  $sql .= " AND tipo = ?";
if ($soloVigentes)      $sql .= " AND fecha_limite >= CURDATE()";
$sql .= " ORDER BY fecha_limite ASC";

$stmt = mysqli_prepare($conexion, $sql);
if ($tipo !== 'Todas') mysqli_stmt_bind_param($stmt, "s", $tipo);
mysqli_stmt_execute($stmt);
$res = mysqli_stmt_get_result($stmt);

$salida = [];
while ($r = mysqli_fetch_assoc($res)) {
    $r['id'] = (int)$r['id'];
    $r['diasRestantes'] = (int)$r['diasRestantes'];
    $salida[] = $r;
}

echo json_encode($salida);
mysqli_close($conexion);
?>