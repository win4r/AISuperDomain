# Change: Add AI Favorites Feature

## Why
Users who frequently use Aila with many AI models configured (27+ in default config) need a way to quickly access their preferred AI models. Currently, users must use the "Switch" button to cycle through groups or manually select from the toolbar, which becomes cumbersome when working with a large number of configured models. A favorites feature would allow users to mark their most-used AI models for quick filtering.

## What Changes
- Add `IsFavorite` boolean property to `CurrentAi` class to track favorite status per AI
- Add a "Favorites" toolbar button on MainPage to toggle between showing all AIs or favorites only
- Display star indicator (⭐) next to favorited AI model names in toolbar
- Add favorite toggle (star button) in SettingPage for each selected AI model
- Persist favorite status in config.json alongside existing CurrentAi configuration

## Impact
- Affected specs: ai-favorites (new capability)
- Affected code:
  - `CurrentAi.cs` - Add IsFavorite property
  - `MainPage.xaml.cs` - Add favorites toolbar button and filtering logic
  - `SettingPage.xaml` - Add star button UI for favorite toggle
  - `SettingPage.xaml.cs` - Add FavoriteButton_Clicked handler and save logic
- No breaking changes - existing configurations will default IsFavorite to false
