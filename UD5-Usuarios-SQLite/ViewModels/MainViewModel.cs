using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UD5_Usuarios_SQLite.Models;
using UD5_Usuarios_SQLite.Services;

namespace UD5_Usuarios_SQLite.ViewModels
{
    /// <summary>
    /// ViewModel principal para la gestión de usuarios.
    /// Usa CommunityToolkit.Mvvm para simplificar el código MVVM.
    /// </summary>
    public partial class MainViewModel : ObservableObject
    {
        private readonly IUsuarioServicio _usuarioServicio;

        #region Propiedades de Binding

        [ObservableProperty]
        private string _nombre = string.Empty;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _edad = string.Empty;

        [ObservableProperty]
        private string _ciudad = string.Empty;

        [ObservableProperty]
        private bool _bbddHabilitada;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PuedeBorrar))]
        private Usuario? _usuarioSeleccionado;

        // Propiedad calculada para habilitar/deshabilitar botón borrar
        public bool PuedeBorrar => UsuarioSeleccionado != null;

        [ObservableProperty]
        private ObservableCollection<Usuario> _usuarios =new();


        public MainViewModel(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;

            // Cargar usuarios al iniciar
            _ = CargarUsuariosAsync();
        }

        [RelayCommand]
        private async Task AnadirUsuarioAsync()
        {
            if (string.IsNullOrWhiteSpace(Nombre) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Ciudad) ||
                !int.TryParse(Edad, out int edadInt))
            {
                await Application.Current!.MainPage!.DisplayAlertAsync("Error!!", "Rellena los campos correctamente", "Ok");
                return;
            }

            var usuario = new Usuario
            {
                Nombre = Nombre,
                Email = Email,
                Edad = edadInt,
                Ciudad = Ciudad
            };

            await _usuarioServicio.GuardarUsuarioAsync(usuario);

            // Limpiar campos
            Nombre = string.Empty;
            Email = string.Empty;
            Edad = string.Empty;
            Ciudad = string.Empty;

            // Recargar lista
            await CargarUsuariosAsync();
        }

        [RelayCommand]
        private async Task BorrarUsuarioAsync()
        {
            if (UsuarioSeleccionado != null)
            {
                await _usuarioServicio.BorrarUsuarioAsync(UsuarioSeleccionado);
                UsuarioSeleccionado = null;
                await CargarUsuariosAsync();
            }
        }

        [RelayCommand]
        private void Salir()
        {
            Application.Current?.Quit();
        }


        private async Task CargarUsuariosAsync()
        {
            var usuarios = await _usuarioServicio.ObtenerUsuariosAsync();
            Usuarios = new ObservableCollection<Usuario>(usuarios);
        }

    }
}