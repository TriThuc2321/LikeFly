using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    public class NewAirportViewModel :ObservableObject
    {
        INavigation navigation;

        public Command BackCommand { get; }

        public NewAirportViewModel() { }
        public NewAirportViewModel(INavigation _navigation)
        {
            this.navigation = _navigation;
            SelectedAirport = DataManager.Ins.CurrentAirport;
            BackCommand = new Command(() =>  navigation.PopAsync());
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

    }
}
