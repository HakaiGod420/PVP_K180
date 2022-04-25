using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PVP_K180.Models
{
    public class Atsakymas
    {
        [DisplayName("Atsakymo ID")]
        public int id_Atsakymas { get; set; }
        [DisplayName("Atsakymas")]
        public string atsakymo_tekstas { get; set; }
        [DisplayName("Atsakovo ID")]
        public int fk_Vartotojasid_Varotojas { get; set; }
        [DisplayName("Klausimo ID")]
        public int fk_Klausimasid_Klausimas { get; set; }
        [DisplayName("Atsakovo slapyvardis)")]
        public string slapyvardis { get; set; }

    }
}