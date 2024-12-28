using CommunityToolkit.Mvvm.Messaging;

namespace Aila;

public partial class ConfigPage
{
    public ConfigPage()
    {
        InitializeComponent();
        InitializeEditorContent();
    }
   
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await InitializeEditorContent();
    }
    
    private async Task InitializeEditorContent()
    {
        try
        {
            string content = await ReadFromFile("config.txt");
            editor.Text = content;
        }
        catch (Exception ex)
        {
            // 使用 DisplayAlert 显示错误消息
            await DisplayAlert("Error", $"Failed to load content: {ex.Message}", "OK");
        }
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        try
        {
            string content = editor.Text.Replace('“', '"')  // 中文左双引号
                .Replace('”', '"')  // 中文右双引号
                .Replace('„', '"')  // 德文低-9引号
                .Replace('«', '"')  // 法文左引号
                .Replace('»', '"')  // 法文右引号
                .Replace('“', '"')  // 德文左引号
                .Replace('”', '"'); // 德文右引号;
            await WriteToFile("config.txt", content);
            // 可以在这里添加一个成功消息
            await DisplayAlert("Success", "Content saved successfully.", "OK");
        }
        catch (Exception ex)
        {
            // 使用 DisplayAlert 显示错误消息
            await DisplayAlert("Error", $"Failed to save content: {ex.Message}", "OK");
        }
        
        // MessagingCenter.Send<ConfigPage>(this, "ConfigUpdated");

        WeakReferenceMessenger.Default.Send(new ConfigurationUpdatedMessage());

    }

    private async void OnCheckConfigUpdateClicked(object sender, EventArgs e)
    {
        await Launcher.OpenAsync("https://github.com/win4r/AISuperDomain/blob/main/config.json");
    }

    private async Task<string> ReadFromFile(string fileName)
    {
        var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
        if (File.Exists(filePath))
        {
            return await File.ReadAllTextAsync(filePath);
        }
        return string.Empty;
    }

    private async Task WriteToFile(string fileName, string content)
    {
        var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
        await File.WriteAllTextAsync(filePath, content);
    }

}