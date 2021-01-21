using System.Threading.Tasks;
using TestTustlySDK.Interfaces;
using Xamarin.Forms;

namespace TestTustlySDK.Helpers
{
    class AppServices
    {
        public static async Task<string> OpenGatewayPage(string url)
        {
            try
            {
                return await DependencyService.Get<IAppServices>().OpenGatewayPage(url);
            }
            catch { }
            return null;
        }
    }
}
