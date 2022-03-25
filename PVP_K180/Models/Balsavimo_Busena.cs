using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PVP_K180.Models
{
    public class Balsavimo_Busena
    {
        [DisplayName("Balsavimo būsenos ID")]
        public int id_Balsavimo_busena { get; set; }

        [DisplayName("Balsavimo būsena")]
        public string name { get; set; }
    }
}