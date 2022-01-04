using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    public class RuleViewModel: ObservableObject
    {
        INavigation navigation;
        public Command Confirm { get; }
        public Command NavigationBack { get; }

        public RuleViewModel() { }

        public RuleViewModel (INavigation navigation)
        {
            this.navigation = navigation;
            Confirm = new Command(() => navigation.PopAsync());
            NavigationBack = new Command(() => navigation.PopAsync());

            Rule = "";
            initRule();
        }

        void initRule()
        {
            var rules = DataManager.Ins.ListRule;


            // Xếp ngày giảm dần
            for (int i = 0; i < rules.Count - 1; i++)
            {
                for (int j = i + 1; j < rules.Count; j++)
                {
                    if (int.Parse(rules[i].DayNum) < int.Parse(rules[j].DayNum))
                    {
                        Rule tmp = new Rule();
                        tmp = rules[i];
                        rules[i] = rules[j];
                        rules[j] = tmp;
                    }
                }
            }

            for (int i =0;i<rules.Count; i++)
            {
                if (i == 0) Rule = "\t\t\t\tNgay sau khi thanh toán hoặc trước " + rules[0].DayNum + " ngày: phí hủy " + rules[0].Deduct + "% tiền vé.\n";
                else
                    Rule += "\t\t\t\tHuỷ " + rules[i].DayNum + " trước khởi hành: Phí huỷ " + rules[i].Deduct + "% tiền vé\n";
            }    


        }

        private string rule;
        public string Rule { 
            set
            {
                rule = value;
                OnPropertyChanged("Rule");
            }
            get { return rule; }
        }
    }
}
