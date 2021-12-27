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
    public partial class BookFlightView : ContentPage
    {
        public BookFlightView()
        {
            InitializeComponent();
            this.BindingContext = new BookFlightViewModel(Navigation);
        }
    }
}