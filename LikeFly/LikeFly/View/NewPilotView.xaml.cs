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
    public partial class NewPilotView : ContentPage
    {
        public NewPilotView()
        {
            InitializeComponent();
            this.BindingContext = new NewPilotViewModel(Navigation, Shell.Current);
        }
    }
}