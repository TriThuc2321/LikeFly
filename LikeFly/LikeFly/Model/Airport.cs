using System;
using System.Collections.Generic;
using System.Text;

namespace LikeFly.Model
{
    public class Airport
    {
        public Airport()
        {
        }

        public string id { get; set; }
        public string name { get; set; }
        public string province { get; set; }
        public string imgSource { get; set; }

        public Airport(string id, string name, string province, string imgSource)
        {
            this.id = id;
            this.name = name;
            this.province = province;
            this.imgSource = imgSource;
        }
    }
}
