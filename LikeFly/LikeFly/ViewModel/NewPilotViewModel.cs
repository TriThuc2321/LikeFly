using LikeFly.Core;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    
    class NewPilotViewModel: ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;
        public NewPilotViewModel() { }
        public NewPilotViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.currentShell = curentShell;
        }

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

        private int selectedRank;
        public int SelectedRank
        {
            get { return selectedRank; }
            set
            {
                selectedRank = value;
                OnPropertyChanged("SelectedRank");
            }
        }
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }

        private string contact;
        public string Contact
        {
            get { return contact; }
            set
            {
                contact = value;
                OnPropertyChanged("Contact");
            }
        }
        private string address;
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }
        private string cmnd;
        public string CMND
        {
            get { return cmnd; }
            set
            {
                cmnd = value;
                OnPropertyChanged("CMND");
            }
        }
        private string birthday;
        public string Birthday
        {
            get { return birthday; }
            set
            {
                birthday = value;
                OnPropertyChanged("Birthday");
            }
        }
        private ImageSource profilePic;
        public ImageSource ProfilePic
        {
            get { return profilePic; }
            set
            {
                profilePic = value;
                OnPropertyChanged("ProfilePic");
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

                if (value == "Admin")
                {
                    CurrUser.rank = 0;
                }
                else if (value == "Management")
                {
                    CurrUser.rank = 1;
                }
                else if (value == "Tour Guide")
                {
                    CurrUser.rank = 2;
                }
                else if (value == "Customer")
                {
                    CurrUser.rank = 3;
                }
                OnPropertyChanged("SelectedType");
            }
        }
    }

    
}
