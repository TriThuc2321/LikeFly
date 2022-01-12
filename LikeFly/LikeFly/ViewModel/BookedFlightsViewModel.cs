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
    public class BookedFlightsViewModel: ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public Command MenuCommand { get; }
        public BookedFlightsViewModel() {}

        public BookedFlightsViewModel(INavigation navigation, Shell current)
        {
            this.navigation = navigation;
            this.currentShell = current;
            //Refresh = new Command(RefreshView);
            MenuCommand = new Command(openMenu);

            BookedTicketsList = new ObservableCollection<BookedTicket>();

            foreach (var tk in DataManager.Ins.ListBookedTickets)
            {
                if (tk.Email == DataManager.Ins.CurrentUser.email)
                    BookedTicketsList.Add(tk);
            }

            DataManager.Ins.CurrentDiscount = null;

            SortingTicket();

           /// checkFlightStatus(SelectedTicket.Flight);

            //if (SelectedTicket != null && SelectedTicket.IsCancel)
            //    Occured = Occured + " (Quý khách đã huỷ vé này)";

        }

        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            BookedTicket result = obj as BookedTicket;
            if (result != null)
            {
                DataManager.Ins.CurrentBookedTicket = result;
                DataManager.Ins.CurrentInvoice = result.Invoice;

                if (result.Invoice.Discount != null && result.Invoice.Discount.id != null && result.Invoice.Discount.id !="")
                    DataManager.Ins.CurrentDiscount = result.Invoice.Discount;
                DataManager.Ins.CurrentFlight = result.Flight;
                DataManager.Ins.CurrentTicketType = result.Invoice.TicketTypes;


                navigation.PushAsync(new BookedTicketDetailView());

                SelectedTicket = null;

            }
        });

        private BookedTicket selectedTicket;
        public BookedTicket SelectedTicket
        {
            get { return selectedTicket; }
            set
            {
                selectedTicket = value;
                OnPropertyChanged("SelectedTicket");

            }
        }

        private string payment;
        public string Payment
        {
            get { return payment; }
            set
            {
                payment = value;
                OnPropertyChanged("Payment");

            }
        }

        private ObservableCollection<BookedTicket> _bookedTicketsList;
        public ObservableCollection<BookedTicket> BookedTicketsList
        {
            get { return _bookedTicketsList; }
            set
            {
                _bookedTicketsList = value;
                OnPropertyChanged("BookedTicketsList");
            }
        }

        private void openMenu(object obj)
        {
            currentShell.FlyoutIsPresented = !currentShell.FlyoutIsPresented;
        }
        void SortingTicket()
        {
            // Xep giam dan
            for (int i = 0; i < BookedTicketsList.Count - 1; i++)
            {
                for (int j = i + 1; j < BookedTicketsList.Count; j++)
                {
                    string datetimeI = BookedTicketsList[i].BookTime;
                    string datetimeJ = BookedTicketsList[j].BookTime;

                    CultureInfo viVn = new CultureInfo("vi-VN");

                    DateTime dtI = DateTime.ParseExact(datetimeI, "dd/MM/yyyy hh:mm:ss tt", viVn);
                    DateTime dtJ = DateTime.ParseExact(datetimeJ, "dd/MM/yyyy hh:mm:ss tt", viVn);

                    if (dtI < dtJ)
                    {
                        BookedTicket tmp = new BookedTicket();
                        tmp = BookedTicketsList[i];
                        BookedTicketsList[i] = BookedTicketsList[j];
                        BookedTicketsList[j] = tmp;
                    }
                }
            }
        }

        public void checkFlightStatus(Flight flight)
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

            string maxDuration = duration[1] == null ?
                (int.Parse(duration[0]) * 60 * 60).ToString() :
                (int.Parse(duration[0]) * 60 * 60 + int.Parse(duration[1]) * 60).ToString();

            // Thoi gian bat dau flight den current time
            double count = interval.TotalSeconds;
            if (count > 0)
            {
                Occured = "Chưa bay";
                return;
            }
            if (count <= 0 && Math.Abs(count) <= int.Parse(maxDuration))
            {
                Occured = "Đang bay";
                return;
            }

            Occured = "Đã bay";
        }

        private string occured;
        public string Occured
        {
            get { return occured; }
            set
            {
                occured = value;
                OnPropertyChanged("Occured");
            }
        }

        //#region Refresh
        //private bool _isRefresh;
        //public bool IsRefresh
        //{
        //    get
        //    {
        //        return _isRefresh;
        //    }

        //    set
        //    {
        //        _isRefresh = value;
        //        OnPropertyChanged("IsRefresh");
        //    }
        //}

        //public Command Refresh { get; }
        //void RefreshView(object obj)
        //{
        //    IsRefresh = true;
        //    BookedTicketsList.Clear();
        //    BookedTicketsList = new ObservableCollection<BookedTicket>();

        //    foreach (var tk in DataManager.Ins.ListBookedTickets)
        //    {
        //        if (tk.email == DataManager.Ins.CurrentUser.email)
        //            BookedTicketsList.Add(tk);
        //    }

        //    SortingTicket();
        //    IsRefresh = false;
        //}
        //#endregion
    }

}
