﻿using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using LikeFly.View;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    public class SuccessBookViewModel: ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public Command GoToHome { get; }
        public SuccessBookViewModel() { }
        public SuccessBookViewModel(INavigation navigation, Shell current)
        {
            this.navigation = navigation;
            this.currentShell = current;

            this.Ticket = DataManager.Ins.CurrentBookedTicket;
            if (DataManager.Ins.CurrentDiscount != null)
                this.Discount = DataManager.Ins.CurrentDiscount;
            this.Invoice = DataManager.Ins.CurrentInvoice;
            this.Flight = DataManager.Ins.CurrentFlight;

            GoToHome = new Command(goHome);

            //  UploadPhoto = new Command(upload);
            SetInformation();
        }

        void goHome(object obj)
        {
            currentShell.GoToAsync($"//{nameof(HomeView)}");
        }

        void SetInformation()
        {
            DurationProcess();

            GetDateTime();

            if (!this.Invoice.IsPaid)
            {
                // PayingVisible = false;
                DisplayUpload = true;
            }
            else
                DisplayUpload = false;

            checkTourStatus(Flight);

            if (Ticket != null && Ticket.IsCancel)
                Occured = Occured + " (Quý khách đã huỷ vé này)";

            if (Invoice.IsPaid)
                Paid = "Đã thanh toán";
            else
                Paid = "Chưa xác nhận thanh toán";

            if (Invoice.Method == "Banking")
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

        }

        void viewDetail(object obj)
        {

            navigation.PushAsync(new DetailTourView());
        }
        void cancelTicket(object obj)
        {
            navigation.PushAsync(new CancelFlightView());
        }
        //public void upload(object obj)
        //{
        //    navigation.PushAsync(new MoMoConfirmView());
        //}

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

        private string processedDuration;
        public string ProcessedDuration
        {
            get { return processedDuration; }
            set
            {
                processedDuration = value;
                OnPropertyChanged("ProcessedDuration");
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

        private bool displayUpload;
        public bool DisplayUpload
        {
            get { return displayUpload; }
            set
            {
                displayUpload = value;
                OnPropertyChanged("DisplayUpload");
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
        private void DurationProcess()
        {
            //if (DataManager.Ins.CurrentFlight.Duration == null) return;
            //string[] _ProcessedDuration = DataManager.Ins.CurrentFlight.Duration.Split('/');
            //string result = _ProcessedDuration[0] + " day(s) - " + _ProcessedDuration[1] + " night(s)";
            //ProcessedDuration = result;
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

        private string _strMoMoVND;
        public string StrMoMoVND
        {
            get { return _strMoMoVND; }
            set
            {
                _strMoMoVND = value;
                OnPropertyChanged("StrMoMoVND");
            }
        }

        private string _startTime;
        public string StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                OnPropertyChanged("StartTime");
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

            if (Invoice.Discount != null)
                StrDiscountMoney = service.FormatMoney(StrDiscountMoney);

            if (Invoice.Vnd != null)
            {
                StrMoMoVND = Invoice.Vnd;
                StrMoMoVND = service.FormatMoney(StrMoMoVND) + " VND";
            }

        }

        void GetDateTime()
        {
            StartTime = "";
            string[] date = Flight.StartDate.Split(' ');
            StartTime = date[0] + " "+ Flight.StartTime;
        }

        public void checkTourStatus(Flight flight)
        {
            CancelVisible = true;

            string[] tourStartTime = flight.StartDate.Split('/');

            string[] splitYear = tourStartTime[2].Split(' ');
            DateTime timeStart = new DateTime(
                int.Parse(splitYear[0]),
                int.Parse(tourStartTime[0]),
                int.Parse(tourStartTime[1])
                );

            string[] duration = flight.Duration.Split('/');


            DateTime currentTime = DateTime.Now.AddDays(0);
            TimeSpan interval = timeStart.Subtract(currentTime);

            string maxDuration = int.Parse(duration[0]) > int.Parse(duration[1]) ? duration[0] : duration[1];

            maxDuration = (int.Parse(maxDuration) * 24 * 60 * 60).ToString();

            // Thoi gian bat dau tour den current time
            double count = interval.Days * 60 * 60 * 24;
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
    }
}

