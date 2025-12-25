# ai-favorites Specification

## Purpose
TBD - created by archiving change add-ai-favorites. Update Purpose after archive.
## Requirements
### Requirement: Favorite AI Models
Users SHALL be able to mark AI models as favorites for quick access. The system SHALL persist favorite status across application restarts in the configuration file.

#### Scenario: Mark AI as favorite in settings
- **WHEN** user taps the empty star icon (☆) next to a selected AI model in Settings
- **THEN** the AI model is marked as favorite and the star icon becomes filled (⭐)

#### Scenario: Unmark AI as favorite in settings
- **WHEN** user taps the filled star icon (⭐) next to a favorited AI model in Settings
- **THEN** the AI model is unmarked as favorite and the star icon becomes empty (☆)

#### Scenario: Favorites persist after save
- **WHEN** user saves configuration with marked favorites and restarts the application
- **THEN** previously marked favorites remain marked

### Requirement: Favorites Quick Access Filter
The system SHALL provide a dedicated toolbar button to filter and display only favorited AI models in the main view.

#### Scenario: Show favorites only
- **WHEN** user clicks the "⭐ Favorites" toolbar button from normal view
- **THEN** only AI models marked as favorites are displayed in the view area
- **AND** the button text changes to "⭐ All AIs"

#### Scenario: Return to all AIs view
- **WHEN** user clicks the "⭐ All AIs" toolbar button from favorites view
- **THEN** all configured AI models are displayed (respecting current group/view settings)
- **AND** the button text changes to "⭐ Favorites"

#### Scenario: Empty favorites view
- **WHEN** user has no favorites marked and clicks "⭐ Favorites"
- **THEN** the view displays no AI models

### Requirement: Favorite Visual Indicator
The system SHALL display a visual star indicator next to favorited AI model names in the toolbar.

#### Scenario: Star indicator in toolbar for favorites
- **WHEN** favorited AI models are displayed in toolbar
- **THEN** their names are prefixed with a filled star (⭐)

#### Scenario: Circle indicator in toolbar for non-favorites
- **WHEN** non-favorited AI models are displayed in toolbar
- **THEN** their names are prefixed with the existing green circle indicator (🟢)

### Requirement: Backwards Compatibility
The system SHALL maintain backwards compatibility with existing configuration files that do not contain favorite information.

#### Scenario: Load legacy configuration
- **WHEN** application loads a configuration file without IsFavorite fields
- **THEN** all AI models default to IsFavorite = false
- **AND** application functions normally without errors

