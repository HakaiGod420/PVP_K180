using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PVP_K180.Models
{
    public class Nuotrauka
    {
        [DisplayName("Nuotraukos ID")]
        public int? id_Nuotrauka { get; set; }

        [DisplayName("Nuotraukos Nuoroda")]
        public string nuotraukos_nuoroda { get; set; }

        [DisplayName("Priskirtas ID")]
        public int? priskirtas_id { get; set; }
    }
}