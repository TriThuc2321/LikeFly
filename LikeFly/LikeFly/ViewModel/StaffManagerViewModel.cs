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
    class StaffManagerViewModel : ObservableObject
    {
        INavigation navigation;
        Shell currentShell;


        public StaffManagerViewModel() { }
        public StaffManagerViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            Init();
        }
        public ICommand NewCommand => new Command<object>((obj) =>
        {
            DataManager.Ins.CurrentUserManager = null;
            navigation.PushAsync(new NewPilotView());
        });
        void Init()
        {
            ListCustomers = new List<User>();
            ListPilots = new List<User>();
            ListManagers = new List<User>();
            ListAdmins = new List<User>();

            foreach (User u in DataManager.Ins.users)
            {
                if (u.rank == 3) ListCustomers.Add(u);
                else if (u.rank == 2) ListPilots.Add(u);
                else if (u.rank == 1) ListManagers.Add(u);
                else ListAdmins.Add(u);
            }

            types = new ObservableCollection<string>();
            types.Add("All");
            types.Add("Admins");
            types.Add("Managers");
            types.Add("Pilots");
            types.Add("Customers");

            SelectedType = "All";

           
        }

        public ICommand NavigationBack => new Command<object>((obj) =>
        {
            navigation.PopAsync();
        });
        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            User result = obj as User;
            if (result != null)
            {
                DataManager.Ins.CurrentUserManager = result;
                navigation.PushAsync(new DetailStaffManagerView());
                SelectedUser = null;
            }
        });
        
        private User selectedUser;
        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }
        private ObservableCollection<User> allList;
        public ObservableCollection<User> AllList
        {
            get { return allList; }
            set
            {
                allList = value;
                OnPropertyChanged("AllList");
            }
        }
        private List<User> listCustomers;
        public List<User> ListCustomers
        {
            get { return listCustomers; }
            set
            {
                listCustomers = value;
                OnPropertyChanged("ListCustomers");
            }
        }
        private List<User> listPilots;
        public List<User> ListPilots
        {
            get { return listPilots; }
            set
            {
                listPilots = value;
                OnPropertyChanged("ListPilots");
            }
        }

        private List<User> listManagers;
        public List<User> ListManagers
        {
            get { return listManagers; }
            set
            {
                listManagers = value;
                OnPropertyChanged("ListManagers");
            }
        }

        private List<User> listAdmins;
        public List<User> ListAdmins
        {
            get { return listAdmins; }
            set
            {
                listAdmins = value;
                OnPropertyChanged("ListAdmins");
            }
        }

        private ObservableCollection<string> types;
        public ObservableCollection<string> Types
        {
            get { return types; }
            set
            {
                types = value;
                OnPropertyChanged("Types");
            }
        }

        private string selectedType;
        public string SelectedType
        {
            get { return selectedType; }
            set
            {
                selectedType = value;
                AllList = new ObservableCollection<User>();
                if (value == "All")
                {
                    foreach (var ite in DataManager.Ins.users)
                    {
                        AllList.Add(ite);
                    }
                }
                else if (value == "Admins")
                {
                    foreach (var ite in ListAdmins)
                    {
                        AllList.Add(ite);
                    }
                }
                else if (value == "Managers")
                {
                    foreach (var ite in ListManagers)
                    {
                        AllList.Add(ite);
                    }
                }
                else if (value == "Pilots")
                {
                    foreach (var ite in ListPilots)
                    {
                        AllList.Add(ite);
                    }
                }
                else if (value == "Customers")
                {
                    foreach (var ite in ListCustomers)
                    {
                        AllList.Add(ite);
                    }
                }


                OnPropertyChanged("SelectedType");
            }
        }

    }
}
