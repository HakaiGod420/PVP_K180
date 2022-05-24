using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PVP_K180.Models
{
    public class Renginys
    {
        [DisplayName("Renginio ID")]
        public int id_Renginys { get; set; }
        [DisplayName("Pavadinimas")]
        public string pavadinimas { get; set; }
        [DisplayName("Aprašymas")]
        public string aprasymas { get; set; }
        [DisplayName("Reitingas")]
        public int reitingas { get; set; }
        [DisplayName("Renginio paskelbimo data")]
        [DisplayFormat(DataFormatString = "{0:yyyy-dd-MM HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime paskelbimo_data { get; set; }
        [DisplayName("Renginio pradžios data")]
        [DisplayFormat(DataFormatString = "{0:yyyy-dd-MM HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime pradzios_data { get; set; }
        [DisplayName("Renginio pabaigos data")]
        [DisplayFormat(DataFormatString = "{0:yyyy-dd-MM HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime pabaigos_data { get; set; }

        [DisplayName("Adresas")]
        [Required]
        public string adresas { get; set; }

        [DisplayName("Kaina")]
        [Required]
        public double kaina { get; set; }

        public string busenos_pavadinimas { get; set; }

        [DisplayName("Ilgumos koordinates")]
        public double zemelapis_ilguma { get; set; }
        [DisplayName("Plokštumos koordinates")]
        public double zemelapis_platuma { get; set; }
        [DisplayName("Būsena")]
        public int renginio_busena { get; set; }
        [DisplayName("Sukurta vartotojo")]
        public int vartotojas_id { get; set; }

        public List<Nuotrauka> gautosNuotraukos { get; set; }
    }
}