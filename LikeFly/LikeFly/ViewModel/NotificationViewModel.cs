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
    public class NotificationViewModel : ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public Command NavigationBack { get; }
        public NotificationViewModel() { }
        public NotificationViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
            ListNotification = new ObservableCollection<Notification>();
            NavigationBack = new Command(() => currentShell.FlyoutIsPresented = !currentShell.FlyoutIsPresented);
            InitList();
        }

        private void InitList()
        {
            foreach (Notification ite in DataManager.Ins.ListNotification)
                if (DataManager.Ins.CurrentUser.email == ite.reciever && ite.IsVisible == "True")
                     ListNotification.Add(ite);
        }

        private ObservableCollection<Notification> listNotification;
        public ObservableCollection<Notification> ListNotification
        {
            get { return listNotification; }
            set
            {
                listNotification = value;
                OnPropertyChanged("ListNotification");
            }
        }
        public ICommand DeleteCommand => new Command<object>(async (obj) =>
        {
            Notification selected = (Notification)obj;
            if (selected == null) return;
            string deletedId = "";
            foreach (Notification ite in ListNotification)
            {
                if (ite.id == selected.id)
                {
                    deletedId = selected.id;
                    ite.IsVisible = "False";
                    await DataManager.Ins.NotiServices.UpdateNoti(selected);
                    ListNotification.Remove(selected);
                    break;
                }

            }
           
        });
        public ICommand SelectedCommand => new Command<object>(async (obj) =>
        {
            Notification selected = obj as Notification;
            if (selected != null)
            {
                if (selected.IsChecked == "False")
                {
                    selected.IsChecked = "True";
                    await DataManager.Ins.NotiServices.UpdateNoti(selected);
                }

                DataManager.Ins.CurrentNoti = selected;

                /*OnPropertyChanged("ListNotification");*/
                navigation.PushAsync(new DetailNotificationView());
                SelectedNoti = null;
            }
        });
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
