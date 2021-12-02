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

        private DataManager()
        {
            FlightsServices = new FlightServices();
            UsersServices = new UsersServices();
            AirportServices = new AirportServices();
            notiServices = new NotificationServices();

            ListNotification = new ObservableCollection<Notification>();
            ListFlights = new ObservableCollection<Flight>();
            ListUsers = new ObservableCollection<User>();
            ListAirports = new ObservableCollection<Airport>();

            CurrentUser = new User();

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

        #region get all func
        async Task getFlightList()
        {
            List<Flight> temp = await FlightsServices.GetAllFlights();
            foreach (Flight p in temp)
            {
                ListFlights.Add(p);
            }
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
            await getFlightList();       
            await GetNotifications();
            await GetAllDiscounts();
            await GetAllInvoices();
            await GetAllBookedTicket();
             await GetAirports();
        }

        async Task GetAllDiscounts() {
            List<Discount> discountsList = await DiscountsServices.GetAllDiscounts();
            foreach (Discount discount in discountsList)
            {
                ListDiscount.Add(discount);
            }
        }

        async Task GetAllBookedTicket()
        {
            List<BookedTicket> bookedTicketsList = await BookedTicketsServices.GetAllBookedTicket();
            foreach (BookedTicket booked in bookedTicketsList)
            {
               // booked.flight = tourList.Find(e => (e.id == booked.tour.id));
              //  booked.invoice = invoicesList.Find(e => (e.id == booked.invoice.id));
                ListBookedTickets.Add(booked);

            }
        }

        async Task GetAllInvoices()
        {
            List<Discount> discountsList = await DiscountsServices.GetAllDiscounts();
            List<Invoice> invoicesList = await InvoicesServices.GetAllInvoice();
            foreach (Invoice invoice in invoicesList)
            {
                if (invoice.discount != null)
                    invoice.discount = discountsList.Find(e => (e.id == invoice.discount.id));
                ListInvoice.Add(invoice);
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

    }
}
