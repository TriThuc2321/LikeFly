using LikeFly.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeFly.Model
{
    public class BookedTicket: ObservableObject
    {

        public BookedTicket(){}

        public BookedTicket(string id, string flightId, Flight flight, string name, string birthday, string contact, string email, string cmnd, string address, string bookTime, Invoice invoice, bool isCancel)
        {
            Id = id;
            FlightId = flightId;
            Flight = flight;
            Name = name;
            Birthday = birthday;
            Contact = contact;
            Email = email;
            Cmnd = cmnd;
            Address = address;
            BookTime = bookTime;
            Invoice = invoice;
            IsCancel = isCancel;
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
        private string flightId { get; set; }
        public string FlightId
        {
            get { return flightId; }
            set
            {
                flightId = value;
                OnPropertyChanged("FlightId");
            }
        }
        private Flight flight;   
        public Flight Flight
        {
            get { return flight; }
            set
            {
                flight = value;
                OnPropertyChanged("Flight");
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

        private string birthday { get; set; }
        public string Birthday
        {
            get { return birthday; }
            set
            {
                birthday = value;
                OnPropertyChanged("Birthday");
            }
        }

        private string contact { get; set; }
        public string Contact
        {
            get { return contact; }
            set
            {
                contact = value;
                OnPropertyChanged("contact");
            }
        }

        private string email { get; set; }
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }

        private string cmnd { get; set; }
        public string Cmnd
        {
            get { return cmnd; }
            set
            {
                cmnd = value;
                OnPropertyChanged("Cmnd");
            }
        }

        private string address { get; set; }
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }

        private string bookTime { get; set; }
        public string BookTime
        {
            get { return bookTime; }
            set
            {
                bookTime = value;
                OnPropertyChanged("BookTime");
            }
        }

        private Invoice invoice { get; set; }
        public Invoice Invoice
        {
            get { return invoice; }
            set
            {
                invoice = value;
                OnPropertyChanged("Invoice");
            }
        }

        private bool isCancel { get; set; }
        public bool IsCancel
        {
            get { return isCancel; }
            set
            {
                isCancel = value;
                OnPropertyChanged("IsCancel");
            }
        }
    }
}
