using LikeFly.Core;
using LikeFly.Model;
using LikeFly.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

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
        public Shell currentShell;
        public INavigation navigation;
        public ICommand LogOutCommand => new Command<object>((obj) =>
        {
            currentShell.GoToAsync($"//{nameof(LoginView)}");
        });

        public bool LoadData = true;
        public List<User> users;
        public List<Airport> airports;
        public List<Flight> flights;
        public List<TicketType> ticketTypes;        
        public List<Discount> discountsList;
        public List<BookedTicket> bookedTicketsList;
        public List<Invoice> invoicesList;
        public List<Review> reviews;

        private DataManager()
        {
            FlightService = new FlightServices();
            UsersServices = new UsersServices();
            AirportServices = new AirportServices();
            notiServices = new NotificationServices();
            TicketTypeService = new TicketTypeServices();
            Search = new SearchService();
            ReviewServices = new ReviewServices();

            ListNotification = new ObservableCollection<Notification>();
            ListFlights = new ObservableCollection<Flight>();
            ListUsers = new ObservableCollection<User>();
            ListAirports = new ObservableCollection<Airport>();
            ListTicketTypes = new ObservableCollection<TicketType>();
            ListRule = new ObservableCollection<Rule>();

            CurrentUser = new User();
            CurrentFlight = new Flight();
            ListDiscount = new ObservableCollection<Discount>();
            DiscountsServices = new DiscountsService();
            ListBookedTickets = new ObservableCollection<BookedTicket>();
            BookedTicketsServices = new BookedTicketServices();
            ListInvoice = new ObservableCollection<Invoice>();
            InvoicesServices = new InvoicesService();
            RuleServices = new RuleServices();

            CurrentDiscount = new Discount();
            CurrentInvoice = new Invoice();
            CurrentBookedTicket = new BookedTicket();
            getAllList();

           
        }
        public List<string> GetDeductInformation(BookedTicket cancelledTicket)
        {
            List<string> result = new List<string>();
            string deductPercent = "100";

            string[] TourStartTime = DataManager.Ins.currentFlight.StartDate.Split('/');
            DateTime time = new DateTime(int.Parse(TourStartTime[2]), int.Parse(TourStartTime[1]), int.Parse(TourStartTime[0]));
            DateTime currrent_time = DateTime.Now.AddDays(0);
            TimeSpan interval = time.Subtract(currrent_time);
            double count = interval.Days;

            foreach (Rule ite in ListRule)
            {
                if (count <= int.Parse(ite.DayNum))
                {
                    deductPercent = ite.Deduct;
                    break;
                }
            }
            string amount = ((int.Parse(cancelledTicket.Invoice.Total) - ((int.Parse(cancelledTicket.Invoice.Total) * int.Parse(deductPercent)) / 100))).ToString();
            deductPercent = (100 - int.Parse(deductPercent)).ToString();

   //         string notificationBody = "Dear, " + cancelledTicket.Name + "\n" +
   //             "You have just canceled your tour ticket: '" + cancelledTicket.Flight.Name + "' departing on " + cancelledTicket.Flight.StartDate + " " + cancelledTicket.Flight.StartTime + ". According to our policy, you will receive a refund of "
   //             + deductPercent + " % of the bill paid. The amount you will be refunded is: " + amount + "$. Thank you for using the service.\n"
   //+ "Transactions will be made when you connect to our transaction office: 0383303061 - Vong Minh Huynh." + "\n---------------\n" + "For any questions and feedback, please contact the hotline: 0834344655 - Pham Vo Di Thien";


            string notificationBody = "Kính gửi khách hàng, " + cancelledTicket.Name + "\n" +
                "Quý khách vừa huỷ chuyến bay: '" + cancelledTicket.Flight.Name + "' khởi hành vào lúc " + cancelledTicket.Flight.StartDate + cancelledTicket.Flight.StartTime + ". Theo quy định của chúng tôi, quý khách sẽ được nhận hoàn tiền "
                + deductPercent + " % trên hoá đơn đã thanh toán. Số tiền quý khách được hoàn là: " + amount + "VND. Cảm ơn vì đã chọn LikeFly!\n"
   + "Để nhận lại khấu hao, xin hãy liên hệ với văn phòng của chúng tôi qua: 0383303061 (Nguyễn Khánh Linh)." + "\n---------------\n" + "Nếu có câu hỏi, xin liên hệ hotline: 0787960456";
            result.Add(notificationBody);
            result.Add(amount);

            return result;
        }

        private async Task SetupAsync(ObservableCollection<Flight> list)
        {
            DateTime current_time = DateTime.Now.AddDays(0);
            double count;
            foreach (Flight ite in list)
            {
                string[] temp = ite.StartDate.Split(' ');
                string[] TourStartTime = temp[0].Split('/');
                DateTime TourStartTime1 = new DateTime(int.Parse(TourStartTime[2]), int.Parse(TourStartTime[1]), int.Parse(TourStartTime[0]));
                TimeSpan interval = current_time.Subtract(TourStartTime1);
                count = interval.Days * 24 + interval.Hours + ((interval.Minutes * 100) / 60) * 0.01;
                if (count > 0)
                {
                    ite.IsOccured = true;
                    await DataManager.Ins.FlightService.UpdateFlight(ite);
                }
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

        async Task GetRule()
        {
            List<Rule> temp = await RuleServices.GetRule();
            foreach (Rule p in temp)
            {
                ruleList.Add(p);
            }
        }

        async Task getAllList()
        {            
            await GetUsers();
            await GetAirports();
            await GetTicketTypes();
            await GetFlights();
            await SetupAsync(ListFlights);
            await GetNotifications();
            await GetAllDiscounts();
            await GetAllInvoices();
            await GetAllBookedTicket();
            await GetRule();
            await GetReviews();
           
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
                 booked.Flight = flights.Find(e => (e.Id == booked.Flight.Id));
                 booked.Invoice = invoicesList.Find(e => (e.Id == booked.Invoice.Id));
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
        async Task GetReviews()
        {
            ListReview = new ObservableCollection<Review>();
            reviews = await ReviewServices.GetAllReviews();
            foreach (Review p in reviews)
            {
                ListReview.Add(p);
            }
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

        private RuleServices ruleServices;
        public RuleServices RuleServices
        {
            get
            {
                return ruleServices;
            }
            set { ruleServices = value; }
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
        private ObservableCollection<Rule> ruleList;
        public ObservableCollection<Rule> ListRule
        {
            get { return ruleList; }
            set
            {
                ruleList = value;
                OnPropertyChanged("ListRule");
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
                if (value.rank == 0 || value.rank == 1)
                {
                    IsAdmin = true;
                    IsPilot = false;
                    IsUser = false;
                }
                else if(value.rank == 2)
                {
                    IsAdmin = false;
                    IsPilot = true;
                    IsUser = false;
                }

                else if(value.rank == 3)
                {
                    IsAdmin = false;
                    IsPilot = false;
                    IsUser = true;
                }
                
                OnPropertyChanged("CurrentUser");
            }
        }

        private User currentUserManager;
        public User CurrentUserManager
        {
            get { return currentUserManager; }
            set
            {
                currentUserManager = value;               
                OnPropertyChanged("CurrentUserManager");          
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
       
        
        private bool isAdmin;
        public bool IsAdmin
        {
            get { return isAdmin; }
            set
            {
                isAdmin = value;
                OnPropertyChanged("IsAdmin");
            }
        }

        private bool isPilot;
        public bool IsPilot
        {
            get { return isPilot; }
            set
            {
                isPilot = value;
                OnPropertyChanged("IsPilot");
            }
        }

        private bool isUser;
        public bool IsUser
        {
            get { return isUser; }
            set
            {
                isUser = value;
                OnPropertyChanged("IsUser");
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

        public ReviewServices reviewServices;
        public ReviewServices ReviewServices
        {
            get { return reviewServices; }
            set
            {
                reviewServices = value;
                OnPropertyChanged("ReviewServices");
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


        private ObservableCollection<Review> listReview;
        public ObservableCollection<Review> ListReview
        {
            get { return listReview; }
            set
            {
                listReview = value;
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
