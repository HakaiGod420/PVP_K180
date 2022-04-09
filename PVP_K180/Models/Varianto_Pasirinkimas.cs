using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVP_K180.Models
{
    public class Varianto_Pasirinkimas
    {
        public int fk_Vartotojasid_Vartotojas { get; set; }
        public int fk_Balsavimo_Variantasid_Balsavimo_Variantas { get; set; }
        public int fk_Balsavimo_Id {get;set;}
    }
}