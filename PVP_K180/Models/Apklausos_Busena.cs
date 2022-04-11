using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PVP_K180.Models
{
    public class Apklausos_Busena
    {
        [DisplayName("Apklausos būsenos ID")]
        public int id_Apklausos_busena { get; set; }

        [DisplayName("Apklausos būsena")]
        public string name { get; set; }
    }
}