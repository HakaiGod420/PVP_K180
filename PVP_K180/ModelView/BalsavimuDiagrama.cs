using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PVP_K180.ModelView
{
    [DataContract]
    public class BalsavimuDiagrama
    {
        public BalsavimuDiagrama()
        {
            this.labels = new List<string>();
            this.colors = new List<string>();
            this.values = new List<int>();
        }
        public List<string> labels { get; set; }
        public List<int> values { get; set; }
        public List<string> colors { get; set; }
    }
}