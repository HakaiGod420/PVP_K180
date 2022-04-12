using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PVP_K180.Models;

namespace PVP_K180.ModelView
{
    public class BalsavimoBusenaPerziura
    {
        [DisplayName("Balsavimo būsena")]
        public IList<SelectListItem> Busenos { get; set; }

        public int balsavimo_Busena { get; set; }
    }
}