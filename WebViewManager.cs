namespace Aila;

public class WebViewManager: ContentView
{
    private Dictionary<string, WebView> _webViews = new();
    public WebView GetWebViewForUrl(string url) 
    {
        if (!_webViews.ContainsKey(url))
        {
            var webView = new WebView
            {
                UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/17.2.1 Safari/605.1.15",
                Source = new UrlWebViewSource { Url = url }
            };
            _webViews[url] = webView;
        }

        return _webViews[url];
    }
    
    // 新增方法：在指定的 WebView 中执行 JavaScript
    public async Task<string> EvaluateJavaScriptAsync(string url, string script)
    {
        if (_webViews.ContainsKey(url))
        {
            return await _webViews[url].EvaluateJavaScriptAsync(script);
        }
        throw new InvalidOperationException("WebView not found for the provided URL.");
    }
    
}