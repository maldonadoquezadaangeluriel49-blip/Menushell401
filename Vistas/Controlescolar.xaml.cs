using System.Net.Http.Json;
using Menushell401.Modelos;

namespace Menushell401.Vistas;

/// <summary>
/// Clase parcial para la página de Control Escolar en .NET MAUI.
/// Gestiona la validación, construcción y envío de reportes hacia el servidor web.
/// </summary>
public partial class Controlescolar : ContentPage
{
    /// <summary>
    /// Propiedad estática que construye la URL del endpoint PHP 
    /// encargado de recibir y procesar los reportes guardados.
    /// </summary>
    private static string Url => $"{Config.BaseUrl}/guardar_reporte.php";

    /// <summary>
    /// Constructor de la clase. Inicializa los elementos gráficos definidos en el archivo XAML.
    /// </summary>
    public Controlescolar()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Evento asíncrono que se dispara al hacer clic en el botón 'Enviar'.
    /// Valida los campos del formulario, deshabilita el botón temporalmente y envía los datos por HTTP POST.
    /// </summary>
    private async void OnEnviarClicked(object sender, EventArgs e)
    {
        // 1. VALIDACIÓN DE CAMPOS: Verificación de que ningún campo obligatorio esté vacío o sin seleccionar
        if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
            string.IsNullOrWhiteSpace(txtMatricula.Text) ||
            pckTipo.SelectedItem == null ||
            string.IsNullOrWhiteSpace(txtDescripcion.Text))
        {
            // Muestra mensaje de advertencia si la validación falla
            await DisplayAlert("Faltan datos", "Completa todos los campos.", "OK");
            return;
        }

        // 2. CREACIÓN DEL OBJETO DE DATOS: Mapea la entrada de texto de la interfaz a un objeto de tipo 'Reporte'
        var reporte = new Reporte
        {
            Nombre = txtNombre.Text.Trim(),
            Matricula = txtMatricula.Text.Trim(),
            Tipo = pckTipo.SelectedItem.ToString()!,
            Descripcion = txtDescripcion.Text.Trim()
        };

        // Prevención de doble clic: Deshabilita el botón mientras dura el proceso de envío
        btnEnviar.IsEnabled = false;

        try
        {
            // Inicializa el cliente HTTP con un límite de tiempo de 10 segundos
            using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };

            // 3. ENVÍO HTTP POST: Petición enviando el objeto 'reporte' formateado a JSON (usando nomenclatura camelCase)
            var respuesta = await http.PostAsJsonAsync(Url, reporte,
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
                });

            // 4. DESERIALIZACIÓN DE LA RESPUESTA: Lee el JSON devuelto por la API web
            var resultado = await respuesta.Content.ReadFromJsonAsync<RespuestaApi>(
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // 5. MANEJO DE RESULTADOS DE LA API
            if (respuesta.IsSuccessStatusCode && resultado?.Ok == true)
            {
                // Notifica al usuario la confirmación de guardado exitoso
                await DisplayAlert("Listo", "Tu reporte fue registrado.", "OK");

                // Limpieza/Reset de los campos del formulario tras la confirmación
                txtNombre.Text = "";
                txtMatricula.Text = "";
                pckTipo.SelectedIndex = -1;
                txtDescripcion.Text = "";
            }
            else
            {
                // Si la API respondió con error o fracaso en la lógica de negocio
                await DisplayAlert("Error", resultado?.Mensaje ?? "No se pudo guardar.", "OK");
            }
        }
        catch (Exception ex)
        {
            // Captura errores de red, tiempos de espera (timeouts) o excepciones inesperadas
            await DisplayAlert("Error", $"{ex.Message}\n\nURL: {Url}", "OK");
        }
        finally
        {
            // Vuelve a habilitar el botón de envío sin importar el resultado del flujo
            btnEnviar.IsEnabled = true;
        }
    }
}