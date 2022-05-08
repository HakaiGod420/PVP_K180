using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVP_K180.ModelView
{
    public class ProjektuPerziura
    {
        public int id_Projektas { get; set; }
        public string pavadinimas { get; set; }
        public string aprasymas { get; set; }
        public string busena { get; set; }
        public int ivertinimas { get; set; }
        public int komenetarai { get; set; }
        public DateTime sukurimo_data { get; set; }
        public string menuo { get; set; }
        public int diena { get; set; }
        public string pradine_nuotrauka { get; set; }
    }
}