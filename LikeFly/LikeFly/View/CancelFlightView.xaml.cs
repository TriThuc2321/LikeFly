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
    public partial class CancelFlightView : ContentPage
    {
        public CancelFlightView()
        {
            InitializeComponent();
            this.BindingContext = new CancelFlightViewModel(Navigation, Shell.Current);
        }
    }
}