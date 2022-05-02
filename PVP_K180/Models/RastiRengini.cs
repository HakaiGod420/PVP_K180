using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace PVP_K180.Models
{
    public class RastiRengini
    {
        public DateTime paskelbimo_data { get; set; }
        public DateTime pabaigos_data { get; set; }
        public int renginio_busena { get; set; }
    }
}