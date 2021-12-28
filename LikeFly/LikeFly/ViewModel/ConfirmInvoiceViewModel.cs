using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    public class ConfirmInvoiceViewModel: ObservableObject
    {
        public INavigation navigation;
        public Shell currentShell;

        public Command NotificaitonCommand { get; }
        public Command MenuCommand { get; }

        public ConfirmInvoiceViewModel() { }
        public ConfirmInvoiceViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
            InitList();
        }

        public ICommand NavigationBack => new Command<object>((obj) =>
        {
          
            navigation.PopAsync();
        });
        private Invoice selectedInvoice;
        public Invoice SelectedInvoice
        {
            get { return selectedInvoice; }
            set
            {
                selectedInvoice = value;
                OnPropertyChanged("SelectedInvoice");
            }
        }
        private BookedTicket selectedBookedTicket;
        public BookedTicket SelectedBookedTicket
        {
            get { return selectedBookedTicket; }
            set
            {
                selectedBookedTicket = value;
                OnPropertyChanged("SelectedBookedTicket");
            }
        }
        private ObservableCollection<BookedTicket> listBookedTicket;
        public ObservableCollection<BookedTicket> ListBookedTicket
        {
            get { return listBookedTicket; }
            set
            {
                listBookedTicket = value;
                OnPropertyChanged("ListBookedTicket");
            }
        }

        private void InitList()
        {

            ListBookedTicket = new ObservableCollection<BookedTicket>();
            foreach (BookedTicket ite in DataManager.Ins.ListBookedTickets)
            {
                if (ite.Invoice.IsPaid == false) ListBookedTicket.Add(ite);
            }
        }
        public ICommand AcceptCommand => new Command<object>((obj) =>
        {
            BookedTicket selected = obj as BookedTicket;
            
            if (selected != null)
            {
                selected.Invoice.IsPaid = true;
                DataManager.Ins.InvoicesServices.UpdateInvoice(selected.Invoice);
                DataManager.Ins.BookedTicketsServices.UpdateBookedTicket(selected);
                ListBookedTicket.Remove(selected);
            }
        });
        public string GeneratePlaceId(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
           
            return randomString;
        }
        public ICommand DeclineCommand => new Command<object>(async (obj) =>
        {
            BookedTicket selected = obj as BookedTicket;
            if (selected != null)
            {
                string message = "Kính gửi " + selected.Name + ",\n\n" +
                "Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi, bạn vừa đặt chuyến bay: '" + selected.Flight.Name + "', Có một số vấn đề xảy ra trong quá trình xử lý thanh toán của bạn, chúng tôi đã hủy vé của bạn để kiểm tra, xin lỗi vì sự bất tiện này." +
                "\n\n---------------------------------------\nĐể biết thêm về thông tin hoàn tiền chi tiết, vui lòng liên hệ:\n 0383303061 - Nguyen Khanh Linh\n hoặc 0834344655 - Pham Vo Di Thien";
                string title = "Thông báo về vé chuyến bay " + selected.Flight.Id.ToUpper();
                await DataManager.Ins.NotiServices.SendNoti(GeneratePlaceId(10).ToString(), selected.Email, message, title, selected.Flight.Id.ToString());
                ListBookedTicket.Remove(selected);
                DataManager.Ins.InvoicesServices.DeleteInvoice(selected.Invoice.Id);
                DataManager.Ins.BookedTicketsServices.DeleteBookedTicket(selected.Id);

            }
        });
        public ICommand SelectedCommand => new Command<object>(async (obj) =>
        {
            BookedTicket selected = obj as BookedTicket;
            if (selected != null)
            {
                DataManager.Ins.CurrentBookedTicket = selected;
                //navigation.PushAsync(new DetailInvoiceView());
                SelectedBookedTicket = null;
            }
        });
       /* private List<Flight> listFlight;
        public List<Flight> ListFlight
        {
            get { return listFlight; }
            set
            {
                listFlight = value;
                OnPropertyChanged("ListFlight");
            }
        }
        private List<Invoice> listInvoice;
        public List<Invoice> ListInvoice
        {
            get { return listInvoice; }
            set
            {
                listInvoice = value;
                OnPropertyChanged("ListInvoice");
            }
        }*/



    }
}
