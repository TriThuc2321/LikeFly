using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using LikeFly.View;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    public class BankingViewModel : ObservableObject
    {
        INavigation navigation;
        string Money = "";

        public Command NavigationBack { get; }
        public Command UploadPhoto { get; }
        public Command RemovePhoto { get; }
        public Command Confirm { get; }

        public BankingViewModel() { }

        public BankingViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            NavigationBack = new Command(() => navigation.PopAsync());
            UploadPhoto = new Command(uploadPhoto);
            Confirm = new Command(confirm);
            RemovePhoto = new Command(removePhoto);


            SetInformation();
        }

        Plugin.Media.Abstractions.MediaFile currentPhoto;
        async void uploadPhoto(object obj)
        {
            await CrossMedia.Current.Initialize();

            var imgData = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions());
            currentPhoto = imgData;

            if (imgData != null)
            {
                ImageLink = ImageSource.FromStream(imgData.GetStream);
                ImageVisible = true;
                RemovePhotoVisible = true;
                PermitConfirm = true;
            }

        }

        void removePhoto(object obj)
        {
            currentPhoto = null;
            RemovePhotoVisible = false;
            PermitConfirm = false;

            ImageLink = "";
            ImageVisible = false;
        }


        async void confirm(object obj)
        {

            PermitConfirmEnable = false;
            if (await checkRemaining() == false)
            {
                return;
            }

            if (await checkDiscount() == false) return;

            if (ImageVisible && currentPhoto != null)
            {
                string url = await DataManager.Ins.InvoicesServices.savePhoto(
                    currentPhoto.GetStream(),
                    DataManager.Ins.CurrentBookedTicket.Invoice.Id
                    );

                DataManager.Ins.CurrentInvoice.IsPaid = false;
                DataManager.Ins.CurrentInvoice.PayingTime = DateTime.Now.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"));
                DataManager.Ins.CurrentInvoice.Photo = url;
                DataManager.Ins.CurrentInvoice.Vnd = Money;
                DataManager.Ins.CurrentInvoice.Method = "Banking";
            }


            DataManager.Ins.CurrentBookedTicket.BookTime = DateTime.Now.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));

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
            await navigation.PushAsync(new SuccessBookView());

        }

        #region money
        private string _strMoney;
        public string StrMoney
        {
            get { return _strMoney; }
            set
            {
                _strMoney = value;
                OnPropertyChanged("StrMoney");
            }
        }
        #endregion

        #region image
        private ImageSource _imageLink;
        public ImageSource ImageLink
        {
            get { return _imageLink; }
            set
            {
                _imageLink = value;
                OnPropertyChanged("ImageLink");
            }
        }

        private bool _imageVisible;
        public bool ImageVisible
        {
            get { return _imageVisible; }
            set
            {
                _imageVisible = value;
                OnPropertyChanged("ImageVisible");
            }
        }
        #endregion



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

        #endregion

        #region RemovePhoto 
        private bool _removePhotoVisible;
        public bool RemovePhotoVisible
        {
            get { return _removePhotoVisible; }
            set
            {
                _removePhotoVisible = value;
                OnPropertyChanged("RemovePhotoVisible");
            }
        }
        #endregion

        void SetInformation()
        {
            PermitConfirmEnable = true;
            PermitConfirm = false;

            ImageLink = "";
            ImageVisible = false;
            RemovePhotoVisible = false;

            Money = DataManager.Ins.CurrentInvoice.Total.ToString();

            int money = int.Parse(DataManager.Ins.InvoicesServices.RoundMoney(int.Parse(Money)));
            StrMoney = DataManager.Ins.InvoicesServices.RoundMoney(money);
            StrMoney = DataManager.Ins.InvoicesServices.FormatMoney(StrMoney);
            SelectedFlight = DataManager.Ins.CurrentFlight;
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
                    DependencyService.Get<IToast>().ShortToast("Mã giảm giá đã hết lượt sử dụng!");
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
        private bool _permitConfirm;
        public bool PermitConfirm
        {
            get { return _permitConfirm; }
            set
            {
                _permitConfirm = value;
                OnPropertyChanged("PermitConfirm");
            }
        }

        private bool _permitConfirmEnable;
        public bool PermitConfirmEnable
        {
            get { return _permitConfirmEnable; }
            set
            {
                _permitConfirmEnable = value;
                OnPropertyChanged("PermitConfirmEnable");
            }
        }

        void checkDateRegulation()
        {
            PermitConfirm = true;
            if (DataManager.Ins.BookedTicketsServices.countBookFlightRegulation(DataManager.Ins.CurrentFlight) < 5)
            {
                PermitConfirm = false;
            }
        }
    }
}
