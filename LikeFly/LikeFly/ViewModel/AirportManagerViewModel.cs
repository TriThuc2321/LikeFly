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
    public class AirportManagerViewModel:ObservableObject
    {
        INavigation navigation;
        Shell currentShell;


        public AirportManagerViewModel() { }
        public AirportManagerViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
            InitList();
        }

        private void InitList()
        {
            ListAirport = DataManager.Ins.ListAirports;
        }
        public ICommand BackCommand => new Command<object>(async (obj) =>
        {
            //await currentShell.GoToAsync($"{nameof(NewAirportView)}");
            await navigation.PopAsync();
        });
        public ICommand BackCommand2 => new Command<object>(async (obj) =>
        {
            navigation.PushAsync(new NewAirportView());
        });

        

        private ObservableCollection<Airport> listAirport;
        public ObservableCollection<Airport> ListAirport
        {
            get { return listAirport; }
            set
            {
                listAirport = value;
                OnPropertyChanged("ListAirport");
            }
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
                navigation.PushAsync(new NewAirportView());
                SelectedAirport = null;
            }
        });
    }
}
