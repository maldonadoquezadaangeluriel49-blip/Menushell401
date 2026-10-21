using Menushell401.Modelos;

namespace Menushell401.Vistas;

[QueryProperty(nameof(Noticia), "noticia")]
public partial class NoticiaDetalle : ContentPage
{
    Noticia? noticia;
    public Noticia? Noticia
    {
        get => noticia;
        set { noticia = value; BindingContext = value; }
    }

    public NoticiaDetalle()
    {
        InitializeComponent();
    }

    private async void btnVolver_Clicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}