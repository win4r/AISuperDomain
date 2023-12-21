namespace AIIntegrator;

using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SixLabors.ImageSharp.Formats.Png;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

public partial class MainPage : ContentPage
{

    const string js_poe_AiPaint_hide_aside_js = "document.querySelectorAll('aside').forEach(function(aside){aside.style.display='none';});";
    const string js_poe_copybutton = "document.oncontextmenu=function(){return false;};document.querySelectorAll('aside').forEach(function(aside){aside.style.display='none';});setTimeout(function(){function waitForElement(selector,callback){const poll=setInterval(function(){const el=document.querySelectorAll('.ChatStopMessageButton_stopButton__QOW41');if(el.length<=0){callback(el);}},500);}waitForElement('button>img',function(el){const allMarkdownContainers=document.querySelectorAll('.ChatMessage_chatMessage__xkgHx:not(.processed)');const lastTwoContainers=Array.from(allMarkdownContainers).slice(-1);lastTwoContainers.forEach((paragraphElement)=>{const copyButton=document.createElement('button');copyButton.textContent='复制📑';copyButton.style.marginLeft='5px';paragraphElement.appendChild(copyButton);copyButton.addEventListener('click',function(event){const targetParagraph=event.currentTarget.parentElement;let childNodes=Array.from(targetParagraph.childNodes);childNodes=childNodes.filter(node=>!node.classList.contains('ChatMessage_botMessageHeader__8yA1R'));let textToCopy='';childNodes.forEach(node=>{textToCopy+=node.textContent;});textToCopy=textToCopy.replace('复制📑','');const tempInput=document.createElement('textarea');tempInput.value=textToCopy;document.body.appendChild(tempInput);tempInput.select();document.execCommand('copy');document.body.removeChild(tempInput);});paragraphElement.classList.add('processed');copyButton.classList.add('Button_buttonBase__Bv9Vx');});});},5000);";
    //禁用右键的JavaScript
    const string js_rightClick = ""; // @"(function() { document.oncontextmenu = function() { return false; }; })();";

    private const string _gitUpdate = "https://github.com/win4r/AISuperDomain/releases";

    List<string> _platformNames = new List<string>();

    //存储配置文件
    string filePath = string.Empty;
    string filePromptsPath = string.Empty;
    string fileWindowsCountPath = string.Empty;
    private string _windows_count = "0";

    private Action<string> toggleWindowsCount;



    Dictionary<string, string[]> dicConfiguration;

    Dictionary<string, string> dicPrompts;

    //string js_poe = $@"document.querySelector('textarea').value = '[message]';document.querySelector('textarea').dispatchEvent(new KeyboardEvent('keydown', {{ keyCode: 13, bubbles: true }}));";

    string js_bing_image = $@"(function(){{let t;document.querySelector('#sb_form_q').value='[message]';clearTimeout(t);t=setTimeout(()=>{{document.querySelector('#create_btn_c').click();}},1000);}})();";
    string js_poe = $@"((textarea = document.querySelector('textarea'), lastValue = textarea.value, textarea.value = '[message]', inputEvent = new Event('input', {{bubbles: true }}), inputEvent.simulated = true, tracker = textarea._valueTracker, tracker ? tracker.setValue(lastValue) : void 0, textarea.dispatchEvent(inputEvent), enterKeyEvent = new KeyboardEvent('keydown', {{key: 'Enter', code: 'Enter', keyCode: 13, which: 13, bubbles: true, cancelable: true }}), textarea.dispatchEvent(enterKeyEvent)));";
    string js_bito = $@"document.querySelector('#user_input1').value = '[message]';document.querySelector('#user_input1').dispatchEvent(new KeyboardEvent('keydown', {{ keyCode: 13, bubbles: true }}));";
    string js_huggingChat = $@"(document.querySelector('textarea').value = '[message]', document.querySelector('textarea').dispatchEvent(new Event('input', {{bubbles: true }})), document.querySelector('textarea').dispatchEvent(new KeyboardEvent('keydown', {{key: 'Enter' }})))";
    string js_bing_update = $@"(document.querySelector('cib-serp').shadowRoot.querySelector('#cib-action-bar-main').shadowRoot.querySelector('cib-text-input').shadowRoot.querySelector('#searchbox').value = '[message]', document.querySelector('cib-serp').shadowRoot.querySelector('#cib-action-bar-main').shadowRoot.querySelector('cib-text-input').shadowRoot.querySelector('#searchbox').dispatchEvent(new Event('input', {{bubbles: true }})), document.querySelector('cib-serp').shadowRoot.querySelector('#cib-action-bar-main').shadowRoot.querySelector('cib-text-input').shadowRoot.querySelector('#searchbox').dispatchEvent(new KeyboardEvent('keydown', {{key: 'Enter' }})));";
    string js_bing_compose = $@"(() => {{ document.querySelector('#prompt_text').value = '[message]'; document.querySelector('#prompt_text').dispatchEvent(new Event('input', {{bubbles: true}})); setTimeout(() => document.querySelector('#compose_button').click(), 500); }})();";

    //string js_bing = $@"(document.querySelector('cib-serp').shadowRoot.querySelector('#cib-action-bar-main').shadowRoot.querySelector('#searchbox').value = '[message]', document.querySelector('cib-serp').shadowRoot.querySelector('#cib-action-bar-main').shadowRoot.querySelector('#searchbox').dispatchEvent(new Event('input', {{bubbles: true }})), document.querySelector('cib-serp').shadowRoot.querySelector('#cib-action-bar-main').shadowRoot.querySelector('#searchbox').dispatchEvent(new KeyboardEvent('keydown', {{key: 'Enter' }})))";
    string js_bard = $@"(function(p, btn) {{ p.innerText = '[message]', setTimeout(function() {{ btn.click(); }}, 500); }})(document.querySelector('.ql-editor.textarea > p'), document.querySelector('.send-button.mdc-icon-button'));";
    string js_lmsys = $@"(function() {{ var buttons = document.querySelectorAll('button.svelte-kqij2n'); if(buttons.length >= 3) {{ buttons[2].click(); }} var elements = document.querySelectorAll('.svelte-1ed2p3z'); elements.forEach(function(element) {{ element.style.display = 'none'; }}); var textareas = document.querySelectorAll('textarea[data-testid=""textbox""]'); var buttons = [document.querySelector('#component-28'), document.querySelector('#component-65'), document.querySelector('#component-89')]; textareas.forEach(function(textarea) {{ textarea.value = '[message]'; textarea.dispatchEvent(new Event('input', {{ bubbles: true }})); }}); setTimeout(function() {{ buttons.forEach(function(button) {{ if(button) button.click(); }}); }}, 1000); }})();";
    string js_BLOOM = $@"((textarea = document.querySelector('textarea'), lastValue = textarea.value, textarea.value = '[message]', inputEvent = new Event('input', {{bubbles: true }}), inputEvent.simulated = true, tracker = textarea._valueTracker, tracker ? tracker.setValue(lastValue) : void 0, textarea.dispatchEvent(inputEvent), enterKeyEvent = new KeyboardEvent('keydown', {{key: 'Enter', code: 'Enter', keyCode: 13, which: 13, bubbles: true, cancelable: true }}), textarea.dispatchEvent(enterKeyEvent)));";
    //string js_StableLM = $@"((t) => (t.value = '[message]', t.dispatchEvent(new Event('input', {{bubbles: true}})), t.dispatchEvent(new KeyboardEvent('keydown', {{key: 'Enter'}})), document.querySelector('#component-10').click()))(document.querySelector('textarea'));";
    //string js_theb = $@"document.querySelector('textarea').value = '[message]';document.querySelector('textarea').dispatchEvent(new Event('input',{{bubbles:!0}}));document.querySelector('textarea').dispatchEvent(new KeyboardEvent('keydown',{{keycode:13}}));setTimeout(function(){{document.querySelector('.n-button__icon').click();document.querySelectorAll('button')[24].click()}},1e3);";
    string js_openai = $@"(function() {{ var textarea = document.querySelector('textarea'); textarea.value = '[message]'; textarea.dispatchEvent(new Event('input', {{bubbles: true}})); setTimeout(function() {{ textarea.dispatchEvent(new KeyboardEvent('keydown', {{key: 'Enter', code: 'Enter', keyCode: 13, charCode: 13, which: 13, bubbles: true, cancelable: true}})); }}, 1000); }})();";
    string js_slackClaude = $@"(function(){{let myDiv=document.querySelector('.ql-editor');let myP=myDiv.querySelector('p');let button=document.querySelector('.c-button-unstyled.c-icon_button.c-icon_button--size_small.c-wysiwyg_container__button.c-wysiwyg_container__button--send.c-icon_button--default');myP.textContent='[message]';let inputEvent=new Event('input',{{bubbles:true}});myP.dispatchEvent(inputEvent);myDiv.addEventListener('input',function(e){{}});setTimeout(function(){{button.click();}},1000);}})();";

    string js_llama = $@"((textarea = document.querySelector('textarea'), lastValue = textarea.value, textarea.value = '[message]', inputEvent = new Event('input', {{bubbles: true }}), inputEvent.simulated = true, tracker = textarea._valueTracker, tracker ? tracker.setValue(lastValue) : void 0, textarea.dispatchEvent(inputEvent), enterKeyEvent = new KeyboardEvent('keydown', {{key: 'Enter', code: 'Enter', keyCode: 13, which: 13, bubbles: true, cancelable: true }}), textarea.dispatchEvent(enterKeyEvent)));";

    //string js_claude = $@"(function(){{document.querySelector('.is-empty.is-editor-empty').innerHTML = '[message]'; setTimeout(function() {{ var elem = document.querySelector('.ProseMirror.p-4'); if (elem) {{ var event = new KeyboardEvent('keydown', {{ 'key': 'Enter', 'code': 'Enter', 'keyCode': 13, 'which': 13, 'bubbles': true, 'cancelable': true }}); elem.dispatchEvent(event); }} else {{ console.log(""Element not found""); }} }}, 1000);}}());";

    string js_claude2 = $@"(function(){{document.querySelector('.is-empty.is-editor-empty').innerHTML = '[message]'; setTimeout(function(){{var elem = document.querySelector('.ProseMirror p'); if (elem) {{var event = new KeyboardEvent('keydown', {{'key': 'Enter', 'code': 'Enter', 'keyCode': 13, 'which': 13, 'bubbles': true, 'cancelable': true}}); setTimeout(function(){{elem.dispatchEvent(event);}}, 500); setTimeout(function(){{var buttonElement = document.querySelector('button[aria-label=""Send Message""]'); if (buttonElement) {{buttonElement.click();}}}}, 1000);}}}}, 1000);}}());";

    string js_perplexityAI_characterAI = $@"(function(){{let a=document.querySelector('textarea'),b=Object.getOwnPropertyDescriptor(window.HTMLTextAreaElement.prototype,""value"").set;b.call(a,'[message]');let c=new Event('input',{{bubbles:true}});a.dispatchEvent(c);let d=new KeyboardEvent('keydown',{{bubbles:true,key:'Enter',keyCode:13}});a.dispatchEvent(d)}})();document.querySelector('.aspect-square.h-10').click();";

    private string js_ImaginewithMetaAI = $@"(function() {{ let textarea = document.querySelector('textarea'); let nativeInputValueSetter = Object.getOwnPropertyDescriptor(window.HTMLTextAreaElement.prototype, ""value"").set; nativeInputValueSetter.call(textarea, '[message]'); let inputEvent = new Event('input', {{ bubbles: true}}); textarea.dispatchEvent(inputEvent); let enterEvent = new KeyboardEvent('keydown', {{ bubbles: true, key: 'Enter', keyCode: 13 }}); textarea.dispatchEvent(enterEvent); }})();";


    string js_qianwen = $@"(function(){{let a=document.querySelector('textarea'),b=Object.getOwnPropertyDescriptor(window.HTMLTextAreaElement.prototype,'value').set;b.call(a,'[message]');a.dispatchEvent(new Event('input',{{bubbles:true}}));a.dispatchEvent(new KeyboardEvent('keydown',{{bubbles:true,key:'Enter',keyCode:13}}));}})();";
    string js_yiyan = $@"(function(){{let a=document.querySelector('textarea'),b=Object.getOwnPropertyDescriptor(window.HTMLTextAreaElement.prototype,'value').set;b.call(a,'[message]');a.dispatchEvent(new Event('input',{{bubbles:true}}));a.dispatchEvent(new KeyboardEvent('keydown',{{bubbles:true,key:'Enter',keyCode:13}}));document.querySelector("".VAtmtpqL"").click();}})();";
    string js_tiangong = $@"(function() {{ document.querySelector('textarea').value = '[message]'; document.querySelector('textarea').dispatchEvent(new Event('input', {{ bubbles: true }})); document.querySelector('textarea').dispatchEvent(new KeyboardEvent('keydown', {{ key: 'Enter' }})); document.querySelector('.sureSubmitDiv').click(); }})();";
    string js_xinghuo = $@"(function(){{ let a=document.querySelector('div#ask-window > textarea'),b=Object.getOwnPropertyDescriptor(window.HTMLTextAreaElement.prototype,'value').set;b.call(a,'[message]');a.dispatchEvent(new Event('input',{{ bubbles:true }}));a.dispatchEvent(new KeyboardEvent('keydown',{{ bubbles:true,key:'Enter',keyCode:13 }}));document.querySelector('div#ask-window > div.ask-window_send__xTavC').click(); }})();";
    string js_chatglm = $@"(function(){{var t=document.querySelector('textarea'),l=t.value,i=new Event('input',{{bubbles:true}});t.value='[message]',i.simulated=true,t._valueTracker&&t._valueTracker.setValue(l),t.dispatchEvent(i),t.dispatchEvent(new KeyboardEvent('keydown',{{key:'Enter',code:'Enter',keyCode:13,which:13,bubbles:true,cancelable:true}})),setTimeout(function(){{document.querySelector('div#search-input-box img').click();}},1000);}})();";
    string js_doubao = $@"(function(){{ let a=document.querySelector('div#root textarea'),b=Object.getOwnPropertyDescriptor(window.HTMLTextAreaElement.prototype,'value').set;b.call(a,'[message]');a.dispatchEvent(new Event('input',{{ bubbles:true }}));a.dispatchEvent(new KeyboardEvent('keydown',{{ bubbles:true,key:'Enter',keyCode:13 }})); }})();";

    string js_baiduAI = $@"((t) => {{ if(t) {{ t.value = '[message]'; t.dispatchEvent(new Event('input', {{bubbles: true}})); t.dispatchEvent(new KeyboardEvent('keydown', {{key: 'Enter', bubbles: true, cancelable: true}})); setTimeout(() => {{ const s = document.querySelector('.c-icon.text-input-send_2GgoL'); s && s.click(); }}, 1000); }} }})(document.querySelector('textarea'));";

    string js_perplexity_lab = $@"((textarea = document.querySelector('textarea'), lastValue = textarea.value, textarea.value = '[message]', inputEvent = new Event('input', {{bubbles: true }}), inputEvent.simulated = true, tracker = textarea._valueTracker, tracker ? tracker.setValue(lastValue) : void 0, textarea.dispatchEvent(inputEvent), enterKeyEvent = new KeyboardEvent('keydown', {{key: 'Enter', code: 'Enter', keyCode: 13, which: 13, bubbles: true, cancelable: true }}), textarea.dispatchEvent(enterKeyEvent)));";

    const string js_falcon_180b = @"((t) => (t.value = '[message]', t.dispatchEvent(new Event('input', {bubbles: true})), t.dispatchEvent(new KeyboardEvent('keydown', {key: 'Enter'})), document.querySelector('#component-17').click()))(document.querySelector('textarea'));";

    const string ini_js_lmsys = @"(function() { var intervalId = setInterval(function() { var elements = document.querySelectorAll('.svelte-1ed2p3z'); if(elements.length > 0) { elements.forEach(function(element) { element.style.display = 'none'; }); clearInterval(intervalId); } }, 2000); })();";


    // const string _ini_js_bingUnOfficial = "(function(){var checkElement = setInterval(function() { var elem = document.querySelector('.n-notification-container.n-notification-container--scrollable.n-notification-container--top-right'); if (elem) { elem.style.display = 'none'; clearInterval(checkElement); } }, 1000);}());";
    const string _ini_js_poe = "";
    const string _ini_js_perplexity_lab = "(function() {let select = document.getElementById('lamma-select'); select.value = '[item]'; let event = new Event('change', { bubbles: true, cancelable: true });select.dispatchEvent(event);})();";
    const string _ini_js_openai = "(document.querySelector('.flex-shrink-0.overflow-x-hidden.dark.bg-gray-900') && window.getComputedStyle(document.querySelector('.flex-shrink-0.overflow-x-hidden.dark.bg-gray-900')).visibility === 'visible') && (function() { var rectElement = document.querySelector('rect'); var event = new MouseEvent('click', { 'view': window, 'bubbles': true, 'cancelable': true }); rectElement.dispatchEvent(event); })();";
    const string _ini_js_bloom = "document.querySelector('header.MuiBox-root.mui-style-x4qroo').style.display = 'none';document.querySelector('div.MuiBox-root.mui-style-1mrd89u').style.display = 'none';";
    const string _ini_js_Bito = "document.getElementById('bitoai_title_content1').style.display = 'none';document.getElementById('signedInDivWeb').style.display = 'none'; document.querySelector('div[style*='text-align: center;font-size: 13px;color: #fff;padding-bottom: 5px']').style.display = 'none';";
    const string _ini_js_freechatgpt = "(function() { Array.from(document.getElementsByClassName('info')).forEach(function(element) { element.style.display = 'none'; }); })();";
    const string _ini_js_LLAM2 = $@"(function() {{ var intervalId = setInterval(function() {{ var tabNav = document.querySelector('.tab-nav'), noticeMarkdown = document.querySelector('#notice_markdown'), component25 = document.querySelector('#component-25'), builtWithSvelte = document.querySelector('.built-with.svelte-1lyswbr'); if (tabNav && noticeMarkdown && component25 && builtWithSvelte) {{ tabNav.style.display = 'none', noticeMarkdown.style.display = 'none', component25.style.display = 'none', builtWithSvelte.style.display = 'none', clearInterval(intervalId); }} }}, 1000); }})();";
    const string _ini_baiduAi = $@"(() => setTimeout(() => document.querySelector('.ai-entry-right').click(), 3000))();";
    const string _ini_falcon_180b = $@"(function check() {{ var el = document.querySelector('#component-6'); el ? el.style.display = 'none' : setTimeout(check, 1000); }})();";

    const string js_poe_getContent = @"(() => { let elements = document.querySelectorAll('.Markdown_markdownContainer__Tz3HQ'); return elements[elements.length - 1].textContent; })()";

    const string js_claude2_getContent = @"(() => { let elements = document.querySelectorAll('.contents'); return elements[elements.length - 2].textContent; })()";

    const string js_bard_getContent = @"(() => { let elements = document.querySelectorAll('.markdown.markdown-main-panel'); return elements[elements.length - 1].textContent; })()";

    const string js_llama2_getContent = @"(() => { let elements = document.querySelectorAll('.text-sm'); return elements[elements.length - 1].textContent; })()";

    const string js_openai_getContent = @"(() => (document.querySelectorAll('.markdown.prose')[document.querySelectorAll('.markdown.prose').length - 1].textContent))();";



    List<string> listCommonJs = new List<string>();

    List<WebView> webViews;

    List<WebView> webViews_Visiable = new();

    List<List<WebView>> webViewGroups;
    public MainPage()
    {
        InitializeComponent();

        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        // 存储AI名称的文件路径
        filePath = Path.Combine(documentsPath, "aime_taConfig.txt");
        filePromptsPath = Path.Combine(documentsPath, "ini_prompts_1.txt");

        fileWindowsCountPath = Path.Combine(documentsPath, "ini_windows_count.txt");

        //toggleWindowsCount = (fileWindowsCountPath) =>
        //{
        //    _windows_count = _windows_count == "0" ? "1" : "0";
        //    File.WriteAllText(fileWindowsCountPath, _windows_count);
        //};

        toggleWindowsCount = (fileWindowsCountPath) =>
                            File.WriteAllText(fileWindowsCountPath, _windows_count = _windows_count == "0" ? "1" : _windows_count == "1" ? "2" : "0");


        // 创建一个字典来保存 WebView 对象
        Dictionary<string, WebView> webViewsDic = new Dictionary<string, WebView>();

        //设置窗口数量配置文件，读取并存储到_windows_count里
        if (File.Exists(fileWindowsCountPath))
        {
            _windows_count = File.ReadAllText(fileWindowsCountPath).ToString();
        }
        else
        {
            File.WriteAllText(fileWindowsCountPath, "0");
        }


        Enumerable.Range(1, 24).ToList().ForEach(i => { var webView = new WebView(); Grid.SetRow(webView, 0); Grid.SetColumn(webView, _windows_count == "0" ? (i - 1) % 2 * 6 : _windows_count == "1" ? (i - 1) % 4 * 3 : (i - 1) % 6 * 2); myGrid.Children.Insert(0, webView); webViewsDic.Add($"webView_{i}", webView); });

        #region 初始化数据和提示词

        dicConfiguration = new Dictionary<string, string[]>()
        {
                       {"bing-DALLE", new string[] { "https://www.bing.com/create", "", "", js_bing_image, ""}},

            {"ImaginewithMetaAI", new string[] { "https://imagine.meta.com/", "", "", js_ImaginewithMetaAI, ""}},


            {"poe-Playground-v2", new string[] { "https://poe.com/Playground-v2", js_poe_AiPaint_hide_aside_js, _ini_js_poe, js_poe, ""}},

            {"poe-StableDiffusionXL", new string[] { "https://poe.com/StableDiffusionXL", js_poe_AiPaint_hide_aside_js, _ini_js_poe, js_poe, ""}},
            {"poe-Midjourneycreators", new string[] { "https://poe.com/Midjourneycreators", js_poe_AiPaint_hide_aside_js, _ini_js_poe, js_poe, ""}},
            {"poe-DSLR-SDXL", new string[] { "https://poe.com/DSLR-SDXL", js_poe_AiPaint_hide_aside_js, _ini_js_poe, js_poe, ""}},
            {"poe-VanGoghPaint", new string[] { "https://poe.com/VanGoghPaint", js_poe_AiPaint_hide_aside_js, _ini_js_poe, js_poe, ""}},
            {"poe-ImaginePixar", new string[] { "https://poe.com/ImaginePixar", js_poe_AiPaint_hide_aside_js, _ini_js_poe, js_poe, "" }},
            {"poe-Paper_model", new string[] { "https://poe.com/Paper_model", js_poe_AiPaint_hide_aside_js, _ini_js_poe, js_poe, "" }},
            {"poe-TattooArt", new string[] { "https://poe.com/TattooArt", js_poe_AiPaint_hide_aside_js, _ini_js_poe, js_poe, "" }},
            {"poe-EasyMovieScene", new string[] { "https://poe.com/EasyMovieScene", js_poe_AiPaint_hide_aside_js, _ini_js_poe, js_poe, "" }},
            {"poe-ImageReal", new string[] { "https://poe.com/ImageReal", js_poe_AiPaint_hide_aside_js, _ini_js_poe, js_poe, "" }},
            {"poe-ImgXL_AnimeArtwork", new string[] { "https://poe.com/ImgXL_AnimeArtwork", js_poe_AiPaint_hide_aside_js, _ini_js_poe, js_poe, "" }},
            {"poe-Photo_magic", new string[] { "https://poe.com/Photo_magic", js_poe_AiPaint_hide_aside_js, _ini_js_poe, js_poe, "" }},
            {"poe-ImgXL_Hyperrealistic", new string[] { "https://poe.com/ImgXL_Hyperrealistic", js_poe_AiPaint_hide_aside_js, _ini_js_poe, js_poe, "" }},
            {"poe-ImgXL_Pro3DModel", new string[] { "https://poe.com/ImgXL_Pro3DModel", js_poe_AiPaint_hide_aside_js, _ini_js_poe, js_poe, "" }},
            {"poe-Kashinano", new string[] { "https://poe.com/Kashinano", js_poe_AiPaint_hide_aside_js, _ini_js_poe, js_poe, "" }},
            {"poe-RealXL", new string[] { "https://poe.com/RealXL", js_poe_AiPaint_hide_aside_js, _ini_js_poe, js_poe, "" }},
            {"poe-AnimeXL", new string[] { "https://poe.com/AnimeXL", js_poe_AiPaint_hide_aside_js, _ini_js_poe, js_poe, "" }},
            {"poe-CinematicXL", new string[] { "https://poe.com/CinematicXL", js_poe_AiPaint_hide_aside_js, _ini_js_poe, js_poe, "" }},
            {"poe-VisualXL", new string[] { "https://poe.com/VisualXL", js_poe_AiPaint_hide_aside_js, _ini_js_poe, js_poe, "" }},
            {"poe-Model_ImagesE", new string[] { "https://poe.com/Model_ImagesE", js_poe_AiPaint_hide_aside_js, _ini_js_poe, js_poe, "" }},

            {"openai-dall-e", new string[] { "https://chat.openai.com/g/g-2fkFE8rbu-dall-e", _ini_js_openai, "", js_openai, "" }},

            {"lmsys", new string[] { "https://chat.lmsys.org/", "", ini_js_lmsys, js_lmsys, ""}},
            {"falcon-180b", new string[] { "https://tiiuae-falcon-180b-demo.hf.space/", "", _ini_falcon_180b, js_falcon_180b, ""}},
            {"poeChatGPT", new string[] {"https://poe.com/ChatGPT", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent}},
            {"poeGemini-Pro", new string[] {"https://poe.com/Gemini-Pro", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent}},
            {"poeMixtral-8x7B-Chat", new string[] {"https://poe.com/Mixtral-8x7B-Chat\n", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent}},
            {"poe-GPT-3.5-Turbo", new string[] { "https://poe.com/GPT-3.5-Turbo", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent}},
            {"poeGPT-3.5-Turbo-Instruct", new string[] { "https://poe.com/GPT-3.5-Turbo-Instruct", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent}},
            {"poeAssistant", new string[] {"https://poe.com/Assistant", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent}},
            {"poeClaude", new string[] { "https://poe.com/Claude-instant", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent }},
            {"poe-Web-Search", new string[] { "https://poe.com/Web-Search", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent }},
            {"poeClaude-instant-100k", new string[] { "https://poe.com/Claude-instant-100k", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent}},
            {"poeClaude2-100k", new string[] { "https://poe.com/Claude-2-100k", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent}},
            {"poePaLM", new string[] { "https://poe.com/Google-PaLM", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent}},
            {"poeLLAMA2", new string[] { "https://poe.com/Llama-2-70b", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent}},
            {"poeCode-Llama-34b", new string[] { "https://poe.com/Code-Llama-34b", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent}},
            {"poeCode-Llama-13b", new string[] { "https://poe.com/Code-Llama-13b", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent}},
            {"poeCode-Llama-7b", new string[] { "https://poe.com/Code-Llama-7b", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent}},
            {"poeGPT4", new string[] { "https://poe.com/GPT-4", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent}},
            {"poeChatGPT-16k", new string[] { "https://poe.com/ChatGPT-16k", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent }},
            {"poeGPT-4-32k", new string[] { "https://poe.com/GPT-4-32k", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent}},
            {"poeSolar-0-70b", new string[] { "https://poe.com/Solar-0-70b", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent}},
            {"poeChatGLM", new string[] { "https://poe.com/ChatGLM", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent }},
            {"poeVicuna-13B-V13", new string[] { "https://poe.com/Vicuna-13B-V13", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent}},

            {"poe-Dalle-3-Generator", new string[] { "https://poe.com/Dalle-3-Generator", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent }},
            {"poeMidjourney", new string[] { "https://poe.com/Midjourney", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent }},
            {"poeStableSDXL", new string[] { "https://poe.com/Stable-SDXL-prompter", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent }},
            {"poeDallE3_S", new string[] { "https://poe.com/DallE3_S", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent }},
            {"poe-DALLEbot", new string[] { "https://poe.com/DALLEbot", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent }},
            {"poeMidjourney1000", new string[] { "https://poe.com/Midjourney1000", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent }},


            {"poeHumanWrite", new string[] { "https://poe.com/HumanWrite", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent }},

            {"poe-fw-mistral-7b", new string[] { "https://poe.com/fw-mistral-7b", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent }},
            {"poe-CIaude-2-100k", new string[] { "https://poe.com/CIaude-2-100k", js_poe_copybutton, _ini_js_poe, js_poe, js_poe_getContent }},



            {"copilot", new string[] { "https://copilot.microsoft.com/", js_rightClick, "", js_bing_update, js_poe_getContent }},
            {"copilotMirror", new string[] { "https://harry-zklcdc-go-proxy-bingai.hf.space/web/?showconv=1#/", js_rightClick, "", js_bing_update, js_poe_getContent }},



            {"Bloom", new string[] { "https://api.together.xyz/bloom-chat?preview=1", js_rightClick, _ini_js_bloom, js_BLOOM, js_poe_getContent }},
            {"bingAI_creative", new string[] { "https://www.bing.com/search?q=Bing+AI&showconv=1&FORM=hpcodx&sydconv=1", js_rightClick, "", js_bing_update, js_poe_getContent }},
            {"bingAI_balance", new string[] { "https://www.bing.com/search?q=Bing+AI&showconv=1&FORM=hpcodx&sydconv=1", js_rightClick, "", js_bing_update, js_poe_getContent }},
            {"bing_chat_edge", new string[] { "https://edgeservices.bing.com/edgesvc/chat?udsframed=1&form=SHORUN&clientscopes=chat,noheader,udsedgeshop,channelstable,&setlang=zh-cn&lightschemeovr=1", js_rightClick, "", js_bing_update, "" }},
            {"bing_chat_edge_2", new string[] { "https://edgeservices.bing.com/edgesvc/chat?udsframed=1&form=SHORUN&clientscopes=chat,noheader,udsedgeshop,channelstable,&setlang=zh-cn&lightschemeovr=1", js_rightClick, "", js_bing_update, "" }},
            {"bing_chat_compose", new string[] { "https://edgeservices.bing.com/edgesvc/compose?udsframed=1&form=SHORUN&clientscopes=chat,noheader,coauthor,channelstable,&lightschemeovr=1&setlang=zh-cn", js_rightClick, "", js_bing_compose, "" }},

            {"perplexity-codellama-34b-instruct", new string[] { "https://labs.perplexity.ai/", "", _ini_js_perplexity_lab.Replace("[item]", "codellama-34b-instruct"), js_perplexity_lab, "" }},
            {"perplexity-llama-2-13b-chat", new string[] { "https://labs.perplexity.ai/", "", _ini_js_perplexity_lab.Replace("[item]", "llama-2-13b-chat"), js_perplexity_lab, "" }},
            {"perplexity-llama-2-70b-chat", new string[] { "https://labs.perplexity.ai/", "", _ini_js_perplexity_lab.Replace("[item]", "llama-2-70b-chat"), js_perplexity_lab, "" }},
            {"perplexity-mistral-7b-instruct", new string[] { "https://labs.perplexity.ai/", "", _ini_js_perplexity_lab.Replace("[item]", "mistral-7b-instruct"), js_perplexity_lab, "" }},

            {"Bito", new string[] { "https://alpha.bito.co/bitoai/", js_rightClick, _ini_js_Bito, js_bito, js_poe_getContent }},
            {"Bard", new string[] { "https://bard.google.com/chat", js_rightClick, "", js_bard, js_bard_getContent }},
            {"LLaMA2", new string[] { "https://llama2.ai/", js_rightClick, _ini_js_LLAM2, js_llama, js_llama2_getContent }},
            {"huggingChat", new string[] { "https://huggingface.co/chat/", js_rightClick, "", js_huggingChat, js_poe_getContent }},
            {"ChatGPTOpenAI", new string[] { "https://chat.openai.com/", _ini_js_openai, "", js_openai, js_openai_getContent }},
            {"GPT4OpenAI", new string[] { "https://chat.openai.com/?model=gpt-4", _ini_js_openai, "", js_openai, js_openai_getContent }},
            {"openai-chatgpt-classic", new string[] { "https://chat.openai.com/g/g-YyyyMT9XH-chatgpt-classic", _ini_js_openai, "", js_openai, js_openai_getContent }},
            {"GPTs-Discovery", new string[] { "https://chat.openai.com/gpts/discovery", _ini_js_openai, "", js_openai, js_openai_getContent }},


            {"OfficialClaude2", new string[] { "https://claude.ai/", js_rightClick, "", js_claude2, js_claude2_getContent }},
            {"perplexityAI", new string[] { "https://www.perplexity.ai/", js_rightClick, "", js_perplexityAI_characterAI, js_poe_getContent }},
            {"qianwen", new string[] { "https://qianwen.aliyun.com/chat", js_rightClick, "", js_qianwen, js_poe_getContent }},
            {"yiyan", new string[] { "https://yiyan.baidu.com/", js_rightClick, "", js_yiyan, js_poe_getContent }},
            {"tiangong", new string[] { "https://neice.tiangong.cn/interlocutionPage", js_rightClick, "", js_tiangong, js_poe_getContent }},
            {"xunfeixinghuo", new string[] { "https://xinghuo.xfyun.cn/desk", "", "", js_xinghuo, "" }},
            {"chatglm", new string[] { "https://chatglm.cn/detail", js_rightClick, "", js_chatglm, "" }},
            {"doubao", new string[] { "https://www.doubao.com/chat/", js_rightClick, "", js_doubao, "" }},
            {"baiduAI", new string[] { "https://www.baidu.com/", js_rightClick, _ini_baiduAi, js_baiduAI, "" }},

            {"agentGPT", new string[] { "https://agentgpt.reworkd.ai/zh", js_rightClick, "", "", "" }},
            {"autoGPT", new string[] { "https://autogpt.thesamur.ai/", js_rightClick, "", "", "" }},
            {"Skype", new string[] { "https://web.skype.com/", js_rightClick, "", "", "" }},
            {"Deepl", new string[] { "https://www.deepl.com/zh/translator", js_rightClick, "", "", "" }},
            {"bazi", new string[] { "https://ok.stoeng.site/hibazi", js_rightClick, "", "", "" }},
            {"ziwei", new string[] { "https://ok.stoeng.site/hiziwei", js_rightClick, "", "", "" }},
            {"stableaudio", new string[] { "https://stableaudio.com/generate", js_rightClick, "", "", "" }},
            {"stable-diffusion-XL", new string[] { "https://clipdrop.co/stable-diffusion", js_rightClick, "", "", "" }},

        };

        dicPrompts = new()
        {
            ["多AI文章整合"] = "请基于下面这两篇文章的主题、信息和情节等，将两篇文章进行整合修改，重新生成一篇新的文章，要做到将两篇文章融合为一篇新的原创文章，但不要直接复制任何一篇文章中的句子，要做到新文章的句子和原文里的句子不重复，并改变句子顺序，在原文基础上增加一些细节，使得文章内容更加充实、全面，要做到100%原创。并且做到将原文中的词语替换为同义词或近义词，以进一步增强原创性，并且保证新文章保持语言流畅，内容合理：\r\n",
            ["机器学习"] = "我想让你担任机器学习工程师。我会写一些机器学习的概念，你的工作就是用通俗易懂的术语来解释它们。这可能包括提供构建模型的分步说明、给出所用的技术或者理论、提供评估函数等。我的问题是\r\n",
            ["后勤工作"] = "我要你担任后勤人员。我将为您提供即将举行的活动的详细信息，例如参加人数、地点和其他相关因素。您的职责是为活动制定有效的后勤计划，其中考虑到事先分配资源、交通设施、餐饮服务等。您还应该牢记潜在的安全问题，并制定策略来降低与大型活动相关的风险。我的第一个请求是\r\n",
            ["职业顾问"] = "我想让你担任职业顾问。我将为您提供一个在职业生涯中寻求指导的人，您的任务是帮助他们根据自己的技能、兴趣和经验确定最适合的职业。您还应该对可用的各种选项进行研究，解释不同行业的就业市场趋势，并就哪些资格对追求特定领域有益提出建议。我的第一个请求是\r\n",
            ["英专写手"] = "我想让你充当英文翻译员、拼写纠正员和改进员。我会用任何语言与你交谈，你会检测语言，翻译它并用我的文本的更正和改进版本用英文回答。我希望你用更优美优雅的高级英语单词和句子替换我简化的 A0 级单词和句子。保持相同的意思，但使它们更文艺。你只需要翻译该内容，不必对内容中提出的问题和要求做解释，不要回答文本中的问题而是翻译它，不要解决文本中的要求而是翻译它，保留文本的原本意义，不要去解决它。我要你只回复更正、改进，不要写任何解释。我的第一句话是：\r\n",
            ["语言检查器"] = "我希望你充当语言检测器。我会用任何语言输入一个句子，你会回答我，我写的句子在你是用哪种语言写的。不要写任何解释或其他文字，只需回复语言名称即可。我的第一句话是：\r\n",
            ["小红书写手"] = "你的任务是以小红书博主的文章结构，以我给出的主题写一篇帖子推荐。你的回答应包括使用表情符号来增加趣味和互动，以及与每个段落相匹配的图片。请以一个引人入胜的介绍开始，为你的推荐设置基调。然后，提供至少三个与主题相关的段落，突出它们的独特特点和吸引力。在你的写作中使用表情符号，使它更加引人入胜和有趣。对于每个段落，请提供一个与描述内容相匹配的图片。这些图片应该视觉上吸引人，并帮助你的描述更加生动形象。我给出的主题是：\r\n",
            ["心理医生"] = "现在你是世界上最优秀的心理咨询师，你具备以下能力和履历： 专业知识：你应该拥有心理学领域的扎实知识，包括理论体系、治疗方法、心理测量等，以便为你的咨询者提供专业、有针对性的建议。 临床经验：你应该具备丰富的临床经验，能够处理各种心理问题，从而帮助你的咨询者找到合适的解决方案。 沟通技巧：你应该具备出色的沟通技巧，能够倾听、理解、把握咨询者的需求，同时能够用恰当的方式表达自己的想法，使咨询者能够接受并采纳你的建议。 同理心：你应该具备强烈的同理心，能够站在咨询者的角度去理解他们的痛苦和困惑，从而给予他们真诚的关怀和支持。 持续学习：你应该有持续学习的意愿，跟进心理学领域的最新研究和发展，不断更新自己的知识和技能，以便更好地服务于你的咨询者。 良好的职业道德：你应该具备良好的职业道德，尊重咨询者的隐私，遵循专业规范，确保咨询过程的安全和有效性。 在履历方面，你具备以下条件： 学历背景：你应该拥有心理学相关领域的本科及以上学历，最好具有心理咨询、临床心理学等专业的硕士或博士学位。 专业资格：你应该具备相关的心理咨询师执业资格证书，如注册心理师、临床心理师等。 工作经历：你应该拥有多年的心理咨询工作经验，最好在不同类型的心理咨询机构、诊所或医院积累了丰富的实践经验。\r\n",
            ["创业点子王"] = "在企业 B2B SaaS 领域中想 3 个创业点子。创业点子应该有一个强大而引人注目的使命，并以某种方式使用人工智能。避免使用加密货币或区块链。创业点子应该有一个很酷很有趣的名字。这些想法应该足够引人注目，这样投资者才会兴奋地投资数百万美元。\r\n",
            ["互联网写手"] = "你是一个专业的互联网文章作者，擅长互联网技术介绍、互联网商业、技术应用等方面的写作。\r\n接下来你要根据用户给你的主题，拓展生成用户想要的文字内容，内容可能是一篇文章、一个开头、一段介绍文字、文章总结、文章结尾等等。\r\n要求语言通俗易懂、幽默有趣，并且要以第一人称的口吻。\r\n",
            ["心灵导师"] = "从现在起你是一个充满哲学思维的心灵导师，当我每次输入一个疑问时你需要用一句富有哲理的名言警句来回答我，并且表明作者和出处\r\n\r\n\r\n要求字数不少于15个字，不超过30字，每次只返回一句且不输出额外的其他信息，你需要使用中文和英文双语输出\r\n\r\n\r\n当你准备好的时候只需要回复“我已经准备好了”（不需要输出任何其他内容）\r\n",
            ["文案携手"] = "我希望你充当文案专员、文本润色员、拼写纠正员和改进员，我会发送中文文本给你，你帮我更正和改进版本。我希望你用更优美优雅的高级中文描述。保持相同的意思，但使它们更文艺。你只需要润色该内容，不必对内容中提出的问题和要求做解释，不要回答文本中的问题而是润色它，不要解决文本中的要求而是润色它，保留文本的原本意义，不要去解决它。我要你只回复更正、改进，不要写任何解释。\r\n",
            ["Linux 终端"] = "我希望你能模拟一个 Linux 终端。我会输入命令，你会回答终端应该显示什么。我要求你仅在一个唯一的代码块内回答终端输出，不要写解释，除非我指示你这样做。当我需要用英语告诉你一些内容时，我会用花括号 {像这样} 将文本括起来。我的第一个命令是：\r\n",
            ["英语翻译和修正"] = "我希望你能模拟英语翻译、拼写修正和改进。我会用任何语言和你交流，你会检测语言，翻译它并用更优美、更优雅、更高层次的英语单词和句子替换我的简单 A0 级别的单词和句子。保持意思相同，但使它们更具文学性。我要求你仅回答修正和改进，不要写解释。我的第一个句子是：\r\n",
            ["模拟面试官"] = "我希望你能扮演一名面试官。我将作为候选人，你将问我关于 position 职位的面试问题。我要求你仅回答作为面试官的问题。不要一次性写下所有的交流。像面试官一样，逐个提问并等待我的答案。不要写解释。一个一个地问我问题，像面试官一样，并等待我的答案。我的第一个句子是：\r\n",
            ["充当旅游指南"] = "我希望你能充当旅游指南。我会告诉你我的位置，你会建议我附近的游览景点。在某些情况下，我还会告诉你我想参观的类型。你还会向我建议附近类型相似的地方。我的第一个请求是：\r\n",
            ["充当抄袭检测器"] = "我希望你能充当抄袭检测器。我会写一些句子，你只需回复在给定句子的语言中未被检测出的抄袭，不能写解释。我的第一个句子是：\r\n"
        };

        #endregion

        // 创建一个映射关系，每个索引对应一组 WebView

        if (_windows_count == "0")
        {
            webViewGroups = Enumerable.Range(0, 12)
                .Select(i => Enumerable.Range(1, 2)
                    .Select(j => webViewsDic[$"webView_{i * 2 + j}"])
                    .ToList())
                .ToList();
        }
        else if (_windows_count == "1")
        {
            webViewGroups = Enumerable.Range(0, 6)
                .Select(i => Enumerable.Range(1, 4)
                    .Select(j => webViewsDic[$"webView_{i * 4 + j}"])
                    .ToList())
                .ToList();
        }
        else
        {
            webViewGroups = Enumerable.Range(0, 4)
                .Select(i => Enumerable.Range(1, 6)
                    .Select(j => webViewsDic[$"webView_{i * 6 + j}"])
                    .ToList())
                .ToList();
        }


        webViews = Enumerable.Range(1, 24).Select(i => webViewsDic[$"webView_{i}"]).ToList();


        //单独加入common webview
        webViews.Add(webView_common);

        SetWebViewsVisibility(false);


        //写入提示词
        _ = PromptsFileManager();


        if (File.Exists(filePath))
        {
            _platformNames.AddRange(
                File.ReadAllTextAsync(filePath)
                    .Result
                    .Split(',')
                    .Select(s => s.Trim())  // 去除每个字符串两端的空白字符
                    .Where(s => !string.IsNullOrEmpty(s))  // 移除所有空字符串
            );
            if (_platformNames.Count > 0)
            {
                LoadAiUrl(_platformNames);
            }
        }
        else
        {
            //MyPopup.IsVisible = true;
            SelectAI.IsVisible = true;
        }

        AiSort.ReloadUrl += ReloadUrls;
        SelectAI.ReloadUrl += ReloadUrls;

        myPicker.SelectedIndex = 0;

        //myPickerSetting.SelectedIndex = 0;

        //提示词功能
        editor1.TextChanged += async (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.NewTextValue) && e.NewTextValue.StartsWith("/"))
            {
                editor1.Text = string.Empty;

                var keys = dicPrompts.Keys.ToArray();
                string action = await Application.Current.MainPage.DisplayActionSheet("👌常用提示词🤗", "关闭❌", null, keys);

                if (action != null && action != "关闭❌")
                {
                    editor1.Text = dicPrompts[action];
                }
            }

            if (!string.IsNullOrEmpty(e.NewTextValue) && e.NewTextValue.StartsWith("#"))
            {
                editor1.Text = string.Empty;

                var keys = dicPrompts.Keys.ToArray();
                string action = await Application.Current.MainPage.DisplayActionSheet("👌选择自定义指令🤗", "关闭❌", null, keys);

                if (action != null && action != "关闭❌")
                {
                    editor1.Text = "[#-#]" + dicPrompts[action] + "[#-#]";
                }
            }

            if (!string.IsNullOrEmpty(e.NewTextValue) && e.NewTextValue.StartsWith("@") && e.NewTextValue.Length >= 4)
            {
                string aiName = editor1.Text.Replace("@", "").ToLower();

                editor1.Text = string.Empty;

                if (!aiName.All(char.IsLetter))
                {
                    await Application.Current.MainPage.DisplayAlert("输入错误", "请输入英文名称", "确定", "取消");

                    return;
                }

                Dictionary<string, string> _poupAIPlatform = new();

                foreach (var _dic in dicConfiguration)
                {
                    if (_dic.Key.ToLower().Contains(aiName))
                    {
                        _poupAIPlatform.Add(_dic.Key, _dic.Value[0]);
                    }
                }

                if (_poupAIPlatform.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("提示", "未查询到对应AI", "确定", "取消");

                    return;
                }

                var keys = _poupAIPlatform.Keys.ToArray();

                string action = await Application.Current.MainPage.DisplayActionSheet("选择AI", "关闭", null, keys);

                if (action != null && action != "关闭")
                {
                    //先清空list
                    listCommonJs.Clear();
                    SetWebViewsVisibility(false);
                    //用webView_common加载独立唤出的AI
                    webView_common.IsVisible = true;
                    Grid.SetColumnSpan(webView_common, 12);
                    //将要执行发送的代码加入list里
                    listCommonJs.Add(dicConfiguration.TryGetValue(action, out var value) && value?.Length > 3 ? value[3] : null);
                    webView_common.Source = new Uri(dicConfiguration.TryGetValue(action, out var value1) ? value1[0] : null);

                    string _pre_js = dicConfiguration.TryGetValue(action, out var uniqueValue) && uniqueValue.Length > 2 ? uniqueValue[1] + uniqueValue[2] : null;
                    webView_common.Navigated += async (s, e) =>
                    {
                        await webView_common.EvaluateJavaScriptAsync(_pre_js);
                    };
                }
            }
        };
    }

    /// <summary>
    /// 在AiSort的ContentView给AI排序之后，重新加载Url
    /// </summary>
    private void ReloadUrls()
    {
        _platformNames.Clear();

        if (File.Exists(filePath))
        {
            _platformNames.AddRange(
                File.ReadAllTextAsync(filePath)
                    .Result
                    .Split(',')
                    .Select(s => s.Trim())  // 去除每个字符串两端的空白字符
                    .Where(s => !string.IsNullOrEmpty(s))  // 移除所有空字符串
            );
        }

        LoadAiUrl(_platformNames);
    }

    private async Task PromptsFileManager()
    {
        //判断是否存在提示词文件，如果存在则读取
        if (File.Exists(filePromptsPath))
        {
            // 文件存在，读取文件内容
            string _strprompts = await File.ReadAllTextAsync(filePromptsPath);
            dicPrompts = JsonConvert.DeserializeObject<Dictionary<string, string>>(_strprompts);
        }
        else
        {
            // 文件不存在，写入自定义内容
            string _strprompts = JsonConvert.SerializeObject(dicPrompts, Formatting.Indented);
            await File.WriteAllTextAsync(filePromptsPath, _strprompts);
        }
    }

    /// <summary>
    /// 用webview加载所选AI
    /// </summary>
    /// <returns></returns>
    private void LoadAiUrl(List<string> listPlatform)
    {
        myPicker.ItemsSource = null; //先设为null可以触发Picker的更新 否则可能更新失败

        myPicker.ItemsSource = listPlatform.Select((item, index) => new { item, index }).GroupBy(x => x.index / (_windows_count == "0" ? 2 : _windows_count == "1" ? 4 : 6)).Select(g => string.Join(", ", g.Select(x => x.item.ToString()))).ToList();

        myPicker.SelectedIndex = 0;

        //清空存储的可见webview
        webViews_Visiable.Clear();

        int index_webview = 0;
        foreach (var aiName in listPlatform)
        {
            //如果所选AI大于总webview的数量减去1(还有一个common webview) 则退出foreach
            if (index_webview == webViews.Count - 1)
            {
                break;
            }

            int currentWebViewIndex = index_webview;

            string[] aiConfig = dicConfiguration[aiName];

            webViews[currentWebViewIndex].Navigated += async (s, e) =>
            {
                //执行预设好的js，禁止右键和隐藏side
                await webViews[currentWebViewIndex].EvaluateJavaScriptAsync(aiConfig[1] + aiConfig[2]);
            };

            webViews[currentWebViewIndex].Source = aiConfig[0];
            webViews[currentWebViewIndex].IsVisible = true;
            webViews_Visiable.Add(webViews[currentWebViewIndex]);

            //设置webview的跨列数目
            Grid.SetColumnSpan(webViews[currentWebViewIndex], _windows_count == "0" ? 6 : _windows_count == "1" ? 3 : 2);

            index_webview++;
        }

        if (_platformNames.Count == 1)
        {
            webViews[1].IsVisible = false;
            // 设置 WebView 的 Grid.ColumnSpan 为 4
            Grid.SetColumnSpan(webViews[0], 12);
        }
    }

    private void myPopupShowPromptsSettingPopup()
    {
        MyPopupPromptsSetting.IsVisible = true;

        foreach (var entry in dicPrompts)
        {
            StackLayout rowLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 0
            };

            Editor keyEditor = new Editor
            {
                Text = entry.Key,
                TextColor = Microsoft.Maui.Graphics.Colors.Green,
                WidthRequest = 100,
                HorizontalOptions = LayoutOptions.Start
            };

            Editor valueEditor = new Editor
            {
                Text = entry.Value,
                TextColor = Microsoft.Maui.Graphics.Colors.Green,
                WidthRequest = 800,
                HorizontalOptions = LayoutOptions.Start
            };

            Button deleteButton = new Button
            {
                Text = "删除",
                BackgroundColor = Microsoft.Maui.Graphics.Colors.Orange,
                TextColor = Microsoft.Maui.Graphics.Colors.White,
                WidthRequest = 60,
                HeightRequest = 40
            };

            deleteButton.Clicked += (s, e) =>
            {
                MyStackLayout.Children.Remove(rowLayout);
            };

            rowLayout.Children.Add(keyEditor);
            rowLayout.Children.Add(valueEditor);
            rowLayout.Children.Add(deleteButton); // Add the delete button into the rowLayout

            if (MyStackLayout.Children.Count > 0)
            {
                MyStackLayout.Children.Insert(MyStackLayout.Children.Count - 1, rowLayout);
            }
            else
            {
                MyStackLayout.Children.Add(rowLayout);
            }
        }
    }
    private void OnAddEditorClicked(object sender, EventArgs e)
    {
        StackLayout rowLayout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Spacing = 0
        };

        Editor keyEditor = new Editor
        {
            Text = "",
            WidthRequest = 100,
            Placeholder = "💟输入标题",
            PlaceholderColor = Microsoft.Maui.Graphics.Colors.Green,
            TextColor = Microsoft.Maui.Graphics.Colors.Green,
            HorizontalOptions = LayoutOptions.Start
        };

        Editor valueEditor = new Editor
        {
            Text = "",
            WidthRequest = 800,
            Placeholder = "👉输入提示词具体内容",
            PlaceholderColor = Microsoft.Maui.Graphics.Colors.Green,
            TextColor = Microsoft.Maui.Graphics.Colors.Green,
            HorizontalOptions = LayoutOptions.Start
        };

        Button deleteButton = new Button
        {
            Text = "删除",
            BackgroundColor = Microsoft.Maui.Graphics.Colors.Orange,
            TextColor = Microsoft.Maui.Graphics.Colors.White,
            WidthRequest = 60,
            HeightRequest = 40

        };

        deleteButton.Clicked += (s, e) =>
        {
            MyStackLayout.Children.Remove(rowLayout);
        };

        rowLayout.Children.Add(keyEditor);
        rowLayout.Children.Add(valueEditor);
        rowLayout.Children.Add(deleteButton); // Add the delete button into the rowLayout

        if (MyStackLayout.Children.Count > 0)
        {
            MyStackLayout.Children.Insert(MyStackLayout.Children.Count - 1, rowLayout);
        }
        else
        {
            MyStackLayout.Children.Add(rowLayout);
        }
    }

    private async void OnMyPopupPromptsSettingConfirmClicked(object sender, EventArgs e)
    {
        dicPrompts.Clear();  // clear the dictionary to prepare for the new entries

        List<StackLayout> rowsToRemove = new List<StackLayout>();

        foreach (StackLayout rowLayout in MyStackLayout.Children)
        {
            if (rowLayout.Children[0] is Editor keyEditor && rowLayout.Children[1] is Editor valueEditor)
            {
                string key = keyEditor.Text;
                string value = valueEditor.Text;

                // It's important to ensure the key is not null or empty, and is not already in the dictionary
                if (!string.IsNullOrEmpty(key) && !dicPrompts.ContainsKey(key))
                {
                    dicPrompts[key] = value;
                }

                rowsToRemove.Add(rowLayout);  // add the row to the list of rows to remove
            }
        }

        foreach (StackLayout row in rowsToRemove)
        {
            MyStackLayout.Children.Remove(row);  // remove the row from MyStackLayout
        }

        // You can now hide the popup if you want
        MyPopupPromptsSetting.IsVisible = false;

        string _strPrompts = JsonConvert.SerializeObject(dicPrompts, Formatting.Indented);
        await File.WriteAllTextAsync(filePromptsPath, _strPrompts);
    }

    private void OnMyPopupPromptsSettingCancelClicked(object sender, EventArgs e)
    {
        List<StackLayout> rowsToRemove = new List<StackLayout>();

        foreach (StackLayout rowLayout in MyStackLayout.Children)
        {
            if (rowLayout.Children[0] is Editor keyEditor && rowLayout.Children[1] is Editor valueEditor)
            {
                rowsToRemove.Add(rowLayout);  // add the row to the list of rows to remove
            }
        }

        foreach (StackLayout row in rowsToRemove)
        {
            MyStackLayout.Children.Remove(row);  // remove the row from MyStackLayout
        }

        MyPopupPromptsSetting.IsVisible = false;
    }

    /// <summary>
    /// 检查更新
    /// </summary>
    private async Task<string> GetStringAsyncWithRetries(string url, int maxRetries = 3)
    {
        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                using var client = new HttpClient();
                return await client.GetStringAsync(url);
            }
            catch
            {
                if (i == maxRetries - 1) throw;  // rethrow the last exception
            }
        }

        return null;  // should not reach here
    }

    private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        // 首先将所有 WebView 设置为不可见
        SetWebViewsVisibility(false);

        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex == -1)
        {
            return;
        }

        var selectedGroup = webViewGroups[selectedIndex];
        foreach (var webView in selectedGroup)
        {
            webView.IsVisible = true;
            //Grid.SetColumnSpan(webView, 1);

        }
        //当picker中只有一项时，将第二个webview隐藏，以便让第一个webview全屏显示
        int pickerCout = picker.Items.Count;
        if (pickerCout == 1)
        {
            webViews[1].IsVisible = false;
        }
    }
    private void OnWebViewRefreshClicked(object sender, EventArgs e)
    {
        var item = sender as MenuFlyoutItem;
        var webView = item?.CommandParameter as WebView;

        if (webView != null)
        {
            webView.Reload();
        }
    }

    private async void OnWebViewTranslateClicked(object sender, EventArgs e)
    {
        var jsTranslate = @"let textToTranslate = '', selection = window.getSelection(); if (selection) { textToTranslate = selection.toString(); } var modal = document.createElement('div'), content = document.createElement('div'), closeButton = document.createElement('span'), text = document.createElement('p'), copyIcon = document.createElement('span'); modal.style.display = 'none'; modal.style.position = 'fixed'; modal.style.zIndex = '1'; modal.style.paddingTop = '100px'; modal.style.left = '0'; modal.style.top = '0'; modal.style.width = '100%'; modal.style.height = '100%'; modal.style.overflow = 'auto'; modal.style.backgroundColor = 'rgba(0,0,0,0.4)'; content.style.backgroundColor = '#fefefe'; content.style.margin = 'auto'; content.style.padding = '20px'; content.style.border = '1px solid #888'; content.style.width = '80%'; closeButton.style.color = '#aaaaaa'; closeButton.style.float = 'right'; closeButton.style.fontSize = '28px'; closeButton.style.fontWeight = 'bold'; closeButton.textContent = '×'; closeButton.onclick = function() { modal.style.display = 'none'; }; closeButton.onmouseover = function() { closeButton.style.color = '#000'; }; closeButton.onmouseout = function() { closeButton.style.color = '#aaaaaa'; }; content.appendChild(closeButton); content.appendChild(text); modal.appendChild(content); document.body.appendChild(modal); function showModal() { modal.style.display = 'block'; } function hideModal() { modal.style.display = 'none'; } window.onclick = function(event) { if (event.target == modal) { hideModal(); } }; copyIcon.style.color = '#aaaaaa'; copyIcon.style.float = 'right'; copyIcon.style.fontSize = '28px'; copyIcon.style.fontWeight = 'bold'; copyIcon.style.marginRight = '15px'; copyIcon.textContent = '⎘'; copyIcon.onclick = function() { navigator.clipboard.writeText(text.textContent).then(function() {}, function(err) {}); }; content.insertBefore(copyIcon, closeButton); textToTranslate = encodeURIComponent(textToTranslate); var url = 'https://translate.googleapis.com/translate_a/single?dt=t&dt=bd&dt=qc&dt=rm&dt=ex&client=gtx&hl=en&sl=auto&tl=zh-cn&q=' + textToTranslate + '&dj=1'; fetch(url).then(response => response.json()).then(data => { var translatedText = data.sentences.map(sentence => sentence.trans).join(' '); text.textContent = translatedText; showModal(); });";

        var item = sender as MenuFlyoutItem;
        var webView = item?.CommandParameter as WebView;

        if (webView != null)
        {
            await webView.EvaluateJavaScriptAsync(jsTranslate);
        }
    }

    private async void OnWebViewCopyClicked(object sender, EventArgs e)
    {
        var item = sender as MenuFlyoutItem;
        var webView = item?.CommandParameter as WebView;

        if (webView != null)
        {
            string js = "function copySelectedText() { var selectedText = window.getSelection().toString(); if (selectedText !== '') { var el = document.createElement('textarea'); el.value = selectedText; document.body.appendChild(el); el.select(); document.execCommand('copy'); document.body.removeChild(el); return '已复制到剪贴板'; } else { return '未选定任何文本'; }}; copySelectedText();";
            await webView.EvaluateJavaScriptAsync(js);
        }
    }

    private async void TranslateAndSendButton_Clicked(object sender, EventArgs e)
    {
        if (editor1.Text != null)
        {
            if (new VocabularyChecker().IsInVocabulary(editor1.Text))
            {
                await DisplayAlert("警告", "禁止输入违禁词!", "OK");

                editor1.Text = string.Empty;

                return;
            }

            string rspJason = string.Empty;

            string msg = editor1.Text.Trim()
                 .Replace("\r", "\\\\n")
                 .Replace("\\", "\\\\")  // Replace \ with \\
                 .Replace("'", "\\'")    // Replace ' with \'
                 .Replace("\"", "\\\"")  // Replace " with \"
                 .Replace("\n", "\\\\n");   // Replace newline with \n;

            string encode = System.Net.WebUtility.UrlEncode(msg);

            string url = $@"https://translate.googleapis.com/translate_a/single?dt=t&dt=bd&dt=qc&dt=rm&dt=ex&client=gtx&hl=en&sl=auto&tl=en&q={encode}&dj=1";

            try
            {
                rspJason = await GetStringAsyncWithRetries(url, 1);

            }
            catch (Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("网络异常提示", "无法访问谷歌翻译接口，请检查你的梯子魔法是否是全局模式。详情: " + ex.Message, "确定");
                });
                return;
            }

            string message = ExtractTrans(rspJason).Trim();

            SendMSG(message);
        }
    }

    /// <summary>
    /// 提取翻译内容
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    private string ExtractTrans(string json)
    {
        JObject jObject = JObject.Parse(json);
        JArray sentences = (JArray)jObject["sentences"];

        StringBuilder sb = new StringBuilder();
        foreach (JObject sentence in sentences)
        {
            if (sentence["trans"] != null)
            {
                string trans = sentence["trans"].ToString();
                sb.AppendLine(trans);
            }
        }

        return sb.ToString()
                .Replace("\\\\n", "")
                .Replace("\r", "\\n")
                .Replace("\\", "\\\\")  // Replace \ with \\
                .Replace("'", "\\'")    // Replace ' with \'
                .Replace("\"", "\\\"")  // Replace " with \"
                .Replace("\n", "\\\\n") // Replace newline with \n.Trim();
                .Replace("\\n", "")
                .Replace("\\\\", "");  // Replace \\ with empty
    }

    private string ExtractStr(string s, string startToken, string endToken)
    {
        int startIndex = s.IndexOf(startToken) + startToken.Length;
        int endIndex = s.IndexOf(endToken, startIndex);
        return s.Substring(startIndex, endIndex - startIndex);
    }

    private async void SendButton_Clicked(object sender, EventArgs e)
    {
        if (editor1.Text != null)
        {
            string editorStr = editor1.Text.Trim();

            if (new VocabularyChecker().IsInVocabulary(editorStr))
            {
                await DisplayAlert("警告", "禁止输入违禁词!", "OK");
                return;
            }

            string message = editor1.Text.Trim().Replace("[#-#]", "")
                .Replace("\r", "\\n")
                .Replace("\\", "\\\\")  // Replace \ with \\
                .Replace("'", "\\'")    // Replace ' with \'
                .Replace("\"", "\\\"")  // Replace " with \"
                .Replace("\n", "\\\\n");   // Replace newline with \n

            SendMSG(message);

            if (editorStr.Contains("[#-#]"))
            {
                editor1.Text = string.Empty;
                editor1.Text = "[#-#]" + ExtractStr(editorStr, "[#-#]", "[#-#]") + "[#-#]" + "\r\n";
            }
            else
            {
                editor1.Text = string.Empty;

            }
        }
    }

    private void SendMSG(string message)
    {
        if (webView_common.IsVisible)
        {
            string _js_webview = listCommonJs[0].Replace("[message]", message);
            webView_common.EvaluateJavaScriptAsync(_js_webview);
            editor1.Text = string.Empty;

            return;
        }

        int flag = 0;
        foreach (var platformName in _platformNames)
        {
            string _js_send = dicConfiguration[platformName][3].Replace("[message]", message);
            webViews_Visiable[flag].EvaluateJavaScriptAsync(_js_send);

            flag++;
        }
    }

    /// <summary>
    /// 设置所有的webview不可见
    /// </summary>
    /// <param name="isVisible"></param>
    private void SetWebViewsVisibility(bool isVisible)
    {
        foreach (var webView in webViews)
        {
            webView.IsVisible = isVisible;
        }
    }


    private async void SwitchButton_Clicked(object sender, EventArgs e)
    {
        if (!File.Exists(filePath))
        {
            await DisplayAlert("提示", "您未选择任何AI，请在设置里选择AI", "确定");
            return;
        }

        if (myPicker.Items.Count > 0)
        {
            // Change the SelectedIndex or SelectedItem of your picker
            // This will trigger the OnPickerSelectedIndexChanged event
            myPicker.SelectedIndex = (myPicker.SelectedIndex + 1) % myPicker.Items.Count;
        }
    }
    #region 已注销的选择功能按钮

    //private async void myPickerSetting_PickerSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    var picker = (Picker)sender;
    //    int selectedIndex = picker.SelectedIndex;

    //    if (selectedIndex == 0)
    //    {
    //        return;
    //    }
    //    if (selectedIndex == 1)
    //    {
    //        //MyPopup.IsVisible = true;

    //        await SelectAI.Reset();
    //        SelectAI.IsVisible = true;

    //    }
    //    if (selectedIndex == 2)
    //    {
    //        await AiSort.Reset();
    //        AiSort.IsVisible = true;
    //    }

    //    if (selectedIndex == 3)
    //    {
    //        MyPopupStableDiffusionSetting.IsVisible = true;
    //    }

    //    if (selectedIndex == 4)
    //    {
    //        toggleWindowsCount(fileWindowsCountPath);

    //        Application.Current.MainPage = new NavigationPage(new MainPage());


    //        //Application.Current.MainPage = new MainPage();



    //        //await Navigation.PopAsync();
    //        //await Navigation.PushAsync(new MainPage());


    //    }

    //    if (selectedIndex == 5)
    //    {
    //        myPopupShowPromptsSettingPopup();
    //    }

    //    if (selectedIndex == 6)
    //    {
    //        if (TranslateAndSendButton.IsVisible)
    //        {
    //            TranslateAndSendButton.IsVisible = false;
    //        }
    //        else
    //        {
    //            TranslateAndSendButton.IsVisible = true;
    //        }
    //    }
    //    if (selectedIndex == 7)
    //    {
    //        await Browser.OpenAsync(_gitUpdate);
    //    }
    //    if (selectedIndex == 8)
    //    {
    //        var result = await Application.Current.MainPage.DisplayAlert("🤗关于作者", "作者微信: stoeng 哔哩哔哩频道：AI超元域  如有问题请咨询作者🤝🤝", "确定", "取消");

    //        if (result)
    //        {
    //            //打开默认浏览器
    //            await Browser.OpenAsync("https://space.bilibili.com/3493277319825652");
    //        }
    //    }

    //    //恢复默认显示
    //    //myPickerSetting.SelectedIndex = 0;
    //}


    #endregion

    /// <summary>
    /// 对接google翻译接口
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task<string> GetHttpResponseAsync(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36 Edg/113.0.1774.50");

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
            else
            {
                await DisplayAlert("错误提示", "翻译请求过于频繁，请稍后再试", "确定");
                return "ERROR";
            }
        }
    }

    private async void GenerateImageStableDifussionButtonClick(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(MyEditorImagePrompts.Text))
        {
            await DisplayAlert("提示", "提示词不能为空！请输入提示词！", "OK");

            return;
        }

        MyPopupStableDiffusionSetting.IsVisible = false;

        SetWebViewsVisibility(false);

        webView_common.IsVisible = true;

        Grid.SetColumnSpan(webView_common, 12);

        webView_common.Source = new Uri("https://runwayml-stable-diffusion-v1-5.hf.space/?__theme=light");

        webView_common.Navigated += OnStableDiffusionNavigated;
    }

    private async void OnStableDiffusionNavigated(object s, WebNavigatedEventArgs e)
    {
        string imgPrompts = MyEditorImagePrompts.Text.Trim();

        string[] lines = imgPrompts.Split(
            new[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None
        );

        if (e.Result == WebNavigationResult.Success)
        {
            //var hiddenScript = "document.querySelector('gradio-app').shadowRoot.querySelector('.output-html > div > p').style.display = 'none';document.querySelector('gradio-app').shadowRoot.getElementById('component-20').style.display = 'none';document.querySelector('gradio-app').shadowRoot.querySelector('footer').style.display = 'none';";
            //await webView_common.EvaluateJavaScriptAsync(hiddenScript);

            await Task.Delay(3000);

            foreach (string line in lines)
            {
                var script = "let e = document.querySelector('gradio-app').shadowRoot.querySelector('div').querySelector('input'); e.value = '[prompt]'; let t = new Event('input', { bubbles: true, cancelable: true }); e.dispatchEvent(t); let n = new KeyboardEvent('keydown', { bubbles: true, cancelable: true, key: 'Enter' }); e.dispatchEvent(n); document.querySelector('gradio-app').shadowRoot.querySelector('div').querySelector('button').click();".Replace("[prompt]", line);
                await webView_common.EvaluateJavaScriptAsync(script);

                await Task.Delay(30000);

                string jsCode = "let elements = document.querySelector('gradio-app').shadowRoot.querySelector('div').querySelectorAll('button > img');" +
                                "let result = [];" +
                                "elements.forEach(function(element) {" +
                                "    let src = element.getAttribute('src');" +
                                "    if (src.startsWith('data:image')) {" +
                                "        result.push(src);" +
                                "    }" +
                                "});" +
                                "result.join('*');";

                //获取输出内容
                string result = await webView_common.EvaluateJavaScriptAsync(jsCode);
                //根据*号拆分
                string[] baseSrc = result.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);

                //输出图片
                foreach (var item in baseSrc)
                {
                    string base64String = item.Split(',').Last().Replace("\\\\n", "").TrimEnd('\\');
                    byte[] imageBytes = Convert.FromBase64String(base64String);

                    // 使用ImageSharp加载图片
                    using var stream = new MemoryStream(imageBytes);
                    using var image = SixLabors.ImageSharp.Image.Load(stream);

                    string fileName = Guid.NewGuid().ToString() + ".png";
                    string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                    string filePath = Path.Combine(downloadsPath, fileName);
                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    image.Save(fileStream, new PngEncoder());
                }

                await Task.Delay(3000);
            }
        }
    }

    private void UseDefaultSDPromptButtonClick(object sender, EventArgs e)
    {
        string SDPrompts = $@"
interior design, frank lloyd wright house cave with forest canopy, dark wood, streaks of light, light fog, living room :: bubbletech –test –ar 9:16
environment living room interior, mid century modern, indoor garden with fountain, retro,m vintage, designer furniture made of wood and plastic, concrete table, wood walls, indoor potted tree, large window, outdoor forest landscape, beautiful sunset, cinematic, concept art, sunstainable architecture, octane render, utopia, ethereal, cinematic light, –ar 16:9 –stylize 45000
Realistic architectural rendering of a capsule multiple house within concrete giant blocks with moss and tall rounded windows with lights in the interior, human scales, fog like london, in the middle of a contemporary city of Tokyo, stylish, generative design, nest, spiderweb structure, silkworm thread patterns, realistic, Designed based on Kengo Kuma, Sou Fujimoto, cinematic, unreal engine, 8K, HD, volume twilight –ar 9:54
beautiful open kitchen in the style of elena of avalor overlooking aerial wide angle view of a solarpunk vibrant city with greenery, interior architecture, kitchen, eating space, rendered in octane, in the style of Luc Schuiten, craig mullins, solarpunk in deviantart, photorealistic, highly detailed, Vincent Callebaut, elena of avalor, highly detailed, –ar 16:9
interior design, open plan, kitchen and living room, modular furniture with cotton textiles, wooden floor, high ceiling, large steel windows viewing a city
Residential home high end futuristic interior, olson kundig::1 Interior Design by Dorothy Draper, maison de verre, axel vervoordt::2 award winning photography of an indoor-outdoor living library space, minimalist modern designs::1 high end indoor/outdoor residential living space, rendered in vray, rendered in octane, rendered in unreal engine, architectural photography, photorealism, featured in dezeen, cristobal palma::2.5 chaparral landscape outside, black surfaces/textures for furnishings in outdoor space::1 –q 2 –ar 4:
photo of a gorgeous young woman in the style of stefan kostic and david la chapelle, coy, shy, alluring, evocative, stunning, award winning, realistic, sharp focus, 8 k high definition, 3 5 mm film photography, photo realistic, insanely detailed, intricate, elegant, art by stanley lau and artgerm
beautiful butterfly anatomy diagram, bold shūji, chart, schematics, infographic, scientific, measurements, abstract, surreal, collage, new media design, poster, colorful highlights, tarot card, glowing ruins, marginalia, 8k, extremely detailed, dark color palette + style of Katsuhiro Otomo + Masamune Shirow + pantone, on black canvas, typography annotations
A pao de queijo foodcart in front of a japanese castle
A large cabin on top of a sunny mountain in the style of Dreamworks, artstation
A delicious ceviche cheesecake slice
alone in the amusement park by Edward Hopper
a highly detailed epic cinematic concept art CG render digital painting artwork costume design: young James Dean as a well-kept neat mechanic in 1950s USSR green dungarees and big boots, reading a book. By Greg Rutkowski, Ilya Kuvshinov, WLOP, Stanley Artgerm Lau, Ruan Jia and Fenghua Zhong, trending on ArtStation, subtle muted cinematic colors, made in Maya, Blender and Photoshop, octane render, excellent composition, cinematic atmosphere, dynamic dramatic cinematic lighting, aesthetic, very inspirational, arthouse
city made out of glass : : close shot : : 3 5 mm, realism, octane render, 8 k, exploration, cinematic, trending on artstation, realistic, 3 5 mm camera, unreal engine, hyper detailed, photo – realistic maximum detail, volumetric light, moody cinematic epic concept art, realistic matte painting, hyper photorealistic, concept art, volumetric light, cinematic epic, octane render, 8 k, corona render, movie concept art, octane render, 8 k, corona render, cinematic, trending on artstation, movie concept art, cinematic composition, ultra – detailed, realistic, hyper
cabela’s tent futuristic pop up family pod, cabin, modular, person in foreground, mountainous forested wilderness open fields, beautiful views, painterly concept art, joanna gaines, environmental concept art, farmhouse, magnolia, concept art illustration by ross tran, by james gurney, by craig mullins, by greg rutkowski trending on artstation
a young blonde male jedi with short hair standing still looking at the sunset concept art by Doug Chiang cinematic, realistic painting, high definition, concept art, portait image, path tracing, serene landscape, high quality, highly detailed, 8K, soft colors, warm colors, turbulent sea, high coherence, anatomically correct, hyperrealistic, concept art, defined face, five fingers, symmetrical
a cute magical flying dog, fantasy art drawn by disney concept artists, golden colour, high quality, highly detailed, elegant, sharp focus, concept art, character concepts, digital painting, mystery, adventure
clear portrait of a superhero concept between spiderman and batman, cottagecore!!, background hyper detailed, character concept, full body, dynamic pose, intricate, highly detailed, digital painting, artstation, concept art, smooth, sharp focus, illustration, art by artgerm and greg rutkowski and alphonse mucha
infinite hyperbolic intricate maze, futuristic eco warehouse made out of dead vines, glass mezzanine level, lots of windows, wood pallets, designed by Aesop, forest house surrounded by massive willow trees and vines, white exterior facade, in full frame, , exterior view, twisted house, 3d printed canopy, clay, earth architecture, cavelike interiors, convoluted spaces, hyper realistic, photorealism, octane render, unreal engine, 4k, –stylize 5000 –ar 1:2
the living room of a cozy wooden house with a fireplace, at night, interior design, d & d concept art, d & d wallpaper, warm, digital art. art by james gurney and larry elmore.
beautiful fashion elegant goddness of water, chic strapless dress, tropical sea background, character design, in the style of artgerm, and wlop, chanel jewelry, cinematic lighting, hyperdetailed, 8 k realistic, symmetrical, global illumination, radiant light, love and mercy, frostbite 3 engine, cryengine, dof, trending on artstation, digital art, crepuscular ray
";
        MyEditorImagePrompts.Text = SDPrompts;

    }

    private void EmptySDPromptsButtonClick(object sender, EventArgs e)
    {
        MyEditorImagePrompts.Text = "";
    }

    private void CancelSDButtonClick(object sender, EventArgs e)
    {
        MyPopupStableDiffusionSetting.IsVisible = false;
    }

    private async void MergeAnswerButton_Clicked(object sender, EventArgs e)
    {
        int claude2Index = File.ReadLines(filePath).Any(line => line.Contains("\"OfficialClaude2\"")) ? File.ReadLines(filePath).TakeWhile(line => !line.Contains("\"OfficialClaude2\"")).Count() - 1 : -1;

        PrepareEditorText();

        List<string> _getContentJS = new();
        List<string> _sendJs = new();
        foreach (var _ai in _platformNames)
        {
            _getContentJS.Add(dicConfiguration.TryGetValue(_ai, out var uniqueValue) && uniqueValue.Length > 4 ? uniqueValue[4] : null);
            _sendJs.Add(dicConfiguration.TryGetValue(_ai, out var value) && value?.Length > 3 ? value[3] : null);
        }

        int indexWebview = 0;

        List<string> _content = new();

        foreach (var _getJs in _getContentJS)
        {
            _content.Add(await GetEvaluateJavaScriptResultAsync(webViews[indexWebview], _getJs));
            indexWebview++;
        }

        for (int i = 0; i < _content.Count; i += 2)
        {
            //如果没获取到内容，那么返回的值其实是字符串，字符串为"null"，而不是真正意义上的空字符如null
            string result1 = _content[i];
            string result2 = i + 1 < _content.Count ? _content[i + 1] : null;

            //if (result1.Equals("null") || result2.Equals("null"))
            if (result1?.Equals("null") == true || result2?.Equals("null") == true)
            {
                await DisplayAlert("错误提示", "未能获取到AI回答的内容，请确保AI已经回答完毕并且你能够看到AI给出的答案", "ok");
                editor1.Text = string.Empty;
                break;
            }

            string mergePrompts = PrepareMergePrompts(result1, result2);

            string mergeJs1 = _sendJs[i].Replace("[message]", mergePrompts);
            string mergeJs2 = _sendJs[i + 1].Replace("[message]", mergePrompts);

            await ProcessWebViewAsync(webViews[i], mergeJs1);
            await ProcessWebViewAsync(webViews[i + 1], mergeJs2);
        }
    }

    private async Task<string> GetEvaluateJavaScriptResultAsync(WebView webView, string script)
    {
        return await webView.EvaluateJavaScriptAsync(script);
    }

    private async Task ProcessWebViewAsync(WebView webView, string mergeJs)
    {
        await webView.EvaluateJavaScriptAsync(mergeJs);
    }
    private void PrepareEditorText()
    {
        if (string.IsNullOrWhiteSpace(editor1.Text))
        {
            editor1.Text = "[#-#]请基于下面这两篇文章的主题、信息和情节等，将两篇文章进行整合修改，重新生成一篇新的文章，要做到将两篇文章融合为一篇新的原创文章，但不要直接复制任何一篇文章中的句子，要做到新文章的句子和原文里的句子不重复，并改变句子顺序，在原文基础上增加一些细节，使得文章内容更加充实、全面，要做到100%原创。并且做到将原文中的词语替换为同义词或近义词，以进一步增强原创性，并且保证新文章保持语言流畅，内容合理：[#-#]" + "\r\n";
        }
    }
    private string PrepareMergePrompts(string result1, string result2)
    {
        return (editor1.Text.Trim() + @$"**文章1**: {result1} **文章2**: {result2}")
            .Trim()
            .Replace("[#-#]", "")
            .Replace("\\", "\\\\")
            .Replace("\r", "\\\r")
            .Replace("\n", "\\\n")
            .Replace("'", "\\'")
            .Replace("\"", "\\\"");
    }

    private async void ToolbarItem_Select_AI_Clicked(object sender, EventArgs e)
    {
        await SelectAI.Reset();
        SelectAI.IsVisible = true;

    }

    private async void ToolbarItem_Ai_Sort_Clicked(object sender, EventArgs e)
    {
        await AiSort.Reset();
        AiSort.IsVisible = true;
    }

    private void ToolbarItem_Ai_Auto_Paint_Clicked(object sender, EventArgs e)
    {
        MyPopupStableDiffusionSetting.IsVisible = true;
    }

    private void ToolbarItem_Switch_Webview_Quantity_Clicked(object sender, EventArgs e)
    {

        toggleWindowsCount(fileWindowsCountPath);

        Application.Current.MainPage = new NavigationPage(new MainPage());
    }




    private void ToolbarItem_Prompts_Setting_Clicked(object sender, EventArgs e)
    {
        myPopupShowPromptsSettingPopup();
    }

    private void ToolbarItem_Show_Translate_button_Clicked(object sender, EventArgs e)
    {
        TranslateAndSendButton.IsVisible = !TranslateAndSendButton.IsVisible;
    }

    private async void ToolbarItem_Check_Updates_Clicked(object sender, EventArgs e)
    {
        await Browser.OpenAsync(_gitUpdate);
    }

    private async void ToolbarItem_About_Clicked(object sender, EventArgs e)
    {
        var result = await DisplayAlert("🤗关于作者", " \n 作者微信: stoeng     \n\n 哔哩哔哩频道：AI超元域  \n\n如有问题请咨询作者🤝🤝", "确定", "取消");

        if (result)
        {
            //打开默认浏览器
            await Browser.OpenAsync("https://space.bilibili.com/3493277319825652");
        }
    }
}

