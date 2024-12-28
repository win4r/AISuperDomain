
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Messaging;

namespace Aila;

public partial class SettingPage
{

    private ObservableCollection<SelectableAiConfig>
        _selectedAiConfigs = new ObservableCollection<SelectableAiConfig>();

    private ObservableCollection<NumberItem> _numberItems = new ObservableCollection<NumberItem>();

    private ConfigurationManager _configurationManager = new ConfigurationManager();

    private AppConfiguration _appConfiguration;


    public SettingPage()
    {
        InitializeComponent();

        _configurationManager.OnDisplayAlertRequested = DisplayAlertAsync;

        // 异步加载配置
        LoadConfigurationAndAiConfigsAsync();

        // 设置第二个CollectionView的数据源
        SelectedAiConfigsCollectionView.ItemsSource = _selectedAiConfigs;

        InitializeNumberCollectionView();
        
        // 订阅消息
        WeakReferenceMessenger.Default.Register<ConfigurationUpdatedMessage>(this, (recipient, message) =>
        {
            // // 当收到配置更新的消息时执行的逻辑
            // if (this.Content is Layout layout)
            // {
            //     layout.Children.Clear();
            // }
            
            // 异步加载配置
            LoadConfigurationAndAiConfigsAsync();

            // 设置第二个CollectionView的数据源
            SelectedAiConfigsCollectionView.ItemsSource = _selectedAiConfigs;

            InitializeNumberCollectionView();

        });

    }

    private void InitializeNumberCollectionView()
    {
        var numbers = new List<string> { "1", "2", "3", "4", "6" }; // 现在直接使用字符串
        string vCount = (_appConfiguration?.ViewsCount?.VCount ?? 0).ToString(); // 转换为字符串
        _numberItems.Clear(); // 清空现有项
        foreach (var number in numbers)
        {
            _numberItems.Add(new NumberItem
            {
                Number = number,
                IsSelected = vCount == number // 比较字符串
            });
        }

        NumberCollectionView.ItemsSource = _numberItems;
    }





    private async void LoadConfigurationAndAiConfigsAsync()
    {
        _appConfiguration = await _configurationManager.LoadConfigurationAsync();
        if (_appConfiguration != null)
        {
            LoadAiConfigs();
            InitializeNumberCollectionView(); // Moved here
        }
    }

    private void LoadAiConfigs()
    {
        // 使用 _appConfiguration 中的数据填充UI
        AiConfigsCollectionView.ItemsSource = _appConfiguration.AiConfig.Select(aiConfig => new SelectableAiConfig
        {
            Id = aiConfig.Id,
            Name = aiConfig.Name,
            IsSelected = _appConfiguration.CurrentAi.Any(cai => cai.Id == aiConfig.Id)
        }).ToList();
    }

    private Task DisplayAlertAsync(string title, string message, string cancel)
    {
        return DisplayAlert(title, message, cancel);
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkBox = sender as CheckBox;
        var selectableAiConfig = checkBox.BindingContext as SelectableAiConfig;
        if (selectableAiConfig != null)
        {
            if (checkBox.IsChecked)
            {
                // 如果CheckBox被选中，则将对应的配置添加到第二个CollectionView中
                if (!_selectedAiConfigs.Any(x => x.Id == selectableAiConfig.Id))
                {
                    _selectedAiConfigs.Add(selectableAiConfig);
                }
            }
            else
            {
                // 如果CheckBox被取消选中，则从第二个CollectionView中移除对应的配置
                var itemToRemove = _selectedAiConfigs.FirstOrDefault(x => x.Id == selectableAiConfig.Id);
                if (itemToRemove != null)
                {
                    _selectedAiConfigs.Remove(itemToRemove);
                }
            }
        }
    }
    
    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        // 更新CurrentAi
        _appConfiguration.CurrentAi.Clear();
        foreach (var selectedItem in _selectedAiConfigs.Where(ai => ai.IsSelected))
        {
            var matchingAiConfig = _appConfiguration.AiConfig.FirstOrDefault(ai => ai.Name == selectedItem.Name);
            if (matchingAiConfig != null)
            {
                _appConfiguration.CurrentAi.Add(new CurrentAi { Id = matchingAiConfig.Id });
            }
        }

        // 更新ViewsCount
        var selectedNumberItem = _numberItems.FirstOrDefault(n => n.IsSelected);
        if (selectedNumberItem != null)
        {
            _appConfiguration.ViewsCount.VCount = int.Parse(selectedNumberItem.Number);
        }

        // 保存配置
        await _configurationManager.SaveConfigurationAsync(_appConfiguration);
        
        // MessagingCenter.Send<SettingPage>(this, "ConfigurationUpdated");
        
        WeakReferenceMessenger.Default.Send(new ConfigurationUpdatedMessage());


    }
}

// 一个辅助类，用于UI绑定
public class SelectableAiConfig : AiConfig
{
    public bool IsSelected { get; set; }
}

public class NumberItem
{
    public string Number { get; set; } // 现在是string类型
    public bool IsSelected { get; set; }
}

