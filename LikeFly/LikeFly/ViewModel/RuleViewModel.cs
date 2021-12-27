using LikeFly.Core;
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
