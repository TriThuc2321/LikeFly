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
        public List<User> users;
        public List<Airport> airports;
        public List<Flight> flights;
        public List<TicketType> ticketTypes;

        private DataManager()
        {
            FlightService = new FlightServices();
            UsersServices = new UsersServices();
            AirportServices = new AirportServices();
            notiServices = new NotificationServices();
            TicketTypeService = new TicketTypeServices();

            ListNotification = new ObservableCollection<Notification>();
            ListFlights = new ObservableCollection<Flight>();
            ListUsers = new ObservableCollection<User>();
            ListAirports = new ObservableCollection<Airport>();
            ListTicketTypes = new ObservableCollection<TicketType>();

            CurrentUser = new User();
            CurrentFlight = new Flight();
            getAllList();
        }
        
        async Task GetUsers()
        {
            users = await UsersServices.GetAllUsers();
            foreach (User p in users)
            {
                ListUsers.Add(p);
            }
        }
        async Task GetAirports()
        {
            airports = await AirportServices.GetAllAirport();
            foreach (Airport p in airports)
            {
                ListAirports.Add(p);
            }
        }
        async Task GetTicketTypes()
        {
            int a = 7;
            ticketTypes = await TicketTypeService.GetAllTickets();
            foreach (TicketType p in ticketTypes)
            {
                ListTicketTypes.Add(p);
            }
        }
        async Task GetFlights()
        {
            flights = await FlightService.GetAllFlights();

            for(int i =0; i< flights.Count; i++)
            {
                flights[i].AirportStart = GetAirportById(flights[i].AirportStartId);
                flights[i].AirportEnd = GetAirportById(flights[i].AirportEndId);

                flights[i].TicketTypes = new ObservableCollection<TicketType>();
                for (int k = 0; k< flights[i].TicketTypeIds.Count; k++)
                {
                    flights[i].TicketTypes.Add(GetTicketTypeById(flights[i].TicketTypeIds[k]));
                }
               

                for (int k =0; k< flights[i].IntermediaryAirportList.Count; k++)
                {
                    flights[i].IntermediaryAirportList[k].Airport = GetAirportById(flights[i].IntermediaryAirportList[k].AirportId);
                }
                
            }

            foreach (Flight p in flights)
            {
                ListFlights.Add(p);
            }
        }
        
        async Task GetNotifications()
        {
            List<Notification> notifications = await NotiServices.GetAllNotification();
            foreach (Notification p in notifications)
            {
                ListNotification.Add(p);
            }
        }
        async Task getAllList()
        {            
            await GetUsers();
            await GetAirports();
            await GetTicketTypes();
            await GetFlights();
            await GetNotifications();
        }
        public Airport GetAirportById(string id)
        {
            foreach(Airport a in airports)
            {
                if (a.Id == id) return a; 
            }
            return null;
        }
        public TicketType GetTicketTypeById(string id)
        {
            foreach (TicketType a in ticketTypes)
            {
                if (a.Id == id) return a;
            }
            return null;
        }
       
        private NotificationServices notiServices;
        public NotificationServices NotiServices
        {
            get
            {
                return notiServices;
            }
            set { notiServices = value; }
        }
        private AirportServices airportServices;
        public AirportServices AirportServices
        {
            get
            {
                return airportServices;
            }
            set { airportServices = value; }
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
        private FlightServices flightService;
        public FlightServices FlightService
        {
            get
            {
                return flightService;
            }
            set { flightService = value; }
        }
        private TicketTypeServices ticketTypeService;
        public TicketTypeServices TicketTypeService
        {
            get
            {
                return ticketTypeService;
            }
            set { ticketTypeService = value; }
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
        private ObservableCollection<User> listUsers;
        public ObservableCollection<User> ListUsers
        {
            get { return listUsers; }
            set
            {
                listUsers = value;
            }
        }
        private ObservableCollection<Airport> listAirports;
        public ObservableCollection<Airport> ListAirports
        {
            get { return listAirports; }
            set
            {
                listAirports = value;
            }
        }
        private ObservableCollection<TicketType> listTicketTypes;
        public ObservableCollection<TicketType> ListTicketTypes
        {
            get { return listTicketTypes; }
            set
            {
                listTicketTypes = value;
            }
        }
        private ObservableCollection<Notification> _notifications;
        public ObservableCollection<Notification> ListNotification
        {
            get { return _notifications; }
            set
            {
                _notifications = value;
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
                OnPropertyChanged("CurrentUser");
            }
        }
        private Airport currentAirport;
        public Airport CurrentAirport
        {
            get { return currentAirport; }
            set
            {
                currentAirport = value;
                OnPropertyChanged("CurrentAirport");
            }
        }
        private Flight currentFlight;
        public Flight CurrentFlight
        {
            get { return currentFlight; }
            set
            {
                currentFlight = value;
                OnPropertyChanged("CurrentFlight");
            }
        }
        private Notification currentNoti;
        public Notification CurrentNoti
        {
            get { return currentNoti; }
            set
            {
                currentNoti = value;
                OnPropertyChanged("CurrentNoti");
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
