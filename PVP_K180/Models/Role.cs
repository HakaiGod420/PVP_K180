using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PVP_K180.Models
{
    public class Role
    {

        [DisplayName("Rolės ID")]
        public int id_Role { get; set; }
        [DisplayName("Rolės pavadinimas")]
        public string name { get; set; }
    }
}