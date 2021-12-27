using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using LikeFly.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    public class DiscountManagerViewModel: ObservableObject
    {
        Shell currentShell;
        INavigation navigation;
        public Command NavigationBack { get; }
        public Command NewDiscountCommand { get; }
        public Command DeleteCommand { get; }
        public DiscountManagerViewModel() { }
        public DiscountManagerViewModel(INavigation navigation, Shell shell)
        {
            this.navigation = navigation;
            this.currentShell = shell;
            Refresh = new Command(RefreshView);
            NewDiscountCommand = new Command(addNew);
            NavigationBack = new Command(() => navigation.PopAsync());

            SetInformation();

        }

       
        void addNew(object obj)
        {
            navigation.PushAsync(new NewDiscountView());
        }

        void SetInformation()
        {
            DiscountList = new ObservableCollection<Discount>();
            foreach (var dc in DataManager.Ins.ListDiscount)
            {
                DiscountList.Add(dc);
            }
            SortingDiscount();
        }

        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            Discount result = obj as Discount;
            if (result != null)
            {
                DataManager.Ins.CurrentDiscount = result;
                SelectedDiscount = null;

                navigation.PushAsync(new EditDiscountView());

            }
        });

        private ObservableCollection<Discount> _discountList;
        public ObservableCollection<Discount> DiscountList
        {
            get { return _discountList; }
            set
            {
                _discountList = value;
                OnPropertyChanged("DiscountList");
            }
        }

        private Discount selectedDiscount;
        public Discount SelectedDiscount
        {
            get { return selectedDiscount; }
            set
            {
                selectedDiscount = value;
                OnPropertyChanged("SelectedDiscount");

            }
        }

        #region Refresh
        private bool _isRefresh;
        public bool IsRefresh
        {
            get
            {
                return _isRefresh;
            }

            set
            {
                _isRefresh = value;
                OnPropertyChanged("IsRefresh");
            }
        }

        public Command Refresh { get; }
        void RefreshView(object obj)
        {
            IsRefresh = true;
            SetInformation();
            IsRefresh = false;
        }
        #endregion

        void SortingDiscount()
        {
            // Xep giam dan
            for (int i = 0; i < DiscountList.Count - 1; i++)
            {
                Discount I = DiscountList[i];

                for (int j = i + 1; j < DiscountList.Count; j++)
                {
                    Discount J = DiscountList[j];
                    if (int.Parse(I.isUsed) > int.Parse(J.isUsed))
                    {
                        Discount tmp = new Discount();
                        DiscountList[j] = I;
                        DiscountList[i] = J;
                        J = tmp;
                    }
                }
            }
        }

    }
}
