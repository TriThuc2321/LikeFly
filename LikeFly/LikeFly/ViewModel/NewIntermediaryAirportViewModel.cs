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
    class NewIntermediaryAirportViewModel : ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;

        public Command NotificaitonCommand { get; }

        public NewIntermediaryAirportViewModel() { }
        public NewIntermediaryAirportViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
            Init();
        }
        public ICommand SaveCommand => new Command<object>((obj) =>
        {
            if(StopHour == 0 && StopMinute == 0)
            {
                DependencyService.Get<IToast>().ShortToast("Vui lòng nhập thời gian dừng");
                return;
            }
            if(SelectedAirport == null)
            {
                DependencyService.Get<IToast>().ShortToast("Không có sân bay nào được chọn");
                return;
            }

            string StopTime = "";
            if (StopMinute == 0) StopTime = StopHour + "h";
            else StopTime = stopHour + "h" + StopMinute;

            DataManager.Ins.CurrentFlight.IntermediaryAirportList.Add(new IntermediaryAirport(SelectedAirport, StopTime));
            DependencyService.Get<IToast>().ShortToast("Thêm sân bay trung gian thành công");
            navigation.PopAsync();
        });
        public ICommand NavigationBack => new Command<object>((obj) =>
        {
            navigation.PopAsync();
        });

        void Init()
        {
            if (DataManager.Ins.CurrentFlight.IntermediaryAirportList == null) DataManager.Ins.CurrentFlight.IntermediaryAirportList = new ObservableCollection<IntermediaryAirport>();
            
            Airports = new ObservableCollection<Airport>();
            foreach (Airport a in DataManager.Ins.ListAirports)
            {
                if (a.Enable && !ExistAirport(a)) Airports.Add(a);
            }

            StopMinute = 0;
            StopHour = 0;
            if(Airports.Count>0)
                SelectedAirport = Airports[0];
        }
        bool ExistAirport(Airport airport)
        {
            foreach(IntermediaryAirport a in DataManager.Ins.CurrentFlight.IntermediaryAirportList)
            {
                if (airport == a.Airport) return true;
            }
            if (airport == DataManager.Ins.CurrentFlight.AirportStart || airport == DataManager.Ins.CurrentFlight.AirportEnd) return true;

            return false;
        }
        private int stopHour;
        public int StopHour
        {
            get { return stopHour; }
            set
            {
                stopHour = value;
                OnPropertyChanged("StopHour");
            }
        }
        private int stopMinute;
        public int StopMinute
        {
            get { return stopMinute; }
            set
            {
                stopMinute = value;
                OnPropertyChanged("StopMinute");
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
