using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PVP_K180.ModelView
{
    public class RenginioBusenaPerziura
    {
        [DisplayName("Renginio būsena")]
        public IEnumerable<SelectListItem> Busenos { get; set; }

        [DisplayName("Renginio būsena")]
        public int renginio_Busena { get; set; }
    }
}