using LikeFly.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeFly.Model
{
    public class Flight: ObservableObject
    {
        public Flight()
        {
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
        private string name { get; set; }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        private string duration { get; set; }
        public string Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                OnPropertyChanged("Duration");
            }
        }
        private string startTime { get; set; }
        public string StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                OnPropertyChanged("StartTime");
            }
        }
        private string startDate { get; set; }
        public string StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
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
        private string description { get; set; }
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }
        private string passengerNumber { get; set; }
        public string PassengerNumber
        {
            get { return passengerNumber; }
            set
            {
                passengerNumber = value;
                OnPropertyChanged("PassengerNumber");
            }
        }
        private bool isOccured { get; set; }
        public bool IsOccured
        {
            get { return isOccured; }
            set
            {
                isOccured = value;
                OnPropertyChanged("IsOccured");
            }
        }
        private string price { get; set; }
        public string Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }

        public Flight(string id, string name, string duration, string startTime, string startDate, string imgSource, string description, string passengerNumber, bool isOccured, string price)
        {
            this.id = id;
            this.name = name;
            this.duration = duration;
            this.startTime = startTime;
            this.startDate = startDate;
            this.imgSource = imgSource;
            this.description = description;
            this.passengerNumber = passengerNumber;
            this.isOccured = isOccured;
            this.price = price;
        }
    }
}
