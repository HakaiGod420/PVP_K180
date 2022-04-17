using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PVP_K180.Models
{
    public class Apklausa
    {
        [DisplayName("Apklausos id")]
        public int id_Apklausa { get; set; }

        [DisplayName("Apklausos aprašymas")]
        public string aprasymas { get; set; }

        [DisplayName("Dalyvių skaičius")]
        public int dalyviu_skaicius { get; set; }

        [DisplayName("Apklausos sukurimo data")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? sukurimo_data { get; set; }

        [DisplayName("Apklausos pabaigos data")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? pabaigos_data { get; set; }

        public int busena { get; set; }

        public int fk_Vartotojasid_Sukurejas { get; set; }

        [DisplayName("Apklausos kūrėjas")]
        public string Sukurejas { get; set; }

        [DisplayName("Būsena")]
        public string busenos_pavadinimas { get; set; }

        public List<Klausimas> klausimai { get; set; }
    }
}