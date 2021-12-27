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
    public partial class DetailFlightView2 : ContentPage
    {
        public DetailFlightView2()
        {
            InitializeComponent();
            this.BindingContext = new DetailFlight2ViewModel(Navigation, Shell.Current);
        }
    }
}