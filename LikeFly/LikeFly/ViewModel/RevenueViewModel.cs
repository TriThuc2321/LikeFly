using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    public class RevenueViewModel : ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        Hashtable monthHashtable = new Hashtable();
        public Command SortByTotalPrice { get; }
        public Command SortByPercent { get; }
        public Command Filter { get; }
        public Command NavigationBack { get; }

        private readonly IMessageService _messageService;
        public RevenueViewModel() { }
        public RevenueViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
            SortByTotalPrice = new Command(SortListByTotal);
            SortByPercent = new Command(SortListByPercent);
            Filter = new Command(FilterList);
            NavigationBack = new Command(() => navigation.PopAsync());
            InitList();
        }

        private void FilterList(object obj)
        {
            if (selectedYear == null && SelectedMonth != null)
            {
                ResetListValue();
                SortByMonth();
            }
            else if (selectedYear != null && selectedMonth == null)
            {
                ResetListValue();
                SortByYear();
            }
            else if (selectedYear == null && selectedMonth == null)
            {
                return;
            }
            else
            {
                ResetListValue();
                SortByMonth();
                SortByYear();
            }
        }

        private void SortByMonth()
        {
            if (selectedMonth == "None") return;
            ObservableCollection<SupportRevenue> newResult = new ObservableCollection<SupportRevenue>();
            foreach (SupportRevenue ite in ListRevenue)
            {
                if (ite.Month == selectedMonth) newResult.Add(ite);
            }
            ListRevenue.Clear();
            ListRevenue = newResult;
        }

        private void SortByYear()
        {
            if (selectedYear == "Không") return;
            ObservableCollection<SupportRevenue> newResult = new ObservableCollection<SupportRevenue>();
            foreach (SupportRevenue ite in ListRevenue)
            {
                if (ite.Year == selectedYear) newResult.Add(ite);
            }
            ListRevenue.Clear();
            ListRevenue = newResult;
        }

        private void InitList()
        {
            //init hashtable and list month for picker
            monthHashtable.Add("1", "1");
            monthHashtable.Add("2", "2");
            monthHashtable.Add("3", "3");
            monthHashtable.Add("4", "4");
            monthHashtable.Add("5", "5");
            monthHashtable.Add("6", "6");
            monthHashtable.Add("7", "7");
            monthHashtable.Add("8", "8");
            monthHashtable.Add("9", "9");
            monthHashtable.Add("10", "10");
            monthHashtable.Add("11", "11");
            monthHashtable.Add("12", "12");
            monthHashtable.Add("Không", "None");
            listMonth = new List<string>();
            listMonth.Add("1");
            listMonth.Add("2");
            listMonth.Add("3");
            listMonth.Add("4");
            listMonth.Add("5");
            listMonth.Add("6");
            listMonth.Add("7");
            listMonth.Add("8");
            listMonth.Add("9");
            listMonth.Add("10");
            listMonth.Add("11");
            listMonth.Add("12");
            listMonth.Add("Không");

            /// init hashtable list year 
            ListYear = new List<string>();
            for (int i = 2020; i <= DateTime.Now.AddDays(0).Year; i++)
            {
                listYear.Add(i.ToString());
            }
            listYear.Add("Không");

            ListFlight = DataManager.Ins.ListFlights;
            ListRevenue = new ObservableCollection<SupportRevenue>();

            foreach (Flight ite in ListFlight)
            {
                if (ite.IsOccured == true)
                {
                    //string[] temp = ite.StartTime.Split('/');
                    string[] TourStartTime = ite.StartDate.Split('/');
                    SupportRevenue supportRevenue = new SupportRevenue(ite, TourStartTime[1], TourStartTime[2], "0", "0", "0", "White", "99");
                    ListRevenue.Add(supportRevenue);
                }
            }
            ListInvoice = new ObservableCollection<Invoice>();

            ListBookedSticket = new ObservableCollection<BookedTicket>();

            foreach (BookedTicket ite in DataManager.Ins.ListBookedTickets)
            {
                if (ite.Invoice.IsPaid == true)
                {
                    foreach (SupportRevenue ite2 in ListRevenue)
                    {
                        if (ite2.host.Id == ite.Flight.Id)
                        {
                            ite2.totalPrice = (int.Parse(ite2.totalPrice) + int.Parse(ite.Invoice.Total)).ToString();
                            ite2.totalticket = (int.Parse(ite2.totalticket) + int.Parse(ite.Invoice.Amount)).ToString();
                            //ite2.percent = ((int.Parse(ite2.totalticket) * 100) / int.Parse(ite2.host.PassengerNumber)).ToString();
                        }
                    }
                    ListBookedSticket.Add(ite);
                }
            }
            //foreach (SupportRevenue ite2 in ListRevenue)
            //{
            //    if (int.Parse(ite2.percent) > 75) ite2.color = "LightGreen";
            //    if (int.Parse(ite2.percent) < 50) ite2.color = "Red";
            //}

            InitBackupList();
        }

        private void InitBackupList()
        {
            ListBackupRevenue = new ObservableCollection<SupportRevenue>();

            foreach (SupportRevenue ite2 in ListRevenue)
                ListBackupRevenue.Add(ite2);
        }
        private void ResetListValue()
        {
            ListRevenue = new ObservableCollection<SupportRevenue>();

            foreach (SupportRevenue ite2 in ListBackupRevenue)
            {
                ite2.Rank = "99";
                ListRevenue.Add(ite2);
            }
        }

        void SortListByTotal(object obj)
        {
            SupportRevenue temp;
            for (int i = 0; i < ListRevenue.Count - 1; i++)
            {
                for (int j = i + 1; j < ListRevenue.Count; j++)
                {
                    if (int.Parse(ListRevenue[i].totalPrice) < int.Parse(ListRevenue[j].totalPrice))
                    {
                        temp = ListRevenue[i];
                        ListRevenue[i] = ListRevenue[j];
                        ListRevenue[j] = temp;
                    }
                }
            }
            for (int i = 0; i < ListRevenue.Count; i++)
                ListRevenue[i].Rank = (i + 1).ToString();



        }
        void SortListByPercent()
        {
            SupportRevenue temp; for (int i = 0; i < ListRevenue.Count - 1; i++)
            {
                for (int j = i + 1; j < ListRevenue.Count; j++)
                {
                    if (int.Parse(ListRevenue[i].percent) < int.Parse(ListRevenue[j].percent))
                    {
                        temp = ListRevenue[i];

                        ListRevenue[i] = ListRevenue[j];
                        ListRevenue[j] = temp;
                    }
                }
            }
            for (int i = 0; i < ListRevenue.Count; i++)
                ListRevenue[i].Rank = (i + 1).ToString();
        }

        private ObservableCollection<BookedTicket> listBookedSticket;
        public ObservableCollection<BookedTicket> ListBookedSticket
        {
            get { return listBookedSticket; }
            set
            {
                listBookedSticket = value;
                OnPropertyChanged("ListBookedSticket");
            }
        }
        private ObservableCollection<SupportRevenue> listRevenue;
        public ObservableCollection<SupportRevenue> ListRevenue
        {
            get { return listRevenue; }
            set
            {
                listRevenue = value;
                OnPropertyChanged("ListRevenue");
            }
        }
        private ObservableCollection<SupportRevenue> listBackupRevenue;
        public ObservableCollection<SupportRevenue> ListBackupRevenue
        {
            get { return listBackupRevenue; }
            set
            {
                listBackupRevenue = value;
                OnPropertyChanged("ListBackupRevenue");
            }
        }
        private ObservableCollection<Flight> listFlight;
        public ObservableCollection<Flight> ListFlight
        {
            get { return listFlight; }
            set
            {
                listFlight = value;
                OnPropertyChanged("ListFlight");
            }
        }
        private ObservableCollection<Invoice> listInvoice;
        public ObservableCollection<Invoice> ListInvoice
        {
            get { return listInvoice; }
            set
            {
                listInvoice = value;
                OnPropertyChanged("ListInvoice");
            }
        }

        private List<string> listMonth;
        public List<string> ListMonth
        {
            get { return listMonth; }
            set
            {
                listMonth = value;
                OnPropertyChanged("ListMonth");
            }
        }

        private string selectedMonth;
        public string SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                selectedMonth = (string)monthHashtable[value];
            }
        }
        private List<string> listYear;
        public List<string> ListYear
        {
            get { return listYear; }
            set
            {
                listYear = value;
                OnPropertyChanged("ListYear");
            }
        }

        private string selectedYear;
        public string SelectedYear
        {
            get { return selectedYear; }
            set
            {
                selectedYear = value;
                OnPropertyChanged("SelectedYear");
            }
        }

    }
    public class SupportRevenue : ObservableObject
    {


        public Flight host { set; get; }
        public string Month { set; get; }
        public string Year { set; get; }
        public string totalticket { set; get; }
        public string totalPrice { set; get; }
        public string percent { set; get; }
        public string color { set; get; }
        public string rank;

        public SupportRevenue(Flight host, string month, string year, string totalticket, string totalPrice, string percent, string color, string rank)
        {
            this.host = host;
            Month = month;
            Year = year;
            this.totalticket = totalticket;
            this.totalPrice = totalPrice;
            this.percent = percent;
            this.color = color;
            Rank = rank;
        }

        public string Rank
        {
            get { return rank; }
            set
            {
                rank = value;
                OnPropertyChanged("Rank");
            }
        }
    }


}
