using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using LikeFly.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    public class PayingMethodViewModel: ObservableObject
    {
        INavigation navigation;
        public Command NavigationBack { get; }
        public Command Confirm { get; }

        public Command OpenRegulation { get; }
        public PayingMethodViewModel() { }
        public PayingMethodViewModel(INavigation navigation)
        {
            this.navigation = navigation;

            SetInformation();
            checkDateRegulation();
            SelectedFlight = DataManager.Ins.CurrentFlight;
            NavigationBack = new Command(() => navigation.PopAsync());
            OpenRegulation = new Command(() => navigation.PushAsync(new RuleView()));
            Confirm = new Command(confirmPress);
        }

        async void confirmPress(object obj)
        {
            if (!IsCheckRegulation)
            {
                DependencyService.Get<IToast>().ShortToast("Quý khách chưa đánh dấu xác nhận đã đọc quy định");
            }

            if (!Banking && !Cash)
            {
                DependencyService.Get<IToast>().ShortToast("Quý khách hãy chọn phương thức dùng để thanh toán");
            }

            if (await checkDiscount() == false)
            {
                return;
            }
            if (await checkRemaining() == false)
            {
                return;
            }

            if (IsCheckRegulation && Banking)
            {
                ConfirmEnable = false;
                DataManager.Ins.CurrentBookedTicket.Invoice.Method = "Banking";
                navigation.PushAsync(new BankingView());
            }
            else if (IsCheckRegulation && Cash)
            {
                ConfirmEnable = false;
                DataManager.Ins.CurrentInvoice.Method = "Cash";
                DataManager.Ins.CurrentInvoice.IsPaid = false;
                DataManager.Ins.CurrentInvoice.PayingTime = DateTime.Now.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"));

                DataManager.Ins.CurrentBookedTicket.BookTime = DateTime.Now.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"));

                await DataManager.Ins.InvoicesServices.AddInvoice(DataManager.Ins.CurrentInvoice);
                await DataManager.Ins.BookedTicketsServices.AddBookedTicket(DataManager.Ins.CurrentBookedTicket);

                if (DataManager.Ins.CurrentDiscount != null)
                {
                    int isUsed = int.Parse(DataManager.Ins.CurrentDiscount.isUsed);
                    isUsed++;
                    DataManager.Ins.CurrentDiscount.isUsed = isUsed.ToString();

                    await DataManager.Ins.DiscountsServices.UpdateDiscount(DataManager.Ins.CurrentDiscount);

                }

                if (DataManager.Ins.CurrentFlight != null)
                {
                    int remaining = DataManager.Ins.CurrentDetailTicketType.Remain;
                    remaining = remaining - int.Parse(DataManager.Ins.CurrentInvoice.Amount);

                    foreach (var detailTicket in SelectedFlight.TicketTypes)
                    {
                        if (detailTicket.TicketType.Id == DataManager.Ins.CurrentDetailTicketType.TicketType.Id)
                        {
                            detailTicket.Remain = remaining;
                            break;
                        }    
                    }    
                   

                    await DataManager.Ins.FlightService.UpdateFlight(DataManager.Ins.CurrentFlight);
                }


                DependencyService.Get<IToast>().ShortToast("Đặt chuyến bay thành công!");
                updateManager();
                // await currentShell.GoToAsync($"//{nameof(HomeView)}");
                await navigation.PushAsync(new SuccessBookView());



            }

        }
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

        #region method
        private string _method;
        public string Method
        {
            get { return _method; }
            set
            {
                _method = value;
                OnPropertyChanged("Method");
            }
        }
        #endregion

        #region regulation
        private string _regulation;
        public string Regulation
        {
            get { return _regulation; }
            set
            {
                _regulation = value;
                OnPropertyChanged("Regulation");
            }
        }

        private bool _isCheckRegulation;
        public bool IsCheckRegulation
        {
            get { return _isCheckRegulation; }
            set
            {
                _isCheckRegulation = value;
                OnPropertyChanged("IsCheckRegulation");
            }
        }
        #endregion

        #region Banking & Cash
        private bool _banking;
        public bool Banking
        {
            get { return _banking; }
            set
            {
                _banking = value;
                OnPropertyChanged("Banking");
            }
        }

        private bool _cash;
        public bool Cash
        {
            get { return _cash; }
            set
            {
                _cash = value;
                OnPropertyChanged("Cash");
            }
        }
        #endregion

        #region TotalPrice 
        private string _total;
        public string Total
        {
            get { return _total; }
            set
            {
                _total = value;
                OnPropertyChanged("Total");
            }
        }
        #endregion

        private bool confirmEnable;
        public bool ConfirmEnable
        {
            get { return confirmEnable; }
            set
            {
                confirmEnable = value;
                OnPropertyChanged("ConfirmEnable");
            }
        }
        async Task<bool> checkRemaining()
        {
            Flight temp = await DataManager.Ins.FlightService.FindFlightById(DataManager.Ins.CurrentFlight.Id);

            int remaining = 0;

            foreach (var detail in temp.TicketTypes)
            {
                if (detail.TicketType.Id == DataManager.Ins.CurrentDetailTicketType.TicketType.Id)
                {
                    remaining = detail.Remain;
                }    
            }    
            if (int.Parse(DataManager.Ins.CurrentInvoice.Amount) <= remaining)
                return true;
            else
                DependencyService.Get<IToast>().ShortToast("Loại ghế này đã hết vé!");

            return false;
        }

        async Task<bool> checkDiscount()
        {
            if (DataManager.Ins.CurrentDiscount == null) return true;
            if (DataManager.Ins.CurrentDiscount != null)
            {
                Discount temp = await DataManager.Ins.DiscountsServices.FindDiscountById(DataManager.Ins.CurrentDiscount.id);
                if (int.Parse(temp.isUsed) < int.Parse(temp.total))
                    return true;
                else
                    DependencyService.Get<IToast>().ShortToast("Mã giảm giá hết lượt sử dụng");
            };

            return false;
        }


        void updateManager()
        {
            DataManager.Ins.CurrentBookedTicket.Flight = DataManager.Ins.CurrentFlight;
            DataManager.Ins.CurrentBookedTicket.Invoice = DataManager.Ins.CurrentInvoice;

            if (DataManager.Ins.CurrentDiscount != null)
                DataManager.Ins.CurrentBookedTicket.Invoice.Discount = DataManager.Ins.CurrentDiscount;


            DataManager.Ins.ListBookedTickets.Add(DataManager.Ins.CurrentBookedTicket);
            DataManager.Ins.ListInvoice.Add(DataManager.Ins.CurrentInvoice);

            DataManager.Ins.CurrentBookedTicket.Invoice = DataManager.Ins.CurrentInvoice;

            if (DataManager.Ins.CurrentDiscount != null)
            {
                DataManager.Ins.CurrentBookedTicket.Invoice.Discount = DataManager.Ins.CurrentDiscount;

                for (int i = 0; i < DataManager.Ins.ListDiscount.Count - 1; i++)
                {
                    if (DataManager.Ins.ListDiscount[i].id == DataManager.Ins.CurrentDiscount.id)
                    {
                        DataManager.Ins.ListDiscount[i] = DataManager.Ins.CurrentDiscount;
                        break;
                    }
                }


            }
        }

        void checkDateRegulation()
        {
            PermitCheckCash = true;
            LaterNotice = "";
            if (DataManager.Ins.BookedTicketsServices.countBookFlightRegulation(DataManager.Ins.CurrentFlight) < 5)
            {
                PermitCheckCash = false;
                LaterNotice = "Chỉ còn dưới 5 ngày nữa sẽ bắt đầu chuyến bay, quý khách chỉ có thể thanh toán bằng hình thức chuyển khoản ngân hàng!";
                Banking = true;
            }
        }

        private string _laterNotice;
        public string LaterNotice
        {
            get { return _laterNotice; }
            set
            {
                _laterNotice = value;
                OnPropertyChanged("LaterNotice");
            }
        }

        private string _laterNoticeVisible;
        public string LaterNoticeVisible
        {
            get { return _laterNoticeVisible; }
            set
            {
                _laterNoticeVisible = value;
                OnPropertyChanged("LaterNoticeVisible");
            }
        }

        private bool _permitCheckCash;
        public bool PermitCheckCash
        {
            get { return _permitCheckCash; }
            set
            {
                _permitCheckCash = value;
                OnPropertyChanged("PermitCheckCash");
            }
        }

        void SetInformation()
        {
            confirmEnable = true;
            Total = DataManager.Ins.CurrentInvoice.Total;

            var service = DataManager.Ins.InvoicesServices;
            Total = service.FormatMoney(Total);
        }


    }
}
