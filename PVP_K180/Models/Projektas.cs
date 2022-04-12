using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PVP_K180.Models
{
    public class Projektas
    {
        [DisplayName("Projekto id")]
        public int id_Projektas { get; set; }

        [DisplayName("Pavadinimas")]
        [Required(ErrorMessage = "Privalo būti įvestas klausimas")]
        public string pavadinimas { get; set; }

        [DisplayName("Aprašymas")]
        [Required(ErrorMessage = "Privalo būti įvestas klausimas")]
        public string aprasymas { get; set; }

        [DisplayName("Būsena")]
        public int busena { get; set; }

        [DisplayName("Kūrėjo ID")]
        public int fk_Vartotojasid_Vartotojas { get; set; }
    }
}