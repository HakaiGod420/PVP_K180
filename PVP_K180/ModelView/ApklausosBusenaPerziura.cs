using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PVP_K180.ModelView
{
    public class ApklausosBusenaPerziura
    {
        [DisplayName("Apklausos būsena")]
        public IEnumerable<SelectListItem> Busenos { get; set; }

        public int apklausos_Busena { get; set; }
    }
}