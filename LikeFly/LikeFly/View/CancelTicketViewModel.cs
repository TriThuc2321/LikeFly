using Xamarin.Forms;

namespace LikeFly.View
{
    internal class CancelTicketViewModel
    {
        private INavigation navigation;
        private Shell current;

        public CancelTicketViewModel(INavigation navigation, Shell current)
        {
            this.navigation = navigation;
            this.current = current;
        }
    }
}