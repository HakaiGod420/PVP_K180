using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PVP_K180.Models
{
    public class Komentaras
    {
        [DisplayName("Komentaro ID")]
        public int id_Komentaras { get; set; }
        [DisplayName("Komentaras")]
        [Required(ErrorMessage = "Privalo būti parašytas komentaras")]
        public string komentaro_tekstas { get; set; }
        [DisplayName("Parašymo data")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime parasymo_data { get; set; }
        [DisplayName("Prisikirtas id")]
        public int priskirtas_id { get; set; }
        [DisplayName("Parašašio id")]
        public int fk_Vartotojasid_Vartotojas { get; set; }
        [DisplayName("Vartotoju slapyvardis")]
        public string varotojo_slapyvardis { get; set;}
        public string nuotrauka_location { get; set; }
    }
}