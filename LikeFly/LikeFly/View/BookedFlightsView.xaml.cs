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
    public partial class BookedFlightsView : ContentPage
    {
        public BookedFlightsView()
        {
            InitializeComponent();
            this.BindingContext = new BookedFlightsViewModel(Navigation, Shell.Current);
        }

        protected virtual void OnAppearing()
        {
            this.BindingContext = new BookedFlightsViewModel(Navigation, Shell.Current);
        }
    }
}