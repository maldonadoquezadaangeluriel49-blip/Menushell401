using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http.Json;
using Menushell401.Modelos;

namespace Menushell401.Vistas;

public partial class Croquis : ContentPage
{
    public ObservableCollection<Lugar> Lugares { get; } = new();

    private static string Url => $"{Config.BaseUrl}/consultaEdificios.php";
    private Location? _miUbicacion;

    public Croquis()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarLugares();
        await Localizar();
    }

    // 1) Trae los lugares del campus desde el PHP (JSON)
    private async Task CargarLugares()
    {
        try
        {
            using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
            var lista = await http.GetFromJsonAsync<List<Lugar>>(Url,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Lugares.Clear();
            foreach (var l in lista ?? new List<Lugar>())
                Lugares.Add(l);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"{ex.Message}\n\nURL: {Url}", "OK");
        }
    }

    // 2) Misma lógica de la clase: permiso + GetLocationAsync
    private async Task Localizar()
    {
        try
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                await DisplayAlertAsync("Error", "Permiso de ubicación denegado.", "OK");
                return;
            }

            var location = await Geolocation.GetLocationAsync(
                new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10)));

            if (location == null)
            {
                await DisplayAlertAsync("Error", "No se pudo obtener la ubicación.", "OK");
                return;
            }

            _miUbicacion = location;
            lat.Text = location.Latitude.ToString("F6");
            lon.Text = location.Longitude.ToString("F6");

            // Distancia de cada lugar a tu posición (se calcula en el teléfono)
            foreach (var l in Lugares)
            {
                double km = Location.CalculateDistance(
                    location, new Location(l.Latitud, l.Longitud), DistanceUnits.Kilometers);

                l.DistanciaTexto = km < 1 ? $"A {km * 1000:F0} m de ti" : $"A {km:F1} km de ti";
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"Error al obtener la ubicación: {ex.Message}", "OK");
        }
    }

    private async void Actualizar_Clicked(object? sender, EventArgs e) => await Localizar();

    // 3) Enlaces de Google Maps (InvariantCulture para que los decimales lleven punto, no coma)
    private static string UrlMapa(double lat, double lon) =>
        $"https://www.google.com/maps/search/?api=1&query={lat.ToString(CultureInfo.InvariantCulture)},{lon.ToString(CultureInfo.InvariantCulture)}";

    private static string UrlRuta(double lat, double lon) =>
        $"https://www.google.com/maps/dir/?api=1&destination={lat.ToString(CultureInfo.InvariantCulture)},{lon.ToString(CultureInfo.InvariantCulture)}&travelmode=walking";

    private async Task AbrirAsync(string url)
    {
        try
        {
            await Launcher.OpenAsync(new Uri(url));
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"Error al abrir el mapa: {ex.Message}", "OK");
        }
    }

    // 4) Mi posición actual en Google Maps
    private async void MiMapa_Clicked(object? sender, EventArgs e)
    {
        if (_miUbicacion == null) await Localizar();
        if (_miUbicacion == null) return;

        await AbrirAsync(UrlMapa(_miUbicacion.Latitude, _miUbicacion.Longitude));
    }

    // 5) El lugar tocado en Google Maps
    private async void VerLugar_Clicked(object? sender, EventArgs e)
    {
        if (sender is not BindableObject obj || obj.BindingContext is not Lugar lugar) return;
        await AbrirAsync(UrlMapa(lugar.Latitud, lugar.Longitud));
    }

    // 6) Ruta a pie hacia el lugar (Google Maps usa tu ubicación como punto de partida)
    private async void ComoLlegar_Clicked(object? sender, EventArgs e)
    {
        if (sender is not BindableObject obj || obj.BindingContext is not Lugar lugar) return;
        await AbrirAsync(UrlRuta(lugar.Latitud, lugar.Longitud));


    }
    // Dirección del campus: se busca por texto, no hace falta conocer las coordenadas
    private const string DireccionCampus =
        "Universidad Politécnica del Valle de México, Av. Mexiquense s/n, esq. Av. Universidad Politécnica, 54910 Tultitlán, Méx.";

    private async void CampusMapa_Clicked(object? sender, EventArgs e) =>
        await AbrirAsync($"https://www.google.com/maps/search/?api=1&query={Uri.EscapeDataString(DireccionCampus)}");

    // Ruta desde donde estés hacia la universidad
    private async void CampusRuta_Clicked(object? sender, EventArgs e) =>
        await AbrirAsync($"https://www.google.com/maps/dir/?api=1&destination={Uri.EscapeDataString(DireccionCampus)}");
}