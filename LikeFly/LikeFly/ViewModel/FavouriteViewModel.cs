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
    public class FavoriteViewModel : ObservableObject
    {
        //INavigation navigation;
        //Shell currentShell;
        //public Command MenuCommand { get; }

        //public FavoriteViewModel() { }
        //public FavoriteViewModel(INavigation navigation, Shell current)
        //{
        //    this.navigation = navigation;
        //    this.currentShell = current;
        //    MenuCommand = new Command(openMenu);

        //    Favourites = new ObservableCollection<FavouriteFlight>();
        //    foreach (var favourites in DataManager.Ins.ListFavouriteFlights)
        //    {
        //        if (favourites.email == DataManager.Ins.CurrentUser.email)
        //            Favourites.Add(favourites);
        //    }

        //    Refresh = new Command(RefreshFavourite);
        //}

        //private ObservableCollection<FavouriteFlight> _favourites;
        //public ObservableCollection<FavouriteFlight> Favourites
        //{
        //    get { return _favourites; }
        //    set
        //    {
        //        _favourites = value;
        //        OnPropertyChanged("Favourites");
        //    }
        //}


        //private Flight selectedFlight;
        //public Flight SelectedFlight
        //{
        //    get { return selectedFlight; }
        //    set
        //    {
        //        selectedFlight = value;
        //        OnPropertyChanged("SelectedFlight");

        //    }
        //}

        //public ICommand SelectedCommand => new Command<object>((obj) =>
        //{
        //    FavouriteFlight result = obj as FavouriteFlight;
        //    if (result != null)
        //    {
        //        DataManager.Ins.CurrentFlight = result.flight;
        //        navigation.PushAsync(new DetailTourView());
        //        SelectedFlight = null;
        //    }
        //});

        //public ICommand UnlovedCommand => new Command<object>((obj) =>
        //{
        //    FavouriteFlight result = obj as FavouriteFlight;

        //    if (result != null)
        //    {
        //        //var decision = page.DisplayAlert("Delete Tour",
        //        //    "Are you sure to delete this tour?",
        //        //    "Delete", "Cancel");

        //        //if (!decision.Result) return;

        //        int index = Favourites.IndexOf(result);

        //        //  Favourites[index].love = "love_white.png";

        //        DataManager.Ins.FavoritesServices.DeleteFavoriteFlight(result.id);
        //        DataManager.Ins.ListFavouriteFlights.Remove(result);
        //        Favourites.Remove(result);
        //    }
        //});

        //private void openMenu(object obj)
        //{
        //    currentShell.FlyoutIsPresented = !currentShell.FlyoutIsPresented;
        //}
        //#region Refresh
        //private bool _isRefresh;
        //public bool IsRefresh
        //{
        //    get
        //    {
        //        return _isRefresh;
        //    }

        //    set
        //    {
        //        _isRefresh = value;
        //        OnPropertyChanged("IsRefresh");
        //    }
        //}

        //public Command Refresh { get; }
        //void RefreshFavourite(object obj)
        //{
        //    IsRefresh = true;
        //    Favourites.Clear();
        //    Favourites = new ObservableCollection<FavouriteFlight>();
        //    foreach (var favourites in DataManager.Ins.ListFavouriteFlights)
        //    {
        //        if (favourites.email == DataManager.Ins.CurrentUser.email)
        //            Favourites.Add(favourites);
        //    }
        //    IsRefresh = false;
        //}
        //#endregion
    }
}
