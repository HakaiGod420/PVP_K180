using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PVP_K180.ModelView
{
    public class Pasto_duomenys
    {
        public int id_vartotojas { get; set; }
        [DisplayName("El. Paštas")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Privalo būti įvestas el. paštas")]
        public string el_pastas { get; set; }
    }
}