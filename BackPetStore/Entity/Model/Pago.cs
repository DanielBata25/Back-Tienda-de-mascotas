using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Pago", Schema = "Tienda")]
    public class Pago : BaseModel
    {
        [ForeignKey("Venta")]
        public int VentaId { get; set; }
        public Venta Venta { get; set; }

        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; }
        public string EstadoPago { get; set; }
        public string Referencia { get; set; }
    }
}
