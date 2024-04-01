using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class LoginViewMoedel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class LoginInstagramViewMoedel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
