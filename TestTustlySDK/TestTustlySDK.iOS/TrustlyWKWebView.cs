using CoreGraphics;
using Foundation;
using SafariServices;
using System;
using UIKit;
using WebKit;

namespace TestTustlySDK.iOS
{
    public class TrustlyWKWebView : UIView, IWKNavigationDelegate, IWKUIDelegate, ISFSafariViewControllerDelegate
    {
        public WKWebView TrustlyView;
        public event EventHandler<string> WebViewNavigating;

        public TrustlyWKWebView(string checkoutUrl, CGRect frame) : base(frame)
        {
            try
            {
                //Create a new content controller. 
                var userContentController = new WKUserContentController();
                //Create configuration
                var configuration = new WKWebViewConfiguration();
                configuration.UserContentController = userContentController;
                configuration.Preferences.JavaScriptCanOpenWindowsAutomatically = true;

                //Crate a webview
                TrustlyView = new WKWebView(frame, configuration);
                TrustlyView.NavigationDelegate = this;
                TrustlyView.UIDelegate = this;
                TrustlyView.NavigationDelegate = this;
                TrustlyView.UIDelegate = this;

                //Add the trustly ScriptOpenURLScheme as a handler for this contentcontroller 
                userContentController.AddScriptMessageHandler(new TrustlyWKScriptOpenURLScheme(TrustlyView), TrustlyWKScriptOpenURLScheme.NAME);

                var url = new NSUrl(checkoutUrl);

                TrustlyView.LoadRequest(new NSUrlRequest(url));
                TrustlyView.AllowsBackForwardNavigationGestures = true;

                AddSubview(TrustlyView);
                TrustlyView.TranslatesAutoresizingMaskIntoConstraints = false;
                TrustlyView.BottomAnchor.ConstraintEqualTo(TrustlyView.Superview.BottomAnchor, 0).Active = true;
                TrustlyView.TopAnchor.ConstraintEqualTo(TrustlyView.Superview.TopAnchor, 0).Active = true;
                TrustlyView.LeadingAnchor.ConstraintEqualTo(TrustlyView.Superview.LeadingAnchor, 0).Active = true;
                TrustlyView.TrailingAnchor.ConstraintEqualTo(TrustlyView.Superview.TrailingAnchor, 0).Active = true;
            }
            catch { }
        }

        public TrustlyWKWebView(NSCoder coder) { }


        [Export("webView:didStartProvisionalNavigation:")]
        public void DidStartProvisionalNavigation(WKWebView webView, WKNavigation navigation)
        {
            WebViewNavigating?.Invoke(this, webView.Url.AbsoluteString);
        }

        [Export("webView:didFinishNavigation:")]
        public void DidFinishNavigation(WKWebView webView, WKNavigation navigation) { }

        [Export("webView:createWebViewWithConfiguration:forNavigationAction:windowFeatures:")]
        public WKWebView CreateWebView(WKWebView webView, WKWebViewConfiguration configuration, WKNavigationAction navigationAction, WKWindowFeatures windowFeatures)
        {
            try
            {
                if (navigationAction.TargetFrame == null)
                {

                    if (UIApplication.SharedApplication.KeyWindow.RootViewController is UIViewController parentViewController && navigationAction.Request.Url is NSUrl url)
                    {
                        var safariView = new SFSafariViewController(url);
                        parentViewController.PresentViewController(safariView, true, null);
                        safariView.Delegate = this;
                    }
                }
            }
            catch { }
            return null;
        }
    }
}