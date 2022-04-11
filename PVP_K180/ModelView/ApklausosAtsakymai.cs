using PVP_K180.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVP_K180.ModelView
{
    public class ApklausosAtsakymai
    {
        public Apklausa apklausa { get; set; }
        public List<Klausimas> klausimai { get; set; }
        public List<string> atsakymai { get; set; }
        public int atsakavo_id { get; set; }
        public int apklausos_id { get; set; }
    }
}