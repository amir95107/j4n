using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Requests
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} وارد کنید")]
        public string Name { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} وارد کنید")]
        public string Email { get; set; }

        [Display(Name = "موبایل")]
        [Required(ErrorMessage = "لطفا {0} وارد کنید")]
        public string Mobile { get; set; }

        [Display(Name = "موارد درخواستی")]
        [Required(ErrorMessage = "لطفا {0} وارد کنید")]
        public string ReuquestItems { get; set; }

        public DateTime ReuquestDate { get; set; }
    }
}
