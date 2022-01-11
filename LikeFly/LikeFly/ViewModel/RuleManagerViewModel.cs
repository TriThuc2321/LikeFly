using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    public class RuleManagerViewModel : ObservableObject
    {
        INavigation navigation;
        Shell currentShell;
        public Command NavigationBack { get; }
        public Command ChangeRuleCommand { get; }
        public RuleManagerViewModel() { }
        public RuleManagerViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;
            NavigationBack = new Command(() => navigation.PopAsync());
            Init();
        }
        private void Init()
        {
            RuleList = new ObservableCollection<Rule>();
            BackupRuleList = new ObservableCollection<Rule>();
            foreach (Rule ite in DataManager.Ins.ListRule)
            {
                RuleList.Add(ite);
                backupruleList.Add(ite);
            }
        }
        private void Backup()
        {
            RuleList = new ObservableCollection<Rule>();
            foreach (Rule ite in BackupRuleList)
            {
                RuleList.Add(ite);
               
            }
        }

        public ICommand SaveCommand => new Command<object>(async (obj) =>
        {
            int flag = 0;
            foreach ( Rule itee in DataManager.Ins.ListRule)
            {
                if (int.Parse(itee.DayNum) < 0 || int.Parse(itee.Deduct) < 0 || int.Parse(itee.Deduct) >= 100)
                {
                    DependencyService.Get<IToast>().ShortToast("Thông tin không hợp lệ !");
                    Backup();
                    flag = 1;
                }

            }
            if (flag == 0)
            {
                UpdateRule();
                DependencyService.Get<IToast>().ShortToast("Lưu thành công!");
            }
                
           
           
        });
        public ICommand DeleteCommand => new Command<object>(async (obj) =>
        {
            DataManager.Ins.RuleServices.DeleteRule((Rule)obj);
            DataManager.Ins.ListRule.Remove((Rule)obj);
            RuleList = DataManager.Ins.ListRule;
            BackupRuleList = DataManager.Ins.ListRule;
            DependencyService.Get<IToast>().ShortToast("Xóa thành công!");
        });
        public ICommand AddCommand => new Command<object>(async (obj) =>
        {
           
            var rand = new Random();
            string id = rand.Next(50, 1000).ToString();
            Rule newRule = new Rule("0", id, "0");
                DataManager.Ins.RuleServices.SendNewRule(id, "0", "0");
                DataManager.Ins.ListRule.Add(newRule);
                RuleList = DataManager.Ins.ListRule;
                BackupRuleList = DataManager.Ins.ListRule;
                DependencyService.Get<IToast>().ShortToast("Thêm thành công!");
         
          

        });


        private async void UpdateRule()
        {
            BackupRuleList = new ObservableCollection<Rule>();
            foreach (Rule ite in DataManager.Ins.ListRule)
            {
                await DataManager.Ins.RuleServices.UpdateRule(ite);
                BackupRuleList.Add(ite);
            }
           
        }

        private ObservableCollection<Rule> ruleList;
        public ObservableCollection<Rule> RuleList
        {
            get { return ruleList; }
            set
            {
                ruleList = value;
                OnPropertyChanged("RuleList");
            }
        }

        private ObservableCollection<Rule> backupruleList;
        public ObservableCollection<Rule> BackupRuleList
        {
            get { return backupruleList; }
            set
            {
                backupruleList = value;
                OnPropertyChanged("BackupRuleList");
            }
        }
    }
}