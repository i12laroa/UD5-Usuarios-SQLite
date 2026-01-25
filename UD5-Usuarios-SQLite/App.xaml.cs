using UD5_Usuarios_SQLite.Views;

namespace UD5_Usuarios_SQLite
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Simplemente crear el Shell, la SplashPage manejará la navegación
            return new Window(new AppShell());
        }
    }
}
