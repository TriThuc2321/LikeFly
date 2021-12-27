using LikeFly.Core;
using LikeFly.Model;
using LikeFly.View;
using LikeFly.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using System.Collections.ObjectModel;

namespace LikeFly.ViewModel
{
    public class BookFlightViewModel : ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public Command PayingMethodCommand { get; }
        public Command NavigationBack { get; }
        public BookFlightViewModel() { }

        float Price, Provisional, DiscountMoney, Total;

        public BookFlightViewModel(INavigation navigation, Shell current)
        {
            this.navigation = navigation;
            this.currentShell = current;
            SelectedFlight = DataManager.Ins.CurrentFlight;

            PayingMethodCommand = new Command(openPayingMethodView);

            NavigationBack = new Command(() => {
                navigation.PopAsync();
            });

            CheckDiscountCommand = new Command(checkDiscount);

            DataManager.Ins.CurrentDiscount = null;
            

            InitTicketTypesPicker();
            SetInformation();
        }
        void InitTicketTypesPicker()
        {
            ListTicketTypes = new ObservableCollection<DetailTicketType>();

            foreach (DetailTicketType tt in SelectedFlight.TicketTypes)
                ListTicketTypes.Add(tt);

            SelectedTicketType = ListTicketTypes[0];
            DataManager.Ins.CurrentDetailTicketType = SelectedTicketType;
        }
        bool checkValidation()
        {
            // Check null
            if (Name == null)
            {
                DependencyService.Get<IToast>().ShortToast("Vui lòng nhập tên hành khách");
                return false;
            }
            else if (Birthday == null)
            {
                DependencyService.Get<IToast>().ShortToast("Vui lòng nhập ngày sinh hành khách");
                return false;
            }
            else if (Contact ==null || Contact == "" )
            {
                DependencyService.Get<IToast>().ShortToast("Vui lòng nhập số điện thoại hành khách");
                return false;

            }
            else if (Cmnd == null || Cmnd == "")
            {
                DependencyService.Get<IToast>().ShortToast("Vui lòng nhập CMND/CCCD hành khách");
                return false;

            }
            else if (Address == null || Address == "")
            {
                DependencyService.Get<IToast>().ShortToast("Vui lòng nhập địa chỉ hành khách");
                return false;

            }  

            UsersServices services = new UsersServices();
            //Check contact
            if (!services.IsPhoneNumber(Contact))
            {                
                DependencyService.Get<IToast>().ShortToast("Số điện thoại có 10,11 số và bắt đầu từ số 0");
                return false;

            }
            //Check cmnd
            if (!services.IsCMND(Cmnd))
            {
                DependencyService.Get<IToast>().ShortToast("CCCD/CMND có 9 hoặc 12 số");
                return false;
            }
            return true;
        }
        void openPayingMethodView(object obj)
        {
           // checkValidation();
            var ticketServices = DataManager.Ins.BookedTicketsServices;
            var invoiceServices = DataManager.Ins.InvoicesServices;

            if (checkValidation())
            {
                string[] birth;
                birth = Birthday.ToString().Split(' ');

                DataManager.Ins.CurrentInvoice = new Invoice()
                {
                    Id = invoiceServices.GenerateInvoiceId(),
                    Discount = new Discount { id = DiscountId },
                    DiscountMoney = this.DiscountMoney.ToString(),
                    IsPaid = false,
                    Amount = "1",
                    Total = this.Total.ToString(),
                    Price = SelectedFlight.Price.ToString(),
                    TicketTypes = SelectedTicketType.TicketType
                };

                DataManager.Ins.CurrentBookedTicket = new BookedTicket()
                {
                    Id = ticketServices.GenerateTicketId(),
                    Flight = new Flight { Id = selectedFlight.Id },
                    Name = Name,
                    Birthday = birth[0],
                    Contact = Contact,
                    Email = Email,
                    Cmnd = Cmnd,
                    Address = Address,
                    IsCancel = false,
                    Invoice = new Invoice
                    {
                        Id = DataManager.Ins.CurrentInvoice.Id
                    }
                };

                navigation.PushAsync(new PayingMethodView());
            }
        }

        void checkDiscount(object obj)
        {
            if (DiscountId == null) return;

            foreach (var discount in DataManager.Ins.ListDiscount)
            {
                if (DiscountId == discount.id)
                {
                    if (discount.isUsed == discount.total)
                    {
                        DependencyService.Get<IToast>().ShortToast("Mã giảm giá đã hết lượt sử dụng");
                    }
                    else
                    {                        
                        DataManager.Ins.CurrentDiscount = discount;
                        DependencyService.Get<IToast>().ShortToast("Áp dụng mã giảm giá" + discount.percent + "% thành công");


                        DiscountMoney = SelectedFlight.Price * int.Parse(discount.percent) / 100;
                        Total = Provisional - DiscountMoney;
                        FormatMoney();

                    }

                    return;
                }
            }
        }
        public Command CheckDiscountCommand { get; }        


        #region user information
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private DateTime _birthday;
        public DateTime Birthday
        {
            get { return _birthday; }
            set
            {
                _birthday = value;
                OnPropertyChanged("Birthday");
            }
        }

        private string _contact;
        public string Contact
        {
            get { return _contact; }
            set
            {
                _contact = value;
                OnPropertyChanged("Contact");
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        private string _cmnd;
        public string Cmnd
        {
            get { return _cmnd; }
            set
            {
                _cmnd = value;
                OnPropertyChanged("Cmnd");
            }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }

        private string _discountId;
        public string DiscountId
        {
            get { return _discountId; }
            set
            {
                _discountId = value;
                OnPropertyChanged("DiscountId");
            }
        }
        
        #endregion     

        #region price
        private string _strTourPrice;
        public string StrTourPrice
        {
            get { return _strTourPrice; }
            set
            {
                _strTourPrice = value;
                OnPropertyChanged("StrTourPrice");
            }
        }

        private string _strProvisional;
        public string StrProvisional
        {
            get { return _strProvisional; }
            set
            {
                _strProvisional = value;
                OnPropertyChanged("StrProvisional");
            }
        }

        private string _strDiscountMoney;
        public string StrDiscountMoney
        {
            get { return _strDiscountMoney; }
            set
            {
                _strDiscountMoney = value;
                OnPropertyChanged("StrDiscountMoney");
            }
        }

        private string _strTotal;
        public string StrTotal
        {
            get { return _strTotal; }
            set
            {
                _strTotal = value;
                OnPropertyChanged("StrTotal");
            }
        }

        #endregion

        #region selectedFlight
        private Flight selectedFlight;
        public Flight SelectedFlight
        {
            get { return selectedFlight; }
            set
            {
                selectedFlight = value;
                OnPropertyChanged("SelectedFlight");
            }
        }
        #endregion
        private DetailTicketType selectedTicketType;
        public DetailTicketType SelectedTicketType
        {
            get { return selectedTicketType; }
            set
            {
                selectedTicketType = value;

                Provisional = Price * SelectedTicketType.TicketType.Percent;
                Total = Provisional - DiscountMoney;
                FormatMoney();

                OnPropertyChanged("SelectedTicketType");
            }
        }

        private ObservableCollection<DetailTicketType> listTicketTypes;
        public ObservableCollection<DetailTicketType> ListTicketTypes
        {
            get { return listTicketTypes; }
            set
            {
                listTicketTypes = value;
                OnPropertyChanged("ListTicketTypes");
            }
        }
        void SetInformation()
        {
            User getUser = DataManager.Ins.CurrentUser;
            Name = getUser.name;
            if (getUser.birthday != "")
            {
                string[] day = getUser.birthday.Split(' ');
                string[] arr = day[0].Split('/');
                Birthday = new DateTime(
                    int.Parse(arr[2]),
                    int.Parse(arr[1]),
                    int.Parse(arr[0])
                    );
            }
            else
                Birthday = new DateTime(1980, 1, 1);

            Contact = getUser.contact;
            Email = getUser.email;
            Cmnd = getUser.cmnd;
            

            Price = SelectedFlight.Price;
            Provisional = Price * SelectedTicketType.TicketType.Percent;
            DiscountMoney = 0;
            Total = Provisional - DiscountMoney;
            FormatMoney();


            DataManager.Ins.CurrentDiscount = null;
            DataManager.Ins.CurrentBookedTicket = null;

            
        }

        void FormatMoney()
        {
            StrProvisional = Provisional.ToString();
            StrTotal = Total.ToString();
            StrTourPrice = Price.ToString();
            StrDiscountMoney = DiscountMoney.ToString();

            var service = DataManager.Ins.InvoicesServices;

            StrTotal = service.FormatMoney(StrTotal) + " VND";
            StrTourPrice = service.FormatMoney(StrTourPrice) + " VND";
            StrDiscountMoney = service.FormatMoney(StrDiscountMoney) + " VND";
            StrProvisional = service.FormatMoney(StrProvisional) + " VND";
        }
    }
}
