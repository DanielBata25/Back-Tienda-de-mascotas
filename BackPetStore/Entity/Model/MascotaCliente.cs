using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("MascotaCliente", Schema = "Tienda")]
    public class MascotaCliente : BaseModel
    {
        public int MascotaId { get; set; }

        public int ClienteId { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        public Mascota Mascota { get; set; }
        public Cliente Cliente { get; set; }
    }
}
