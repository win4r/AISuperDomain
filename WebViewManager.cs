namespace Aila;

public class WebViewManager: ContentView
{
    private Dictionary<string, WebView> _webViews = new();
    private Dictionary<string, DateTime> _lastAccessTime = new();
    private const int WebViewCacheTimeoutMinutes = 10; // Dispose unused WebViews after 10 minutes

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

        // Update last access time
        _lastAccessTime[url] = DateTime.Now;

        return _webViews[url];
    }

    // 新增方法：在指定的 WebView 中执行 JavaScript
    public async Task<string> EvaluateJavaScriptAsync(string url, string script)
    {
        if (_webViews.ContainsKey(url))
        {
            // Update last access time
            _lastAccessTime[url] = DateTime.Now;
            return await _webViews[url].EvaluateJavaScriptAsync(script);
        }
        throw new InvalidOperationException("WebView not found for the provided URL.");
    }

    // Clean up unused WebViews to free memory
    public void CleanupUnusedWebViews()
    {
        var now = DateTime.Now;
        var urlsToRemove = new List<string>();

        foreach (var kvp in _lastAccessTime)
        {
            var timeSinceLastAccess = now - kvp.Value;
            if (timeSinceLastAccess.TotalMinutes > WebViewCacheTimeoutMinutes)
            {
                urlsToRemove.Add(kvp.Key);
            }
        }

        foreach (var url in urlsToRemove)
        {
            if (_webViews.TryGetValue(url, out var webView))
            {
                // Dispose of WebView resources
                webView.Source = null;
                _webViews.Remove(url);
            }
            _lastAccessTime.Remove(url);
        }
    }

    // Get count of cached WebViews for monitoring
    public int GetCachedWebViewCount() => _webViews.Count;

}