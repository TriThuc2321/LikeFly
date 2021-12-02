using LikeFly.View;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LikeFly
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
             MainPage = new MainPage();
            //MainPage = new NavigationPage(new DiscountManagerView());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
