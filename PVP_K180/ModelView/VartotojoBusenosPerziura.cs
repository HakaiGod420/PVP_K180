using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PVP_K180.ModelView
{
    public class VartotojoBusenosPerziura
    {
        [DisplayName("Vartotojo būsena")]
        public IList<SelectListItem> Busenos { get; set; }

        public int? vartotojo_busena { get; set; }
    }
}