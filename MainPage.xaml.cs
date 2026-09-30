using System.Text;
using System.Text.Json;

namespace Menushell401
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // --- MANEJADORES DE EVENTOS VINCULADOS EN EL XAML ---
        private async void OnPopularesClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Noticias");
        }

        private async void OnNovedadesClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Comunicados");
        }

        private async void OnIrAVerClicked(object sender, EventArgs e)
        {
            // Lógica para el botón IR A VER
        }

        // --- LÓGICA DE INICIO DE SESIÓN ---
        private async void IniciarSesion_Clicked(object sender, EventArgs e)
        {
            var entryCorreo = this.FindByName<Entry>("txtCorreo");
            var entryPassword = this.FindByName<Entry>("txtPassword");

            string correo = entryCorreo?.Text ?? "";
            string password = entryPassword?.Text ?? "";

            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Atención", "Por favor ingresa correo y contraseña.", "OK");
                return;
            }

            var datosLogin = new
            {
                correo = correo,
                password = password
            };

            string jsonContenido = JsonSerializer.Serialize(datosLogin);
            var content = new StringContent(jsonContenido, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = "http://localhost/diario_upvm/api_login.php";

                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonRespuesta = await response.Content.ReadAsStringAsync();

                        using (JsonDocument doc = JsonDocument.Parse(jsonRespuesta))
                        {
                            bool success = doc.RootElement.GetProperty("success").GetBoolean();
                            string message = doc.RootElement.GetProperty("message").GetString();

                            if (success)
                            {
                                string nombreUsuario = doc.RootElement.GetProperty("usuario").GetProperty("nombre").GetString();
                                await DisplayAlert("Éxito", $"¡Bienvenido {nombreUsuario}!", "OK");
                                await Shell.Current.GoToAsync("//Noticias");
                            }
                            else
                            {
                                await DisplayAlert("Error", message, "OK");
                            }
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo conectar con el servidor XAMPP.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Excepción", "Error de red: " + ex.Message, "OK");
                }
            }
        }
    }
}