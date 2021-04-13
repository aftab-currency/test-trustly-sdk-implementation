using CoreGraphics;
using System;
using TestTustlySDK.iOS.Native;
using UIKit;

namespace TestTustlySDK.iOS
{
    public partial class PayTransferViewController : UIViewController
    {
        //The adress should be https.
        //Replace this with the correct trustly. 
        string URL = "";

        public PayTransferViewController(string webPageUrl)
        {
            URL = webPageUrl;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {
                var window = UIApplication.SharedApplication.KeyWindow;
                nfloat topPadding = window.SafeAreaInsets.Top;
                nfloat bottomPadding = window.SafeAreaInsets.Bottom;

                View.BackgroundColor = UIColor.White;

                UIStackView container = new UIStackView();
                View.AddSubview(container);
                container.Axis = UILayoutConstraintAxis.Vertical;
                container.TranslatesAutoresizingMaskIntoConstraints = false;
                container.Spacing = 15;
                container.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor).Active = true;
                container.TopAnchor.ConstraintEqualTo(TopLayoutGuide.GetBottomAnchor()).Active = true;
                container.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor).Active = true;
                container.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;


                UIStackView topBar = new UIStackView();
                container.AddArrangedSubview(topBar);
                topBar.Axis = UILayoutConstraintAxis.Horizontal;
                topBar.Distribution = UIStackViewDistribution.FillProportionally;
                topBar.Alignment = UIStackViewAlignment.Center;
                topBar.Spacing = 14;
                topBar.BackgroundColor = UIColor.White;
                topBar.TranslatesAutoresizingMaskIntoConstraints = false;
                topBar.LeadingAnchor.ConstraintEqualTo(container.LeadingAnchor, 8).Active = true;
                topBar.TopAnchor.ConstraintEqualTo(TopLayoutGuide.GetBottomAnchor(), 17).Active = true;
                topBar.TrailingAnchor.ConstraintEqualTo(container.TrailingAnchor).Active = true;

                UIButton btnCloseWebView = new UIButton(UIButtonType.System);
                btnCloseWebView.Frame = new CGRect(btnCloseWebView.Frame.GetMidX(), btnCloseWebView.Frame.GetMidY(), 140, 140);
                //btnCloseWebView.CenterYAnchor.ConstraintEqualTo(topBar.CenterYAnchor).Active = true;
                btnCloseWebView.SetImage(UIImage.FromBundle("close.png"), UIControlState.Normal);
                btnCloseWebView.TintAdjustmentMode = UIViewTintAdjustmentMode.Automatic;
                btnCloseWebView.TintColor = UIColor.FromRGB(147, 16, 25);
                btnCloseWebView.Layer.CornerRadius = 5;
                topBar.AddArrangedSubview(btnCloseWebView);
                btnCloseWebView.LeadingAnchor.ConstraintEqualTo(topBar.LayoutMarginsGuide.LeadingAnchor).Active = true;
                btnCloseWebView.TouchUpInside += async (sender, e) =>
                {
                    AppServices._tcs.SetResult("close");
                    await DismissViewControllerAsync(true);
                };

                var txtTitle = new UILabel();
                txtTitle.Text = "Transfer Funds";
                //txtTitle.CenterYAnchor.ConstraintEqualTo(topBar.CenterYAnchor).Active = true;
                txtTitle.Font = UIFont.FromName("PoppinsLatin-Bold", 17f);
                topBar.AddArrangedSubview(txtTitle);

                //Setup trustly
                //Webview pass in the correct trustly URL 
                //Pass in the frame size of the view that will hold it (note: we will anchor the view later to be correct fullscreen). 

                TrustlyWKWebView webView = new TrustlyWKWebView(URL, this.View.Frame);
                webView.WebViewNavigating += TrustlyView_Navigating;
                container.AddArrangedSubview(webView);
                webView.TranslatesAutoresizingMaskIntoConstraints = false;
                webView.LeadingAnchor.ConstraintEqualTo(container.LeadingAnchor).Active = true;
                webView.TrailingAnchor.ConstraintEqualTo(container.TrailingAnchor).Active = true;
                webView.BottomAnchor.ConstraintEqualTo(container.BottomAnchor, bottomPadding).Active = true;
            }
            catch { }
        }

        private async void TrustlyView_Navigating(object sender, string url)
        {
            try
            {
                if (url.Contains("/transfer-") || url.Contains("/my-transactions"))
                {
                    AppServices._tcs.SetResult(url);
                    await DismissViewControllerAsync(true);
                }
            }
            catch { }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.		
        }
    }
}
