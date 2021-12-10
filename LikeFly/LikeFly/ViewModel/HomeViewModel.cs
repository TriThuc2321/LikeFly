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
using System.Globalization;

namespace LikeFly.ViewModel
{
    class HomeViewModel: ObservableObject
    {       
        INavigation navigation;
        Shell currentShell;

        public Command MenuCommand { get; }
        public Command NotificaitonCommand { get; }
        public Command SearchHandler { get; }


        public HomeViewModel() { }
        public HomeViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
            MenuCommand = new Command(openMenu);
            NotificaitonCommand = new Command(openNotifi);
            SearchHandler = new Command(SearchHandle);

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
            navigation.PushAsync(new NotificationView());
        }

        private void SearchHandle()
        {
            if(String.IsNullOrWhiteSpace(FromPlaceText) || String.IsNullOrWhiteSpace(ToPlaceText))
            {
                DependencyService.Get<IToast>().ShortToast("Bạn muốn tìm chuyến bay nào?");
                return;
            }
            else
            {
                DataManager.Ins.Search.RefreshDataSearch();
                DataManager.Ins.Search.FromPlaceText = FromPlaceText;
                DataManager.Ins.Search.ToPlaceText = ToPlaceText;

                CultureInfo viVn = new CultureInfo("vi-VN");
                DataManager.Ins.Search.StartDate = StartDate.ToString("d", viVn);
                DataManager.Ins.Search.GetSearchResult();
                if (DataManager.Ins.Search.ResultList.Count > 0)
                {
                    navigation.PushAsync(new SearchResultView());
                    //DependencyService.Get<IToast>().ShortToast(DataManager.Ins.Search.ResultList.Count.ToString());
                }
                else
                {
                    DependencyService.Get<IToast>().ShortToast("Xin lỗi! Hiện tại chưa có chuyến bay bạn cần tìm");
                    return;
                }

            }
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

        private string fromPlaceText;
        public string FromPlaceText
        {
            get { return fromPlaceText; }
            set
            {
                fromPlaceText = value;
                OnPropertyChanged("FromPlaceText");
            }
        }
        private string toPlaceText;
        public string ToPlaceText
        {
            get { return toPlaceText; }
            set
            {
                toPlaceText = value;
                OnPropertyChanged("ToPlaceText");
            }
        }

        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
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
