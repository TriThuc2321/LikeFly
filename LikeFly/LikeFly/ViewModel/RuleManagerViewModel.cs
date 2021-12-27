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
            foreach (Rule ite in DataManager.Ins.ListRule)
            {
                RuleList.Add(ite);
            }
        }

        public ICommand SaveCommand => new Command<object>(async (obj) =>
        {
            UpdateRule();
            DependencyService.Get<IToast>().ShortToast("Lưu thành công!");
        });
        public ICommand DeleteCommand => new Command<object>(async (obj) =>
        {
            DataManager.Ins.RuleServices.DeleteRule((Rule)obj);
            DataManager.Ins.ListRule.Remove((Rule)obj);
            RuleList = DataManager.Ins.ListRule;
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
            DependencyService.Get<IToast>().ShortToast("Thêm thành công!");

        });


        private async void UpdateRule()
        {
            foreach (Rule ite in DataManager.Ins.ListRule)
            {
                await DataManager.Ins.RuleServices.UpdateRule(ite);
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
    }
}