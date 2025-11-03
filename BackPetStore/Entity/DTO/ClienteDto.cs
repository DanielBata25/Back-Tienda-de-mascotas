using Entity.DTOs.Base;

namespace Entity.DTOs.Cliente
{
    public class ClienteDto : BaseDTO
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
    }
}
