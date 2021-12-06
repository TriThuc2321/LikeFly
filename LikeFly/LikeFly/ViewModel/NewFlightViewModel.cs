using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            ObservableCollection<IntermediaryAirport> intermediary = new ObservableCollection<IntermediaryAirport>();
            intermediary.Add(new IntermediaryAirport(new Airport(), "2h30", "A03"));
            intermediary.Add(new IntermediaryAirport(new Airport(), "2h30", "A03"));

            List<string> ticketIds = new List<string>();
            ticketIds.Add("TT01");
            ticketIds.Add("TT02");
            ticketIds.Add("TT03");
            ticketIds.Add("TT04");


            Flight temp = new Flight("01", "HN-TSN", "5h", "8:30","30/12/2021","01","Hà Nội - TP Hồ Chí Minh",100,false,2000000,"A01","A02", intermediary, ticketIds);
            await DataManager.Ins.FlightService.AddFlight(temp);
        });

    }
}
