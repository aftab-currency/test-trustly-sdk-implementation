using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Webkit;
using System;

namespace TestTustlySDK.Droid
{
    public class TrustlyWebView : WebView
    {
        public event EventHandler<string> WebViewNavigating;
        public event EventHandler<string> WebViewNavigated;

        public TrustlyWebView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            System.Diagnostics.Debug.WriteLine("TrustlyWebView thingy ");
        }

        public TrustlyWebView(Activity activity, string url)
            : base(activity)
        {
            System.Diagnostics.Debug.WriteLine("TrustlyWebView");
            try
            {
                ConfigWebSettings(this);

                // Enable javascript and DOM Storage
                SetWebViewClient(new NavigationClient(this));
                SetWebChromeClient(new TrustlyWebChromeClient());

                // Add TrustlyJavascriptInterface
                AddJavascriptInterface(
                        new TrustlyJavascriptInterface(activity),
                        TrustlyJavascriptInterface.Name);

                /*LayoutParameters =
                     new LayoutParams(
                     new ViewGroup.LayoutParams(
                         ViewGroup.LayoutParams.MatchParent,
                         ViewGroup.LayoutParams.MatchParent));
                */
                LoadUrl(url);
            }
            catch (WebSettingsException ex)
            {
                System.Diagnostics.Debug.WriteLine("WebView",
                    "configWebView: Could not config WebSettings: " + ex.Message);
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("WebView",
                    "configWebView: Unknown Problem happened");
            }
        }

        public void InitTrustlyView(Activity activity, string url)
        {
            System.Diagnostics.Debug.WriteLine("InitTrustlyView");
            try
            {
                ConfigWebSettings(this);

                // Enable javascript and DOM Storage
                SetWebViewClient(new NavigationClient(this));
                SetWebChromeClient(new TrustlyWebChromeClient());

                // Add TrustlyJavascriptInterface
                AddJavascriptInterface(
                        new TrustlyJavascriptInterface(activity),
                        TrustlyJavascriptInterface.Name);

                /* LayoutParameters =
                     new LayoutParams(
                     new ViewGroup.LayoutParams(
                         ViewGroup.LayoutParams.MatchParent,
                         ViewGroup.LayoutParams.MatchParent));
                */
                LoadUrl(url);
            }
            catch (WebSettingsException ex)
            {
                System.Diagnostics.Debug.WriteLine("WebView",
                    "configWebView: Could not config WebSettings");
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("WebView",
                    "configWebView: Unknown Problem happened");
            }

        }

        private void ConfigWebSettings(TrustlyWebView mainView)
        {
            System.Diagnostics.Debug.WriteLine("ConfigWebSettings");
            try
            {
                WebSettings webSettings = mainView.Settings;
                webSettings.JavaScriptEnabled = true;
                webSettings.DomStorageEnabled = true;
                webSettings.JavaScriptCanOpenWindowsAutomatically = true;
                webSettings.SetSupportMultipleWindows(true);
            }
            catch (Exception ex)
            {
                throw new WebSettingsException(ex.Message);
            }
        }

        private class NavigationClient : WebViewClient
        {
            private readonly TrustlyWebView webView;

            public NavigationClient(TrustlyWebView webView)
            {
                this.webView = webView;
            }

            public override void OnPageStarted(WebView view, string url, Bitmap favicon)
            {
                base.OnPageStarted(view, url, favicon);
                System.Diagnostics.Debug.WriteLine("OnPageStarted");
                webView.WebViewNavigating.Invoke(webView, url);
            }

            public override void OnPageFinished(WebView view, string url)
            {
                base.OnPageFinished(view, url);
                System.Diagnostics.Debug.WriteLine("OnPageFinished");
                webView.WebViewNavigated.Invoke(webView, url);
            }
        }

    }
}