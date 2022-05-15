using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PVP_K180.ModelView
{
    public class AtsitikimuKeitimasPerziura
    {
        [DisplayName("Atsitikimo id")]
        public int id_Atstikimas { get; set; }

        [DisplayName("Komentaras")]
        public string komentaras { get; set; }

        [DisplayName("Atsitikimo būsena")]
        public int atsitikimo_busena { get; set; }

        public int tvirtintojas { get; set; }

        public IList<SelectListItem> atsitikimo_busenos { get; set; }
    }
}