using System.Text;
using CommunityToolkit.Mvvm.Messaging;
using ReverseMarkdown;
using System.IO; // For MemoryStream, StreamWriter
using CommunityToolkit.Maui.Storage; // For FileSaver
// Potentially CommunityToolkit.Maui also if DisplayAlert is from there, but it's usually from Page
using Microsoft.Maui.Storage; // For FileSystem (if needed, though FileSaver handles paths)


namespace Aila;

public partial class MainPage
{
    private const string StrFlag = "#`#";
        
    private List<(string url, int row, int column, int columnSpan)> _webViewsToAdd;

    private Grid _grid;
    
    private AppConfiguration _configuration;

    private ConfigurationManager _configurationManager = new();
    
    private int _currentGroupIndex; // 从第一组开始 这里删除了=0，如果出问题就加入

    private int _viewsCount;

    private WebViewManager _webViewManager = new WebViewManager();

    private Dictionary<WebView, (string Url, int Row, int Column, int ColumnSpan)> _webViewsInfo = new();

    private WebView _focusedWebView;
    private ToolbarItem _exportMarkdownButton;

    // 添加 Editor 到第二行的前10列
    private Editor _editor = new Editor
    {
        Placeholder =
            "👉👉👉Enter your question here. To use prompt commands, simply type '/' followed by the command. Type '#' to access persistent commands. Press \u21e5 + \u23ce to submit or Press Tab + Enter to submit..",
        VerticalOptions = LayoutOptions.Start,
        FontSize = 18, // 直接指定字体大小，假设中等字体大小约为18pt
        MaximumHeightRequest = 300,
        AutoSize = EditorAutoSizeOption.TextChanges
    };
    
    private Task DisplayAlertAsync(string title, string message, string cancel)
    {
        return DisplayAlert(title, message, cancel);
    }

    public MainPage()
    {
        InitializeComponent();
        
        InitializeToolbarItems(); // Call to setup toolbar items including Export button
        InitializePageContent();
        
        _configurationManager.OnDisplayAlertRequested = DisplayAlertAsync;

        _editor.TextChanged += Editor_TextChanged;
        
        // 订阅消息
        WeakReferenceMessenger.Default.Register<ConfigurationUpdatedMessage>(this, (recipient, message) =>
        {
            // 当收到配置更新的消息时执行的逻辑
            if (this.Content is Layout layout)
            {
                layout.Children.Clear();
            }
            // ToolbarItems might need re-evaluation if config changes affect it,
            // but Home and Export buttons are static in terms of existence.
            InitializePageContent(); 
            
            _configurationManager.OnDisplayAlertRequested = DisplayAlertAsync;
            
        });
    }

    private void InitializeToolbarItems()
    {
        // Home Button (ensure it's always first or present)
        var homeButtonText = "\ud83c\udfe0 Home";
        var homeToolbarItem = ToolbarItems.FirstOrDefault(ti => ti.Text == homeButtonText);
        if (homeToolbarItem == null)
        {
            homeToolbarItem = new ToolbarItem { Text = homeButtonText };
            homeToolbarItem.Clicked += (s, args) => RestoreWebViewsLayout();
            ToolbarItems.Insert(0, homeToolbarItem); // Ensure it's at the beginning
        }
        else // If it exists, ensure it's at the beginning
        {
            ToolbarItems.Remove(homeToolbarItem);
            ToolbarItems.Insert(0, homeToolbarItem);
        }

        // Export Markdown Button
        _exportMarkdownButton = new ToolbarItem
        {
            Text = "Export as MD",
            IconImageSource = null, 
            Order = ToolbarItemOrder.Primary, // Primary to appear alongside AI names if desired, or Secondary
            Priority = 1, // Adjust as needed, lower numbers often appear first within an Order
            IsVisible = false 
        };
        _exportMarkdownButton.Clicked += OnExportToMarkdownClicked;
        
        // Add Export button after Home, if not already present
        if (!ToolbarItems.Contains(_exportMarkdownButton))
        {
            ToolbarItems.Insert(1, _exportMarkdownButton);
        }
    }
    
    private async void OnExportToMarkdownClicked(object sender, EventArgs e)
    {
        if (_focusedWebView == null)
        {
            await DisplayAlert("Export Error", "No WebView is currently focused. Please select an AI view to make it full screen first.", "OK");
            return;
        }

        try
        {
            string htmlContent = await _focusedWebView.EvaluateJavaScriptAsync("document.documentElement.outerHTML;");

            if (string.IsNullOrEmpty(htmlContent))
            {
                await DisplayAlert("Export Content", "Failed to retrieve HTML content from the focused WebView.", "OK");
            }
            else
            {
                var converter = new Converter();
                string markdownContent = converter.Convert(htmlContent);

                // Save the Markdown content to a file
                var defaultFileName = $"AilaExport_{DateTime.Now:yyyyMMddHHmmss}.md";
                using var stream = new MemoryStream(Encoding.UTF8.GetBytes(markdownContent ?? ""));
                
                // Ensure CancellationToken.None is passed or handle cancellation appropriately
                var fileSaverResult = await FileSaver.Default.SaveAsync(defaultFileName, stream, CancellationToken.None);

                if (fileSaverResult.IsSuccessful)
                {
                    await DisplayAlert("Success", $"Markdown saved to: {fileSaverResult.FilePath}", "OK");
                }
                else
                {
                    string errorMessage = fileSaverResult.Exception?.Message ?? "Unknown error during saving.";
                    if (fileSaverResult.Exception is CommunityToolkit.Maui.Storage.FileSaverException fse && fse.Message.Contains("Operation cancelled"))
                    {
                        errorMessage = "Save operation was cancelled by the user.";
                    }
                    await DisplayAlert("Error", $"Failed to save Markdown: {errorMessage}", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception or display a more specific error message
            await DisplayAlert("JavaScript Execution Error", $"An error occurred while trying to get HTML content: {ex.Message}", "OK");
        }
    }

    private async void Editor_TextChanged(object? sender, TextChangedEventArgs e)
    {
        // 确保用户输入的是第一个字符
        if (e.NewTextValue.Length == 1)
        {
            // 检查文本是否以"@"字符开始
            if (e.NewTextValue.StartsWith("/"))
            {
                // 如果是，触发指定的方法
                ShowPopupWithCollectionView();
            }
            else if (e.NewTextValue.StartsWith("#"))
            {
                // 如果是，调用自定义的方法
                ShowPopupWithCollectionView();
            }
        }
        
        // 确保编辑器内容长度足够包含两个字符
        if (!string.IsNullOrEmpty(e.NewTextValue) && e.NewTextValue.Length >= 2)
        {
            // 检查文本的最后两个字符是否是 '\t\n'
            string lastTwoChars = e.NewTextValue.Substring(e.NewTextValue.Length - 2);
            if (lastTwoChars.Equals("\t\n") || lastTwoChars.Equals("\n\t"))
            {
                // 如果是，移除这两个字符并显示提示
                var editor = sender as Editor;
                if (editor != null)
                {
                    // 移除最后两个字符
                    editor.Text = e.NewTextValue.Remove(e.NewTextValue.Length - 2);
                }
                // 调用发送功能
                await SendMessage();
            }
        }
    }
    private void ShowPopupWithCollectionView()
    {
        // 创建 CollectionView 并设置数据源
        var collectionView = new CollectionView
        {
            ItemsSource = _configuration.Prompts,
            SelectionMode = SelectionMode.Single,

            ItemTemplate = new DataTemplate(() =>
            {
                var frame = new Frame
                {
                    BackgroundColor = Colors.White,

                    Padding = 10,
                    CornerRadius = 5,
                    // BorderColor = Colors.LightGray,
                    Content = new Label
                    {
                        TextColor = Colors.Black,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Start
                    }
                };

                frame.Content.SetBinding(Label.TextProperty, nameof(Prompts.Title));

                return frame;
            }),
        };

        // 创建 Frame 用于包装 collectionView
        var frameForCollectionView = new Frame
        {
            Content = collectionView,
            Padding = 0 // 根据需要调整
        };

        collectionView.SelectionChanged += (sender, e) =>
        {
            if (e.CurrentSelection.Count > 0)
            {
                var selectedItem = e.CurrentSelection[0] as Prompts;
                if (selectedItem != null)
                {
                    // 根据 ID 查询对应的 Prompt
                    var prompt = _configuration.Prompts.FirstOrDefault(p => p.Id == selectedItem.Id)?.Prompt;
                    if (!string.IsNullOrEmpty(prompt))
                    {
                        // 检查文本是否以 "/" 开头
                        if (_editor.Text.StartsWith("/"))
                        {
                            // 移除第一个字符 "/"
                            _editor.Text = _editor.Text.Substring(1);
                            
                            collectionView.SelectedItem = null; // 清除选择
                            
                            _editor.Text = prompt;
                            
                            // 防止光标移动到文本开头
                            _editor.CursorPosition = _editor.Text.Length;

                        }
                        // 检查文本是否以 "#" 开头
                        if (_editor.Text.StartsWith("#"))
                        {
                            // 移除第一个字符 "/"
                            _editor.Text = _editor.Text.Substring(1);
                            
                            collectionView.SelectedItem = null; // 清除选择
                            
                            _editor.Text =StrFlag+ prompt+StrFlag+"\n";
                            
                            // 防止光标移动到文本开头
                            _editor.CursorPosition = _editor.Text.Length;

                            
                        }

                        if (_grid.Children.Contains(frameForCollectionView))
                        {
                            _grid.Children.Remove(frameForCollectionView);
                        }
                    }
                }
            }
        };
        
        // 将 frame 添加到 Grid 的第一行和第一列
        Grid.SetRow(frameForCollectionView, 0);
        Grid.SetColumn(frameForCollectionView, 0);
        Grid.SetColumnSpan(frameForCollectionView, 3);

        _grid.Children.Add(frameForCollectionView);
    }

    private async void InitializePageContent()
    {
        var configuration = await _configurationManager.LoadConfigurationAsync();

        if (configuration == null)
        {
            return;
        }
        _configuration = configuration;

        _grid = new Grid
        {
            ColumnDefinitions = GenerateColumnDefinitions(12),
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = GridLength.Auto }
            }
        };
        
        _webViewsToAdd = new List<(string url, int row, int column, int columnSpan)>();

        int currentAiCount = _configuration.CurrentAi.Count;

        _viewsCount = _configuration.ViewsCount.VCount;

        // 当每次只显示一个WebView时
        if (_viewsCount == 1)
        {
            for (int i = 0; i < currentAiCount; i++)
            {
                var currentAi = _configuration.CurrentAi[i];
                var aiConfig = _configuration.AiConfig.FirstOrDefault(ai => ai.Id == currentAi.Id);
                if (aiConfig != null)
                {
                    // 对于只有一个WebView的情况，占据全部列
                    int column = 0; // 从第0列开始
                    int columnSpan = 12; // 占据所有12列

                    // 添加到待添加列表中
                    _webViewsToAdd.Add((aiConfig.Url, 0, column, columnSpan));
                }
            }

            // 将WebView添加到网格中，并设置可见性
            foreach (var (url, row, column, columnSpan) in _webViewsToAdd)
            {
                var webView = _webViewManager.GetWebViewForUrl(url);
                // 只有第一组的WebView初始可见
                webView.IsVisible = _webViewsToAdd.IndexOf((url, row, column, columnSpan)) < 1;
                
                AddItemToGrid(webView, row, column, columnSpan);
            }
        }

        // 当每次同时显示2个WebView时
        if (_viewsCount == 2)
        {
            for (int i = 0; i < currentAiCount; i++)
            {
                var currentAi = _configuration.CurrentAi[i];
                var aiConfig = _configuration.AiConfig.FirstOrDefault(ai => ai.Id == currentAi.Id);
                if (aiConfig != null)
                {
                    // 基本列位置和跨度设置
                    int column = (i % 2) * 6; // 默认第一个WebView占据1-6列，第二个占据7-12列
                    int columnSpan = 6; // 默认每个WebView占据6列

                    // 特殊情况处理：如果只有一个WebView，并且viewsCount为2，则让它占据所有12列
                    if (currentAiCount == 1)
                    {
                        column = 0; // 从第0列开始
                        columnSpan = 12; // 占据所有12列
                    }

                    // 添加到待添加列表中
                    _webViewsToAdd.Add((aiConfig.Url, 0, column, columnSpan));
                }
            }

            // 将WebView添加到网格中，并设置可见性
            foreach (var (url, row, column, columnSpan) in _webViewsToAdd)
            {
                var webView = _webViewManager.GetWebViewForUrl(url);
                webView.IsVisible = true; // 在初始化时，你可能希望所有WebView都可见
                Grid.SetColumn(webView, column);
                Grid.SetColumnSpan(webView, columnSpan);
                AddItemToGrid(webView, row, column, columnSpan);
            }
        }

        // 当每次同时显示3个WebView时
        if (_viewsCount == 3)
        {
            for (int i = 0; i < currentAiCount; i++)
            {
                var currentAi = _configuration.CurrentAi[i];
                var aiConfig = _configuration.AiConfig.FirstOrDefault(ai => ai.Id == currentAi.Id);
                if (aiConfig != null)
                {
                    // 基本列位置和跨度设置，先假设为正常情况每个WebView占4列
                    int column = (i % 3) * 4;
                    int columnSpan = 4;

                    // 对于第一组WebView数量不足3个的情况进行调整
                    if (_currentGroupIndex == 0) // 只有在处理第一组时才需要考虑
                    {
                        int firstGroupCount = Math.Min(currentAiCount, 3); // 第一组的WebView数量
                        if (firstGroupCount == 1)
                        {
                            columnSpan = 12; // 如果第一组只有一个WebView，则占据所有12列
                        }
                        else if (firstGroupCount == 2)
                        {
                            columnSpan = 6; // 如果第一组有两个WebView，则每个占6列
                        }
                        // 如果第一组有三个WebView，则默认每个占4列，无需调整
                    }

                    // 添加到待添加列表中
                    _webViewsToAdd.Add((aiConfig.Url, 0, column, columnSpan));
                }
            }

            // 将WebView添加到网格中，并设置可见性
            foreach (var (url, row, column, columnSpan) in _webViewsToAdd)
            {
                var webView = _webViewManager.GetWebViewForUrl(url);
                // 只有第一组的WebView初始可见，其余隐藏
                webView.IsVisible = _webViewsToAdd.IndexOf((url, row, column, columnSpan)) < _viewsCount;
                AddItemToGrid(webView, row, column, columnSpan);
            }
        }

        //同时显示4个webview的时候
        if (_viewsCount == 4)
        {
            for (int i = 0; i < currentAiCount; i++)
            {
                var currentAi = _configuration.CurrentAi[i];
                var aiConfig = _configuration.AiConfig.FirstOrDefault(ai => ai.Id == currentAi.Id);
                if (aiConfig != null)
                {
                    if (currentAiCount == 1)
                    {
                        // 对于只有一个 WebView 的情况，占据全部列
                        _webViewsToAdd.Add((aiConfig.Url, 0, 0, 12));
                    }
                    else if (currentAiCount == 2)
                    {
                        // 对于有两个 WebView 的情况，根据索引分配列
                        int column = i * 6; // 第一个 WebView 从第0列开始，第二个从第6列开始
                        _webViewsToAdd.Add((aiConfig.Url, 0, column, 6));
                    }
                    else if (currentAiCount == 3)
                    {
                        // 对于有三个 WebView 的情况，每个 WebView 分别占据4列
                        int column = i * 4; // 第一个从第0列开始，依此类推
                        _webViewsToAdd.Add((aiConfig.Url, 0, column, 4));
                    }
                    else
                    {
                        // 使用模运算确保每4个 WebView 的列位置相同，保持原有逻辑
                        int column = (i % 4) * 3; // 每个 WebView 占3列
                        _webViewsToAdd.Add((aiConfig.Url, 0, column, 3));
                    }
                }
            }

            // 将每个 WebView 添加到网格中，并根据索引设置可见性
            foreach (var (url, row, column, columnSpan) in _webViewsToAdd)
            {
                var webView = _webViewManager.GetWebViewForUrl(url);
                webView.IsVisible = true; // 根据新逻辑调整可见性
                AddItemToGrid(webView, row, column, columnSpan);
            }
        }

        // 当每次同时显示6个WebView时
        if (_viewsCount == 6)
        {
            for (int i = 0; i < currentAiCount; i++)
            {
                var currentAi = _configuration.CurrentAi[i];
                var aiConfig = _configuration.AiConfig.FirstOrDefault(ai => ai.Id == currentAi.Id);
                if (aiConfig != null)
                {
                    int columnSpan; // 声明列跨度变量
                    // 根据当前Ai的数量和位置动态计算列和列跨度
                    if (currentAiCount == 1)
                    {
                        // 只有一个WebView时，占据所有12列
                        columnSpan = 12;
                    }
                    else if (currentAiCount == 2)
                    {
                        // 两个WebViews时，每个占6列
                        columnSpan = 6;
                    }
                    else if (currentAiCount == 3)
                    {
                        // 三个WebViews时，每个占4列
                        columnSpan = 4;
                    }
                    else if (currentAiCount == 4)
                    {
                        // 四个WebViews时，每个占3列
                        columnSpan = 3;
                    }
                    else
                    {
                        // 五个以上WebViews时，每个占2列，直到六个为一组
                        columnSpan = 2;
                    }

                    // 当前WebView应该开始的列
                    int column = (i % 6) * (12 / Math.Min(6, currentAiCount)); // 计算开始列位置

                    // 添加到待添加列表中
                    _webViewsToAdd.Add((aiConfig.Url, 0, column, columnSpan));
                }
            }

            // 将WebView添加到网格中，并设置可见性
            foreach (var (url, row, column, columnSpan) in _webViewsToAdd)
            {
                var webView = _webViewManager.GetWebViewForUrl(url);
                // 只有第一组的WebView初始可见
                webView.IsVisible = _webViewsToAdd.IndexOf((url, row, column, columnSpan)) < 6;
                AddItemToGrid(webView, row, column, columnSpan);
            }
        }

        AddItemToGrid(_editor, 1, 0, 10);

        AddButtonToGrid( "\u27a2 Submit", OnSubmitClicked, 1, 10, 1, "Click to Send your question to AI");
        AddButtonToGrid( "\u21b9 Switch", OnSwitchClicked, 1, 11, 1, "Click to Switch AI");

        Content = _grid;

        if (_viewsCount == 2)
        {
            // 切换到对应的 WebView 组，如果只有一个组，则不需要切换
            if (currentAiCount > 2)
            {
                SwitchToGroup(_currentGroupIndex);
            }
        }

        if (_viewsCount == 3)
        {
            // 切换到对应的 WebView 组，如果只有一个组，则不需要切换
            if (currentAiCount > 3)
            {
                SwitchToGroup(_currentGroupIndex);
            }
        }

        //同时显示4个webview的时候
        if (_viewsCount == 4)
        {
            // 切换到对应的 WebView 组，如果只有一个组，则不需要切换
            if (currentAiCount > 4)
            {
                SwitchToGroup(_currentGroupIndex);
            }
        }

        //同时显示4个webview的时候
        if (_viewsCount == 6)
        {
            // 切换到对应的 WebView 组，如果只有一个组，则不需要切换
            if (currentAiCount > 6)
            {
                SwitchToGroup(_currentGroupIndex);
            }
        }

        UpdateToolbarItemsForCurrentGroup();
    }

    private ColumnDefinitionCollection GenerateColumnDefinitions(int count)
    {
        var columnDefinitions = new ColumnDefinitionCollection();
        for (int i = 0; i < count; i++)
        {
            columnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        }

        return columnDefinitions;
    }

    private void AddButtonToGrid(string text, EventHandler onClicked, int row, int column,
        int columnSpan, string toolTip)
    {
        var button = new Button
        {
            Text = text,
            HeightRequest = 50,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.End
        };
        ToolTipProperties.SetText(button, toolTip);

        button.Clicked += onClicked;
        AddItemToGrid(button, row, column, columnSpan);
    }


    private void AddItemToGrid(View item, int row, int column, int columnSpan)
    {
        _grid.Children.Add(item);
        Grid.SetRow(item, row);
        Grid.SetColumn(item, column);
        Grid.SetColumnSpan(item, columnSpan);
    }

    private async void OnSubmitClicked(object? sender, EventArgs e)
    {
       await SendMessage();
    }

    private async Task SendMessage()
    {
        if (string.IsNullOrEmpty(_editor.Text))
        {
            // Editor 的内容为空，因此直接返回不执行后续代码
            return;
        }

        string userInput = EscapeJavaScriptString(_editor.Text);

        // 遍历 CurrentAi 列表
        foreach (var currentAi in _configuration.CurrentAi)
        {
            var aiConfig = _configuration.AiConfig.FirstOrDefault(ai => ai.Id == currentAi.Id);

            if (aiConfig != null && !string.IsNullOrEmpty(aiConfig.Script))
            {
                // 替换 script 中的占位符 "[message]" 为用户输入的文本
                string scriptToExecute = aiConfig.Script.Replace("[message]", userInput);

                await _webViewManager.EvaluateJavaScriptAsync(aiConfig.Url, scriptToExecute);
            }
        }

        if (_editor.Text.Contains(StrFlag))
        {
            _editor.Text = RetainHashTagContent(_editor.Text)+"\n";
        }
        else
        {
            _editor.Text = string.Empty;
        }

        _editor.Focus();
    }
    
    
    private string RetainHashTagContent(string originalText)
    {
        var startIndex = 0;
        var endIndex = 0;
        var retainedText = new StringBuilder();

        // 更新搜索的开始和结束标记
        var startTag = StrFlag;
        var endTag = StrFlag;

        while ((startIndex = originalText.IndexOf(startTag, startIndex)) != -1 && 
               (endIndex = originalText.IndexOf(endTag, startIndex + startTag.Length)) != -1)
        {
            // 包括开始和结束标记本身在内截取内容
            var contentLength = (endIndex + endTag.Length) - startIndex;
            var content = originalText.Substring(startIndex, contentLength);
            retainedText.Append(content + " "); // 添加空格作为分隔符
            startIndex = endIndex + endTag.Length; // 移动到下一个开始标记的位置
        }

        return retainedText.ToString();
    }


    private void UpdateToolbarItemsForCurrentGroup()
    {
        // 首先清除现有的 ToolbarItems
        ToolbarItems.Clear();

        // 动态计算基于 viewsCount 的当前组的 WebView 范围
        int groupSize = _viewsCount; // 使用当前的viewsCount确定每组的大小
        int start = _currentGroupIndex * groupSize;
        int end = Math.Min(start + groupSize, _configuration.CurrentAi.Count);

        //
        // // 创建并添加新的ToolbarItem
        var toolbarItem = new ToolbarItem
        {
            Text = "\ud83c\udfe0 Home", // 显示名称
        };

        toolbarItem.Clicked += (sender, e) =>
        {
            // 这里添加你希望在点击时执行的代码
            RestoreWebViewsLayout(); // 例如，调用 RestoreWebViewsLayout 方法
        };

        ToolbarItems.Add(toolbarItem);

        // 为当前组的每个 WebView 添加 ToolbarItem
        for (int i = start; i < end; i++)
        {
            var currentAi = _configuration.CurrentAi[i];
            var aiConfig = _configuration.AiConfig.FirstOrDefault(ai => ai.Id == currentAi.Id);
            if (aiConfig != null)
            {
                // 创建并添加新的ToolbarItem
                ToolbarItems.Add(new ToolbarItem
                {
                    Text = "\ud83d\udfe2" + aiConfig.Name, // 显示AI配置的名称
                    Command = new Command(() => ExecuteLoadWebViewCommand(aiConfig.Url))
                });
            }
        }
    }

    // 新增方法：恢复 WebView 控件到其存储的布局位置
    private void RestoreWebViewsLayout()
    {
        SetVisibilityForEditorsAndButtons(_grid, true);

        foreach (var item in _webViewsInfo)
        {
            WebView webView = item.Key;
            webView.IsVisible = true; // Make all previously stored webviews visible in their original layout

            var layoutInfo = item.Value;
            Grid.SetRow(webView, layoutInfo.Row);
            Grid.SetRowSpan(webView, 1); // Restore original row span
            Grid.SetColumn(webView, layoutInfo.Column);
            Grid.SetColumnSpan(webView, layoutInfo.ColumnSpan);
        }
        // No need to clear _webViewsInfo here as it's repopulated if another view goes full screen
        // or used if switching groups. It should be cleared before FindVisibleWebViewsAndInfo if that's the intent.

        if (_exportMarkdownButton != null)
        {
            _exportMarkdownButton.IsVisible = false;
        }
        _focusedWebView = null;
        UpdateToolbarItemsForCurrentGroup(); // Refresh AI names in toolbar
    }

// 定义命令执行的方法
    private void ExecuteLoadWebViewCommand(string url)
    {
        // First, ensure all webviews are restored to their multi-view layout and visible
        // This also clears _focusedWebView and hides the export button.
        RestoreWebViewsLayout(); 
        
        // Then, hide editor and buttons for full-screen mode
        SetVisibilityForEditorsAndButtons(_grid, false);
        
        _webViewsInfo.Clear(); // Clear before finding new layout info for full screen

        // Find all currently visible WebViews and their layout to store them before going full screen
        // However, this should ideally happen BEFORE RestoreWebViewsLayout if we need to save their state prior to reset.
        // For full-screen, we are primarily concerned with the one becoming full-screen.
        // The existing _webViewsToAdd list (from InitializePageContent) might be more relevant for original positions.
        // Let's refine: _webViewsInfo should store the multi-view layout when it's active.
        // When going full-screen, we don't need to FindVisibleWebViewsAndInfo again for _webViewsInfo.
        // _webViewsInfo is populated in InitializePageContent via AddItemToGrid and implicitly by how WebViews are laid out.
        // The current FindVisibleWebViewsAndInfo would capture the state *after* RestoreWebViewsLayout if called here.
        // It's better to ensure _webViewsInfo is correctly populated when the multi-view layout is established.

        bool foundTargetWebView = false;
        foreach (var child in _grid.Children) // Iterate through children of the grid
        {
            if (child is WebView webView)
            {
                var webViewSourceUrl = (webView.Source as UrlWebViewSource)?.Url;
                if (webViewSourceUrl == url)
                {
                    // Store original layout if not already stored, or if it's the first time this view goes full screen
                    // This part is tricky if _webViewsInfo is not reliably populated with the multi-view state.
                    // Assuming _webViewsInfo is populated correctly during InitializePageContent or group switching.
                    if (!_webViewsInfo.ContainsKey(webView)) // Simplified: ensure it's in info, though ideally info is built once for multi-view
                    {
                         //This logic is flawed as GetRow/Col might be 0 if not previously set in a multi-view grid
                         //_webViewsInfo[webView] = (Url: webViewSourceUrl, Row: Grid.GetRow(webView), Column: Grid.GetColumn(webView), ColumnSpan: Grid.GetColumnSpan(webView));
                    }

                    Grid.SetRow(webView, 0);
                    Grid.SetRowSpan(webView, 2); // Span across both rows (WebView row and Editor/Button row)
                    Grid.SetColumn(webView, 0);
                    Grid.SetColumnSpan(webView, 12); // Span all columns
                    webView.IsVisible = true;
                    _focusedWebView = webView;
                    if (_exportMarkdownButton != null)
                    {
                        _exportMarkdownButton.IsVisible = true;
                    }
                    foundTargetWebView = true;
                }
                else
                {
                    webView.IsVisible = false; // Hide other WebViews
                }
            }
        }
        if(foundTargetWebView) {
             UpdateToolbarItemsForFocusedView(); // Show only Home and Export
        }
    }
    
    private void UpdateToolbarItemsForFocusedView()
    {
        var homeButtonText = "\ud83c\udfe0 Home";
        var itemsToRemove = ToolbarItems.Where(item => item.Text != homeButtonText && item != _exportMarkdownButton).ToList();
        foreach (var item in itemsToRemove)
        {
            ToolbarItems.Remove(item);
        }
        // Ensure Home and Export buttons are correctly ordered if needed, but they should persist.
    }


    private void SetVisibilityForEditorsAndButtons(Grid grid, bool isVisible)
    {
        foreach (var child in grid.Children)
        {
            if (child is Editor editor)
            {
                editor.IsVisible = isVisible;
            }
            else if (child is Button button)
            {
                button.IsVisible = isVisible;
            }
        }
    }

    private void FindVisibleWebViewsAndInfo(View view)
    {
        if (view is WebView webView && webView.IsVisible)
        {
            string url = string.Empty;
            // 确定 WebView.Source 的类型并相应地获取 URL
            if (webView.Source is UrlWebViewSource urlSource)
            {
                url = urlSource.Url; // 获取实际的 URL 字符串
            }

            var row = Grid.GetRow(webView);
            var column = Grid.GetColumn(webView);
            var columnSpan = Grid.GetColumnSpan(webView);

            // 将 WebView 及其信息添加到字典中
            _webViewsInfo.Add(webView, (Url: url, Row: row, Column: column, ColumnSpan: columnSpan));
        }

        // 如果当前视图是布局容器，遍历其子视图
        if (view is Layout layout)
        {
            foreach (var child in layout.Children)
            {
                if (child is View childView)
                {
                    FindVisibleWebViewsAndInfo(childView);
                }
            }
        }
    }

    private void OnSwitchClicked(object? sender, EventArgs e)
    {
        int numberOfGroups;

        int numberOfWebViews = _configuration.CurrentAi.Count;

        // 根据viewsCount计算组数
        if (_viewsCount == 1)
        {
            numberOfGroups = numberOfWebViews; // 当每组显示1个WebView时
        }
        else if (_viewsCount == 2)
        {
            numberOfGroups = (int)Math.Ceiling(numberOfWebViews / 2.0); // 当每组显示2个WebView时
        }
        else if (_viewsCount == 3)
        {
            numberOfGroups = (int)Math.Ceiling(numberOfWebViews / 3.0); // 当每组显示3个WebView时
        }
        else if (_viewsCount == 4)
        {
            numberOfGroups = (int)Math.Ceiling(numberOfWebViews / 4.0); // 当每组显示4个WebView时
        }
        else if (_viewsCount == 6)
        {
            numberOfGroups = (int)Math.Ceiling(numberOfWebViews / 6.0); // 当每组显示6个WebView时
        }
        else
        {
            // 如果viewsCount不是1、2、3、4或6，默认处理逻辑（可选）
            numberOfGroups = 1; // 默认为一个组
        }

        // 递增当前组索引，如果超过最后一组，则重置为第一组
        _currentGroupIndex = (_currentGroupIndex + 1) % numberOfGroups;

        UpdateWebViewVisibility();
        UpdateToolbarItemsForCurrentGroup();
    }

    public void SwitchToGroup(int targetGroupIndex)
    {
        int numberOfWebViews = _configuration.CurrentAi.Count;
        int numberOfGroups = (int)Math.Ceiling(numberOfWebViews / (double)_viewsCount);

        if (targetGroupIndex >= 0 && targetGroupIndex < numberOfGroups)
        {
            _currentGroupIndex = targetGroupIndex;
            UpdateWebViewVisibility();
            UpdateToolbarItemsForCurrentGroup();
        }
    }

    private void UpdateWebViewVisibility()
    {
        int numberOfWebViews = _configuration.CurrentAi.Count;
        int groupSize = _viewsCount; // 使用当前的viewsCount确定每组的大小
        int startGroupIndex = _currentGroupIndex * groupSize;
        int endGroupIndex = Math.Min(startGroupIndex + groupSize, numberOfWebViews);

        // 重新计算当前组内应该可见的WebView数量
        int visibleWebViewsCount = endGroupIndex - startGroupIndex;

        for (int i = 0; i < _webViewsToAdd.Count; i++)
        {
            var webView = (WebView)_grid.Children[i]; // 获取正确的WebView引用
            bool isVisible = i >= startGroupIndex && i < endGroupIndex;
            webView.IsVisible = isVisible;

            if (isVisible)
            {
                int columnSpan;
                // 根据当前组内可见的WebView数量调整列占用
                switch (visibleWebViewsCount)
                {
                    case 1:
                        columnSpan = 12; // 单个WebView占据所有列
                        break;
                    case 2:
                        columnSpan = 6; // 两个WebView，每个占6列
                        break;
                    case 3:
                        columnSpan = 4; // 三个WebView，每个占4列
                        break;
                    case 4:
                        columnSpan = 3; // 四个WebView，每个占3列
                        break;
                    case 5:
                        columnSpan = 12 / 5; // 当有5个WebView时，尽量平均分配，实际操作中可能需要调整以适应实际布局
                        break;
                    case 6:
                        columnSpan = 2; // 每组正好6个WebView时，每个WebView占据2列
                        break;
                    default:
                        columnSpan = 12; // 默认设置，以防万一
                        break;
                }

                Grid.SetColumnSpan(webView, columnSpan);
                // 计算并设置WebView的起始列，确保它们在网格中正确对齐
                int column = ((i - startGroupIndex) % groupSize) * (12 / visibleWebViewsCount);
                Grid.SetColumn(webView, column);
            }
            else
            {
                webView.IsVisible = false; // 非当前组的WebView隐藏
            }
        }
    }

    /// <summary>
    /// 字符串转义
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private string EscapeJavaScriptString(string input)
    {
        // 首先，手动替换已知的特殊字符
        string escapedInput = input
            .Replace("\\", "\\\\") // 转义反斜杠
            .Replace("\r", "\\r") // 转义回车符
            .Replace("\n", "\\n") // 转义换行符
            .Replace("\"", "\\\"") // 转义双引号
            .Replace("'", "\\'") // 转义单引号
            .Replace("\t", "\\t"); // 转义制表符

        // 然后，使用JavaScriptStringEncode进行进一步的转义，并添加外部双引号
        return System.Web.HttpUtility.JavaScriptStringEncode(escapedInput, addDoubleQuotes: false);
    }
}