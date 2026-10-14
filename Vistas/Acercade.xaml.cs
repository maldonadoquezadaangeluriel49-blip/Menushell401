using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Menushell401.Modelos;

namespace Menushell401.Vistas;

public partial class Acercade : ContentPage
{
    public ObservableCollection<AcercaItem> Items { get; set; } = new();

    private static string Url => $"{Config.BaseUrl}/acerca.php";
    private bool _cargando;

    public Acercade()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Cargar();
    }

    private async Task Cargar()
    {
        if (_cargando) return;
        _cargando = true;
        refresh.IsRefreshing = true;

        try
        {
            using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
            var datos = await http.GetFromJsonAsync<List<AcercaItem>>(Url,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Items.Clear();
            foreach (var d in datos ?? new List<AcercaItem>())
                Items.Add(d);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"{ex.Message}\n\nURL: {Url}", "OK");
        }
        finally
        {
            refresh.IsRefreshing = false;
            _cargando = false;
        }
    }

    private async void OnRefresh(object sender, EventArgs e)
    {
        await Cargar();
    }
}