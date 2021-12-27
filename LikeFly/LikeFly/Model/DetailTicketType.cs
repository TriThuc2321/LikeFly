using LikeFly.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeFly.Model
{
    public class DetailTicketType: ObservableObject
    {
        public DetailTicketType(TicketType ticketType, int remain, int total)
        {
            TicketType = ticketType;
            Remain = remain;
            Total = total;
        }

        public DetailTicketType()
        {
        }

        private TicketType ticketType;
        public TicketType TicketType
        {
            get { return ticketType; }
            set
            {
                ticketType = value;
                OnPropertyChanged("TicketType");
            }
        }
        private int remain;
        public int Remain
        {
            get { return remain; }
            set
            {
                remain = value;
                OnPropertyChanged("Remain");
            }
        }
        private int total;       

        public int Total
        {
            get { return total; }
            set
            {
                total = value;
                OnPropertyChanged("Total");
            }
        }
    }
}
