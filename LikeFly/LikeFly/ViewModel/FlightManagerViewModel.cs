using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using LikeFly.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Flights = DataManager.Ins.ListFlights;
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

        private ObservableCollection<Flight> flights;
        public ObservableCollection<Flight> Flights
        {
            get { return flights; }
            set
            {
                flights = value;
                OnPropertyChanged("Flights");
            }
        }
        private Flight selectedFlight;
        public Flight SelectedFlight
        {
            get { return selectedFlight; }
            set
            {
                selectedFlight = value;
                OnPropertyChanged("SelectedFlight");
            }
        }

        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            Flight result = obj as Flight;
            if (result != null)
            {
                DataManager.Ins.CurrentFlight = result;
                navigation.PushAsync(new DetailFlightView());
                SelectedFlight = null;
            }
        });
    }
}
