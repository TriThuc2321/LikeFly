using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LikeFly.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LikeFly.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PilotView : ContentPage
    {
        public PilotView()
        {
            InitializeComponent();
            this.BindingContext = new PilotViewModel(Navigation, Shell.Current);
        }
    }
}