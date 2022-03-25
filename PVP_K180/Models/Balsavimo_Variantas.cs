using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PVP_K180.Models
{
    public class Balsavimo_Variantas
    {
        [DisplayName("Balsavimo varianto ID")]
        public int id_Balsavimo_Variantas { get; set; }

        [DisplayName("Balsavimo variantas")]
        [Required(ErrorMessage = "Privalo būti įvestas variantas")]
        public string balsavimo_variantas { get; set; }

        public int pasirinkusiu_skaicius { get; set; }
        public  int fk_Balsavimasid_Balsavimas {get; set;}
    }
}