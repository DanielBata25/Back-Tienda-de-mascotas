using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Producto", Schema = "Tienda")]
    public class Producto : BaseModel
    {
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string Descripcion { get; set; }
        public int Stock { get; set; }
        public string Estado { get; set; }

        // Relaciones
        public List<VentaProducto> Ventas { get; set; } = new List<VentaProducto>();
    }
}
