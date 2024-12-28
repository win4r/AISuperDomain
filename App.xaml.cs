namespace Aila;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        var configurationManager = new ConfigurationManager();
        Task.Run(async () => await configurationManager.InitializeDefaultConfigurationAsync()).Wait();
        
        // //测试删除配置文件
        // var filePath = Path.Combine(FileSystem.AppDataDirectory, "config.txt");
        //
        // // 检查文件是否存在
        // if (File.Exists(filePath))
        // {
        //     try
        //     {
        //         File.Delete(filePath);
        //         // 可选：如果您有界面反馈机制，可以在这里通知用户文件已被删除
        //         Console.WriteLine("Configuration file deleted successfully.");
        //     }
        //     catch (Exception ex)
        //     {
        //         // 如果删除过程中出现错误，可以在这里处理异常
        //         // 例如，记录日志或通知用户错误信息
        //         Console.WriteLine($"Failed to delete configuration file: {ex.Message}");
        //     }
        // }
        // else
        // {
        //     // 文件不存在时的处理逻辑
        //     Console.WriteLine("Configuration file does not exist.");
        // }
        
        MainPage = new AppShell();
        // MainPage = new NavigationPage(new MainPage());
    }
}