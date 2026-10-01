using System.Net.Http.Json;
using Menushell401.Modelos;

namespace Menushell401.Vistas;

public partial class Noticias : ContentPage
{
    // Windows: localhost | Emulador Android: 10.0.2.2 | Celular físico: usa la IP de tu PC (ej. 192.168.1.50)
    private static string Url =>
        DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2/diario/noticias.php"
            : "http://localhost/diario/noticias.php";

    private bool _cargando;

    public Noticias()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarNoticias();
    }

    private async Task CargarNoticias()
    {
        if (_cargando) return;
        _cargando = true;
        refresh.IsRefreshing = true;

        try
        {
            using var http = new HttpClient();
            var noticias = await http.GetFromJsonAsync<List<Noticia>>(Url,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            listaNoticias.ItemsSource = noticias;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
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
}