using LikeFly.Core;
using LikeFly.Database;
using LikeFly.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LikeFly.ViewModel
{
    public class RegisterViewModel : ObservableObject
    {
        INavigation navigation;
        Shell curentShell;
        public Command AddCommand { get; }
        public RegisterViewModel() { }
        public RegisterViewModel(INavigation navigation, Shell curentShell)
        {
            this.navigation = navigation;
            this.curentShell = curentShell;
            AddCommand = new Command(AddCommandhandler);
          

        }

        private void AddCommandhandler(object obj)
        {
            List<string> imgsource = new List<string>();
            imgsource.Add("aa");
            imgsource.Add("aaa");
            imgsource.Add("bbbb");
            DataManager.Ins.FlightsServices.AddFlight(new Flight("bbb", "aaa","aaa", imgsource, "sdwdwd"));
        }
    }
}
