# Project Context

## Purpose
Aila (AISuperDomain) is a cross-platform desktop application that integrates multiple leading AI models into a single unified interface. Users can pose questions and receive simultaneous answers from various AI models (ChatGPT, Gemini, Claude, Copilot, etc.), enabling comparison of different AI perspectives and insights.

## Tech Stack
- .NET MAUI (Multi-platform App UI) for cross-platform desktop/mobile application
- C# as the primary programming language
- XAML for UI definitions
- WebView components for embedding AI chat interfaces
- JSON configuration for AI models and prompts
- CommunityToolkit.Mvvm for messaging patterns
- CommunityToolkit.Maui for file operations
- ReverseMarkdown for HTML to Markdown conversion

## Project Conventions

### Code Style
- C# naming conventions (PascalCase for public members, _camelCase for private fields)
- XAML files paired with code-behind (.xaml.cs)
- Configuration stored in config.json (loaded from AppDataDirectory as config.txt)
- Comments in Chinese and English mixed

### Architecture Patterns
- MVVM-lite pattern with code-behind for simplicity
- WebView-based integration for AI services (no API calls, uses web interfaces)
- Grid-based responsive layout with dynamic column definitions (12-column grid)
- Configuration-driven AI model definitions with JavaScript injection scripts
- Messenger pattern (WeakReferenceMessenger) for cross-component communication

### Testing Strategy
- Manual testing on Windows and macOS platforms
- WebView script testing through browser DevTools

### Git Workflow
- Feature branches for new features
- Conventional commits preferred (feat:, fix:, chore:)

## Domain Context
- Each AI model is defined with: id, name, url, script (JavaScript to inject messages), iniScript
- WebViews display AI chat interfaces directly embedded in the app
- Users can display 1, 2, 3, 4, or 6 AI models simultaneously (ViewsCount)
- CurrentAi list defines which AI models are active for the user
- Prompt templates support "/" for quick commands and "#" for persistent prompts
- Export to Markdown feature converts AI responses to markdown files

## Important Constraints
- No API keys required - uses public web interfaces directly
- Must work on Windows (.NET 8 required) and macOS
- WebView scripts must be compatible with target AI websites (may break when sites update)
- Multi-language support (EN, ZH-CN, ZH-TW, JA, KO, FR)

## External Dependencies
- Various AI web services (ChatGPT, Gemini, Claude, Copilot, Perplexity, etc.)
- CommunityToolkit.Maui for file saving operations
- ReverseMarkdown for HTML to Markdown conversion
