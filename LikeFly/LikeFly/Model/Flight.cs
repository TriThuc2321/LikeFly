using LikeFly.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private int passengerNumber { get; set; }
        public int PassengerNumber
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
        private int price { get; set; }
        public int Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }
        /*private string airportStartId;
        public string AirportStartId
        {
            get { return airportStartId; }
            set
            {
                airportStartId = value;
                OnPropertyChanged("AirportStartId");
            }
        }*/
        private Airport airportStart;
        public Airport AirportStart
        {
            get { return airportStart; }
            set
            {
                airportStart = value;
                OnPropertyChanged("AirportStart");
            }
        }
        /*private string airportEndId;
        public string AirportEndId
        {
            get { return airportEndId; }
            set
            {
                airportEndId = value;
                OnPropertyChanged("AirportEnd");
            }
        }*/
        private Airport airportEnd;
        public Airport AirportEnd
        {
            get { return airportEnd; }
            set
            {
                airportEnd = value;
                OnPropertyChanged("AirportEnd");
            }
        }

        private ObservableCollection<IntermediaryAirport> intermediaryAirportList;
        public ObservableCollection<IntermediaryAirport> IntermediaryAirportList
        {
            get { return intermediaryAirportList; }
            set
            {
                intermediaryAirportList = value;
                OnPropertyChanged("IntermediaryAirportList");
            }
        }


        /*private List<string> ticketTypeIds;
        public List<string> TicketTypeIds
        {
            get { return ticketTypeIds; }
            set
            {
                ticketTypeIds = value;
                OnPropertyChanged("TicketTypeIds");
            }
        }*/
        private ObservableCollection<DetailTicketType> ticketTypes;
        public ObservableCollection<DetailTicketType> TicketTypes
        {
            get { return ticketTypes; }
            set
            {
                ticketTypes = value;
                OnPropertyChanged("TicketTypes");
            }
        }

        public Flight(string id, string name, string duration, string startTime, string startDate, string imgSource, string description, int passengerNumber, bool isOccured, int price, Airport airportStart, Airport airportEnd, ObservableCollection<IntermediaryAirport> intermediaryAirportList, ObservableCollection<DetailTicketType> ticketTypes)
        {
            Id = id;
            Name = name;
            Duration = duration;
            StartTime = startTime;
            StartDate = startDate;
            ImgSource = imgSource;
            Description = description;
            PassengerNumber = passengerNumber;
            IsOccured = isOccured;
            Price = price;
            AirportStart = airportStart;
            AirportEnd = airportEnd;
            IntermediaryAirportList = intermediaryAirportList;
            TicketTypes = ticketTypes;
        }
    }
}
