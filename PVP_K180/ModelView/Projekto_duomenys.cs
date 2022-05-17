using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PVP_K180.Models;

namespace PVP_K180.ModelView
{
    public class Projekto_duomenys
    {
        public Projektas projektas { get; set; }
        [Display(Name = "Pasirinkti nuotraukas")]
        public HttpPostedFileBase[] nuotraukos { get; set; }
    }
}