using LikeFly.Core;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace LikeFly.Database
{
    public class DataManager : ObservableObject
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
        public List<Discount> discountsList;
        public List<BookedTicket> bookedTicketsList;
        public List<Invoice> invoicesList;

        private DataManager()
        {
            FlightService = new FlightServices();
            UsersServices = new UsersServices();
            AirportServices = new AirportServices();
            notiServices = new NotificationServices();
            TicketTypeService = new TicketTypeServices();
            Search = new SearchService();

            ListNotification = new ObservableCollection<Notification>();
            ListFlights = new ObservableCollection<Flight>();
            ListUsers = new ObservableCollection<User>();
            ListAirports = new ObservableCollection<Airport>();
            ListTicketTypes = new ObservableCollection<TicketType>();

            CurrentUser = new User();
            CurrentFlight = new Flight();
            ListDiscount = new ObservableCollection<Discount>();
            DiscountsServices = new DiscountsService();
            ListBookedTickets = new ObservableCollection<BookedTicket>();
            BookedTicketsServices = new BookedTicketServices();
            ListInvoice = new ObservableCollection<Invoice>();
            InvoicesServices = new InvoicesService();

            CurrentDiscount = new Discount();
            CurrentInvoice = new Invoice();
            CurrentBookedTicket = new BookedTicket();
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
                if (p.Enable == true)
                ListAirports.Add(p);
            }
        }
        async Task GetTicketTypes()
        {
            ticketTypes = await TicketTypeService.GetAllTickets();
            foreach (TicketType p in ticketTypes)
            {
                ListTicketTypes.Add(p);
            }
        }
        async Task GetFlights()
        {
            flights = await FlightService.GetAllFlights();
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
            await GetAllDiscounts();
            await GetAllInvoices();
            await GetAllBookedTicket();

            /*foreach (var f in ListFlights)
            {
                f.TicketTypes = new ObservableCollection<DetailTicketType>();
                f.TicketTypes.Add(new DetailTicketType(new TicketType("TT01", "Phổ thông", 1, true), 100, 100));
                f.TicketTypes.Add(new DetailTicketType(new TicketType("TT02", "Phổ thông đặc biệt", (float)1.2, true), 100, 100));
                f.TicketTypes.Add(new DetailTicketType(new TicketType("TT03", "Thương gia", (float)1.3, true), 100, 100));
                f.TicketTypes.Add(new DetailTicketType(new TicketType("TT04", "Hạng nhất", (float)1.5, true), 100, 100));

                await FlightService.UpdateFlight(f);
            }*/
            /*await UsersServices.addUser(new User("pl02@gmail.com", "e10adc3949ba59abbe56e057f20f883e", "Nguyễn Thị Công", "032111223", "22/07/1992", "23343783", "defaultUser.png", "Đồng Nai", 0, 1));
            await UsersServices.addUser(new User("pl03@gmail.com", "e10adc3949ba59abbe56e057f20f883e", "Trần Văn Công", "056666424", "21/02/1990", "223455323", "defaultUser.png", "An Giang", 0, 1));*/
        }
        public Airport GetAirportById(string id)
        {
            foreach(Airport a in airports)
            {
                if (a.Id == id) return a; 
            }
            return null;
        }       

        async Task GetAllDiscounts()
        {
            discountsList = await DiscountsServices.GetAllDiscounts();
            foreach (Discount discount in discountsList)
            {
                ListDiscount.Add(discount);
            }
        }

        async Task GetAllBookedTicket()
        {
            bookedTicketsList = await BookedTicketsServices.GetAllBookedTicket();
            foreach (BookedTicket booked in bookedTicketsList)
            {
                // booked.flight = tourList.Find(e => (e.id == booked.tour.id));
                //  booked.invoice = invoicesList.Find(e => (e.id == booked.invoice.id));
                ListBookedTickets.Add(booked);
            }
        }

        async Task GetAllInvoices()
        {
            invoicesList = await InvoicesServices.GetAllInvoice();
            foreach (Invoice invoice in invoicesList)
            {
                if (invoice.Discount != null)
                    invoice.Discount = discountsList.Find(e => (e.id == invoice.Discount.id));
                ListInvoice.Add(invoice);
            }
        }

        public TicketType GetTicketTypeById(string id)
        {
            foreach (TicketType a in ticketTypes)
            {
                if (a.Id == id) return a;
            }
            return null;
        }
            
        private SearchService search;
        public SearchService Search
        {
            get { return search; }
            set
            {
                search = value;
            }
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

        private ObservableCollection<Discount> discountList;
        public ObservableCollection<Discount> ListDiscount
        {
            get { return discountList; }
            set
            {
                discountList = value;
                OnPropertyChanged("ListDiscount");
            }
        }


        private DiscountsService discountServices;
        public DiscountsService DiscountsServices
        {
            get
            {
                return discountServices;
            }
            set { discountServices = value; }
        }

        private ObservableCollection<Invoice> invoiceList;
        public ObservableCollection<Invoice> ListInvoice
        {
            get { return invoiceList; }
            set
            {
                invoiceList = value;
                OnPropertyChanged("ListInvoice");
            }
        }


        private InvoicesService invoiceServices;
        public InvoicesService InvoicesServices
        {
            get
            {
                return invoiceServices;
            }
            set { invoiceServices = value; }
        }

        private BookedTicket currentBookedTicket;
        public BookedTicket CurrentBookedTicket
        {
            get { return currentBookedTicket; }
            set
            {
                currentBookedTicket = value;
                OnPropertyChanged("CurrentBookedTicket");
            }
        }

        private Invoice currentInvoice;
        public Invoice CurrentInvoice
        {
            get { return currentInvoice; }
            set
            {
                currentInvoice = value;
                OnPropertyChanged("CurrentInvoice");
            }
        }

        


        private ObservableCollection<BookedTicket> bookedTicketList;
        public ObservableCollection<BookedTicket> ListBookedTickets
        {
            get { return bookedTicketList; }
            set
            {
                bookedTicketList = value;
                OnPropertyChanged("ListBookedTickets");
            }
        }

        private ObservableCollection<FavouriteFlight> listFavouriteFlights;
        public ObservableCollection<FavouriteFlight> ListFavouriteFlights
        {
            get { return listFavouriteFlights; }
            set
            {
                listFavouriteFlights = value;
                OnPropertyChanged("ListFavouriteFlights");
            }
        }   
        private BookedTicketServices bookedTicketServices;
        public BookedTicketServices BookedTicketsServices
        {
            get
            {
                return bookedTicketServices;
            }
            set { bookedTicketServices = value; }
        }

        private Discount currentDiscount;
        public Discount CurrentDiscount
        {
            get { return currentDiscount; }
            set
            {
                currentDiscount = value;

            }
        }
        private TicketType currentTicketType;
        public TicketType CurrentTicketType
        {
            get { return currentTicketType; }
            set
            {
                currentTicketType = value;

            }
        }

        private DetailTicketType currentDetailTicketType;
        public DetailTicketType CurrentDetailTicketType
        {
            get { return currentDetailTicketType; }
            set
            {
                currentDetailTicketType = value;

            }
        }



    }
}
