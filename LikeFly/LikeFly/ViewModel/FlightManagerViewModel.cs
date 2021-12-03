using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using LikeFly.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    class FlightManagerViewModel : ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;

        public FlightManagerViewModel() { }
        public FlightManagerViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
        }
        public ICommand NavigationBack => new Command<object>((obj) =>
        {
             navigation.PopAsync();
        });
        public ICommand NewFlightCommand => new Command<object>((obj) =>
        {
            DataManager.Ins.CurrentFlight = new Flight();
            navigation.PushAsync(new NewFlightView());
        });

    }
}
