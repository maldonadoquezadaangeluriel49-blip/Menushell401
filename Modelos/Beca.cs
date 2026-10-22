using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Menushell401.Modelos;

public class Beca : INotifyPropertyChanged
{
    // Vienen del JSON
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Tipo { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public string Requisitos { get; set; } = "";
    public string Monto { get; set; } = "";
    public string FechaLimite { get; set; } = "";
    public int DiasRestantes { get; set; }

    // Estado local: ¿el usuario activó el recordatorio?
    private bool _recordatorio;
    public bool Recordatorio
    {
        get => _recordatorio;
        set
        {
            _recordatorio = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TextoBoton));
            OnPropertyChanged(nameof(ColorBoton));
        }
    }

    // Propiedades calculadas para la interfaz
    public string TextoPlazo => DiasRestantes switch
    {
        < 0 => "Convocatoria cerrada",
        0 => "Cierra hoy",
        1 => "Cierra mañana",
        _ => $"Cierra en {DiasRestantes} días"
    };

    public Color ColorPlazo => DiasRestantes switch
    {
        < 0 => Colors.Gray,
        <= 3 => Color.FromArgb("#D32F2F"),
        <= 10 => Color.FromArgb("#EF6C00"),
        _ => Color.FromArgb("#2E7D32")
    };

    public string TextoBoton =>
        DiasRestantes < 0 ? "Convocatoria cerrada" :
        Recordatorio ? "🔔 Recordatorio activado (toca para quitar)" :
                       "🔔 Recordarme antes de que cierre";

    public Color ColorBoton =>
        DiasRestantes < 0 ? Colors.Gray :
        Recordatorio ? Color.FromArgb("#2E7D32") : Color.FromArgb("#D32F2F");

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? nombre = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
}