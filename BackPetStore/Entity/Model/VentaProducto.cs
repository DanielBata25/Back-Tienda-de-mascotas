using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("VentaProducto", Schema = "Tienda")]
    public class VentaProducto : BaseModel
    {
        public int VentaId { get; set; }
        public int ProductoId { get; set; }

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public Venta Venta { get; set; }
        public Producto Producto { get; set; }
    }
}
