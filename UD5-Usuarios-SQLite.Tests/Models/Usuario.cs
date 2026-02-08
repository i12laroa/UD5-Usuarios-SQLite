using SQLite;

namespace UD5_Usuarios_SQLite.Tests.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100), Unique]
        public string? Nombre { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(2)]
        public int Edad { get; set; }

        [MaxLength(100)]
        public string? Ciudad { get; set; }
    }
}
