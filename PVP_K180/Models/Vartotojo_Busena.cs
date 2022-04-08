using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PVP_K180.Models
{
    public class Vartotojo_Busena
    {
        [DisplayName("Vartotjo būsenos ID")]
        public int id_Vartotojo_Busena { get; set; }

        [DisplayName("Vartotojo būsena")]
        public string name { get; set; }
    }
}