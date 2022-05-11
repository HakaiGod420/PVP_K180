using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PVP_K180.Models;

namespace PVP_K180.Models
{
    public class Atsitikimas
    {
        [DisplayName("Atsitikimo id")]
        public int id_Atstikimas { get; set; }

        [DisplayName("Atsitikimo sukurimo data")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime paskelbimo_data { get; set; }

        [DisplayName("Aprašymas")]
        public string aprasymas { get; set; }

        public double zemelapio_ilguma { get; set; }
        public double zemelapio_platuma { get; set; }

        [DisplayName("Komentaras")]
        public string komentaras { get; set; }

        [DisplayName("Atsitikimo tipas")]
        [Required(ErrorMessage = "Privalo būti pasirinktas tipas")]
        public int atsitikimo_tipas { get; set; }
        public int atsitikimo_busena { get; set; }

        public int? fk_Vartotojasid_Tvirtintojas { get; set; }
        public int fk_Vartotojasid_Pranesejas { get; set; }

        [DisplayName("Tvirtintojas")]
        public string tvirtintojas { get; set; }
        [DisplayName("Pranešėjas")]
        public string pranesejas { get; set; }

        [DisplayName("Tipas")]
        public string tipas { get; set; }

        [DisplayName("Būsena")]
        public string busena { get; set; }

        public IList<SelectListItem> atsitikimo_tipai { get; set; }

    }
}