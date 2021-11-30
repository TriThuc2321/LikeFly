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

namespace LikeFly.ViewModel
{
    class HomeViewModel: ObservableObject
    {       
        INavigation navigation;
        Shell currentShell;

        public Command MenuCommand { get; }

        public HomeViewModel() { }
        public HomeViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
            MenuCommand = new Command(openMenu);

            ProfilePic = DataManager.Ins.CurrentUser.profilePic;
        }
        private void openMenu(object obj)
        {
            currentShell.FlyoutIsPresented = !currentShell.FlyoutIsPresented;
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
    }
}
