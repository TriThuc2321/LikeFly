﻿using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using LikeFly.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace LikeFly.ViewModel
{
    class HomeViewModel: ObservableObject
    {       
        INavigation navigation;

        public Command MenuCommand { get; }
        public Command NotificaitonCommand { get; }
        public Command ToursCommand { get; }
        public Command FavoriteCommand { get; }
        public Command MyTourCommand { get; }

        public HomeViewModel() { }
        public HomeViewModel(INavigation navigation)
        {
            this.navigation = navigation;

            /*MenuCommand = new Command(openMenu);
            NotificaitonCommand = new Command(openNotifi);
            ToursCommand = new Command(openTours);
            FavoriteCommand = new Command(openFavorite);
            MyTourCommand = new Command(openMyTour);  */      

            Places = DataManager.Ins.ListPlace;
        }
        #region open view
        /*private void openMenu(object obj)
        {
            navigation.PushAsync(new MenuView());
        }
        private void openNotifi(object obj)
        {
            navigation.PushAsync(new NotificationView());
        }
        private void openTours(object obj)
        {
            navigation.PushAsync(new ToursView());
        }
        private void openFavorite(object obj)
        {
            navigation.PushAsync(new FavoriteView());
        }
        private void openMyTour(object obj)
        {
            navigation.PushAsync(new MyTourView());
        }*/
        #endregion
        

        private ObservableCollection<Place> _places;
        public ObservableCollection<Place> Places
        {
            get { return _places; }
            set
            {
                _places = value;
                OnPropertyChanged("Places");
            }
        }
    }
}
