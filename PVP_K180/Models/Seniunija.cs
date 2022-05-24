using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PVP_K180.Models
{
    public class Seniunija
    {

        [DisplayName("Seniunijos ID")]
        public int id_Seniunyja { get; set; }


        [DisplayName("Seniunijos pavadinimas")]
        [Required(ErrorMessage = "Privalo būti įvestas pavadinimas")]
        public string seniunyjos_pavadinimas { get; set; }

        [DisplayName("Seniunijos aprašymas")]
        [AllowHtml]
        [Required(ErrorMessage = "Privalo būti įvestas pavadinimas")]
        public string aprasymas { get; set; }

        [DisplayName("Žemėlapio ilguma")]
        public float? zemelapis_ilguma { get; set; }

        [DisplayName("Žemėlapio platuma")]
        public float? zemelapis_platuma { get; set; }

        [DisplayName("Seniunijos el. paštas")]
        [Required(ErrorMessage = "Privalo būti įvestas pavadinimas")]
        public string el_pastas { get; set; }

        [DisplayName("Seniunijos tel. nr.")]
        [Required(ErrorMessage = "Privalo būti įvestas pavadinimas")]
        public string tel_nr { get; set; }

        [DisplayName("Seniunijos adresas")]
        [Required(ErrorMessage = "Privalo būti įvestas pavadinimas")]
        public string adresas { get; set; }
    }
}