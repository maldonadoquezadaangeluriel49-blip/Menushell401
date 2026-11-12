using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Menushell401.Modelos;
using Menushell401.Services;

namespace Menushell401.Vistas;

public partial class Asistente : ContentPage
{
    public ObservableCollection<ChatMessage> Messages { get; set; } = new();
    private readonly DeepSeekService _deepSeekService = new();

    public Asistente()
    {
        InitializeComponent();
        ChatMessagesList.ItemsSource = Messages;

        // Mensaje de bienvenida
        Messages.Add(new ChatMessage
        {
            Content = "¡Hola! Soy el asistente del Diario UPVM. Pregúntame sobre becas, noticias o eventos.",
            SenderName = "Asistente UPVM",
            HorizontalAlignment = LayoutOptions.Start,
            BackgroundColor = Color.FromArgb("#424242")
        });
    }

    private async void SendBtn_Clicked(object? sender, EventArgs e)
    {
        string prompt = MessageEntry.Text?.Trim() ?? "";
        if (string.IsNullOrEmpty(prompt)) return;

        // 1) Mostramos el mensaje del usuario
        Messages.Add(new ChatMessage
        {
            Content = prompt,
            SenderName = "Tú",
            HorizontalAlignment = LayoutOptions.End,
            BackgroundColor = Color.FromArgb("#D32F2F")
        });
        MessageEntry.Text = string.Empty;
        SendBtn.IsEnabled = false;          // evita enviar dos veces mientras espera
        ScrollAlFinal();

        // 2) Traemos datos reales de la BD (JSON) y se los damos al chatbot
        string contexto = await ObtenerContextoAsync();
        string respuesta = await _deepSeekService.GetCodeResponseAsync(prompt, contexto);

        // 3) Mostramos la respuesta
        Messages.Add(new ChatMessage
        {
            Content = respuesta,
            SenderName = "Asistente UPVM",
            HorizontalAlignment = LayoutOptions.Start,
            BackgroundColor = Color.FromArgb("#424242")
        });
        SendBtn.IsEnabled = true;
        ScrollAlFinal();
    }

    // Consulta el PHP de becas y devuelve su JSON como texto
    private async Task<string> ObtenerContextoAsync()
    {
        try
        {
            using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(8) };
            var resp = await http.PostAsJsonAsync($"{Config.BaseUrl}/consultaBecas.php",
                new { tipo = "Todas", soloVigentes = true });

            string json = await resp.Content.ReadAsStringAsync();
            return json.Length > 4000 ? json[..4000] : json;   // limita el tamaño
        }
        catch
        {
            return "";   // si falla, el chatbot responde sin datos
        }
    }

    private void ScrollAlFinal()
    {
        if (Messages.Count > 0)
            ChatMessagesList.ScrollTo(Messages.Count - 1, position: ScrollToPosition.End, animate: true);
    }
}