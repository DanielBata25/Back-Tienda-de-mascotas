using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    [Table("Login", Schema = "security")]
    class Login
    {
        public string Password { set; get; }
        public string Username { set; get; }
    }
}
