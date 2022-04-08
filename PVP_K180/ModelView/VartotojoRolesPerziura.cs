using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PVP_K180.ModelView
{
    public class VartotojoRolesPerziura
    {
        [DisplayName("Vartotojo role")]
        public IList<SelectListItem> role { get; set; }

        public int vartotojo_role { get; set; }
    }
}