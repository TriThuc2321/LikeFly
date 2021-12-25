using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    public class CancelFlightViewModel : ObservableObject
    {
        Shell currentShell;
        INavigation navigation;
        public CancelFlightViewModel() { }
        public Command NavigationBack { get; }
        public Command CancelTicket { get; }

        public CancelFlightViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            CancelTicket = new Command(cancelTicket);
            NavigationBack = new Command(() => navigation.PopAsync());

            SetInformation();

        }

        async void cancelTicket(object obj)
        {
            if (IsCheckRegulation)
            {
                BookedTicket booked = DataManager.Ins.CurrentBookedTicket;
                booked.IsCancel = true;
                DataManager.Ins.CurrentBookedTicket = booked;

                await DataManager.Ins.BookedTicketsServices.UpdateBookedTicket(booked);

                if (DataManager.Ins.CurrentDiscount != null)
                {
                    int isUsed = int.Parse(DataManager.Ins.CurrentDiscount.isUsed);
                    isUsed--;
                    DataManager.Ins.CurrentDiscount.isUsed = isUsed.ToString();

                    await DataManager.Ins.DiscountsServices.UpdateDiscount(DataManager.Ins.CurrentDiscount);

                }

                if (DataManager.Ins.CurrentFlight != null)
                {
                    int remaining = DataManager.Ins.CurrentDetailTicketType.Remain;
                    remaining = remaining + int.Parse(DataManager.Ins.CurrentInvoice.Amount);

                    foreach (var detailTicket in SelectedTicket.Flight.TicketTypes)
                    {
                        if (detailTicket.TicketType.Id == DataManager.Ins.CurrentDetailTicketType.TicketType.Id)
                        {
                            detailTicket.Remain = remaining;
                            break;
                        }
                    }


                    await DataManager.Ins.FlightService.UpdateFlight(DataManager.Ins.CurrentFlight);
                }

                await updateManager();
                DependencyService.Get<IToast>().LongToast("Huỷ vé thành công!");
                navigation.RemovePage(navigation.NavigationStack[navigation.NavigationStack.Count - 2]);
                await navigation.PopAsync();
                //await currentShell.GoToAsync($"//{nameof(HomeView)}");
            }
        }

        async Task updateManager()
        {
            if (DataManager.Ins.CurrentDiscount != null)
            {
                DataManager.Ins.CurrentBookedTicket.Invoice.Discount = DataManager.Ins.CurrentDiscount;

                for (int i = 0; i < DataManager.Ins.ListDiscount.Count - 1; i++)
                {
                    if (DataManager.Ins.ListDiscount[i].id == DataManager.Ins.CurrentDiscount.id)
                    {
                        DataManager.Ins.ListDiscount[i] = DataManager.Ins.CurrentDiscount;
                        break;
                    }
                }
            }


            for (int i = 0; i < DataManager.Ins.ListFlights.Count - 1; i++)
            {
                if (DataManager.Ins.ListFlights[i].Id == DataManager.Ins.CurrentFlight.Id)
                {
                    DataManager.Ins.ListFlights[i] = DataManager.Ins.CurrentFlight;
                    break;
                }
            }

            //string notiId = DataManager.Ins.GeneratePlaceId(10);
            //string noti = DataManager.Ins.GetDeductInformation(DataManager.Ins.CurrentBookedTicket)[0];

            //DataManager.Ins.NotiServices.ListMyNoti_System.Add(new Notification(notiId, "System", DataManager.Ins.CurrentUser.email, "False", 1, noti, DateTime.Now, "True", DataManager.Ins.currentTour.id, "Canceled Ticket: " + DataManager.Ins.currentTour.name));

            //await DataManager.Ins.NotiServices.SendNoti(
            //    DataManager.Ins.GeneratePlaceId(10),
            //    "System",
            //    DataManager.Ins.CurrentUser.email,
            //    1,
            //    noti,
            //    "Canceled Ticket: " + DataManager.Ins.currentTour.name,
            //    DataManager.Ins.currentTour.id
            //    );

        }

        void SetInformation()
        {
            SelectedTicket = DataManager.Ins.CurrentBookedTicket;
            IsCheckRegulation = false;
            Deduct = "";
        }

        private string _regulation;
        public string Regulation
        {
            get { return _regulation; }
            set
            {
                _regulation = value;
                OnPropertyChanged("Regulation");
            }
        }

        private bool _isCheckRegulation;
        public bool IsCheckRegulation
        {
            get { return _isCheckRegulation; }
            set
            {
                _isCheckRegulation = value;
                OnPropertyChanged("IsCheckRegulation");
            }
        }

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

        private string deduct;
        public string Deduct
        {
            get { return deduct; }
            set
            {
                deduct = value;
                OnPropertyChanged("Deduct");
            }
        }
    }
}
