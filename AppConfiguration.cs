namespace Aila;

public class AppConfiguration
{
    public List<AiConfig> AiConfig { get; set; }
    public List<CurrentAi> CurrentAi { get; set; }
    public ViewsCount ViewsCount { get; set; }
    
    public List<Prompts> Prompts { get; set; } = new List<Prompts>();



}