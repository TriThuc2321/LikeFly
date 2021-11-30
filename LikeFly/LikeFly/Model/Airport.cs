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
        public string title { get; set; }
        public List<string> imgSource { get; set; }

        public Airport(string id, string name, string title, List<string> imgSource)
        {
            this.id = id;
            this.name = name;
            this.title = title;
            this.imgSource = imgSource;
        }
    }
}
