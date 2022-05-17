using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PVP_K180.Models
{
    public class Renginio_Busena
    {
        [DisplayName("Renginio būsenos ID")]
        public int id_Renginio_busena { get; set; }

        [DisplayName("Renginio būsena")]
        public string name { get; set; }
    }
}