using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVP_K180.Models
{
    public class Nuotrauka
    {
        public int? id_Nuotrauka { get; set; }
        public string nuotraukos_nuoroda { get; set; }
        public int? priskirtas_id { get; set; }
    }
}