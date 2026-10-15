<?php
header('Content-Type: application/json; charset=utf-8');
include('conexion.php');

try {
    $entrada = json_decode(file_get_contents("php://input"), false);
    if (!is_object($entrada)) {
        $entrada = (object) ['dia' => 0, 'mes' => 0, 'anio' => 0, 'mesCompleto' => 'todo'];
    }

    $dia  = (int)($entrada->dia ?? 0);
    $mes  = (int)($entrada->mes ?? 0);
    $anio = (int)($entrada->anio ?? 0);
    $modo = $entrada->mesCompleto ?? 'todo';

    if ($modo === 'si') {
        $stmt = mysqli_prepare($conexion, "SELECT titulo, fecha, contenido FROM calendario WHERE YEAR(fecha)=? AND MONTH(fecha)=? ORDER BY fecha ASC");
        mysqli_stmt_bind_param($stmt, "ii", $anio, $mes);
    } elseif ($modo === 'no') {
        $fechaExacta = sprintf("%04d-%02d-%02d", $anio, $mes, $dia);
        $stmt = mysqli_prepare($conexion, "SELECT titulo, fecha, contenido FROM calendario WHERE fecha=? ORDER BY id DESC");
        mysqli_stmt_bind_param($stmt, "s", $fechaExacta);
    } else {
        $stmt = mysqli_prepare($conexion, "SELECT titulo, fecha, contenido FROM calendario ORDER BY fecha ASC");
    }

    mysqli_stmt_execute($stmt);
    $datos = mysqli_stmt_get_result($stmt);

    $resultado = [];
    while ($reg = mysqli_fetch_assoc($datos)) {
        $resultado[] = [
            'titulo'    => $reg['titulo'],
            'fecha'     => date('d/m/Y', strtotime($reg['fecha'])),
            'contenido' => $reg['contenido']
        ];
    }

    echo json_encode($resultado, JSON_UNESCAPED_UNICODE);
    mysqli_stmt_close($stmt);
    mysqli_close($conexion);
} catch (Throwable $e) {
    http_response_code(500);
    echo json_encode(['error' => $e->getMessage()]);
}
?>