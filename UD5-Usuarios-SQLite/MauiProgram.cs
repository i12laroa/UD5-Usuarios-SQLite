using Microsoft.Extensions.Logging;
using UD5_Usuarios_SQLite.Services;
using UD5_Usuarios_SQLite.ViewModels;
using UD5_Usuarios_SQLite.Views;

namespace UD5_Usuarios_SQLite
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // ============================================
            // REGISTRO DE SERVICIOS - PATRÓN SINGLETON
            // ============================================
            // El servicio se registra como Singleton, lo que garantiza
            // una única instancia durante toda la vida de la aplicación.
            // Esto reemplaza el patrón Singleton manual con GetInstance().
            builder.Services.AddSingleton<IUsuarioServicio, UsuarioServicio>();

            // ============================================
            // REGISTRO DE VIEWMODELS
            // ============================================
            // Los ViewModels se registran como Transient para que cada
            // página tenga su propia instancia.
            builder.Services.AddTransient<MainViewModel>();

            // ============================================
            // REGISTRO DE PÁGINAS
            // ============================================
            // Las páginas se registran para permitir la inyección
            // de dependencias en sus constructores.
            builder.Services.AddTransient<MainPage>();

            // Registrar App para inyección de dependencias
            builder.Services.AddSingleton<App>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
