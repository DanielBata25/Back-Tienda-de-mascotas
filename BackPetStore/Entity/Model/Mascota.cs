using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Mascota", Schema = "Tienda")]
    public class Mascota : BaseModel
    {
        public string Nombre { get; set; }
        public string Especie { get; set; }
        public string Raza { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public string Notas { get; set; }

        // Relaciones
        public List<MascotaCliente> Clientes { get; set; } = new List<MascotaCliente>();
    }
}
