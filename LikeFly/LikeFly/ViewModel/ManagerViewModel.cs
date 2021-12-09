using LikeFly.Database;
using LikeFly.View;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    class ManagerViewModel
    {
        public INavigation navigation;
        public Shell currentShell;

        public Command AirportCommand { get; }
        public Command FlightCommand { get; }
        public Command TicketTypeCommand { get; }
        public Command MenuCommand { get; }
        public Command StaffCommand { get; }
        public Command RevenueCommand { get; }
        public Command DiscountCommand { get; }
        public Command RuleCommand { get; }

        public ManagerViewModel() { }
        public ManagerViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.currentShell = curentShell;

            MenuCommand = new Command(() => currentShell.FlyoutIsPresented = !currentShell.FlyoutIsPresented);

            AirportCommand = new Command(() => navigation.PushAsync(new AirportManagerView()));
            FlightCommand = new Command(() => navigation.PushAsync(new FlightManagerView()));
            DiscountCommand = new Command(()=> navigation.PushAsync(new DiscountManagerView()));
            TicketTypeCommand = new Command(()=> navigation.PushAsync(new TicketTypeManagerView()));
            //TicketTypeCommand = new Command(() =>  DataManager.Ins.TicketTypeService.AddTicket(new Model.TicketType("TT01", "Phổ thông", 1, true)));

            
            
        }


    }
}
