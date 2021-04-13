using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using System;

namespace TestTustlySDK.Droid
{
    [Activity(Label = "PayTransferActivity", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
         ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop, ResizeableActivity = false)]
    [IntentFilter(new[] { Android.Content.Intent.ActionView },
                  DataSchemes = new[] { "trustlyOpenURLScheme" },
                  Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable })]
    public class PayTransferActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            var webview = (TrustlyWebView)FindViewById(Resource.Id.trustlyWebView);
            var url = Intent.GetStringExtra("url");
            webview.InitTrustlyView(this, url);

            webview.WebViewNavigated += Webview_WebViewNavigated;
            webview.WebViewNavigating += Webview_WebViewNavigating;
            var btnClose_page = (ImageButton)FindViewById(Resource.Id.btnClose_page);

            btnClose_page.Click += BtnClose_Click;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.PutExtra("url", "close");
            SetResult(Result.Ok, intent);
            Finish();
        }
        private void Webview_WebViewNavigating(object sender, string e)
        {
            string url = e;
            if (url.Contains("/transfer-success") || url.Contains("/transfer-pending"))
            {
                var intent = new Intent();
                intent.PutExtra("url", "success");
                SetResult(Result.Ok, intent);
                Finish();
            }
            else if (url.Contains("/transfer-failed"))
            {
                var intent = new Intent();
                intent.PutExtra("url", "error");
                SetResult(Result.Ok, intent);
                Finish();
            }
            else if (url.Contains("/my-transactions"))
            {
                var intent = new Intent();
                intent.PutExtra("url", "close");
                SetResult(Result.Ok, intent);
                Finish();
            }
        }

        private void Webview_WebViewNavigated(object sender, string e) { }

        public event System.Action<int, Result, Intent> ActivityResult;
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);
            if (this.ActivityResult != null)
                this.ActivityResult(requestCode, resultCode, intent);
        }

        public override void OnBackPressed()
        {
            var intent = new Intent();
            intent.PutExtra("url", "close");
            SetResult(Result.Ok, intent);
            base.OnBackPressed();
        }
    }
}