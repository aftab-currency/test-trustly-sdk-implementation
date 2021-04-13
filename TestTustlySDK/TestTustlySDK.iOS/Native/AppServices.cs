using System.Threading.Tasks;
using TestTustlySDK.Interfaces;
using TestTustlySDK.iOS.Native;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(AppServices))]
namespace TestTustlySDK.iOS.Native
{
    public class AppServices : IAppServices
    {
        public static TaskCompletionSource<string> _tcs;
        public async Task<string> OpenGatewayPage(string url)
        {
            string result = "close";
            try
            {
                _tcs = new TaskCompletionSource<string>();
                var task = _tcs.Task;
                var window = UIApplication.SharedApplication.KeyWindow;
                var webView = new PayTransferViewController(url);
                webView.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
                await window.RootViewController.PresentViewControllerAsync(webView, true);
                result = await task;
            }
            catch { }
            return result;
        }
    }
}