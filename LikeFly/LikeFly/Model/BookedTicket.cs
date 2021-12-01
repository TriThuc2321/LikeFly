using System;
using System.Collections.Generic;
using System.Text;

namespace LikeFly.Model
{
    public class BookedTicket
    {
        public string id { get; set; }
        public Flight flight { get; set; }
        public string name { get; set; }
        public string birthday { get; set; }
        public string contact { get; set; }
        public string email { get; set; }
        public string cmnd { get; set; }
        public string address { get; set; }
        public string bookTime { get; set; }
        public Invoice invoice { get; set; }
        public bool isCancel { get; set; }
    }
}
