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
    public partial class EditTicketTypeView : ContentPage
    {
        public EditTicketTypeView()
        {
            InitializeComponent();
            this.BindingContext = new EditTicketTypeViewModel(Navigation, Shell.Current);

        }
    }
}