namespace AIMeta
{
    public partial class MainPage : ContentPage
    {
        string jsCode_Preload = @"

// 检查是否存在指定元素
const buttonElementExists = document.querySelector('.Markdown_markdownContainer__UyYrv > button');

if (!buttonElementExists) {
  // 如果指定元素不存在，则延迟3秒后执行下面的代码
  setTimeout(function() {
    document.querySelectorAll('.Markdown_markdownContainer__UyYrv').forEach((paragraphElement) => {
      const copyButton = document.createElement('button');
      copyButton.textContent = '复制';
      copyButton.style.marginLeft = '5px';
      paragraphElement.appendChild(copyButton);

      copyButton.addEventListener('click', function(event) {
        const targetParagraph = event.currentTarget.parentElement;
        const textToCopy = targetParagraph.textContent.trim();
        const tempInput = document.createElement('textarea');
        tempInput.value = textToCopy;
        document.body.appendChild(tempInput);
        tempInput.select();
        document.execCommand('copy');
        document.body.removeChild(tempInput);
      });
    });
  }, 3000);
}
";

        //string jsCode_Preload = @"(()=>{window.onload=function(){setTimeout(()=>{document.querySelectorAll('.Markdown_markdownContainer__UyYrv').forEach(e=>{const t=document.createElement('button');t.textContent='复制',t.style.marginLeft='5px',e.appendChild(t),t.addEventListener('click',t=>{const n=t.currentTarget.parentElement,o=n.textContent.trim(),c=document.createElement('textarea');c.value=o,document.body.appendChild(c),c.select(),document.execCommand('copy'),document.body.removeChild(c)})})},3e3)}})();";

        string jsCode_SendMSG = @"((textarea = document.querySelector('textarea'), lastValue = textarea.value, textarea.value = '[message]', inputEvent = new Event('input', { bubbles: true }), inputEvent.simulated = true, tracker = textarea._valueTracker, tracker ? tracker.setValue(lastValue) : void 0, textarea.dispatchEvent(inputEvent), enterKeyEvent = new KeyboardEvent('keydown', {key: 'Enter', code: 'Enter', keyCode: 13, which: 13, bubbles: true, cancelable: true }), textarea.dispatchEvent(enterKeyEvent)));";


        string jsCode_AppendLastElement = @"setTimeout(function() {
                                            function waitForElement(selector, callback) {
                                                const poll = setInterval(function() {
                                                    const el = document.querySelectorAll('.Button_buttonBase__0QP_m.Button_tertiary__yq3dG.ChatStopMessageButton_stopButton__LWNj6');
        
                                                    if (el.length <= 0) {
                                                        clearInterval(poll);
                                                        callback(el);
                                                    }
                                                }, 500); // 每半秒检查一次
                                            }

                                            waitForElement('button > img', function(el) {
	                                            const allMarkdownContainers = document.querySelectorAll('.Message_botMessageBubble__CPGMI');
	                                            const lastTwoContainers = Array.from(allMarkdownContainers).slice(-1); 

	                                            lastTwoContainers.forEach((paragraphElement) => {
	                                            const copyButton = document.createElement('button');
	                                            copyButton.textContent = '复制';
	                                            copyButton.style.marginLeft = '5px';
	                                            paragraphElement.appendChild(copyButton);

	                                            copyButton.addEventListener('click', function(event) {
	                                            const targetParagraph = event.currentTarget.parentElement;
	                                            const textToCopy = targetParagraph.textContent.trim();
	                                            const tempInput = document.createElement('textarea');
	                                            tempInput.value = textToCopy;
	                                            document.body.appendChild(tempInput);
	                                            tempInput.select();
	                                            document.execCommand('copy');
	                                            document.body.removeChild(tempInput);
	                                            });
                                                 });
                                            });
                                            }, 5000);
                                            ";

        List<WebView> webViews;
        List<WebView> gpt4WebViews;

        public MainPage()
        {
            InitializeComponent();

            webViews = new List<WebView>() { webview1, webview2, webview3,/* webview4,*/ webview5, /*webview6*/ };
            gpt4WebViews = new List<WebView>() { webview7, webview8, webview9 };

            //无需付费
            webview1.Source = new Uri("https://poe.com/ChatGPT");
            webview2.Source = new Uri("https://poe.com/Assistant");
            webview3.Source = new Uri("https://poe.com/Claude-instant");
            //webview4.Source = new Uri("https://poe.com/Claude-instant-100k");
            webview5.Source = new Uri("https://poe.com/Claude-2-100k");
            //webview6.Source = new Uri("https://poe.com/Google-PaLM");

            webview7.Source = new Uri("https://poe.com/GPT-4-32k"); 
            webview8.Source = new Uri("https://poe.com/GPT-4");
            webview9.Source = new Uri("https://poe.com/ChatGPT-16k"); 

            foreach (WebView wb in webViews)
            {
                wb.Navigated += async (s, e) =>
                {
                    if (e.Result == WebNavigationResult.Success)
                    {
                        await wb.EvaluateJavaScriptAsync(jsCode_Preload);
                    }
                };
            }
        }

        private async void SendMSG(string message)
        {
            foreach (var wb in webViews)
            {
                await wb.EvaluateJavaScriptAsync(jsCode_SendMSG.Replace("[message]", message));

                await wb.EvaluateJavaScriptAsync(jsCode_AppendLastElement);
            }
        }

        private async void ButtonSend_Clicked(object sender, EventArgs e)
        {
            if (myEditor.Text != null)
            {
                if (new VocabularyChecker().IsInVocabulary(myEditor.Text))
                {
                    await DisplayAlert("警告", "禁止输入违禁词!", "OK");

                    myEditor.Text = string.Empty;

                    return;
                }

                string message = myEditor.Text.Trim()
                    .Replace("\r", "\\n")
                    .Replace("\\", "\\\\")  // Replace \ with \\
                    .Replace("'", "\\'")    // Replace ' with \'
                    .Replace("\"", "\\\"")  // Replace " with \"
                    .Replace("\n", "\\\\n")
                    .Replace("\\n","\\\\n");   // Replace newline with \n

                SendMSG(message);
                myEditor.Text = string.Empty;

            }
        }

        int currentIndex = 0;
        private void ButtonSwitch_Clicked(object sender, EventArgs e)
        {
            // Set the currently visible WebView to invisible
            if (webViews[currentIndex].IsVisible)
            {
                webViews[currentIndex].IsVisible = false;
            }
            // Calculate the next index using modulo operator
            currentIndex = (currentIndex + 1) % webViews.Count;

            // Set the next WebView to visible
            if (!webViews[currentIndex].IsVisible)
            {
                webViews[currentIndex].IsVisible = true;
            }
        }

        int gptCurrentIndex = 0;
        private void ButtonGPT4Switch_Clicked(Object sender, EventArgs e)
        {
            foreach (var wb in webViews)
            {
                wb.IsVisible = false;
            }

            if (gpt4WebViews[gptCurrentIndex].IsVisible)
            {
                gpt4WebViews[gptCurrentIndex].IsVisible = false;
            }
            // Calculate the next index using modulo operator
            gptCurrentIndex = (gptCurrentIndex + 1) % gpt4WebViews.Count;

            // Set the next WebView to visible
            if (!gpt4WebViews[gptCurrentIndex].IsVisible)
            {
                gpt4WebViews[gptCurrentIndex].IsVisible = true;
            }
        }
    }
}