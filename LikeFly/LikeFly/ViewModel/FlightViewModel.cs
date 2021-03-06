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
    class FlightViewModel : ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;

        public Command NotificaitonCommand { get; }
        public Command MenuCommand { get; }

        public FlightViewModel() { }
        public FlightViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            MenuCommand = new Command(() => currentShell.FlyoutIsPresented = !currentShell.FlyoutIsPresented);

            Flights = new ObservableCollection<Flight>();
            foreach (var f in DataManager.Ins.ListFlights)
            {
                if (!f.IsOccured)
                    Flights.Add(f);
            }    
        }

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
