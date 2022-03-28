using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PVP_K180.ModelView
{
    public class Nuotrauku_Duomenys
    {
        public int priskirtas_id { get; set; }
        [Display(Name = "Pasirinkti nuotraukas")]
        public HttpPostedFileBase[] nuotraukos { get; set; }
    }
}