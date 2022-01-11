using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    public class DetailNotificationViewModel : ObservableObject
    {
        INavigation navigation;
        public DetailNotificationViewModel() { }
        public Command NavigationBack { get; }
        public DetailNotificationViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            NavigationBack = new Command(() => navigation.PopAsync());

            SelectedNoti = DataManager.Ins.CurrentNoti;

            foreach (Notification ite in DataManager.Ins.ListNotification)
            {
                if (ite.id == SelectedNoti.id)
                {
                    ite.IsChecked = "True";
                    SelectedNoti.IsChecked = "True";
                    break;
                }
            }


        }

        private Notification selectedNoti;
        public Notification SelectedNoti
        {
            get { return selectedNoti; }
            set
            {
                selectedNoti = value;
                OnPropertyChanged("SelectedNoti");
            }
        }
    }
}
