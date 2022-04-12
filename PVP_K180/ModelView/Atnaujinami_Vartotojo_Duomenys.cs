using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PVP_K180.ModelView
{
    public class Atnaujinami_Vartotojo_Duomenys
    {
        [DisplayName("Vartotojo ID")]
        public int id_Vartotojas { get; set; }

        [DisplayName("Gimimo data")]
        [DisplayFormat(DataFormatString = "{0:yyyy-dd-MM}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Privalo būti įvestas gimimo data")]
        public DateTime? gimimo_data { get; set; }

        [DisplayName("Vardas")]
        [Required(ErrorMessage = "Privalo būti įvestas vardas")]
        public string vardas { get; set; }
        [DisplayName("Pavardė")]
        [Required(ErrorMessage = "Privalo būti įvestas pavardė")]
        public string pavarde { get; set; }
        [DisplayName("Telefono numeris")]
        [Required(ErrorMessage = "Privalo būti įvestas telefono numeris")]
        public string tel_nr { get; set; }

    }
}