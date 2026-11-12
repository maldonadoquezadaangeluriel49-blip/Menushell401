using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Menushell401.Modelos;

public class Lugar : INotifyPropertyChanged
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public double Latitud { get; set; }
    public double Longitud { get; set; }

    // Se llena cuando tenemos tu ubicación ("A 350 m de ti")
    private string _distanciaTexto = "";
    public string DistanciaTexto
    {
        get => _distanciaTexto;
        set { _distanciaTexto = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? nombre = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
}