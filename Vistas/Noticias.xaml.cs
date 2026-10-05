using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Menushell401.Modelos;

namespace Menushell401.Vistas;

/// <summary>
/// Clase parcial que representa la vista de Noticias en .NET MAUI.
/// Hereda de ContentPage y gestiona la carga asíncrona e interfaz de noticias.
/// </summary>
public partial class Noticias : ContentPage
{
    /// <summary>
    /// Colección observable que almacena el listado de noticias.
    /// Notifica automáticamente a la interfaz (XAML) cuando se agregan, eliminan o limpian elementos.
    /// </summary>
    public ObservableCollection<Noticia> MisNoticias { get; set; } = new();

    /// <summary>
    /// Propiedad estática de solo lectura que construye la URL del endpoint PHP 
    /// a partir de la configuración global de la aplicación.
    /// </summary>
    private static string Url => $"{Config.BaseUrl}/noticias.php";

    /// <summary>
    /// Bandera o flag para evitar peticiones HTTP concurrentes/duplicadas si la carga ya está en proceso.
    /// </summary>
    private bool _cargando;

    /// <summary>
    /// Constructor de la página. Inicializa los componentes visuales de XAML 
    /// y asigna el BindingContext a sí misma para permitir el Data Binding.
    /// </summary>
    public Noticias()
    {
        InitializeComponent();
        BindingContext = this;
    }

    /// <summary>
    /// Evento del ciclo de vida de la página que se ejecuta cuando esta aparece en pantalla.
    /// Inicia la primera carga de noticias de forma asíncrona.
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarNoticias();
    }

    /// <summary>
    /// Método encargado de realizar la petición HTTP GET a la API web para obtener las noticias
    /// y poblar la colección observable en la interfaz.
    /// </summary>
    private async Task CargarNoticias()
    {
        // Control de concurrencia: detiene la ejecución si ya hay una solicitud en curso
        if (_cargando) return;
        _cargando = true;

        // Activa el indicador visual de recarga (por ejemplo, el spinner del RefreshView)
        refresh.IsRefreshing = true;

        try
        {
            // Inicializa el cliente HTTP con un tiempo de espera límite de 10 segundos
            using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };

            // Realiza la petición GET y deserializa la respuesta JSON directamente a una lista de objetos Noticia
            var noticias = await http.GetFromJsonAsync<List<Noticia>>(Url,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Limpia los datos antiguos antes de insertar los nuevos para refrescar la lista
            MisNoticias.Clear();
            foreach (var n in noticias ?? new List<Noticia>())
                MisNoticias.Add(n);
        }
        catch (Exception ex)
        {
            // Muestra un mensaje de alerta asíncrono al usuario en caso de falla en la conexión o en el servidor
            await DisplayAlert("Error", $"{ex.Message}\n\nURL: {Url}", "OK");
        }
        finally
        {
            // Garantiza que el indicador visual y la bandera de estado se restablezcan al finalizar
            refresh.IsRefreshing = false;
            _cargando = false;
        }
    }

    /// <summary>
    /// Evento del control RefreshView que se activa cuando el usuario desliza hacia abajo para actualizar.
    /// </summary>
    private async void OnRefresh(object sender, EventArgs e)
    {
        await CargarNoticias();
    }
}