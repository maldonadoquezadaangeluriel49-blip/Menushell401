using Menushell401.Modelos;
using Plugin.LocalNotification;
using Plugin.LocalNotification.Core.Models;

namespace Menushell401;

public static class Notificaciones
{
    // 1) Se llama una vez al iniciar la app: qué hacer cuando el usuario toca una notificación
    public static void Iniciar()
    {
        LocalNotificationCenter.Current.NotificationActionTapped += async e =>
        {
            string destino = e.Request.ReturningData ?? "noticias";
            await MainThread.InvokeOnMainThreadAsync(() => Shell.Current.GoToAsync($"//{destino}"));
        };
    }

    // 2) Pide permiso solo si todavía no lo tenemos
    public static async Task PedirPermiso()
    {
        if (!await LocalNotificationCenter.Current.AreNotificationsEnabled())
            await LocalNotificationCenter.Current.RequestNotificationPermission();
    }

    // 3) Método base: todas las notificaciones pasan por aquí
    public static async Task Mostrar(int id, string titulo, string descripcion,
                                     string destino = "noticias", DateTime? cuando = null)
    {
        var request = new NotificationRequest
        {
            NotificationId = id,
            Title = titulo,
            Description = descripcion,
            ReturningData = destino
        };

        if (cuando != null)
            request.Schedule = new NotificationRequestSchedule { NotifyTime = cuando };

        await LocalNotificationCenter.Current.Show(request);
    }

    // 4) Noticias nuevas: compara con la última noticia que ya conocíamos
    public static async Task ProcesarNuevas(IEnumerable<Noticia> noticias)
    {
        var lista = noticias.ToList();
        if (lista.Count == 0) return;

        int maxId = lista.Max(n => n.Id);
        int ultimo = Preferences.Get("ultimo_id_noticia", -1);

        if (ultimo == -1)                       // primera vez: solo guardamos el punto de partida
        {
            Preferences.Set("ultimo_id_noticia", maxId);
            return;
        }

        var nuevas = lista.Where(n => n.Id > ultimo).OrderByDescending(n => n.Id).ToList();
        if (nuevas.Count == 0) return;

        Preferences.Set("ultimo_id_noticia", maxId);

        string titulo = nuevas.Count == 1 ? "Nueva noticia" : $"{nuevas.Count} noticias nuevas";
        await Mostrar(1001, titulo, nuevas[0].Titulo, "noticias");
    }

    // 5) Recordatorio de evento: avisa el día anterior a las 9:00 a. m.
    public static async Task ProgramarRecordatorio(int idEvento, string titulo, DateTime fechaEvento)
    {
        DateTime aviso = fechaEvento.Date.AddDays(-1).AddHours(9);
        if (aviso <= DateTime.Now) return;      // si la hora ya pasó, no tiene sentido programarla

        await Mostrar(2000 + idEvento, "Evento mañana", titulo, "calendario", aviso);
    }
}