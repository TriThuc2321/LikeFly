using LikeFly.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LikeFly.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StaffManagerView : ContentPage
    {
        public StaffManagerView()
        {
            InitializeComponent();            
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this.BindingContext = new StaffManagerViewModel(Navigation, Shell.Current);
        }
    }
}