using Android.OS;
using Android.Support.CustomTabs;
using Android.Webkit;
using System;

namespace TestTustlySDK.Droid
{
    public class TrustlyWebChromeClient : WebChromeClient
    {
        public override bool OnCreateWindow(WebView view, bool isDialog, bool isUserGesture, Message resultMsg)
        {
            WeakReference<WebView> tabView =
                new WeakReference<WebView>(new WebView(view.Context));

            WebView webview;
            tabView.TryGetTarget(out webview);

            if (webview != null)
            {
                webview.SetWebViewClient(new Client());
                WebView.WebViewTransport transport = (WebView.WebViewTransport)resultMsg.Obj;
                transport.WebView = webview;
                resultMsg.SendToTarget();

                return true;
            }
            return false;
        }

        private class Client : WebViewClient
        {

            public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
            {
                CustomTabsIntent.Builder builder = new CustomTabsIntent.Builder();
                CustomTabsIntent customTab = builder.Build();
                customTab.LaunchUrl(view.Context, request.Url);

                return true;
            }
        }
    }
}
