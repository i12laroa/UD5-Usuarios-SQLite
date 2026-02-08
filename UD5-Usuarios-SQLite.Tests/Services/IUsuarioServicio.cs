using UD5_Usuarios_SQLite.Tests.Models;

namespace UD5_Usuarios_SQLite.Tests.Services
{
    public interface IUsuarioServicio
    {
        Task<List<Usuario>> ObtenerUsuariosAsync();
        Task<int> GuardarUsuarioAsync(Usuario user);
        Task<int> BorrarUsuarioAsync(Usuario user);
        Task InitializeAsync();
    }
}
