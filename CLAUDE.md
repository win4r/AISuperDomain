# CLAUDE.md - AI Assistant Guide for Aila

This document provides comprehensive guidance for AI assistants working with the Aila codebase.

## Project Overview

**Aila** (AI Super Domain) is a cross-platform desktop application built with .NET MAUI that allows users to interact with 24+ AI models simultaneously from a unified interface. Users can submit queries and receive responses from multiple AI services like ChatGPT, Gemini, Claude, Copilot, and many others in a single view.

### Key Features
- Multi-AI integration (24+ models)
- Dynamic grid display (1, 2, 3, 4, or 6 AI responses)
- Full-screen mode for individual AI responses
- Export responses to Markdown
- Custom prompt templates with "/" suggestions
- Persistent prompts with "#" prefix
- JSON-based configuration for adding/modifying AI models

## Tech Stack

| Component | Technology |
|-----------|------------|
| Framework | .NET MAUI 8.0 |
| Language | C# |
| Pattern | MVVM (CommunityToolkit.Mvvm 8.2.2) |
| UI Toolkit | CommunityToolkit.Maui 7.0.1 |
| Markdown | ReverseMarkdown 3.2.0 |
| Platforms | Windows, macOS, iOS, Android |

## Directory Structure

```
AISuperDomain/
├── Root C# Files (Core Application)
│   ├── App.xaml(.cs)              # Application entry point
│   ├── AppShell.xaml(.cs)         # Navigation shell with flyout menu
│   ├── MainPage.xaml(.cs)         # Primary chat interface (~39KB)
│   ├── SettingPage.xaml(.cs)      # AI model selection & display settings
│   ├── ConfigPage.xaml(.cs)       # JSON configuration editor
│   ├── MauiProgram.cs             # MAUI initialization
│   ├── ConfigurationManager.cs    # Core config management (~800 lines)
│   ├── WebViewManager.cs          # WebView instance management
│   └── Data Models:
│       ├── AiConfig.cs            # AI configuration model
│       ├── AppConfiguration.cs    # App configuration model
│       ├── CurrentAi.cs           # Selected AI model
│       ├── ViewsCount.cs          # Display count setting
│       └── Prompts.cs             # Prompt templates
│
├── Platforms/                     # Platform-specific implementations
│   ├── Android/                   # MainActivity, AndroidManifest.xml
│   ├── iOS/                       # AppDelegate, Program.cs
│   ├── MacCatalyst/               # macOS-specific code
│   ├── Windows/                   # WinUI 3 / MSIX packaging
│   └── Tizen/                     # Samsung Tizen (optional)
│
├── Resources/
│   ├── AppIcon/                   # Application icons (SVG)
│   ├── Splash/                    # Splash screen (SVG)
│   ├── Images/                    # Application images
│   ├── Fonts/                     # OpenSans fonts
│   ├── Styles/
│   │   ├── Colors.xaml            # Color palette (Purple #512BD4 primary)
│   │   └── Styles.xaml            # XAML styling (~24KB)
│   └── Raw/                       # Raw assets
│
├── Properties/
│   └── launchSettings.json        # Debug launch configuration
│
├── Configuration
│   ├── Aila.csproj                # Project file
│   ├── Aila.sln                   # Solution file
│   └── config.json                # AI model configurations (~40KB)
│
└── Documentation
    ├── README.md                  # English
    ├── README_ZH-CN.md            # Simplified Chinese
    ├── README_ZH-TW.md            # Traditional Chinese
    ├── README_JA-JP.md            # Japanese
    ├── README_KO-KR.md            # Korean
    └── README_FR-FR.md            # French
```

## Key Files Reference

### Core Application Files

| File | Purpose | Key Responsibilities |
|------|---------|---------------------|
| `MainPage.xaml.cs` | Primary UI | Grid layout, WebView rendering, query submission, Markdown export |
| `ConfigurationManager.cs` | Config management | Load/save config, default AI models, JavaScript injection |
| `SettingPage.xaml.cs` | Settings UI | AI selection, display count configuration |
| `ConfigPage.xaml.cs` | Config editor | Direct JSON configuration editing |
| `WebViewManager.cs` | WebView utils | URL caching, JavaScript evaluation |

### Data Models

```csharp
// AiConfig.cs - AI model configuration
public class AiConfig {
    public int Id { get; set; }
    public string Name { get; set; }      // Display name (e.g., "ChatGPT")
    public string Url { get; set; }       // AI service URL
    public string Script { get; set; }    // JavaScript for form interaction
    public string IniScript { get; set; } // Initialization script
}
```

## Development Commands

### Building
```bash
# Build for all platforms
dotnet build

# Build for specific platform
dotnet build -f net8.0-windows10.0.19041.0
dotnet build -f net8.0-maccatalyst
dotnet build -f net8.0-android
dotnet build -f net8.0-ios
```

### Running
```bash
# Run on Windows
dotnet run -f net8.0-windows10.0.19041.0

# Run on macOS
dotnet run -f net8.0-maccatalyst
```

### Publishing
```bash
# Publish for Windows (MSIX)
dotnet publish -f net8.0-windows10.0.19041.0 -c Release

# Publish for macOS
dotnet publish -f net8.0-maccatalyst -c Release
```

## Code Conventions

### Namespace
All code uses the `Aila` namespace:
```csharp
namespace Aila;
```

### Patterns & Architecture
- **MVVM Pattern**: Uses CommunityToolkit.Mvvm for view models
- **Messaging**: `WeakReferenceMessenger` for cross-component communication
- **Async/Await**: Heavy use of async patterns for I/O and long operations
- **Data Binding**: XAML data binding for UI-data synchronization

### Naming Conventions
- **Classes**: PascalCase (e.g., `ConfigurationManager`, `AiConfig`)
- **Methods**: PascalCase (e.g., `InitializeDefaultConfigurationAsync`)
- **Properties**: PascalCase (e.g., `DisplayAlertDelegate`)
- **Private fields**: camelCase or with underscore prefix
- **Async methods**: Suffix with `Async`

### File Organization
- One primary class per file
- File name matches class name
- XAML files paired with `.xaml.cs` code-behind

## Configuration System

### Config File Location
Configuration is stored in `config.txt` in the app's data directory:
```csharp
var filePath = Path.Combine(FileSystem.AppDataDirectory, "config.txt");
```

### AI Configuration Structure
Each AI model requires:
1. **id**: Unique integer identifier
2. **name**: Display name shown in UI
3. **url**: Base URL for the AI service
4. **script**: JavaScript to inject query into AI's input field
5. **iniScript**: Optional initialization script

### JavaScript Injection Pattern
Scripts use `[message]` placeholder for the user's query:
```javascript
(function() {
    var textarea = document.querySelector('textarea');
    textarea.value = '[message]';
    textarea.dispatchEvent(new Event('input', {bubbles: true}));
    // ... submit logic
})();
```

## Platform-Specific Notes

### Windows
- Requires .NET 8 runtime
- Uses MSIX packaging
- Minimum Windows 10 version: 10.0.17763.0

### macOS
- Minimum macOS version: 13.1 (Catalyst)
- Supports both x64 and arm64 architectures

### Android
- Minimum Android API: 21.0
- Uses AndroidAppCompat activity

### iOS
- Minimum iOS version: 11.0
- Uses standard AppDelegate pattern

## Common Development Tasks

### Adding a New AI Model
1. Edit `config.json` or modify `ConfigurationManager.cs` default config
2. Add entry with unique `id`, `name`, `url`, `script`, and optional `iniScript`
3. Test JavaScript injection script in browser DevTools first

### Modifying UI Layout
1. Edit XAML files (`.xaml`) for structure
2. Edit code-behind (`.xaml.cs`) for logic
3. Update `Styles.xaml` for styling changes

### Adding New Features
1. Follow MVVM pattern
2. Use `WeakReferenceMessenger` for component communication
3. Add async handling for I/O operations
4. Update relevant pages (`MainPage`, `SettingPage`, etc.)

## Testing Considerations

- Test on multiple platforms when making UI changes
- Verify JavaScript injection works for new AI integrations
- Test with different display count configurations (1, 2, 3, 4, 6)
- Verify export to Markdown functionality

## Important Notes for AI Assistants

1. **Nullable Enabled**: The project has `<Nullable>enable</Nullable>` - handle null references appropriately
2. **Implicit Usings**: Common namespaces are implicitly included
3. **Comments**: Some comments are in Chinese (项目有中文注释)
4. **Config Persistence**: Configuration changes need to be saved via `ConfigurationManager`
5. **WebView Security**: Be cautious with JavaScript injection scripts
6. **Cross-Platform**: Changes should consider all target platforms

## Git Workflow

### Branch Naming
- Feature branches: `feature/<description>`
- Bug fixes: `fix/<description>`
- Claude AI branches: `claude/<session-id>`

### Commit Style
Recent commits use concise messages:
- `feat: Implement Export to Markdown functionality`
- `Update README_ZH-TW.md`

## External Resources

- **Demo Video**: https://b23.tv/B1OXY2V (Bilibili)
- **Releases**: https://github.com/win4r/AISuperDomain/releases
- **Support**: GitHub Issues or win4r@outlook.com

## License

MIT License - See LICENSE file for details.
