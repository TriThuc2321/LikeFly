using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{

    public class NewDiscountViewModel : ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public Command NavigationBack { get; }
        public Command AddCommand { get; }
        public NewDiscountViewModel() { }
        public NewDiscountViewModel(INavigation navigation, Shell current)
        {
            this.navigation = navigation;
            this.currentShell = current;
            NavigationBack = new Command(() => navigation.PopAsync());

            AddCommand = new Command(Add);

            Notice = "";
            NoticeVisible = false;

            Percent = Total = "";
        }

        async void Add(object obj)
        {
            if (await checkValid())
            {
                Discount discount = new Discount()
                {
                    id = Id,
                    percent = Percent,
                    total = Total,
                    isUsed = "0"
                };

                await DataManager.Ins.DiscountsServices.addDiscount(discount);
                DataManager.Ins.ListDiscount.Add(discount);

                DependencyService.Get<IToast>().ShortToast("Thêm mã giảm giá thành công!");


                await navigation.PopAsync();

            }
        }

        async Task<bool> checkValid()
        {
            Notice = "";
            NoticeVisible = false;

            if (Id == "")
            {
                Notice += "Mã giảm giá bị trống. Xin hãy bổ sung!";
                NoticeVisible = true;
                return false;
            }

            if (Percent == "")
            {
                Notice += "Phần trăm giảm giá còn trống. Xin hãy bổ sung! ";
                NoticeVisible = true;
                return false;
            }

            if (Total == "")
            {
                Notice += "Số lượt giảm giá còn trống. Xin hãy bổ sung!";
                NoticeVisible = true;
                return false;
            }

            if (Id.Length > 8)
            {
                Notice += "Mã giảm giá có số kí tự tối đa là 8. Xin hãy kiểm tra lại!";
                NoticeVisible = true;
                return false;
            }

            Discount tmp = await DataManager.Ins.DiscountsServices.FindDiscountById(Id);
            if (tmp != null)
            {
                Notice += "Mã giảm giá đã tồn tại. Hãy nhập một mã khác!";
                NoticeVisible = true;
                return false;
            }

            if (int.Parse(Percent) > 100)
            {
                Notice += "Phần trăm giảm giá tối đa là 100%. Hãy thử lại ";
                NoticeVisible = true;
                return false;
            }


            if (Percent.Contains(".") || Percent.Contains(","))
            {
                Notice += "Phần trăm giảm giá là một số nguyên. Hãy thử lại!";
                NoticeVisible = true;
                return false;
            }

            if (Total.Contains(".") || Total.Contains(","))
            {
                Notice += "Số lượt giảm giá là số nguyên. Hãy thử lại!";
                NoticeVisible = true;
                return false;
            }

            return true;
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");

            }
        }

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

        private string _percent;
        public string Percent
        {
            get { return _percent; }
            set
            {
                _percent = value;
                OnPropertyChanged("Percent");

            }
        }

        private string _notice;
        public string Notice
        {
            get { return _notice; }
            set
            {
                _notice = value;
                OnPropertyChanged("Notice");

            }
        }

        private bool _noticeVisible;
        public bool NoticeVisible
        {
            get { return _noticeVisible; }
            set
            {
                _noticeVisible = value;
                OnPropertyChanged("NoticeVisible");

            }
        }

        bool IsNumber(string pValue)
        {
            foreach (Char c in pValue)
            {
                if (!Char.IsDigit(c))
                    return false;
            }
            return true;
        }
    }
}
