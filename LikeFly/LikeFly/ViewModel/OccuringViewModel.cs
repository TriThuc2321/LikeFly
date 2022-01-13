using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using LikeFly.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    public class OccuringViewModel: ObservableObject
    {
        Shell currentShell;
        INavigation navigation;
        public Command MenuCommand { get; }
        public OccuringViewModel() { }

        public OccuringViewModel(INavigation navigation, Shell shell)
        {
            this.currentShell = shell;
            this.navigation = navigation;
            TicketList = new ObservableCollection<BookedTicket>();
            MenuCommand = new Command(openMenu);
            SetInformation();
            SortingTicket();
        }

        public bool checkFlightStatus(Flight flight)
        {
            /// CancelVisible = true;

            string[] flightStartDate = flight.StartDate.Split('/');
            string[] duration = flight.Duration.Split('h');
            string[] flightStartTime = flight.StartTime.Split(':');
            DateTime timeStart = new DateTime(
                int.Parse(flightStartDate[2]),
                int.Parse(flightStartDate[1]),
                int.Parse(flightStartDate[0]),
                int.Parse(flightStartTime[0]),
                int.Parse(flightStartTime[1]),
                0
                );

            DateTime currentTime = DateTime.Now.AddDays(0);
            TimeSpan interval = timeStart.Subtract(currentTime);

            /// string maxDuration = int.Parse(duration[0]) > int.Parse(duration[1]) ? duration[0] : duration[1];

            string maxDuration = duration[1] == "" ?
                (int.Parse(duration[0]) * 60 * 60).ToString() :
                (int.Parse(duration[0]) * 60 * 60 + int.Parse(duration[1]) * 60).ToString();

            // Thoi gian bat dau flight den current time
            var count = interval.TotalSeconds;
            if (count > 0)
            {
                
                return false;
            }
            if (count <= 0 && Math.Abs(count) <= int.Parse(maxDuration))
            {
                return true;
            }

            return false;   
        }
        void SetInformation()
        {
            foreach (var ticket in DataManager.Ins.ListBookedTickets)
            {
                if (ticket.Email == DataManager.Ins.CurrentUser.email && checkFlightStatus(ticket.Flight) && !ticket.IsCancel)
                {
                    TicketList.Add(ticket);
                }
            }
        }

        private void openMenu(object obj)
        {
            currentShell.FlyoutIsPresented = !currentShell.FlyoutIsPresented;
        }
        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            BookedTicket result = obj as BookedTicket;
            if (result != null)
            {
                DataManager.Ins.CurrentBookedTicket = result;
                DataManager.Ins.CurrentFlight = result.Flight;
                DataManager.Ins.CurrentInvoice = result.Invoice;
                navigation.PushAsync(new DetailFlightView2());
                SelectedTicket = null;
            }
        });

        private ObservableCollection<BookedTicket> _ticketList;
        public ObservableCollection<BookedTicket> TicketList
        {
            get { return _ticketList; }
            set
            {
                _ticketList = value;
                OnPropertyChanged("TicketList");
            }
        }


        private BookedTicket selectedTicket;
        public BookedTicket SelectedTicket
        {
            get { return selectedTicket; }
            set
            {
                selectedTicket = value;
                OnPropertyChanged("SelectedTour");

            }
        }

        void SortingTicket()
        {
            // Xep giam dan
            for (int i = 0; i < TicketList.Count - 1; i++)
            {
                for (int j = i + 1; j < TicketList.Count; j++)
                {
                    string datetimeI = TicketList[i].BookTime;
                    string datetimeJ = TicketList[j].BookTime;

                    CultureInfo viVn = new CultureInfo("vi-VN");

                    DateTime dtI = DateTime.ParseExact(datetimeI, "dd/MM/yyyy hh:mm:ss tt", viVn);
                    DateTime dtJ = DateTime.ParseExact(datetimeJ, "dd/MM/yyyy hh:mm:ss tt", viVn);

                    if (dtI < dtJ)
                    {
                        BookedTicket tmp = new BookedTicket();
                        tmp = TicketList[i];
                        TicketList[i] = TicketList[j];
                        TicketList[j] = tmp;
                    }
                }
            }
        }
    }
}

