using LikeFly.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeFly.Model
{
    public class Notification : ObservableObject
    {
        public string id { get; set; }
        public string isChecked;
        public string IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");

            }
        }
        public string isVisible;
        public string IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                OnPropertyChanged("IsVisible");

            }
        }
        public string reciever { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public DateTime when { get; set; }
        public string flightId { get; set; }
        public Notification() { }

        public Notification(string id, string isChecked, string isVisible, string reciever, string title, string body, DateTime when, string flightId)
        {
            this.id = id;
            IsChecked = isChecked;
            IsVisible = isVisible;
            this.reciever = reciever;
            this.title = title;
            this.body = body;
            this.when = when;
            this.flightId = flightId;
        }
    }
}
