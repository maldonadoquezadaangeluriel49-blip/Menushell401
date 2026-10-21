namespace Menushell401
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Vistas.NoticiaDetalle), typeof(Vistas.NoticiaDetalle));
            Notificaciones.Iniciar();
        }
    }
}
