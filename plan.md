# RC2K.Presentation.HstEraser — Avalonia HST Editor

## Problem
Create a desktop Avalonia UI application that allows users to load, view, and edit `.hst` high-score files from the RC2K rally game. Users should be able to browse top-10 results per rally/stage/mode, and delete individual entries (zero out centiseconds). The app must match the dark-themed design from `design.jpg`.

## Approach
- Create a new Avalonia MVVM project at `src/RC2K.Presentation.HstEraser`
- Reference `RC2K.Parser` for HST file parsing
- Extend the parser to support **write-back** (track byte offsets, zero centiseconds in raw bytes)
- Target `net10.0`
- Use **CommunityToolkit.Mvvm** for MVVM pattern (lighter than ReactiveUI)
- Add project to `RC2K.sln`

## Design Colors (from design.jpg)
- App background: `#808080` (medium gray)
- Content panel: `#333333` (dark charcoal)
- Table/grid: `#000000` (black) with `#444444` row borders
- Text: `#FFFFFF` (white)
- Position numbers: `#FFFFFF`
- Buttons (Load, Update): `#C0C0C0` (light gray) with dark text
- Toggle buttons: white when active, gray when inactive
- ComboBox: white background, dark text

## Data Model Mapping
HST file structure (per `TimeEntriesCollection`):
- 6 rallies × 6 stages × 10 entries = 360 entries per mode (Sim/Arc × Normal/TT = 4 modes)
- Rally → StageCode mapping:
  - Rally 1 (Vauxhall): codes 41-46
  - Rally 2 (Pirelli): codes 61-66
  - Rally 3 (Scottish): codes 31-36
  - Rally 4 (SEAT Jim Clark): codes 71-76
  - Rally 5 (Stena): codes 51-56
  - Rally 6 (SONY Manx): codes 21-26
- Stage names come from `RC2K.Resources/Properties/stages-info.json`

## UI Layout (matching design.jpg)
```
┌──────────────────────────────────────────────────────────┐
│ [ComboBox: Rally ▼]  [Simulation|Arcade] [TimeTrial|Normal]  [<] [>] │
│                                                                       │
│ Stage Name – Simulation (Time Trial)                                  │
│                                                                       │
│ ┌─────┬─────┬──────────┬──────────┬────────┐                         │
│ │  #  │ NAT │  DRIVER  │   TIME   │ DELETE │                         │
│ ├─────┼─────┼──────────┼──────────┼────────┤                         │
│ │  1  │     │ Name     │ m:ss.ff  │  🗑️   │                         │
│ │ ... │     │          │          │        │                         │
│ │ 10  │     │          │          │  🗑️   │                         │
│ └─────┴─────┴──────────┴──────────┴────────┘                         │
│                                                                       │
│ HST file: [________________________]  [Load]           [UPDATE]       │
└──────────────────────────────────────────────────────────┘
```

## Todos

### 1. parser-write-support
**Extend RC2K.Parser for write-back capability**
- Add `ByteOffset` property to `TimeEntry` to record stream position of the entry start
- Store `CentisecondsOffset` = ByteOffset + 20 (5 × Int32 before Centiseconds)
- Create `HstWriter` class that:
  - Takes the raw byte[] of the file and a list of modified TimeEntries
  - Writes 0 at the CentisecondsOffset for zeroed entries
  - Saves modified byte[] back to file
- Modify `TimeEntry` constructor to capture `reader.BaseStream.Position` before reading

### 2. create-avalonia-project
**Create the Avalonia project skeleton**
- Create folder `src/RC2K.Presentation.HstEraser`
- Create `.csproj` targeting net10.0 with Avalonia packages:
  - Avalonia (12.x)
  - Avalonia.Desktop
  - Avalonia.Themes.Fluent
  - CommunityToolkit.Mvvm
- Reference `RC2K.Parser` and `RC2K.Resources`
- Create new Solution, RC2K.HstEraser.sln and add there proper projects
- Create entry point: `Program.cs`, `App.axaml/cs`

### 3. create-viewmodels
**Create MVVM ViewModels**
- `MainViewModel`:
  - `SelectedRally` (ComboBox, 6 rallies from stages-info.json)
  - `IsArcade` (toggle: Simulation/Arcade)
  - `IsTimeTrial` (toggle: Normal/TimeTrial)
  - `StageIndex` (0-5, controlled by left/right arrows)
  - `StageName` (computed: "Stage Name – Simulation (Time Trial)")
  - `Entries` ObservableCollection of `ScoreEntryViewModel` (10 items)
  - `HstFilePath` string
  - `LoadCommand`, `UpdateCommand`
  - `NextStageCommand`, `PrevStageCommand`
  - `DeleteEntryCommand(int position)`
- `ScoreEntryViewModel`:
  - `Position` (1-10)
  - `Nat` (string)
  - `DriverName` (string)
  - `Time` (formatted string from centiseconds, "m:ss.ff")
  - `Centiseconds` (raw value)

### 4. create-main-view
**Create the MainWindow AXAML UI**
- Match the dark theme from design.jpg
- Top row: ComboBox + toggle buttons + navigation arrows
- Title row: stage name with mode info
- DataGrid with 5 columns (#, NAT, DRIVER, TIME, DELETE)
- Bottom row: file path TextBox, Load button, Update button
- Delete column: icon button (trash can) per row
- Style everything with dark background, white text, matching the design colors

### 5. wire-up-logic
**Connect ViewModel to Parser and Writer**
- Load: read HST file into byte[] + parse with HighScores
- Map rally/stage/mode selection to correct StageEntry
- Populate the 10-entry table
- Delete: zero centiseconds in memory + mark entry as modified
- Update: use HstWriter to write modified byte[] back to file
- Navigation: changing rally/stage/mode refreshes the table
- File picker or text input for HST file path

### 6. build-and-verify
**Build the project, fix any issues**
- Run `dotnet build` on the new project
- Verify all references resolve
- Fix any compilation errors

## Dependencies
- `create-avalonia-project` depends on `parser-write-support` (need writer before full app)
- `create-viewmodels` depends on `create-avalonia-project`
- `create-main-view` depends on `create-avalonia-project`
- `wire-up-logic` depends on `create-viewmodels` + `create-main-view` + `parser-write-support`
- `build-and-verify` depends on all above
