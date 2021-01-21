using Android.App;
using Android.Content;
using Plugin.CurrentActivity;
using System.Threading.Tasks;
using TestTustlySDK.Droid.Native;
using TestTustlySDK.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(AppServices))]
namespace TestTustlySDK.Droid.Native
{
    public class AppServices : IAppServices
    {
        public async Task<string> OpenGatewayPage(string url)
        {
            var activity = (MainActivity)CrossCurrentActivity.Current.Activity;
            var listener = new ActivityResultListener(activity);

            Intent intent = new Intent(CrossCurrentActivity.Current.Activity, typeof(PayTransferActivity));
            intent.PutExtra("url", url);
            activity.StartActivityForResult(intent, 2);

            return await listener.Task;
        }
        private class ActivityResultListener
        {
            private TaskCompletionSource<string> Complete = new TaskCompletionSource<string>();

            public Task<string> Task { get { return this.Complete.Task; } }

            public ActivityResultListener(MainActivity activity)
            {
                activity.ActivityResult += OnActivityResult;
            }

            private void OnActivityResult(int requestCode, Result resultCode, Intent data)
            {
                try
                {
                    var url = data.GetStringExtra("url");
                    this.Complete.TrySetResult(url);
                }
                catch { }
            }
        }
    }
}