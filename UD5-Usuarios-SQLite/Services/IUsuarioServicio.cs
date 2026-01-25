using UD5_Usuarios_SQLite.Models;

namespace UD5_Usuarios_SQLite.Services
{
    /// <summary>
    /// Interfaz para el servicio de usuarios.
    /// Permite la inyección de dependencias y facilita el testing.
    /// </summary>
    public interface IUsuarioServicio
    {
        Task<List<Usuario>> ObtenerUsuariosAsync();
        Task<int> GuardarUsuarioAsync(Usuario user);
        Task<int> BorrarUsuarioAsync(Usuario user);
    }
}
