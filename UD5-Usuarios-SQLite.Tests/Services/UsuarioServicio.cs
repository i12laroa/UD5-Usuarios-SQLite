using SQLite;
using UD5_Usuarios_SQLite.Tests.Models;

namespace UD5_Usuarios_SQLite.Tests.Services
{
    /// <summary>
    /// Versión testable del servicio de usuarios.
    /// Acepta la ruta de la base de datos por parámetro para poder usarla en tests.
    /// </summary>
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly SQLiteAsyncConnection _conn;

        public UsuarioServicio(string dbPath)
        {
            _conn = new SQLiteAsyncConnection(dbPath);
        }

        public async Task InitializeAsync()
        {
            await _conn.CreateTableAsync<Usuario>();
        }

        public async Task<List<Usuario>> ObtenerUsuariosAsync()
        {
            return await _conn.Table<Usuario>().ToListAsync();
        }

        public async Task<int> GuardarUsuarioAsync(Usuario user)
        {
            return await _conn.InsertAsync(user);
        }

        public async Task<int> BorrarUsuarioAsync(Usuario user)
        {
            return await _conn.DeleteAsync(user);
        }

        public async Task CloseAsync()
        {
            await _conn.CloseAsync();
        }
    }
}
