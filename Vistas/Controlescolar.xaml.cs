using System.Net.Http.Json;
using Menushell401.Modelos;

namespace Menushell401.Vistas;

public partial class Controlescolar : ContentPage
{
    // Windows: localhost | Emulador Android: 10.0.2.2 | Celular físico: IP de tu PC
    private static string Url =>
        DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2/diario/guardar_reporte.php"
            : "http://localhost/diario/guardar_reporte.php";

    public Controlescolar()
    {
        InitializeComponent();
    }

    private async void OnEnviarClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
            string.IsNullOrWhiteSpace(txtMatricula.Text) ||
            pckTipo.SelectedItem == null ||
            string.IsNullOrWhiteSpace(txtDescripcion.Text))
        {
            await DisplayAlert("Faltan datos", "Completa todos los campos.", "OK");
            return;
        }

        var reporte = new Reporte
        {
            Nombre = txtNombre.Text.Trim(),
            Matricula = txtMatricula.Text.Trim(),
            Tipo = pckTipo.SelectedItem.ToString()!,
            Descripcion = txtDescripcion.Text.Trim()
        };

        btnEnviar.IsEnabled = false;

        try
        {
            using var http = new HttpClient();
            var respuesta = await http.PostAsJsonAsync(Url, reporte,
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
                });

            var resultado = await respuesta.Content.ReadFromJsonAsync<RespuestaApi>(
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (respuesta.IsSuccessStatusCode && resultado?.Ok == true)
            {
                await DisplayAlert("Listo", "Tu reporte fue registrado.", "OK");
                txtNombre.Text = "";
                txtMatricula.Text = "";
                pckTipo.SelectedIndex = -1;
                txtDescripcion.Text = "";
            }
            else
            {
                await DisplayAlert("Error", resultado?.Mensaje ?? "No se pudo guardar.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            btnEnviar.IsEnabled = true;
        }
    }
}