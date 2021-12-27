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
    class PilotViewModel : ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;

        public Command MenuCommand { get; }
        public Command SortButtonHandle { get; }

        public PilotViewModel() { }
        public PilotViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.currentShell = curentShell;

            MenuCommand = new Command(() => currentShell.FlyoutIsPresented = !currentShell.FlyoutIsPresented);
            SortButtonHandle = new Command(openSortMenu);
            GetListFlightYouWorkOn();
        }

        void GetListFlightYouWorkOn()
        {
            Flights = new ObservableCollection<Flight>();
            if (DataManager.Ins.CurrentUser.rank == 0)
            {
                foreach (Flight i in DataManager.Ins.ListFlights)
                {
                    Flights.Add(i);
                }
            }
            else
            {
                List<Flight> temp1 = new List<Flight>();

                foreach (Flight ite in DataManager.Ins.ListFlights)
                {
                    temp1.Add(ite);
                }

                List<Flight> temp = new List<Flight>();
                List<Flight> result = new List<Flight>();


                //Lay email guider
                string yourEmail = DataManager.Ins.CurrentUser.email;


                //Loc email
                //temp = temp1.FindAll(e => e.tourGuide.Exists(p => p == yourEmail));
                foreach (var e in temp1)
                {
                    foreach (var p in e.ListPilots)
                    {
                        if (p.email == yourEmail)
                        {
                            temp.Add(e);
                            break;
                        }
                    }
                }

                foreach (var plc in temp)
                    if (!result.Contains(plc))
                        result.Add(plc);

                foreach (Flight ite3 in result)
                {
                    Flights.Add(ite3);
                }
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
        async void openSortMenu()
        {
            //DependencyService.Get<IToast>().ShortToast("Here is Open Sort");
            string action = await Application.Current.MainPage.DisplayActionSheet("Sort with: ", "Cancel", null, "Occur soon", "Occur lately");
            switch (action)
            {
                case "Occur soon":
                    OccurSoonSort();
                    break;
                case "Occur lately":
                    OccurLatelySort();
                    break;
            }
        }
        public void OccurSoonSort()
        {
            DateTime now = DateTime.Now;
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
                        DateTime dti = DateTime.Parse(Flights[i].StartDate);
                        DateTime dtj = DateTime.Parse(Flights[j].StartDate);
                        if (dtj < dti)
                        {
                            Flight temp = new Flight();
                            temp = Flights[i];
                            Flights[i] = Flights[j];
                            Flights[j] = temp;
                        }
                    }
                }
            }
        }
        public void OccurLatelySort()
        {
            DateTime now = DateTime.Now;
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
                        DateTime dti = DateTime.Parse(Flights[i].StartDate);
                        DateTime dtj = DateTime.Parse(Flights[j].StartDate);
                        if (dtj > dti)
                        {
                            Flight temp = new Flight();
                            temp = Flights[i];
                            Flights[i] = Flights[j];
                            Flights[j] = temp;
                        }
                    }
                }
            }
        }
    }
}
