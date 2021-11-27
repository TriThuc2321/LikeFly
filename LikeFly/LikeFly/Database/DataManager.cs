using LikeFly.Core;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace LikeFly.Database
{
    public class DataManager: ObservableObject
    {
        private static DataManager _ins;
        public static DataManager Ins
        {
            get
            {
                if (_ins == null) _ins = new DataManager();
                return _ins;
            }
            set { _ins = value; }
        }

        public bool LoadData = true;
        public List<User> usersTemp;
        private DataManager()
        {
            FlightsServices = new FlightServices();
            UsersServices = new UsersServices();

            ListFlights = new ObservableCollection<Flight>();
            ListUser = new ObservableCollection<User>();

            CurrentUser = new User();
            getAllList();
        }
        async Task getAllList()
        {
            getFlightList();
        }
        #region Get List Func
        async Task getFlightList()
        {
            List<Flight> temp = await FlightsServices.GetAllFlights();
            foreach (Flight p in temp)
            {
                ListFlights.Add(p);
            }
            
        }

        #endregion 

        private FlightServices flightsServices;
        public FlightServices FlightsServices
        {
            get
            {
                return flightsServices;
            }
            set { flightsServices = value; }
        }
        private UsersServices usersServices;
        public UsersServices UsersServices
        {
            get
            {
                return usersServices;
            }
            set { usersServices = value; }
        }
       
        private ObservableCollection<Flight> _flights;
        public ObservableCollection<Flight> ListFlights
        {
            get { return _flights; }
            set
            {
                _flights = value;
            }
        }
        private ObservableCollection<User> _users;
        public ObservableCollection<User> ListUser
        {
            get { return _users; }
            set
            {
                _users = value;
            }
        }
        private User currentUser;
        public User CurrentUser
        {
            get { return currentUser; }
            set
            {
                currentUser = value;
                ProfilePic = value.profilePic;
                CurrentName = value.name;
                if (value.rank == 0)
                {
                    IsManager = true;
                }
                else
                {
                    IsManager = false;
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
        private string currentName;
        public string CurrentName
        {
            get { return currentName; }
            set
            {
                currentName = value;
                OnPropertyChanged("CurrentName");
            }
        }
        private bool isManager;
        public bool IsManager
        {
            get { return isManager; }
            set
            {
                isManager = value;
                OnPropertyChanged("IsManager");
            }
        }
        private string USDcurrency;
        public string USDCurrency
        {
            get { return USDcurrency; }
            set
            {
                USDcurrency = value;
            }
        }
        private string verifyCode;
        public string VerifyCode
        {
            get { return verifyCode; }
            set
            {
                verifyCode = value;
            }
        }
    }
}
