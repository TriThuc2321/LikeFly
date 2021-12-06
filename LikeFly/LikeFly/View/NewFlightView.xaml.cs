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
    public partial class NewFlightView : ContentPage
    {
        public NewFlightView()
        {
            InitializeComponent();
            this.BindingContext = new NewFlightViewModel(Navigation, Shell.Current);
        }
    }
}