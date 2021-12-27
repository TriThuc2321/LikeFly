using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using LikeFly.View;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    class SearchResultViewModel : ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;

        public Command BackCommand { get; }
        public Command SortButtonHandle { get; }

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
        public SearchResultViewModel() { }
        public SearchResultViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            BackCommand = new Command(() => navigation.PopAsync());
            SortButtonHandle = new Command(openSortMenu);
            Flights = DataManager.Ins.Search.ResultList;
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
        async void openSortMenu()
        {
            //DependencyService.Get<IToast>().ShortToast("Here is Open Sort");
            string action = await Application.Current.MainPage.DisplayActionSheet("Sort wwith: ", "Cancel", null, "Increasing price", "Decreasing price");
            switch (action)
            {
                case "Increasing price":
                    IncreasingPriceSort();
                    break;
                case "Decreasing price":
                    DecreasingPriceSort();
                    break;
            }
        }

        private void IncreasingPriceSort()
        {
            if (Flights.Count == 1)
            {
                return;
            }
            else
            {
                for (int i = 0; i < Flights.Count; i++)
                {
                    for (int j = i + 1; j < Flights.Count; j++)
                    {
                        if (Flights[j].Price < Flights[i].Price)
                        {
                            Flight temp = new Flight();
                            temp = Flights[i];
                            Flights[i] = Flights[j];
                            Flights[j] = temp;
                        }
                    }
                }
                return;
            }
        }
        private void DecreasingPriceSort()
        {
            if (Flights.Count == 1)
            {
                return;
            }
            else
            {
                for (int i = 0; i < Flights.Count; i++)
                {
                    for (int j = i + 1; j < Flights.Count; j++)
                    {
                        if (Flights[j].Price > Flights[i].Price)
                        {
                            Flight temp = new Flight();
                            temp = Flights[i];
                            Flights[i] = Flights[j];
                            Flights[j] = temp;
                        }
                    }
                }
                return;
            }
        }
       
    }
}
