using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Cliente", Schema = "Tienda")]
    public class Cliente : BaseModel
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }

        public string Password { get; set; }

        public List<Venta> Ventas { get; set; } = new List<Venta>();
        public List<MascotaCliente> Mascotas { get; set; } = new List<MascotaCliente>();
    }
}
