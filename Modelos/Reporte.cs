namespace Menushell401.Modelos;

public class Reporte
{
    public string Nombre { get; set; } = "";
    public string Matricula { get; set; } = "";
    public string Tipo { get; set; } = "";
    public string Descripcion { get; set; } = "";
}

public class RespuestaApi
{
    public bool Ok { get; set; }
    public string Mensaje { get; set; } = "";
}