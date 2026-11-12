using System.Text;
using System.Text.Json;

namespace Menushell401.Services;

public class DeepSeekService
{
    private readonly HttpClient _httpClient;

    // Aquí se define la "personalidad" y el tema del chatbot
    private const string SystemPrompt =
        "Eres el asistente virtual del Diario UPVM, una app escolar universitaria. " +
        "Ayudas a los estudiantes con dudas sobre noticias, becas, eventos, comunicados, " +
        "calendario, horarios y control escolar. " +
        "Responde siempre en español, de forma breve, clara y amable. " +
        "Si no tienes la información, dilo y sugiere revisar la sección correspondiente de la app " +
        "o acudir a control escolar. No inventes fechas, requisitos ni montos.";

    public DeepSeekService()
    {
        _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Secrets.ApiKey}");
    }

    public async Task<string> GetCodeResponseAsync(string userPrompt, string contexto = "")
    {
        // Si hay datos de la BD, se los pasamos al modelo como contexto
        string sistema = string.IsNullOrEmpty(contexto)
            ? SystemPrompt
            : SystemPrompt + "\n\nDatos actuales de la app (JSON):\n" + contexto;

        var requestBody = new
        {
            model = Secrets.Model,
            messages = new[]
            {
                new { role = "system", content = sistema },
                new { role = "user", content = userPrompt }
            },
            stream = false
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync(Secrets.ApiUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();

            // Si falla, mostramos el motivo que explica el servidor
            if (!response.IsSuccessStatusCode)
                return $"Error {(int)response.StatusCode}: {responseString}";

            using var doc = JsonDocument.Parse(responseString);

            // Navegamos por el JSON para obtener el texto de la respuesta
            return doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "";
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}