using TestTustlySDK.Helpers;
using Xamarin.Forms;

namespace TestTustlySDK
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            btnGo.Clicked += async (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(txtUrl.Text))
                {
                    var result = await AppServices.OpenGatewayPage(txtUrl.Text);
                    if (result == "success")
                        await DisplayAlert("Success", "Transaction created successfully", "Ok");
                    else if (result == "error")
                        await DisplayAlert("Error", "Transaction failed to complete", "Ok");
                }
                else
                    await DisplayAlert("Required", "URL is required.", "Ok");
            };
        }
    }
}
