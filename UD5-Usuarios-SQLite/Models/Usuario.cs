using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;


namespace UD5_Usuarios_SQLite.Models
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
