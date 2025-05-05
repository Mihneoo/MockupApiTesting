using System.Runtime.CompilerServices;
using MockupApiTesting.ViewModel;

namespace MockupApiTesting
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

        private void SearchUserButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserIdEntry.Text))
            {
                DisplayAlert("Search Error", "Wrong ID, please input a number", "Sige Dol");
            }
            return;
        }
    }

}
