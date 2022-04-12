using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PVP_K180.ModelView
{
    public class NaujienuSarasas
    {
        public int naujienos_id { get; set; }
        [AllowHtml]
        public string trumpasAprasas { get; set; }
        public string naujienos_antraste { get; set; }
        public string pirma_nuotrauka { get; set; }
    }
}