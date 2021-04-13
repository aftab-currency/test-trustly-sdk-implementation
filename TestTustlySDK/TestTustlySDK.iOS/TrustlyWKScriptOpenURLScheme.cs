using Foundation;
using UIKit;
using WebKit;

namespace TestTustlySDK.iOS
{
    public class TrustlyWKScriptOpenURLScheme : NSObject, IWKScriptMessageHandler
    {
        public static string NAME = "trustlyOpenURLScheme";
        WKWebView webView;

        public TrustlyWKScriptOpenURLScheme(WKWebView webView)
        {
            this.webView = webView;
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            try
            {
                var parsed = GetParsedJSON(message.Body.Description as object);

                var callback = parsed["callback"].ToString();
                var urlscheme = parsed["urlscheme"].ToString();
                var appUrl = new NSUrl(urlscheme);

                var canOpenApplicationUrl = UIApplication.SharedApplication.CanOpenUrl(appUrl);

                if (canOpenApplicationUrl)
                {
                    UIApplication.SharedApplication.OpenUrl(appUrl);
                }

                var js = string.Format("{0}({1},\"{2}\");", callback, canOpenApplicationUrl.ToString().ToLowerInvariant(), urlscheme);

                webView.EvaluateJavaScript(js, null);
            }
            catch { }
        }

        /**
            Helper function that will try to parse AnyObject to JSON and return as NSDictionary
            :param: AnyObject
            :returns: JSON object as NSDictionary if parsing is successful, otherwise nil
            */
        private NSDictionary GetParsedJSON(object obj)
        {
            try
            {
                var jsonString = obj.ToString();
                var jsonData = NSData.FromString(jsonString, NSStringEncoding.UTF8);
                var parsed = NSJsonSerialization.Deserialize(jsonData, NSJsonReadingOptions.AllowFragments, out NSError error) as NSDictionary;

                return parsed;
            }
            catch { }
            return null;
        }
    }
}