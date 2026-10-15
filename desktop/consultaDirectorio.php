<?php
header('Content-Type: application/json; charset=utf-8');
include('conexion.php');

try {
    $entrada = json_decode(file_get_contents("php://input"), false);
    $departamento = is_object($entrada) ? ($entrada->departamento ?? 'Todos') : 'Todos';

    if ($departamento === 'Todos') {
        $stmt = mysqli_prepare($conexion, "SELECT nombre, departamento, correo, extension FROM directorio ORDER BY nombre ASC");
    } else {
        $stmt = mysqli_prepare($conexion, "SELECT nombre, departamento, correo, extension FROM directorio WHERE departamento=? ORDER BY nombre ASC");
        mysqli_stmt_bind_param($stmt, "s", $departamento);
    }

    mysqli_stmt_execute($stmt);
    $datos = mysqli_stmt_get_result($stmt);

    $resultado = [];
    while ($reg = mysqli_fetch_assoc($datos)) {
        $resultado[] = $reg;
    }

    echo json_encode($resultado, JSON_UNESCAPED_UNICODE);
    mysqli_stmt_close($stmt);
    mysqli_close($conexion);
} catch (Throwable $e) {
    http_response_code(500);
    echo json_encode(['error' => $e->getMessage()]);
}
?>