using LikeFly.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeFly.Model
{
    public class Invoice: ObservableObject
    {
        private string id;
        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private string discountId;
        public string DiscountId
        {
            get { return discountId; }
            set
            {
                discountId = value;
                OnPropertyChanged("DiscountId");
            }
        }

        private Discount discount;
        public Discount Discount
        {
            get { return discount; }
            set
            {
                discount = value;
                OnPropertyChanged("Discount");
            }
        }

        private string discountMoney;
        public string DiscountMoney
        {
            get { return discountMoney; }
            set
            {
                discountMoney = value;
                OnPropertyChanged("DiscountMoney");
            }
        }

        private string price;
        public string Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }

        private bool isPaid;
        public bool IsPaid
        {
            get { return isPaid; }
            set
            {
                isPaid = value;
                OnPropertyChanged("IsPaid");
            }
        }
        private string payingTime;
        public string PayingTime
        {
            get { return payingTime; }
            set
            {
                payingTime = value;
                OnPropertyChanged("PayingTime");
            }
        }
        private string amount;
        public string Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                OnPropertyChanged("Amount");
            }
        }

        private string method;
        public string Method
        {
            get { return method; }
            set
            {
                method = value;
                OnPropertyChanged("Method");
            }
        }

        private string total;
        public string Total
        {
            get { return total; }
            set
            {
                total = value;
                OnPropertyChanged("Total");
            }
        }

        private string photo;
        public string Photo
        {
            get { return photo; }
            set
            {
                photo = value;
                OnPropertyChanged("Photo");
            }
        }

        private string vnd;
        public string Vnd
        {
            get { return vnd; }
            set
            {
                vnd = value;
                OnPropertyChanged("Vnd");
            }
        }

        private TicketType ticketTypes;
        public TicketType TicketTypes
        {
            get { return ticketTypes; }
            set
            {
                ticketTypes = value;
                OnPropertyChanged("TicketTypes");
            }
        }
    }
}
