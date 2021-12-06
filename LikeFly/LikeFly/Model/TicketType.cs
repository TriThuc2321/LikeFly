using LikeFly.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeFly.Model
{
    public class TicketType : ObservableObject
    {
        public TicketType() { }

        public TicketType(string id, string name, float percent, bool isUsed)
        {
            Id = id;
            Name = name;
            Percent = percent;
            IsUsed = isUsed;
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
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        float percent;
        public float Percent
        {
            get { return percent; }
            set
            {
                percent = value;
                OnPropertyChanged("Percent");
            }
        }
        bool isUsed;
        public bool IsUsed
        {
            get { return isUsed; }
            set
            {
                isUsed = value;
                OnPropertyChanged("IsUsed");
            }
        }
    }
}
