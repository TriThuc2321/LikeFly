using LikeFly.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeFly.Model
{
    public class Rule : ObservableObject
    {
        private string dayNum { get; set; }
        public string DayNum
        {
            get { return dayNum; }
            set
            {
                dayNum = value;
                OnPropertyChanged("DayNum");
            }
        }
        private string id { get; set; }
        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private string deduct { get; set; }
        public string Deduct
        {
            get { return deduct; }
            set
            {
                deduct = value;
                OnPropertyChanged("Deduct");
            }
        }
        public Rule()
        {
        }

        public Rule(string dayNum, string id, string deduct)
        {
            this.dayNum = dayNum;
            this.id = id;
            this.deduct = deduct;
        }
    }
}