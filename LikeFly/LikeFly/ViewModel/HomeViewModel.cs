using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using LikeFly.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LikeFly.ViewModel
{
    class HomeViewModel: ObservableObject
    {       
        INavigation navigation;
        Shell currentShell;

        public Command MenuCommand { get; }
        public Command NotificaitonCommand { get; }

        public HomeViewModel() { }
        public HomeViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
            MenuCommand = new Command(openMenu);
            NotificaitonCommand = new Command(openNotifi);

            ProfilePic = DataManager.Ins.CurrentUser.profilePic;
            Airports = DataManager.Ins.ListAirports;
        }
        private Airport selectedAirport;
        public Airport SelectedAirport
        {
            get { return selectedAirport; }
            set
            {
                selectedAirport = value;
                OnPropertyChanged("SelectedAirport");
            }
        }

        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            Airport result = obj as Airport;
            if (result != null)
            {
                DataManager.Ins.CurrentAirport = result;
                navigation.PushAsync(new FlightView());
                SelectedAirport = null;
            }
        });
        private void openMenu(object obj)
        {
            currentShell.FlyoutIsPresented = !currentShell.FlyoutIsPresented;
        }
        private void openNotifi(object obj)
        {
            //navigation.PushAsync(new NotificationView());
        }

        private string profilePic;
        public string ProfilePic
        {
            get { return profilePic; }
            set
            {
                profilePic = value;
                OnPropertyChanged("ProfilePic");
            }
        }
        private ObservableCollection<Airport> airports;
        public ObservableCollection<Airport> Airports
        {
            get { return airports; }
            set
            {
                airports = value;
                OnPropertyChanged("Airports");
            }
        }
    }
}
