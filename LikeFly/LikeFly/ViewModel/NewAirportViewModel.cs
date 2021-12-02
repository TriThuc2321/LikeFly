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
    class NewAirportViewModel : ObservableObject
    {
        INavigation navigation;
        Shell currentShell;

        public Command SaveCommand { get; }
        public Command NavigationBack { get; }

        public NewAirportViewModel() { }
        public NewAirportViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            SaveCommand = new Command(saveHandleAsync);
            NavigationBack = new Command(() => navigation.PopAsync());
        }

        private async void saveHandleAsync(object obj)
        {
            List<string> temp = new List<string>();
            temp.Add("11");
            temp.Add("12");
            Airport airport = new Airport("A01", "11", "11", temp);
            await DataManager.Ins.AirportServices.AddAirport(airport);
        }
    }
}
