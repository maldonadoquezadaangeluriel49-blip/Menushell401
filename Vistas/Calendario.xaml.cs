using Menushell401.Modelos;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;

namespace Menushell401.Vistas;

public partial class Calendario : ContentPage
{
    public Calendario()
    {
        InitializeComponent();
        var culture = new CultureInfo("es-ES");
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        dpk_fecha.Date = DateTime.Today;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Consultar("no");
    }

    private async void dpk_fecha_DateSelected(object? sender, DateChangedEventArgs e)
    {
        await Consultar("no");
    }

    private async void btnMes_Clicked(object? sender, EventArgs e)
    {
        await Consultar("si");
    }

    async Task Consultar(string mesCompleto)
    {
        try
        {
            indicador.IsVisible = true;
            indicador.IsRunning = true;

            DateTime d = dpk_fecha.Date ?? DateTime.Today;
            var json = JsonConvert.SerializeObject(new
            {
                dia = d.Day.ToString(),
                mes = d.Month.ToString(),
                anio = d.Year.ToString(),
                mesCompleto
            });

            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
            var resp = await client.PostAsync(
                $"{Config.BaseUrl}/consultaCalendario.php",
                new StringContent(json, Encoding.UTF8, "application/json"));

            var texto = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode || texto.TrimStart().StartsWith("<"))
            {
                await DisplayAlertAsync("Respuesta del servidor",
                    $"Código: {(int)resp.StatusCode}\n{texto.Substring(0, Math.Min(texto.Length, 300))}",
                    "OK");
                return;
            }

            var lista = JsonConvert.DeserializeObject<List<EventoCalendario>>(texto) ?? new List<EventoCalendario>();
            lstEventos.ItemsSource = lista;
            lblVacio.IsVisible = lista.Count == 0;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
        finally
        {
            indicador.IsRunning = false;
            indicador.IsVisible = false;
        }
    }
}