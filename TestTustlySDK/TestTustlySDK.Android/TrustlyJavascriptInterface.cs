
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net;
using Android.Webkit;
using Java.Interop;
using Java.Lang;

namespace TestTustlySDK.Droid
{
    public class TrustlyJavascriptInterface : Object
    {
        public const string Name = "TrustlyAndroid";
        Activity activity;

        public TrustlyJavascriptInterface(Activity a)
        {
            activity = a;
        }

        /**
         * Will open the URL, then return result
         * @param String packageName
         * @param String URIScheme
         * @return boolean isOpened
         */
        [Export("openURLScheme")]
        [JavascriptInterface]
        public bool OpenURLScheme(Java.Lang.String packageName, Java.Lang.String URIScheme)
        {
            if (IsPackageInstalledAndEnabled(packageName.ToString(), activity))
            {
                Intent intent = new Intent();
                intent.SetPackage(packageName.ToString());
                intent.SetAction(Intent.ActionView);
                intent.SetData(Uri.Parse(URIScheme.ToString()));
                activity.StartActivityForResult(intent, 0);

                return true;
            }
            else
            {
                Intent intent = new Intent();
                intent.SetAction(Intent.ActionView);
                intent.SetData(Uri.Parse(URIScheme.ToString()));
                activity.StartActivityForResult(intent, 0);
            }

            return false;
        }

        /**
         * Helper function that will verify that URL can be opened, then return result
         * @param String packageName
         * @param Context context
         * @return boolean canBeOpened
         */
        private bool IsPackageInstalledAndEnabled(string packageName, Context context)
        {
            PackageManager pm = context.PackageManager;
            try
            {
                pm.GetPackageInfo(packageName, PackageInfoFlags.Activities);
                ApplicationInfo ai = context.PackageManager.GetApplicationInfo(packageName, 0);
                return ai.Enabled;
            }
            catch (PackageManager.NameNotFoundException ex)
            {

            }

            return false;
        }
    }
}
