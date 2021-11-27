using System;
using System.Collections.Generic;
using System.Text;

namespace LikeFly.Model
{
    public class Flight
    {
        public Flight()
        {
        }

        public Flight(string id, string country, string title, List<string> imgSource, string description)
        {
            this.id = id;
            this.country = country;
            this.title = title;
            this.imgSource = imgSource;
            this.description = description;
        }

        public string id { get; set; }
        public string country { get; set; }
        public string title { get; set; }
        public List<string> imgSource { get; set; }
        public string description { get; set; }

    }
}
