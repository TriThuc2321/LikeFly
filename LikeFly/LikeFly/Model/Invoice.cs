using LikeFly.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeFly.Model
{
    public class Invoice: ObservableObject
    {
        public string id { get; set; }
        public Discount discount { get; set; }
        public string discountMoney { get; set; }
        public string price { get; set; }
        public bool isPaid;
        public bool IsPaid
        {
            get { return isPaid; }
            set
            {
                isPaid = value;
                OnPropertyChanged("IsPaid");
            }
        }
        public string payingTime { get; set; }
        public string amount { get; set; }
        public string method { get; set; }
        public string total { get; set; }
        public string photoMomo { get; set; }
        public string momoVnd { get; set; }
    }
}
