using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using LikeFly.View;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    public class BookedTicketDetailViewModel : ObservableObject
    {
        INavigation navigation;
        Shell shell;
        public Command NavigationBack { get; }
        public BookedTicketDetailViewModel() { }
        public Command UploadPhoto { get; }
        public Command CancelTicket { get; }
        public Command ViewDetail { get; }
        public BookedTicketDetailViewModel(INavigation navigation, Shell shell)
        {
            this.shell = shell;
            this.navigation = navigation;
            // Refresh = new Command(RefreshView);

            CancelTicket = new Command(cancelTicket);
            ViewDetail = new Command(viewDetail);

            NavigationBack = new Command(() => navigation.PopAsync());
            //  UploadPhoto = new Command(upload);
            SetInformation();

        }

        void SetInformation()
        {
            PayingTimeVisible = true;

            this.Ticket = DataManager.Ins.CurrentBookedTicket;
            if (DataManager.Ins.CurrentDiscount != null)
                this.Discount = DataManager.Ins.CurrentDiscount;
            this.Invoice = DataManager.Ins.CurrentInvoice;
            this.Flight = DataManager.Ins.CurrentFlight;

            float provisional = Invoice.TicketTypes.Percent * int.Parse(Invoice.Price);
            StrProvisional = provisional.ToString();

            PayingVisible = Invoice.Method == "Banking" ? true : false;

            checkFlightStatus(Flight);

            if (Ticket != null && Ticket.IsCancel)
            {
                Occured = Occured + " (Đã huỷ chuyến bay này)";
                CancelVisible = false;
            }

            if (Invoice.IsPaid)
                Paid = "Yes";
            else
                Paid = "No";

            if (Invoice.Method == "MoMo")
                payingPhotoVisible = true;
            else payingPhotoVisible = false;

            if (Discount != null)
            {
                if (this.Discount.id != null)
                    DiscountVisible = true;
            }
            else
                DiscountVisible = false;

            FormatMoney();

            if (Invoice.PayingTime == "" || Invoice.PayingTime == null)
                PayingTimeVisible = false;

        }

        void viewDetail(object obj)
        {
            navigation.PushAsync(new DetailFlightView());
        }
        void cancelTicket(object obj)
        {
            navigation.PushAsync(new CancelFlightView());
        }


        private BookedTicket _ticket;
        public BookedTicket Ticket
        {
            get { return _ticket; }
            set
            {
                _ticket = value;
                OnPropertyChanged("Ticket");

            }
        }

        private Discount _discount;
        public Discount Discount
        {
            get { return _discount; }
            set
            {
                _discount = value;
                OnPropertyChanged("Discount");

            }
        }

        private Invoice _invoice;
        public Invoice Invoice
        {
            get { return _invoice; }
            set
            {
                _invoice = value;
                OnPropertyChanged("Invoice");

            }
        }

        private Flight _flight;
        public Flight Flight
        {
            get { return _flight; }
            set
            {
                _flight = value;
                OnPropertyChanged("Flight");

            }
        }




        private bool payingPhotoVisible;
        public bool PayingPhotoVisible
        {
            get { return payingPhotoVisible; }
            set
            {
                payingPhotoVisible = value;
                OnPropertyChanged("PayingPhotoVisible");
            }
        }

        private bool discountVisible;
        public bool DiscountVisible
        {
            get { return discountVisible; }
            set
            {
                discountVisible = value;
                OnPropertyChanged("DiscountVisible");
            }
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

        private string paid;
        public string Paid
        {
            get { return paid; }
            set
            {
                paid = value;
                OnPropertyChanged("Paid");
            }
        }

        private bool payingVisible;
        public bool PayingVisible
        {
            get { return payingVisible; }
            set
            {
                payingVisible = value;
                OnPropertyChanged("PayingVisible");
            }
        }

        private bool payingTimeVisible;
        public bool PayingTimeVisible
        {
            get { return payingTimeVisible; }
            set
            {
                payingTimeVisible = value;
                OnPropertyChanged("PayingTimeVisible");
            }
        }

        private bool cancelVisible;
        public bool CancelVisible
        {
            get { return cancelVisible; }
            set
            {
                cancelVisible = value;
                OnPropertyChanged("CancelVisible");
            }
        }


        private string _strBasePrice;
        public string StrBasePrice
        {
            get { return _strBasePrice; }
            set
            {
                _strBasePrice = value;
                OnPropertyChanged("StrBaserPrice");
            }
        }

        private string _strProvisional;
        public string StrProvisional
        {
            get { return _strProvisional; }
            set
            {
                _strProvisional = value;
                OnPropertyChanged("StrProvisional");
            }
        }

        private string _strDiscountMoney;
        public string StrDiscountMoney
        {
            get { return _strDiscountMoney; }
            set
            {
                _strDiscountMoney = value;
                OnPropertyChanged("StrDiscountMoney");
            }
        }

        private string _strTotal;
        public string StrTotal
        {
            get { return _strTotal; }
            set
            {
                _strTotal = value;
                OnPropertyChanged("StrTotal");
            }
        }


        void FormatMoney()
        {
            StrTotal = Invoice.Total;
            StrBasePrice = Invoice.Price;
            if (Invoice.Discount != null)
                StrDiscountMoney = Invoice.DiscountMoney;


            var service = DataManager.Ins.InvoicesServices;

            StrTotal = service.FormatMoney(StrTotal);
            StrBasePrice = service.FormatMoney(StrBasePrice);

            if (Invoice.Discount != null) { 
                StrDiscountMoney = service.FormatMoney(StrDiscountMoney);
            }

            StrProvisional = service.FormatMoney(StrProvisional);

        }

        public void checkFlightStatus(Flight flight)
        {
            CancelVisible = true;

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
            double count = interval.TotalSeconds;
            if (count > 0)
            {
                Occured = "Chưa bay";
                return;
            }
            if (count <= 0 && Math.Abs(count) <= int.Parse(maxDuration))
            {
                Occured = "Đang bay";
                CancelVisible = false;
                return;
            }

            Occured = "Đã bay";
            CancelVisible = false;
        }

        //    #region Refresh
        //    private bool _isRefresh;
        //    public bool IsRefresh
        //    {
        //        get
        //        {
        //            return _isRefresh;
        //        }

        //        set
        //        {
        //            _isRefresh = value;
        //            OnPropertyChanged("IsRefresh");
        //        }
        //    }

        //    public Command Refresh { get; }
        //    void RefreshView(object obj)
        //    {
        //        IsRefresh = true;
        //        SetInformation();
        //        IsRefresh = false;
        //    }
        //    #endregion
    }

}
