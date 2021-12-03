using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    class NewFlightViewModel : ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;

        public Command NotificaitonCommand { get; }

        public NewFlightViewModel() { }
        public NewFlightViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
        }
        public ICommand NavigationBack => new Command<object>((obj) =>
        {
            navigation.PopAsync();
        });
        public ICommand SaveCommand => new Command<object>(async (obj) =>
        {
            Flight temp = new Flight("01", "01", "01", "01", "01", "01", "01", "01",false, "01");

            await DataManager.Ins.FlightsServices.AddFlight(temp);
        });

    }
}
