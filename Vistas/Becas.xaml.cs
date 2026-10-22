using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Menushell401.Modelos;

namespace Menushell401.Vistas;

public partial class Becas : ContentPage
{
    public ObservableCollection<Beca> MisBecas { get; } = new();

    private static string Url => $"{Config.BaseUrl}/consultaBecas.php";
    private bool _cargando;

    private string TipoActual => pkTipo.SelectedItem?.ToString() ?? "Todas";

    public Becas()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Notificaciones.PedirPermiso();
        await CargarBecas();
    }

    private async Task CargarBecas()
    {
        if (_cargando) return;
        _cargando = true;
        refresh.IsRefreshing = true;

        try
        {
            using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };

            // Mandamos el filtro como JSON y recibimos la lista como JSON
            var resp = await http.PostAsJsonAsync(Url,
                new { tipo = TipoActual, soloVigentes = swVigentes.IsToggled });
            resp.EnsureSuccessStatusCode();

            var becas = await resp.Content.ReadFromJsonAsync<List<Beca>>(
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            MisBecas.Clear();
            foreach (var b in becas ?? new List<Beca>())
            {
                // Recuperamos si el usuario ya había activado el recordatorio
                b.Recordatorio = Preferences.Get($"recordatorio_beca_{b.Id}", false);
                MisBecas.Add(b);
            }

            // Solo con la lista completa podemos saber si hay becas nuevas
            if (TipoActual == "Todas")
                await Notificaciones.ProcesarNuevasBecas(MisBecas);
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

    private async void OnRefresh(object? sender, EventArgs e) => await CargarBecas();
    private async void OnFiltroChanged(object? sender, EventArgs e) => await CargarBecas();
    private async void OnVigentesToggled(object? sender, ToggledEventArgs e) => await CargarBecas();

    // Activa o quita el recordatorio de la beca tocada
    private async void OnRecordatorioClicked(object? sender, EventArgs e)
    {
        if (sender is not BindableObject obj || obj.BindingContext is not Beca beca) return;

        if (beca.DiasRestantes < 0)
        {
            await DisplayAlertAsync("Convocatoria cerrada",
                "Esta beca ya no está disponible.", "OK");
            return;
        }

        string clave = $"recordatorio_beca_{beca.Id}";

        if (beca.Recordatorio)
        {
            Notificaciones.CancelarAvisoBeca(beca);
            Preferences.Remove(clave);
            beca.Recordatorio = false;
        }
        else
        {
            await Notificaciones.PedirPermiso();
            await Notificaciones.ActivarAvisoBeca(beca);
            Preferences.Set(clave, true);
            beca.Recordatorio = true;
        }
    }
}