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
        public Command StayPlaceCommand { get; }
        public Command TourCommand { get; }
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

            AirportCommand = new Command(staffHandle);
            DiscountCommand = new Command(discountHandle);
            
        }
        private void staffHandle(object obj)
        {
            navigation.PushAsync(new AirportView());
        }

        void discountHandle(object obj)
        {
            navigation.PushAsync(new DiscountManagerView());
        }
    }
}
