using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Menushell401.Modelos;

namespace Menushell401.Vistas;

public partial class Noticias : ContentPage
{
    public ObservableCollection<Noticia> MisNoticias { get; set; } = new();

    private static string Url => $"{Config.BaseUrl}/noticias.php";

    private bool _cargando;

    public Noticias()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Notificaciones.PedirPermiso();   // primero el permiso
        await CargarNoticias();                // una sola carga

        // PRUEBA TEMPORAL: borra esta línea cuando confirmes que funciona
        await Notificaciones.Mostrar(1, "Prueba", "Las notificaciones funcionan", "noticias",
                                     DateTime.Now.AddSeconds(5));
    }

    private async Task CargarNoticias()
    {
        if (_cargando) return;
        _cargando = true;
        refresh.IsRefreshing = true;

        try
        {
            using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };

            var noticias = await http.GetFromJsonAsync<List<Noticia>>(Url,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            MisNoticias.Clear();
            foreach (var n in noticias ?? new List<Noticia>())
                MisNoticias.Add(n);

            await Notificaciones.ProcesarNuevas(MisNoticias);   // esta sí va aquí
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"{ex.Message}\n\nURL: {Url}", "OK");
        }
        finally
        {
            refresh.IsRefreshing = false;
            _cargando = false;
        }
    }

    private async void OnRefresh(object sender, EventArgs e)
    {
        await CargarNoticias();
    }

    // Se ejecuta al tocar una tarjeta: abre la página de detalle
    private async void OnNoticiaTapped(object? sender, TappedEventArgs e)
    {
        if (sender is not BindableObject obj || obj.BindingContext is not Noticia noticia)
            return;

        await Shell.Current.GoToAsync(nameof(NoticiaDetalle),
            new Dictionary<string, object> { ["noticia"] = noticia });
    }
}