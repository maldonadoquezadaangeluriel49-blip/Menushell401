namespace Menushell401.Modelos;

public class Noticia
{
    public int Id { get; set; }
    public string Titulo { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public string? Imagen { get; set; }

    public bool TieneImagen => !string.IsNullOrEmpty(Imagen);
}