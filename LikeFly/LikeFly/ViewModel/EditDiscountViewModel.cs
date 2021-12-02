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
    public class EditDiscountViewModel : ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public EditDiscountViewModel() { }
        public EditDiscountViewModel(INavigation navigation, Shell current)
        {
            this.navigation = navigation;
            this.currentShell = current;

            NavigationBack = new Command(() => navigation.PopAsync());
            DeleteCommand = new Command(delete);
            EditCommand = new Command(Edit);

            Notice = "";
            NoticeVisible = false;

            Percent = Total = "";

            PercentEnable = TotalEnable = true;

            SetInformation();
        }

        public Command NavigationBack { get; }
        public Command EditCommand { get; }

        public Command DeleteCommand { get; }

        async void delete(object obj)
        {
            //Discount result = obj as Discount;

            //if (result != null && result.isUsed != "0")
            //{
            //    DependencyService.Get<IToast>().ShortToast("Cannot delete this discount!");
            //    return;
            //}
            //else if (result != null)
            //{
            //    Discount tmp = await DataManager.Ins.DiscountsServices.FindDiscountById(result.id);
            //    if (tmp != null)
            //    {
            //        if (tmp.isUsed != "0")
            //        {
            //            DependencyService.Get<IToast>().ShortToast("Cannot delete this discount! Someone used this account");
            //            return;
            //        }
            //    }
            //    DiscountList.Remove(result);
            //    DataManager.Ins.ListDiscount.Remove(result);
            //    await DataManager.Ins.DiscountsServices.DeleteDiscount(result.id);
            }

        void SetInformation()
        {
            Discount discount = DataManager.Ins.CurrentDiscount;
            Id = discount.id;
            Total = discount.total;
            Percent = discount.percent;

            if (discount.isUsed != "0")
            {
                TotalEnable = PercentEnable = false;
                Notice = "Cannot edit this discount since this was used.";
                NoticeVisible = true;
            }
        }
        async void Edit(object obj)
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

                await DataManager.Ins.DiscountsServices.UpdateDiscount(discount);

                for (int i = 0; i < DataManager.Ins.ListDiscount.Count; i++)
                {
                    string id = DataManager.Ins.ListDiscount[i].id;
                    if (discount.id == id)
                    {
                        DataManager.Ins.ListDiscount[i] = discount;
                    }
                }
                DependencyService.Get<IToast>().ShortToast("Editing discount successfully!");

                await navigation.PopAsync();

            }
        }

        async Task<bool> checkValid()
        {
            Notice = "";
            NoticeVisible = false;

            //if (Id == "")
            //{
            //    Notice += "Please enter discount ID. ";
            //    NoticeVisible = true;
            //    return false;
            //}

            if (Percent == "")
            {
                Notice += "Please enter percent. ";
                NoticeVisible = true;
                return false;
            }

            if (Total == "")
            {
                Notice += "Please enter total. ";
                NoticeVisible = true;
                return false;
            }

            if (Id.Length > 10)
            {
                Notice += "Discount ID length is less than 10. ";
                NoticeVisible = true;
                return false;
            }

            Discount tmp = await DataManager.Ins.DiscountsServices.FindDiscountById(Id);
            if (tmp != null && tmp.isUsed != "0")
            {
                Notice += "This discount is used. Cannot edit";
                NoticeVisible = true;
                return false;
            }

            if (int.Parse(Percent) > 100)
            {
                Notice += "Percent must be less than 100. ";
                NoticeVisible = true;
                return false;
            }


            if (Percent.Contains(".") || Percent.Contains(","))
            {
                Notice += "Please enter an interger number for percent. ";
                NoticeVisible = true;
                return false;
            }

            if (Total.Contains(".") || Total.Contains(","))
            {
                Notice += "Please enter an interger number for total. ";
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

        private bool _totalEnable;
        public bool TotalEnable
        {
            get { return _totalEnable; }
            set
            {
                _totalEnable = value;
                OnPropertyChanged("TotalEnable");

            }
        }

        private bool _percentEnable;
        public bool PercentEnable
        {
            get { return _percentEnable; }
            set
            {
                _percentEnable = value;
                OnPropertyChanged("PercentEnable");

            }
        }

    }
}
