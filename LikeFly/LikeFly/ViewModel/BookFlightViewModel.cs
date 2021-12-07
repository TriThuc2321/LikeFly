﻿using LikeFly.Core;
using LikeFly.Model;
using LikeFly.View;
using LikeFly.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;

namespace LikeFly.ViewModel
{
    public class BookFlightViewModel: ObservableObject
    {
        //INavigation navigation;
        //public Command PayingMethodCommand { get; }
        //public Command NavigationBack { get; }
        //public BookFlightViewModel() { }

        //string TourPrice, Provisional, DiscountMoney, Total;

        //public BookFlightViewModel(INavigation navigation)
        //{
        //    this.navigation = navigation;
        //   // SelectedFlight = DataManager.Ins.SelectedFlight;
        //    PayingMethodCommand = new Command(openPayingMethodView);

        //    IncreaseAmountCommand = new Command(increase);
        //    DescreaseAmountCommand = new Command(decrease);
        //    Amount = 1;

        //    NavigationBack = new Command(() => navigation.PopAsync());


        //    CheckDiscountCommand = new Command(checkDiscount);

        //    DataManager.Ins.CurrentDiscount = null;

        //    SetInformation();
        //}

        //bool checkValidation()
        //{
        //    // Check null
        //    if (Name == null)
        //    {
        //        DependencyService.Get<IToast>().ShortToast("Name is null! Please enter your name");
        //        return false;
        //    }
        //    else if (Birthday == null)
        //    {
        //        DependencyService.Get<IToast>().ShortToast("Birthday is null! Please enter your birthday");
        //        return false;
        //    }
        //    else if (Contact == "")
        //    {
        //        DependencyService.Get<IToast>().ShortToast("Contact is null! Please enter your contact");
        //        return false;

        //    }
        //    else if (Cmnd == "")
        //    {
        //        DependencyService.Get<IToast>().ShortToast("Identity card ID is null! Please enter");
        //        return false;

        //    }
        //    else if (Address == "")
        //    {
        //        DependencyService.Get<IToast>().ShortToast("Address is null! Please enter your address");
        //        return false;

        //    }
        //    else if (Amount == 0)
        //    {
        //        DependencyService.Get<IToast>().ShortToast("Amount is null! Please enter your amount");
        //        return false;

        //    }

        //    //Check amount
        //    if (Amount == 0)
        //    {
        //        AmountNotice = "Choose your amount";
        //        AmountNoticeColor = Color.Red;
        //        AmountNoticeVisible = true;
        //        DependencyService.Get<IToast>().ShortToast("Amount is 0! Please enter your amount");
        //        return false;


        //    }
        //    else if (Amount > int.Parse(selectedFlight.remaining))
        //    {
        //        AmountNotice = "This tour ticket has no more turn";
        //        AmountNoticeVisible = true;
        //        AmountNoticeColor = Color.DarkOrange;
        //        DependencyService.Get<IToast>().ShortToast("This tour ticket has no more turn!");
        //        return false;

        //    }

        //    UsersServices services = new UsersServices();
        //    //Check contact
        //    if (!services.IsPhoneNumber(Contact))
        //    {
        //        ContactNotice = "Incorrect phone number";
        //        ContactNoticeColor = Color.Red;
        //        ContactNoticeVisible = true;
        //        DependencyService.Get<IToast>().ShortToast("Incorrect phone number!");
        //        return false;

        //    }
        //    //Check cmnd
        //    if (!services.IsCMND(Cmnd))
        //    {
        //        CmndNotice = "Incorrect identity card ID";
        //        CmndNoticeColor = Color.Red;
        //        CmndNoticeVisible = true;
        //        DependencyService.Get<IToast>().ShortToast("Incorrect identity card ID!");
        //        return false;

        //    }

        //    return true;


        //}
        //void openPayingMethodView(object obj)
        //{
        //    var ticketServices = DataManager.Ins.BookedTicketsServices;
        //    var invoiceServices = DataManager.Ins.InvoicesServices;

        //    if (checkValidation())
        //    {
        //        string[] birth;
        //        birth = Birthday.ToString().Split(' ');

        //        DataManager.Ins.CurrentInvoice = new Invoice()
        //        {
        //            id = invoiceServices.GenerateInvoiceId(),
        //            discount = new Discount { id = DiscountId },
        //            discountMoney = DiscountMoney,
        //            isPaid = false,
        //            amount = Amount.ToString(),
        //            total = Total,
        //            price = TourPrice
        //        };

        //        DataManager.Ins.CurrentBookedTicket = new BookedTicket()
        //        {
        //            id = ticketServices.GenerateTicketId(),
        //            flight = new Flight { id = selectedFlight.id },
        //            name = Name,
        //            birthday = birth[0],
        //            contact = Contact,
        //            email = Email,
        //            cmnd = Cmnd,
        //            address = Address,
        //            isCancel = false,
        //            invoice = new Invoice
        //            {
        //                id = DataManager.Ins.CurrentInvoice.id
        //            }
        //        };

        //        navigation.PushAsync(new PayingMethodView());
        //    }
        //}

        //void checkDiscount(object obj)
        //{
        //    if (DiscountId == null)
        //    {
        //        DiscountNotice = "Please enter the discount ID";
        //        DiscountNoticeVisible = true;
        //        DiscountNoticeColor = Color.Red;
        //        return;
        //    }
        //    else
        //    {
        //        foreach (var discount in DataManager.Ins.ListDiscount)
        //            if (DiscountId == discount.id)
        //            {
        //                if (discount.isUsed == discount.total)
        //                {
        //                    DiscountNotice = "This discount has no more turn";
        //                    DiscountNoticeVisible = true;
        //                    DiscountNoticeColor = Color.DarkOrange;
        //                }
        //                else
        //                {
        //                    DiscountNotice = "Successfully code applied";
        //                    DiscountNoticeVisible = true;
        //                    DiscountNoticeColor = Color.ForestGreen;

        //                    DataManager.Ins.CurrentDiscount = discount;

        //                    Provisional = (Amount * int.Parse(selectedFlight.basePrice)).ToString();
        //                    if (discount != null)
        //                        DiscountMoney = (int.Parse(discount.percent) * int.Parse(Provisional) / 100).ToString();
        //                    Total = (int.Parse(Provisional) - int.Parse(DiscountMoney)).ToString();
        //                    FormatMoney();

        //                }

        //                return;
        //            }
        //    }

        //    DiscountNotice = "Incorrect discount ID";
        //    DiscountNoticeVisible = true;
        //    DiscountNoticeColor = Color.Red;
        //}
        //public Command CheckDiscountCommand { get; }

        //#region command Amount
        //public Command IncreaseAmountCommand { get; }
        //void increase(object obj)
        //{
        //    if (Amount < int.Parse(selectedFlight.remaining))
        //    {
        //        Amount++;
        //        Provisional = (Amount * int.Parse(selectedFlight.basePrice)).ToString();
        //        if (DataManager.Ins.CurrentDiscount != null)
        //            DiscountMoney = (int.Parse(DataManager.Ins.CurrentDiscount.percent) * int.Parse(Provisional) / 100).ToString();
        //        Total = (int.Parse(Provisional) - int.Parse(DiscountMoney)).ToString();
        //        FormatMoney();
        //    }
        //    else
        //    {
        //        AmountNoticeColor = Color.Red;
        //    }

        //}

        //public Command DescreaseAmountCommand { get; }
        //void decrease(object obj)
        //{
        //    if (Amount > 1)
        //    {
        //        Amount--;
        //        Provisional = (Amount * int.Parse(selectedFlight.basePrice)).ToString();
        //        if (DataManager.Ins.CurrentDiscount != null)
        //            DiscountMoney = (int.Parse(DataManager.Ins.CurrentDiscount.percent) * int.Parse(Provisional) / 100).ToString();
        //        Total = (int.Parse(Provisional) - int.Parse(DiscountMoney)).ToString();
        //        FormatMoney();
        //    }
        //}

        //#endregion


        //#region user information
        //private string _name;
        //public string Name
        //{
        //    get { return _name; }
        //    set
        //    {
        //        _name = value;
        //        OnPropertyChanged("Name");
        //    }
        //}

        //private DateTime _birthday;
        //public DateTime Birthday
        //{
        //    get { return _birthday; }
        //    set
        //    {
        //        _birthday = value;
        //        OnPropertyChanged("Birthday");
        //    }
        //}

        //private string _contact;
        //public string Contact
        //{
        //    get { return _contact; }
        //    set
        //    {
        //        _contact = value;
        //        OnPropertyChanged("Contact");
        //    }
        //}

        //private string _email;
        //public string Email
        //{
        //    get { return _email; }
        //    set
        //    {
        //        _email = value;
        //        OnPropertyChanged("Email");
        //    }
        //}

        //private string _cmnd;
        //public string Cmnd
        //{
        //    get { return _cmnd; }
        //    set
        //    {
        //        _cmnd = value;
        //        OnPropertyChanged("Cmnd");
        //    }
        //}

        //private string _address;
        //public string Address
        //{
        //    get { return _address; }
        //    set
        //    {
        //        _address = value;
        //        OnPropertyChanged("Address");
        //    }
        //}

        //private string _discountId;
        //public string DiscountId
        //{
        //    get { return _discountId; }
        //    set
        //    {
        //        _discountId = value;
        //        OnPropertyChanged("DiscountId");
        //    }
        //}

        //private int _amount;
        //public int Amount
        //{
        //    get { return _amount; }
        //    set
        //    {
        //        _amount = value;
        //        OnPropertyChanged("Amount");
        //    }
        //}
        //#endregion

        //#region contact
        //private string _contactNotice;
        //public string ContactNotice
        //{
        //    get { return _contactNotice; }
        //    set
        //    {
        //        _contactNotice = value;
        //        OnPropertyChanged("ContactNotice");
        //    }
        //}

        //private Color _contactNoticeColor;
        //public Color ContactNoticeColor
        //{
        //    get { return _contactNoticeColor; }
        //    set
        //    {
        //        _contactNoticeColor = value;
        //        OnPropertyChanged("ContactNoticeColor");
        //    }
        //}

        //private bool _contactNoticeVisible;
        //public bool ContactNoticeVisible
        //{
        //    get { return _contactNoticeVisible; }
        //    set
        //    {
        //        _contactNoticeVisible = value;
        //        OnPropertyChanged("ContactNoticeVisible");
        //    }
        //}

        //#endregion

        //#region email
        //private string _emailNotice;
        //public string EmailNotice
        //{
        //    get { return _emailNotice; }
        //    set
        //    {
        //        _emailNotice = value;
        //        OnPropertyChanged("EmailNotice");
        //    }
        //}

        //private Color _emailNoticeColor;
        //public Color EmailNoticeColor
        //{
        //    get { return _emailNoticeColor; }
        //    set
        //    {
        //        _emailNoticeColor = value;
        //        OnPropertyChanged("EmailNoticeColor");
        //    }
        //}

        //private bool _emailNoticeVisible;
        //public bool EmailNoticeVisible
        //{
        //    get { return _emailNoticeVisible; }
        //    set
        //    {
        //        _emailNoticeVisible = value;
        //        OnPropertyChanged("EmailNoticeVisible");
        //    }
        //}

        //#endregion

        //#region cmnd
        //private string _cmndNotice;
        //public string CmndNotice
        //{
        //    get { return _cmndNotice; }
        //    set
        //    {
        //        _cmndNotice = value;
        //        OnPropertyChanged("CmndNotice");
        //    }
        //}

        //private Color _cmndNoticeColor;
        //public Color CmndNoticeColor
        //{
        //    get { return _cmndNoticeColor; }
        //    set
        //    {
        //        _cmndNoticeColor = value;
        //        OnPropertyChanged("CmndNoticeColor");
        //    }
        //}

        //private bool _cmndNoticeVisible;
        //public bool CmndNoticeVisible
        //{
        //    get { return _cmndNoticeVisible; }
        //    set
        //    {
        //        _cmndNoticeVisible = value;
        //        OnPropertyChanged("CmndNoticeVisible");
        //    }
        //}
        //#endregion

        //#region discount
        //private string _discountNotice;
        //public string DiscountNotice
        //{
        //    get { return _discountNotice; }
        //    set
        //    {
        //        _discountNotice = value;
        //        OnPropertyChanged("DiscountNotice");
        //    }
        //}

        //private Color _discountNoticeColor;
        //public Color DiscountNoticeColor
        //{
        //    get { return _discountNoticeColor; }
        //    set
        //    {
        //        _discountNoticeColor = value;
        //        OnPropertyChanged("DiscountNoticeColor");
        //    }
        //}

        //private bool _discountNoticeVisible;
        //public bool DiscountNoticeVisible
        //{
        //    get { return _discountNoticeVisible; }
        //    set
        //    {
        //        _discountNoticeVisible = value;
        //        OnPropertyChanged("DiscountNoticeVisible");
        //    }
        //}
        //#endregion

        //#region amount
        //private string _amountNotice;
        //public string AmountNotice
        //{
        //    get { return _amountNotice; }
        //    set
        //    {
        //        _amountNotice = value;
        //        OnPropertyChanged("AmountNotice");
        //    }
        //}

        //private Color _amountNoticeColor;
        //public Color AmountNoticeColor
        //{
        //    get { return _amountNoticeColor; }
        //    set
        //    {
        //        _amountNoticeColor = value;
        //        OnPropertyChanged("AmountNoticeColor");
        //    }
        //}

        //private bool _amountNoticeVisible;
        //public bool AmountNoticeVisible
        //{
        //    get { return _amountNoticeVisible; }
        //    set
        //    {
        //        _amountNoticeVisible = value;
        //        OnPropertyChanged("AmountNoticeVisible");
        //    }
        //}


        //#endregion

        //#region price
        //private string _strTourPrice;
        //public string StrTourPrice
        //{
        //    get { return _strTourPrice; }
        //    set
        //    {
        //        _strTourPrice = value;
        //        OnPropertyChanged("StrTourPrice");
        //    }
        //}

        //private string _strProvisional;
        //public string StrProvisional
        //{
        //    get { return _strProvisional; }
        //    set
        //    {
        //        _strProvisional = value;
        //        OnPropertyChanged("StrProvisional");
        //    }
        //}

        //private string _strDiscountMoney;
        //public string StrDiscountMoney
        //{
        //    get { return _strDiscountMoney; }
        //    set
        //    {
        //        _strDiscountMoney = value;
        //        OnPropertyChanged("StrDiscountMoney");
        //    }
        //}

        //private string _strTotal;
        //public string StrTotal
        //{
        //    get { return _strTotal; }
        //    set
        //    {
        //        _strTotal = value;
        //        OnPropertyChanged("StrTotal");
        //    }
        //}

        //#endregion

        //#region selectedFlight
        //private Flight selectedFlight;
        //public Flight SelectedFlight
        //{
        //    get { return selectedFlight; }
        //    set
        //    {
        //        selectedFlight = value;
        //        OnPropertyChanged("SelectedFlight");
        //    }
        //}
        //#endregion
        //void SetInformation()
        //{
        //    User getUser = DataManager.Ins.CurrentUser;
        //    Name = getUser.name;
        //    if (getUser.birthday != "")
        //    {
        //        string[] day = getUser.birthday.Split(' ');
        //        string[] arr = day[0].Split('/');
        //        Birthday = new DateTime(
        //            int.Parse(arr[2]),
        //            int.Parse(arr[0]),
        //            int.Parse(arr[1])
        //            );
        //    }
        //    else
        //        Birthday = new DateTime(1980, 1, 1);

        //    Contact = getUser.contact;
        //    ContactNoticeVisible = false;

        //    Email = getUser.email;
        //    EmailNotice = "This email is your register account email";
        //    EmailNoticeColor = Color.ForestGreen;
        //    EmailNoticeVisible = true;

        //    Cmnd = getUser.cmnd;
        //    ContactNoticeVisible = false;

        //    Address = getUser.address;

        //    AmountNotice = "This tour is remaining " + SelectedFlight.remaining + " tickets";
        //    AmountNoticeColor = Color.ForestGreen;
        //    AmountNoticeVisible = true;

        //    DiscountNotice = "Enter and press to check the validation of your code";
        //    DiscountNoticeColor = Color.ForestGreen;
        //    DiscountNoticeVisible = true;

        //    TourPrice = Provisional = Total = selectedFlight.basePrice;

        //    DiscountMoney = "0";

        //    DataManager.Ins.CurrentDiscount = null;
        //    DataManager.Ins.CurrentBookedTicket = null;

        //    FormatMoney();
        //}

        //void FormatMoney()
        //{
        //    StrProvisional = Provisional;
        //    StrTotal = Total;
        //    StrTourPrice = TourPrice;
        //    StrDiscountMoney = DiscountMoney;

        //    var service = DataManager.Ins.InvoicesServices;

        //    StrTotal = service.FormatMoney(StrTotal) + " USD";
        //    StrTourPrice = service.FormatMoney(StrTourPrice) + " USD";
        //    StrDiscountMoney = service.FormatMoney(StrDiscountMoney) + " USD";
        //    StrProvisional = service.FormatMoney(StrProvisional) + " USD";
        //}
    }
}
