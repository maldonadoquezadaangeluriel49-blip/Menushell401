using Menushell401.Modelos;
using Newtonsoft.Json;
using System.Text;

namespace Menushell401.Vistas;

public partial class Directorio : ContentPage
{
    // Deben coincidir con los valores de la columna departamento de la tabla
    readonly List<string> departamentos = new()
    {
        "Todos", "Control escolar", "Biblioteca", "Sistemas", "Becas"
    };

    public Directorio()
    {
        InitializeComponent();
        pkrDepartamento.ItemsSource = departamentos;
        pkrDepartamento.SelectedIndex = 0; // dispara la primera consulta
    }

    private async void pkrDepartamento_SelectedIndexChanged(object? sender, EventArgs e)
    {
        await Consultar();
    }

    async Task Consultar()
    {
        if (pkrDepartamento.SelectedIndex < 0) return;

        try
        {
            indicador.IsVisible = true;
            indicador.IsRunning = true;

            var json = JsonConvert.SerializeObject(new
            {
                departamento = departamentos[pkrDepartamento.SelectedIndex]
            });

            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
            var resp = await client.PostAsync(
                $"{Config.BaseUrl}/consultaDirectorio.php",
                new StringContent(json, Encoding.UTF8, "application/json"));

            var texto = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode || texto.TrimStart().StartsWith("<"))
            {
                await DisplayAlertAsync("Respuesta del servidor",
                    $"Código: {(int)resp.StatusCode}\n{texto.Substring(0, Math.Min(texto.Length, 300))}",
                    "OK");
                return;
            }

            var lista = JsonConvert.DeserializeObject<List<ContactoDirectorio>>(texto) ?? new List<ContactoDirectorio>();
            lstDirectorio.ItemsSource = lista;
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