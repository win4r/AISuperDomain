using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace AIIntegratorWV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string _js_poe = "document.querySelector('aside.PageWithSidebarLayout_leftSidebar__Y6XQo').style.display = 'none';";

        const string _txtboxMSG = "作者微信：stoeng  此应用永久免费！请在这里输入你要查询的内容...【注意：如果打开空白，说明你的梯子魔法的ip有问题】";

        List<string> lstWebFlag = new List<string>() { "poeChatGPTClaude", "poeSageDragonfly", "BingTwoMode", "BitoHugging", "ThebLmsys", "BingMirrorBard" };

        public MainWindow()
        {
            InitializeComponent();


            txtMSG.Text = _txtboxMSG;

            ComboBoxItem newItem = new ComboBoxItem();


            foreach (string item in lstWebFlag)
            {
                newItem.Content = item;

                //cmboxAILists.Items.Add(newItem);

            }

            wvChatGPT.NavigationCompleted += wvChatGPT_NavigationCompleted;
            wvClaude.NavigationCompleted += wvClaude_NavigationCompleted;
            wvSage.NavigationCompleted += wvSage_NavigationCompleted;
            wvDragonfly.NavigationCompleted += wvDragonfly_NavigationCompleted;
            wvBito.NavigationCompleted += WvBito_Navigated;
            wvGPT4.NavigationCompleted += WvGPT4_NavigationCompleted;
            wvClaudePlus.NavigationCompleted += WvClaudePlus_NavigationCompleted;
            wvBingMirror.NavigationCompleted += WvBingMirror_NavigationCompleted;
            wvlmsys.NavigationCompleted += Wvlmsys_NavigationCompleted;

            wvbingcreative.Source = new Uri("https://www.bing.com/search?q=Bing+AI&showconv=1&FORM=hpcodx");
            wvbingbalance.Source = new Uri("https://www.bing.com/search?q=Bing+AI&showconv=1&FORM=hpcodx");

            wvBito.Source = new Uri("https://alpha.bito.co/bitoai/");
            wvHugging.Source = new Uri("https://huggingface.co/chat/");

            wvBingMirror.Source = new Uri("https://bing.vcanbb.top/web/#/");
            wvBard.Source = new Uri("https://bard.google.com/");

            wvTheb.Source = new Uri("https://chatbot.theb.ai/#/chat/");
            wvlmsys.Source = new Uri("https://chat.lmsys.org/");
        }
        
        private void TextBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (txtMSG.Text == _txtboxMSG)
            {
                txtMSG.Text = string.Empty;
            }
        }
        private async void Wvlmsys_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            await wvlmsys.ExecuteScriptAsync("document.getElementById('notice_markdown').style.display = 'none';");
        }
        private async void WvBingMirror_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            await wvBingMirror.ExecuteScriptAsync("document.querySelector('.fixed.top-6.right-6.cursor-pointer.n-image.n-image--preview-disabled').style.display = 'none';");
        }
        private async void wvChatGPT_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            await wvChatGPT.ExecuteScriptAsync(_js_poe);
        }
        private async void wvClaude_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            await wvClaude.ExecuteScriptAsync(_js_poe);
        }
        private async void wvSage_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            await wvSage.ExecuteScriptAsync(_js_poe);
        }
        private async void wvDragonfly_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            await wvDragonfly.ExecuteScriptAsync(_js_poe);
        }

        private async void WvGPT4_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            await wvGPT4.ExecuteScriptAsync(_js_poe);
        }

        private async void WvClaudePlus_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            await wvClaudePlus.ExecuteScriptAsync(_js_poe);
        }

        private async void WvBito_Navigated(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            await wvBito.ExecuteScriptAsync("document.getElementById('bitoai_title_content1').style.display = 'none';document.getElementById('signedInDivWeb').style.display = 'none'; document.querySelector(\"div[style*='text-align: center;font-size: 13px;color: #fff;padding-bottom: 5px']\").style.display = \"none\";");
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {

            if (!this.txtMSG.Text.Equals(string.Empty))
            {
                string message = this.txtMSG.Text.Trim().Replace("\r\n", "\\n");

                this.txtMSG.Text = string.Empty;

                string js_poe = $@"((textarea = document.querySelector('textarea'), lastValue = textarea.value, textarea.value = '{message}', inputEvent = new Event('input', {{bubbles: true }}), inputEvent.simulated = true, tracker = textarea._valueTracker, tracker ? tracker.setValue(lastValue) : void 0, textarea.dispatchEvent(inputEvent), enterKeyEvent = new KeyboardEvent('keydown', {{key: 'Enter', code: 'Enter', keyCode: 13, which: 13, bubbles: true, cancelable: true }}), textarea.dispatchEvent(enterKeyEvent)));";

                string js_bito = $@"document.querySelector('#user_input1').value = '{message}';document.querySelector('#user_input1').dispatchEvent(new KeyboardEvent('keydown', {{ keyCode: 13, bubbles: true }}));";

                string js_huggingChat = $@"(document.querySelector('textarea').value = '{message}', document.querySelector('textarea').dispatchEvent(new Event('input', {{bubbles: true }})), document.querySelector('textarea').dispatchEvent(new KeyboardEvent('keydown', {{key: 'Enter' }})))";

                string js_bing = $@"(document.querySelector('cib-serp').shadowRoot.querySelector('#cib-action-bar-main').shadowRoot.querySelector('cib-text-input').shadowRoot.querySelector('#searchbox').value = '[message]', document.querySelector('cib-serp').shadowRoot.querySelector('#cib-action-bar-main').shadowRoot.querySelector('cib-text-input').shadowRoot.querySelector('#searchbox').dispatchEvent(new Event('input', {{bubbles: true }})), document.querySelector('cib-serp').shadowRoot.querySelector('#cib-action-bar-main').shadowRoot.querySelector('cib-text-input').shadowRoot.querySelector('#searchbox').dispatchEvent(new KeyboardEvent('keydown', {{key: 'Enter' }})));";

                string js_theb = $@"document.querySelector('textarea').value = '{message}';document.querySelector('textarea').dispatchEvent(new Event('input',{{bubbles:!0}}));document.querySelector('textarea').dispatchEvent(new KeyboardEvent('keydown',{{keycode:13}}));setTimeout(function(){{document.querySelector('.n-button__icon').click();document.querySelectorAll('button')[24].click()}},1e3);";

                string js_lmsys = $@"document.querySelector('.scroll-hide.svelte-4xt1ch').value = '{message}'; document.querySelector('.scroll-hide.svelte-4xt1ch').dispatchEvent(new Event('input', {{ bubbles: true }})); document.querySelector('.scroll-hide.svelte-4xt1ch').dispatchEvent(new KeyboardEvent('keydown', {{keycode: 13 }})); document.querySelector('.lg.secondary.svelte-1ipelgc').click();";

                string js_bard = $@"document.querySelector('textarea').value = '{message}'; document.querySelector('textarea').dispatchEvent(new Event('input', {{ bubbles: true }})); document.querySelector('textarea').dispatchEvent(new KeyboardEvent('keydown', {{ keycode: 13 }}));document.querySelector('textarea').dispatchEvent(new KeyboardEvent('keydown', {{key: 'Enter' }}))";

                if (lstWebFlag.Contains("poeChatGPTClaude"))
                {
                    wvChatGPT.ExecuteScriptAsync(js_poe);
                    wvClaude.ExecuteScriptAsync(js_poe);
                }

                if (lstWebFlag.Contains("poeSageDragonfly"))
                {

                    wvSage.ExecuteScriptAsync(js_poe);
                    wvDragonfly.ExecuteScriptAsync(js_poe);

                }
                if (lstWebFlag.Contains("BingTwoMode"))
                {
                    wvbingcreative.ExecuteScriptAsync(js_bing);
                    wvbingbalance.ExecuteScriptAsync(js_bing);

                }
                if (lstWebFlag.Contains("BitoHugging"))
                {
                    wvBito.ExecuteScriptAsync(js_bito);
                    wvHugging.ExecuteScriptAsync(js_huggingChat);

                }
                if (lstWebFlag.Contains("ThebLmsys"))
                {
                    wvTheb.ExecuteScriptAsync(js_theb);
                    wvlmsys.ExecuteScriptAsync(js_lmsys);

                }
                if (lstWebFlag.Contains("BingMirrorBard"))
                {
                    wvBingMirror.ExecuteScriptAsync(js_bing);
                    wvBard.ExecuteScriptAsync(js_bard);
                }
            }
        }

        private void myComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string? selectedTag = string.Empty;

            ComboBoxItem selectedItem = myComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                selectedTag = selectedItem.Content.ToString();
            }

            if (selectedTag == "poeChatGPTClaude")
            {
                ShowChatGPTClaude();
            }
            if (selectedTag == "poeSageDragonfly")
            {
                ShowSageDragonfly();
            }
            if (selectedTag == "BingTwoMode")
            {
                ShowBing();
            }
            if (selectedTag == "BitoHugging")
            {
                ShowBitoHugging();
            }
            if (selectedTag == "ThebLmsys")
            {
                ShowThebLmsys();
            }
            if (selectedTag == "BingMirrorBard")
            {
                ShowBingMirrorBard();
            }
        }

        //int _switchAI_flag = 0;
        private void SwitchButton_Click(object sender, RoutedEventArgs e)
        {
            int nextIndex = (myComboBox.SelectedIndex + 1) % myComboBox.Items.Count;
            myComboBox.SelectedIndex = nextIndex;
        }

        int _autoGPT_flag = 0;
        private void AutoGPTButton_Click(object sender, RoutedEventArgs e)
        {
            ShowAgentAutoGPT();

            if (_autoGPT_flag == 0)
            {
                wvAgentGPT.Source = new Uri("https://agentgpt.reworkd.ai/zh");
                wvAutoGPT.Source = new Uri("https://autogpt.thesamur.ai/");
                _autoGPT_flag = 1;
            }
        }

        //int _gptclient_flag = 0;
        //private void GPTClientButton_Click(object sender, RoutedEventArgs e)
        //{
        //    ShowGPTClient();

        //    if (_gptclient_flag == 0)
        //    {
        //        wvChatClient1.Source = new Uri("https://www.chatbotui.com/");
        //        wvChatClient2.Source = new Uri("https://www.chatwithgpt.ai/");

        //        _gptclient_flag = 1;

        //    }
        //}

        private void SvaeSettingButton_Click(object sender, RoutedEventArgs e)
        {
            lstWebFlag = new List<string>() { "poeChatGPTClaude", "poeSageDragonfly", "BingTwoMode", "BitoHugging", "ThebLmsys", "BingMirrorBard" };

            foreach (string flag in lstWebFlag)
            {
                bool found = false;

                foreach (ComboBoxItem item in myComboBox.Items)
                {
                    if (item.Content.ToString() == flag)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    ComboBoxItem newItem = new ComboBoxItem();
                    newItem.Content = flag;
                    myComboBox.Items.Add(newItem);
                }
            }


            if (ckbPoeGPTClaude.IsChecked == false)
            {
                lstWebFlag.Remove("poeChatGPTClaude");

                foreach (ComboBoxItem item in myComboBox.Items)
                {
                    if (item.Content.ToString() == "poeChatGPTClaude")
                    {
                        myComboBox.Items.Remove(item);
                        break;
                    }
                }


            }
            if (ckbPoeSageDragonfly.IsChecked == false)
            {
                lstWebFlag.Remove("poeSageDragonfly");

                foreach (ComboBoxItem item in myComboBox.Items)
                {
                    if (item.Content.ToString() == "poeSageDragonfly")
                    {
                        myComboBox.Items.Remove(item);
                        break;
                    }
                }

            }
            if (ckbbingTwoMode.IsChecked == false)
            {
                lstWebFlag.Remove("BingTwoMode");

                foreach (ComboBoxItem item in myComboBox.Items)
                {
                    if (item.Content.ToString() == "BingTwoMode")
                    {
                        myComboBox.Items.Remove(item);
                        break;
                    }
                }

            }
            if (ckbBitoHugging.IsChecked == false)
            {
                lstWebFlag.Remove("BitoHugging");

                foreach (ComboBoxItem item in myComboBox.Items)
                {
                    if (item.Content.ToString() == "BitoHugging")
                    {
                        myComboBox.Items.Remove(item);
                        break;
                    }
                }

            }
            if (ckbThebLmsys.IsChecked == false)
            {
                lstWebFlag.Remove("ThebLmsys");

                foreach (ComboBoxItem item in myComboBox.Items)
                {
                    if (item.Content.ToString() == "ThebLmsys")
                    {
                        myComboBox.Items.Remove(item);
                        break;
                    }
                }

            }
            if (ckbBingMirrorBard.IsChecked == false)
            {
                lstWebFlag.Remove("BingMirrorBard");

                foreach (ComboBoxItem item in myComboBox.Items)
                {
                    if (item.Content.ToString() == "BingMirrorBard")
                    {
                        myComboBox.Items.Remove(item);
                        break;
                    }
                }

            }

            myComboBox.SelectedIndex = 0;
            panel.Visibility = Visibility.Collapsed;


        }

        int _gpt4_flag = 0;
        private void GPT4BardButton_Click(object sender, RoutedEventArgs e)
        {
            ShowGPT4Bard();

            if (_gpt4_flag == 0)
            {
                wvOra.Source = new Uri("https://ora.ai/openai/gpt4");
                wvBard.Source = new Uri("https://bard.google.com/");

                _gpt4_flag = 1;

            }
        }

        private void ShowBingMirrorBard()
        {
            wvChatGPT.Visibility = Visibility.Hidden;
            wvClaude.Visibility = Visibility.Hidden;

            wvSage.Visibility = Visibility.Hidden;
            wvDragonfly.Visibility = Visibility.Hidden;

            wvbingcreative.Visibility = Visibility.Hidden;
            wvbingbalance.Visibility = Visibility.Hidden;

            wvBito.Visibility = Visibility.Hidden;
            wvHugging.Visibility = Visibility.Hidden;

            wvAgentGPT.Visibility = Visibility.Hidden;
            wvAutoGPT.Visibility = Visibility.Hidden;

            wvOra.Visibility = Visibility.Hidden;
            //wvBard.Visibility = Visibility.Hidden;

            wvTheb.Visibility = Visibility.Hidden;
            wvlmsys.Visibility = Visibility.Hidden;

            wvBingMirror.Visibility = Visibility.Visible;
            wvBard.Visibility = Visibility.Visible;
        }



        private void VisiablePoeChatGPTClaude(int i = 1)
        {
            if (i == 1)
            {
                wvChatGPT.Visibility = Visibility.Visible;
                wvClaude.Visibility = Visibility.Visible;
            }
            else
            {
                wvChatGPT.Visibility = Visibility.Hidden;
                wvClaude.Visibility = Visibility.Hidden;

            }
        }
        private void VisiablePoeSageDragonfly(int i = 1)
        {
            if (i == 1)
            {
                wvSage.Visibility = Visibility.Visible;
                wvDragonfly.Visibility = Visibility.Visible;
            }
            else
            {
                wvSage.Visibility = Visibility.Hidden;
                wvDragonfly.Visibility = Visibility.Hidden;

            }
        }

        private void VisiableBingTwoMode(int i = 1)
        {
            if (i == 1)
            {
                wvbingcreative.Visibility = Visibility.Visible;
                wvbingbalance.Visibility = Visibility.Visible;
            }
            else
            {
                wvbingcreative.Visibility = Visibility.Hidden;
                wvbingbalance.Visibility = Visibility.Hidden;

            }
        }

        private void VisiableBitoHugging(int i = 1)
        {
            if (i == 1)
            {
                wvBito.Visibility = Visibility.Visible;
                wvHugging.Visibility = Visibility.Visible;
            }
            else
            {
                wvBito.Visibility = Visibility.Hidden;
                wvHugging.Visibility = Visibility.Hidden;

            }
        }
        private void VisiableThebLmsys(int i = 1)
        {
            if (i == 1)
            {
                wvTheb.Visibility = Visibility.Visible;
                wvlmsys.Visibility = Visibility.Visible;
            }
            else
            {
                wvTheb.Visibility = Visibility.Hidden;
                wvlmsys.Visibility = Visibility.Hidden;

            }
        }
        private void VisiableBingMirrorBard(int i = 1)
        {
            if (i == 1)
            {
                wvBingMirror.Visibility = Visibility.Visible;
                wvBard.Visibility = Visibility.Visible;
            }
            else
            {
                wvBingMirror.Visibility = Visibility.Hidden;
                wvBard.Visibility = Visibility.Hidden;

            }
        }

        private void ShowThebLmsys()
        {
            wvChatGPT.Visibility = Visibility.Hidden;
            wvClaude.Visibility = Visibility.Hidden;

            wvSage.Visibility = Visibility.Hidden;
            wvDragonfly.Visibility = Visibility.Hidden;

            wvbingcreative.Visibility = Visibility.Hidden;
            wvbingbalance.Visibility = Visibility.Hidden;

            wvBito.Visibility = Visibility.Hidden;
            wvHugging.Visibility = Visibility.Hidden;

            wvAgentGPT.Visibility = Visibility.Hidden;
            wvAutoGPT.Visibility = Visibility.Hidden;

            wvOra.Visibility = Visibility.Hidden;
            //wvBard.Visibility = Visibility.Hidden;

            wvTheb.Visibility = Visibility.Visible;
            wvlmsys.Visibility = Visibility.Visible;

            wvBingMirror.Visibility = Visibility.Hidden;
            wvBard.Visibility = Visibility.Hidden;
        }

        private void ShowChatGPTClaude()
        {
            wvChatGPT.Visibility = Visibility.Visible;
            wvClaude.Visibility = Visibility.Visible;

            wvSage.Visibility = Visibility.Hidden;
            wvDragonfly.Visibility = Visibility.Hidden;

            wvbingcreative.Visibility = Visibility.Hidden;
            wvbingbalance.Visibility = Visibility.Hidden;

            wvBito.Visibility = Visibility.Hidden;
            wvHugging.Visibility = Visibility.Hidden;

            wvAgentGPT.Visibility = Visibility.Hidden;
            wvAutoGPT.Visibility = Visibility.Hidden;

            wvOra.Visibility = Visibility.Hidden;
            //wvBard.Visibility = Visibility.Hidden;

            wvTheb.Visibility = Visibility.Hidden;
            wvlmsys.Visibility = Visibility.Hidden;

            wvBingMirror.Visibility = Visibility.Hidden;
            wvBard.Visibility = Visibility.Hidden;

        }

        private void ShowSageDragonfly()
        {
            wvChatGPT.Visibility = Visibility.Hidden;
            wvClaude.Visibility = Visibility.Hidden;

            wvSage.Visibility = Visibility.Visible;
            wvDragonfly.Visibility = Visibility.Visible;

            wvbingcreative.Visibility = Visibility.Hidden;
            wvbingbalance.Visibility = Visibility.Hidden;

            wvBito.Visibility = Visibility.Hidden;
            wvHugging.Visibility = Visibility.Hidden;

            wvAgentGPT.Visibility = Visibility.Hidden;
            wvAutoGPT.Visibility = Visibility.Hidden;

            wvOra.Visibility = Visibility.Hidden;
            //wvBard.Visibility = Visibility.Hidden;

            wvTheb.Visibility = Visibility.Hidden;
            wvlmsys.Visibility = Visibility.Hidden;

            wvBingMirror.Visibility = Visibility.Hidden;
            wvBard.Visibility = Visibility.Hidden;

        }

        private void ShowBing()
        {
            wvChatGPT.Visibility = Visibility.Hidden;
            wvClaude.Visibility = Visibility.Hidden;

            wvSage.Visibility = Visibility.Hidden;
            wvDragonfly.Visibility = Visibility.Hidden;

            wvbingcreative.Visibility = Visibility.Visible;
            wvbingbalance.Visibility = Visibility.Visible;

            wvBito.Visibility = Visibility.Hidden;
            wvHugging.Visibility = Visibility.Hidden;

            wvAgentGPT.Visibility = Visibility.Hidden;
            wvAutoGPT.Visibility = Visibility.Hidden;

            wvOra.Visibility = Visibility.Hidden;
            //wvBard.Visibility = Visibility.Hidden;

            wvTheb.Visibility = Visibility.Hidden;
            wvlmsys.Visibility = Visibility.Hidden;

            wvBingMirror.Visibility = Visibility.Hidden;
            wvBard.Visibility = Visibility.Hidden;

        }


        private void ShowBitoHugging()
        {
            wvChatGPT.Visibility = Visibility.Hidden;
            wvClaude.Visibility = Visibility.Hidden;

            wvSage.Visibility = Visibility.Hidden;
            wvDragonfly.Visibility = Visibility.Hidden;

            wvbingcreative.Visibility = Visibility.Hidden;
            wvbingbalance.Visibility = Visibility.Hidden;

            wvBito.Visibility = Visibility.Visible;
            wvHugging.Visibility = Visibility.Visible;

            wvAgentGPT.Visibility = Visibility.Hidden;
            wvAutoGPT.Visibility = Visibility.Hidden;

            wvOra.Visibility = Visibility.Hidden;
            //wvBard.Visibility = Visibility.Hidden;

            wvTheb.Visibility = Visibility.Hidden;
            wvlmsys.Visibility = Visibility.Hidden;

            wvBingMirror.Visibility = Visibility.Hidden;
            wvBard.Visibility = Visibility.Hidden;

        }


        private void ShowAgentAutoGPT()
        {
            wvChatGPT.Visibility = Visibility.Hidden;
            wvClaude.Visibility = Visibility.Hidden;

            wvSage.Visibility = Visibility.Hidden;
            wvDragonfly.Visibility = Visibility.Hidden;

            wvbingcreative.Visibility = Visibility.Hidden;
            wvbingbalance.Visibility = Visibility.Hidden;

            wvBito.Visibility = Visibility.Hidden;
            wvHugging.Visibility = Visibility.Hidden;

            wvAgentGPT.Visibility = Visibility.Visible;
            wvAutoGPT.Visibility = Visibility.Visible;

            wvOra.Visibility = Visibility.Hidden;
            //wvBard.Visibility = Visibility.Hidden;

            wvTheb.Visibility = Visibility.Hidden;
            wvlmsys.Visibility = Visibility.Hidden;

            wvBingMirror.Visibility = Visibility.Hidden;
            wvBard.Visibility = Visibility.Hidden;

        }


        private void ShowGPT4Bard()
        {
            wvChatGPT.Visibility = Visibility.Hidden;
            wvClaude.Visibility = Visibility.Hidden;

            wvSage.Visibility = Visibility.Hidden;
            wvDragonfly.Visibility = Visibility.Hidden;

            wvbingcreative.Visibility = Visibility.Hidden;
            wvbingbalance.Visibility = Visibility.Hidden;

            wvBito.Visibility = Visibility.Hidden;
            wvHugging.Visibility = Visibility.Hidden;

            wvAgentGPT.Visibility = Visibility.Hidden;
            wvAutoGPT.Visibility = Visibility.Hidden;

            wvOra.Visibility = Visibility.Visible;
            //wvBard.Visibility = Visibility.Visible;

            wvTheb.Visibility = Visibility.Hidden;
            wvlmsys.Visibility = Visibility.Hidden;

            wvBingMirror.Visibility = Visibility.Hidden;
            wvBard.Visibility = Visibility.Hidden;

        }

        int _flag_gpt4 = 0;
        int _gpt4_loaded = 0;
        int _gpt4_1_loaded = 0;

        private void GPT4Button_Click(object sender, RoutedEventArgs e)
        {
            wvChatGPT.Visibility = Visibility.Hidden;
            wvClaude.Visibility = Visibility.Hidden;

            wvSage.Visibility = Visibility.Hidden;
            wvDragonfly.Visibility = Visibility.Hidden;

            wvbingcreative.Visibility = Visibility.Hidden;
            wvbingbalance.Visibility = Visibility.Hidden;

            wvBito.Visibility = Visibility.Hidden;
            wvHugging.Visibility = Visibility.Hidden;

            wvAgentGPT.Visibility = Visibility.Hidden;
            wvAutoGPT.Visibility = Visibility.Hidden;

            wvOra.Visibility = Visibility.Hidden;
            //wvBard.Visibility = Visibility.Hidden;

            wvTheb.Visibility = Visibility.Hidden;
            wvlmsys.Visibility = Visibility.Hidden;

            wvBingMirror.Visibility = Visibility.Hidden;
            wvBard.Visibility = Visibility.Hidden;



            if (_flag_gpt4 == 0)
            {
                wvFore.Visibility = Visibility.Hidden;
                //wvYou.Visibility = Visibility.Hidden;

                wvGPT4.Visibility = Visibility.Visible;
                wvClaudePlus.Visibility = Visibility.Visible;

                if (_gpt4_loaded == 0)
                {
                    wvGPT4.Source = new Uri("https://poe.com/GPT-4");
                    wvClaudePlus.Source = new Uri("https://poe.com/Claude%2B");

                    _gpt4_loaded = 1;

                }

                _flag_gpt4 = 1;

            }
            else if (_flag_gpt4 == 1)
            {
                wvGPT4.Visibility = Visibility.Hidden;
                wvClaudePlus.Visibility = Visibility.Hidden;

                wvFore.Visibility = Visibility.Visible;
                wvOra.Visibility = Visibility.Visible;
                //wvYou.Visibility = Visibility.Visible;


                if (_gpt4_1_loaded == 0)
                {
                    wvFore.Source = new Uri("https://chat.forefront.ai/");
                    wvOra.Source = new Uri("https://ora.ai/openai/gpt4");
                    //wvYou.Source = new Uri("https://you.com/search?q=AI%E8%B6%85%E5%85%83%E5%9F%9F&fromSearchBar=true&tbm=youchat");

                    _gpt4_1_loaded = 1;

                }

                _flag_gpt4 = 0;

            }

        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            panel.Visibility = Visibility.Visible;

            wvFore.Visibility = Visibility.Hidden;
            //wvYou.Visibility = Visibility.Hidden;
            wvDragonfly.Visibility = Visibility.Hidden;
            wvClaude.Visibility = Visibility.Hidden;
            wvGPT4.Visibility = Visibility.Hidden;
            wvHugging.Visibility = Visibility.Hidden;
            wvlmsys.Visibility = Visibility.Hidden;
            wvOra.Visibility = Visibility.Hidden;
            wvSage.Visibility = Visibility.Hidden;
            wvTheb.Visibility = Visibility.Hidden;
            wvAgentGPT.Visibility = Visibility.Hidden;
            wvAutoGPT.Visibility = Visibility.Hidden;
            //wvBard.Visibility = Visibility.Hidden;
            wvbingbalance.Visibility = Visibility.Hidden;
            wvbingcreative.Visibility = Visibility.Hidden;
            wvBito.Visibility = Visibility.Hidden;
            wvChatGPT.Visibility = Visibility.Hidden;

            wvTheb.Visibility = Visibility.Hidden;
            wvlmsys.Visibility = Visibility.Hidden;

            wvBingMirror.Visibility = Visibility.Hidden;
            wvBard.Visibility = Visibility.Hidden;



        }
    }
}
