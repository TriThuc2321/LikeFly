using LikeFly.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeFly.Model
{
    public class Airport : ObservableObject
    {
        public Airport()
        {
        }

        public Airport(string id, string name, string province, string imgSource, bool enable)
        {
            Id = id;
            Name = name;
            Province = province;
            ImgSource = imgSource;
            Enable = enable;
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
            set {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        private string province { get; set; }
        public string Province
        {
            get { return province; }
            set
            {
                province = value;
                OnPropertyChanged("Province");
            }
        }
        private string imgSource { get; set; }
        public string ImgSource
        {
            get { return imgSource; }
            set
            {
                imgSource = value;
                OnPropertyChanged("ImgSource");
            }
        }

        private bool enable { get; set; }
        public bool Enable
        {
            get { return enable; }
            set
            {
                enable = value;
                OnPropertyChanged("Enable");
            }
        }


        
    }
}
