using LikeFly.Core;
using LikeFly.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    class AirportViewModel : ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;

        public Command NotificaitonCommand { get; }

        public AirportViewModel() { }
        public AirportViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;           
        }
        public ICommand NewAirportCommand => new Command<object>(async (obj) =>
        {
            //await currentShell.GoToAsync($"{nameof(NewAirportView)}");
            await navigation.PushAsync(new NewAirportView());
        });
    }
}
