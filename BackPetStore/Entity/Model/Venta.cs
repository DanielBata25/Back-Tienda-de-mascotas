using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Venta", Schema = "Tienda")]
    public class Venta : BaseModel
    {
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public DateTime FechaVenta { get; set; }
        public string Estado { get; set; }
        public string Canal { get; set; }
        public string Observaciones { get; set; }

        // Relaciones
        public List<Pago> Pagos { get; set; } = new List<Pago>();
        public List<VentaProducto> Productos { get; set; } = new List<VentaProducto>();
    }
}
