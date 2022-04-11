﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PVP_K180.Models
{
    public class Klausimas
    {
        [DisplayName("Apklausos klausimo ID")]
        public int id_Klausimas { get; set; }

        [DisplayName("Klausimas")]
        [Required(ErrorMessage = "Privalo būti įvestas klausimas")]
        public string balsavimo_variantas { get; set; }

        public int fk_Balsavimasid_Balsavimas { get; set; }
    }
}