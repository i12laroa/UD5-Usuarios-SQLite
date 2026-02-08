using UD5_Usuarios_SQLite.Tests.Models;
using UD5_Usuarios_SQLite.Tests.Services;
using Xunit;

namespace UD5_Usuarios_SQLite.Tests
{
    /// <summary>
    /// Tests unitarios para el servicio de usuarios con SQLite. Proyecto con XUNIT
    /// Cada test usa una base de datos temporal que se elimina al finalizar.
    /// </summary>
    public class UsuarioServicioTests : IAsyncLifetime
    {
        private readonly string _dbPath;
        private readonly UsuarioServicio _servicio;

        public UsuarioServicioTests()
        {
            // Cada test usa un fichero de BD único para evitar conflictos
            _dbPath = Path.Combine(Path.GetTempPath(), $"test_usuarios_{Guid.NewGuid()}.db");
            _servicio = new UsuarioServicio(_dbPath);
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            await _servicio.CloseAsync();

            if (File.Exists(_dbPath))
                File.Delete(_dbPath);
        }

        // =============================================
        // TESTS DE INICIALIZACIÓN
        // =============================================

        [Fact]
        public async Task InitializeAsync_CreaLaTabla_SinErrores()
        {
            // Act
            await _servicio.InitializeAsync();

            // Assert - Si no lanza excepción, la tabla se creó correctamente
            var usuarios = await _servicio.ObtenerUsuariosAsync();
            Assert.NotNull(usuarios);
            Assert.Empty(usuarios);
        }

        // =============================================
        // TESTS DE GUARDAR (INSERT)
        // =============================================

        [Fact]
        public async Task GuardarUsuarioAsync_InsertaUnUsuario_Correctamente()
        {
            // Arrange
            await _servicio.InitializeAsync();
            var usuario = new Usuario
            {
                Nombre = "Alberto",
                Email = "alberto@test.com",
                Edad = 30,
                Ciudad = "Lucena"
            };

            // Act
            int resultado = await _servicio.GuardarUsuarioAsync(usuario);

            // Assert
            Assert.Equal(1, resultado); // 1 fila insertada
        }

        [Fact]
        public async Task GuardarUsuarioAsync_AsignaIdAutoIncrement()
        {
            // Arrange
            await _servicio.InitializeAsync();
            var usuario = new Usuario
            {
                Nombre = "María",
                Email = "maria@test.com",
                Edad = 25,
                Ciudad = "Cabra"
            };

            // Act
            await _servicio.GuardarUsuarioAsync(usuario);

            // Assert
            var usuarios = await _servicio.ObtenerUsuariosAsync();
            Assert.Single(usuarios);
            Assert.True(usuarios[0].Id > 0);
        }

        [Fact]
        public async Task GuardarUsuarioAsync_VariosUsuarios_IncrementaId()
        {
            // Arrange
            await _servicio.InitializeAsync();

            var usuario1 = new Usuario { Nombre = "User1", Email = "u1@test.com", Edad = 20, Ciudad = "Sevilla" };
            var usuario2 = new Usuario { Nombre = "User2", Email = "u2@test.com", Edad = 22, Ciudad = "Valencia" };

            // Act
            await _servicio.GuardarUsuarioAsync(usuario1);
            await _servicio.GuardarUsuarioAsync(usuario2);

            // Assert
            var usuarios = await _servicio.ObtenerUsuariosAsync();
            Assert.Equal(2, usuarios.Count);
            Assert.True(usuarios[1].Id > usuarios[0].Id);
        }

        // =============================================
        // TESTS DE OBTENER (SELECT)
        // =============================================

        [Fact]
        public async Task ObtenerUsuariosAsync_SinDatos_DevuelveListaVacia()
        {
            // Arrange
            await _servicio.InitializeAsync();

            // Act
            var usuarios = await _servicio.ObtenerUsuariosAsync();

            // Assert
            Assert.Empty(usuarios);
        }

        [Fact]
        public async Task ObtenerUsuariosAsync_ConDatos_DevuelveTodosLosUsuarios()
        {
            // Arrange
            await _servicio.InitializeAsync();
            await _servicio.GuardarUsuarioAsync(new Usuario { Nombre = "Ana", Email = "ana@test.com", Edad = 28, Ciudad = "Málaga" });
            await _servicio.GuardarUsuarioAsync(new Usuario { Nombre = "Luis", Email = "luis@test.com", Edad = 35, Ciudad = "Bilbao" });
            await _servicio.GuardarUsuarioAsync(new Usuario { Nombre = "Carmen", Email = "carmen@test.com", Edad = 40, Ciudad = "Zaragoza" });

            // Act
            var usuarios = await _servicio.ObtenerUsuariosAsync();

            // Assert
            Assert.Equal(3, usuarios.Count);
        }

        [Fact]
        public async Task ObtenerUsuariosAsync_VerificaDatosCorrectos()
        {
            // Arrange
            await _servicio.InitializeAsync();
            await _servicio.GuardarUsuarioAsync(new Usuario
            {
                Nombre = "Pedro",
                Email = "pedro@test.com",
                Edad = 33,
                Ciudad = "Granada"
            });

            // Act
            var usuarios = await _servicio.ObtenerUsuariosAsync();

            // Assert
            var pedro = usuarios[0];
            Assert.Equal("Pedro", pedro.Nombre);
            Assert.Equal("pedro@test.com", pedro.Email);
            Assert.Equal(33, pedro.Edad);
            Assert.Equal("Granada", pedro.Ciudad);
        }

        // =============================================
        // TESTS DE BORRAR (DELETE)
        // =============================================

        [Fact]
        public async Task BorrarUsuarioAsync_EliminaElUsuario()
        {
            // Arrange
            await _servicio.InitializeAsync();
            var usuario = new Usuario { Nombre = "Borrable", Email = "borrar@test.com", Edad = 50, Ciudad = "Cádiz" };
            await _servicio.GuardarUsuarioAsync(usuario);

            var usuarios = await _servicio.ObtenerUsuariosAsync();
            var usuarioGuardado = usuarios[0];

            // Act
            int resultado = await _servicio.BorrarUsuarioAsync(usuarioGuardado);

            // Assert
            Assert.Equal(1, resultado); // 1 fila eliminada
            var usuariosDespues = await _servicio.ObtenerUsuariosAsync();
            Assert.Empty(usuariosDespues);
        }

        [Fact]
        public async Task BorrarUsuarioAsync_SoloBorraElIndicado()
        {
            // Arrange
            await _servicio.InitializeAsync();
            await _servicio.GuardarUsuarioAsync(new Usuario { Nombre = "Queda1", Email = "q1@test.com", Edad = 20, Ciudad = "A" });
            await _servicio.GuardarUsuarioAsync(new Usuario { Nombre = "SeBorra", Email = "sb@test.com", Edad = 25, Ciudad = "B" });
            await _servicio.GuardarUsuarioAsync(new Usuario { Nombre = "Queda2", Email = "q2@test.com", Edad = 30, Ciudad = "C" });

            var todos = await _servicio.ObtenerUsuariosAsync();
            var aBorrar = todos.First(u => u.Nombre == "SeBorra");

            // Act
            await _servicio.BorrarUsuarioAsync(aBorrar);

            // Assert
            var restantes = await _servicio.ObtenerUsuariosAsync();
            Assert.Equal(2, restantes.Count);
            Assert.DoesNotContain(restantes, u => u.Nombre == "SeBorra");
            Assert.Contains(restantes, u => u.Nombre == "Queda1");
            Assert.Contains(restantes, u => u.Nombre == "Queda2");
        }

        // =============================================
        // TESTS DE MODELO (validaciones del modelo)
        // =============================================

        [Fact]
        public void Usuario_PropiedadesPorDefecto()
        {
            // Act
            var usuario = new Usuario();

            // Assert
            Assert.Equal(0, usuario.Id);
            Assert.Null(usuario.Nombre);
            Assert.Null(usuario.Email);
            Assert.Equal(0, usuario.Edad);
            Assert.Null(usuario.Ciudad);
        }

        [Fact]
        public async Task GuardarUsuario_ConNombreNull_PermiteInsert()
        {
            // Arrange
            await _servicio.InitializeAsync();
            var usuario = new Usuario { Email = "sin_nombre@test.com", Edad = 20, Ciudad = "Test" };

            // Act
            int resultado = await _servicio.GuardarUsuarioAsync(usuario);

            // Assert
            Assert.Equal(1, resultado);
        }
    }
}
