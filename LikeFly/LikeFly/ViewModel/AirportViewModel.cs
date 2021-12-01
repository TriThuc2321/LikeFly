using LikeFly.Core;
using LikeFly.View;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    class AirportViewModel : ObservableObject
    {
        INavigation navigation;
        Shell currentShell;

        public Command NewAirportCommand { get; }
        public Command NotificaitonCommand { get; }

        public AirportViewModel() { }
        public AirportViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            NewAirportCommand = new Command(newAirportHandle);
        }

        private void newAirportHandle(object obj)
        {
            navigation.PushAsync(new NewAirportView());
        }
    }
}
