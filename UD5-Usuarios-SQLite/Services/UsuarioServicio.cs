using SQLite;
using UD5_Usuarios_SQLite.Models;

namespace UD5_Usuarios_SQLite.Services
{
    /// <summary>
    /// Servicio para gestionar las operaciones CRUD de usuarios con SQLite.
    /// El patrón Singleton se implementa a través del contenedor de inyección de dependencias
    /// en MauiProgram.cs mediante AddSingleton.
    /// </summary>
    public class UsuarioServicio : IUsuarioServicio
    {
        //Readonly solo nos permite asignar la conexión en la declaración o en el constructor.
        //fuera de esos dos casos no está permitido. Esto se hace para no asignar nuevas referencias (otro fichero de conexión).
        private readonly SQLiteAsyncConnection _conn;

        public UsuarioServicio()
        {
            // Ruta para la base de datos
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "usuarios.db");
            _conn = new SQLiteAsyncConnection(dbPath);
          //  _conn.CreateTableAsync<Usuario>().Wait();
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
    }
}
