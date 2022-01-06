using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    class DetailStaffManagerViewModel : ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;

        public DetailStaffManagerViewModel() { }
        public DetailStaffManagerViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.currentShell = curentShell;

            CurrUser = DataManager.Ins.CurrentUserManager;

            Status = new StatusObj();
            if (CurrUser.isEnable)
            {                
                Status.Text = "Hoạt động";
                Status.Color = "Green";
                Status.Icon = "active.png";
            }
            else
            {
                Status.Text = "Đã khóa";
                Status.Color = "DarkRed";
                Status.Icon = "blocked.png";
            }
        }
        public ICommand NavigationBack => new Command<object>((obj) =>
        {
            navigation.PopAsync();
        });
        public ICommand BlockedCommand => new Command<object>((obj) =>
        {
            if (!CurrUser.isEnable)
            {
                CurrUser.isEnable = true;
                Status.Text = "Hoạt động";
                Status.Color = "Green";
                Status.Icon = "active.png";
            }
            else
            {
                CurrUser.isEnable = false;
                Status.Text = "Đã khóa";
                Status.Color = "DarkRed";
                Status.Icon = "blocked.png";
            }
            
        });
        public ICommand SaveCommand => new Command<object>(async (obj) =>
        {
            await DataManager.Ins.UsersServices.UpdateUser(CurrUser);

            for(int i =0; i< DataManager.Ins.ListUsers.Count; i++)
            {
                if(DataManager.Ins.ListUsers[i].email == CurrUser.email)
                {
                    DataManager.Ins.ListUsers[i] = CurrUser;
                    DataManager.Ins.users[i] = CurrUser;
                }
            }
            await navigation.PopAsync();
        });
        private User currUser;
        public User CurrUser
        {
            get { return currUser; }
            set
            {
                currUser = value;
                OnPropertyChanged("CurrUser");
            }
        }
        private StatusObj status;
        public StatusObj Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }
        public class StatusObj: ObservableObject
        {
            public StatusObj() { }

            private string color;
            public string Color
            {
                get { return color; }
                set
                {
                    color = value;
                    OnPropertyChanged("Color");
                }
            }
            private string text;
            public string Text
            {
                get { return text; }
                set
                {
                    text = value;
                    OnPropertyChanged("Text");
                }
            }
            private string icon;
            public string Icon
            {
                get { return icon; }
                set
                {
                    icon = value;
                    OnPropertyChanged("Icon");
                }
            }

        }
    }
}
