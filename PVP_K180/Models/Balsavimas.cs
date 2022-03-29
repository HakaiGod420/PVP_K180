using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PVP_K180.Models;

namespace PVP_K180.Models
{
    public class Balsavimas
    {
        [DisplayName("Balsavimo id")]
        public int id_Balsavimas { get; set; }

        [DisplayName("Balsavimo aprašymas")]
        public string balsavimo_aprasymas { get; set; }

        [DisplayName("Klausimas")]
        [Required(ErrorMessage = "Privalo būti įvestas klausimas")]
        public string klausimas { get; set; }
        [DisplayName("Dalyvių skaičius")]
        public int dalyviu_skaicius { get; set; }

        [DisplayName("Balsavimo sukurimo data")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? sukurimo_data { get; set; }

        [DisplayName("Balsavimo pabaigos data")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? pabaigos_data { get; set; }

        public int busena { get; set; }

        public int fk_Vartotojasid_Sukurejas { get; set; }

        [DisplayName("Balsavimo kūrėjas")]
        public string Sukurejas { get; set; }

        [DisplayName("Būsena")]
        public string busenos_pavadinimas { get; set; }

        public List<Balsavimo_Variantas> balsavimo_variantai { get;set;}
    }
}