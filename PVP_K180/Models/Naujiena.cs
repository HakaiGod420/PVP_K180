using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PVP_K180.Models
{
    public class Naujiena
    {
        [DisplayName("Naujienos ID")]
        public int id_Naujiena { get; set; }

        [DisplayName("Pavadinimas")]
        [Required(ErrorMessage = "Privalo būti įvestas pavadinimas")]
        public string pavadinimas { get; set; }

        [DisplayName("Naujienos tekstas")]
        [Required(ErrorMessage = "Privalo būti įvestas naujienos tekstas")]
        public string naujienos_tekstas { get; set; }

        [DisplayName("Naujienos sukurimo data")]
        [DisplayFormat(DataFormatString = "{0:yyyy-dd-MM}", ApplyFormatInEditMode = true)]
        public DateTime? naujienos_sukurimo_data { get; set; }

        [DisplayName("Sukurta vartotojo")]
        public int naujienos_kurejo_ID { get; set; }

        List<Nuotrauka> nuotraukos { get; set; }
    }
}