using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RC2K.Parser;
using RC2K.Parser.Models.Hst;
using RC2K.Resources;
using RC2K.Resources.DAOs;

namespace RC2K.Presentation.HstEraser.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private static readonly int[] RallyBaseCodes = [41, 61, 31, 71, 51, 21];

    private byte[]? _fileBytes; // loaded hst file in memory
    private HighScores? _highScores; // readonly parsed hst file
    private readonly HashSet<long> _pendingDeletions = [];

    public ObservableCollection<string> Rallies { get; } = [];
    public ObservableCollection<ScoreEntryViewModel> Entries { get; } = [
        new ScoreEntryViewModel() {
            Car = "C",
            Centiseconds = 8433,
            DriverName = "ketrab",
            Nat = "pl",
            Position = 1,
        }
        ];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StageName))]
    private int _selectedRallyIndex;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StageName))]
    private bool _isArcade;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StageName))]
    private bool _isTimeTrial;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StageName))]
    private int _stageIndex;

    [ObservableProperty]
    private string _hstFilePath = string.Empty;

    [ObservableProperty]
    private string _statusText = string.Empty;

    private IReadOnlyList<RallyInfoDao> _rallyInfos = [];

    public string StageName
    {
        get
        {
            if (_rallyInfos.Count == 0 ||
                SelectedRallyIndex < 0 ||
                SelectedRallyIndex >= _rallyInfos.Count)
            {
                return string.Empty;
            }

            var rally = _rallyInfos[SelectedRallyIndex];
            if (StageIndex < 0 || StageIndex >= rally.Stages.Count)
            {
                return string.Empty;
            }

            string stageName = rally.Stages[StageIndex].Name;
            string mode = IsArcade ? "Arcade" : "Simulation";
            string gameType = IsTimeTrial ? "Time Trial" : "Normal";
            return $"{stageName} - {mode} ({gameType})";
        }
    }

    public MainViewModel()
    {
        _rallyInfos = Stages.GetRallyInfos();
        foreach (var rally in _rallyInfos)
        {
            Rallies.Add(rally.Name);
        }

        if (Rallies.Count > 0)
        {
            SelectedRallyIndex = 0;
        }
    }

    partial void OnSelectedRallyIndexChanged(int value) => RefreshEntries();
    partial void OnIsArcadeChanged(bool value) => RefreshEntries();
    partial void OnIsTimeTrialChanged(bool value) => RefreshEntries();
    partial void OnStageIndexChanged(int value) => RefreshEntries();

    [RelayCommand]
    private void NextStage() => StageIndex = (StageIndex + 1) % 6;

    [RelayCommand]
    private void PrevStage() => StageIndex = (StageIndex + 5) % 6;

    [RelayCommand]
    private void SetSimulation() => IsArcade = false;

    [RelayCommand]
    private void SetArcade() => IsArcade = true;

    [RelayCommand]
    private void SetTimeTrial() => IsTimeTrial = true;

    [RelayCommand]
    private void SetNormal() => IsTimeTrial = false;

    [RelayCommand]
    private void DeleteEntry(ScoreEntryViewModel? entry)
    {
        if (entry is null || entry.Centiseconds == 0)
        {
            return;
        }

        entry.Centiseconds = 0;
        entry.IsDeleted = true;
        _pendingDeletions.Add(entry.CentisecondsOffset);
    }

    [RelayCommand]
    private void Load()
    {
        if (string.IsNullOrWhiteSpace(HstFilePath))
        {
            StatusText = "Please specify an HST file path.";
            return;
        }

        if (!File.Exists(HstFilePath))
        {
            StatusText = "File not found.";
            return;
        }

        try
        {
            _pendingDeletions.Clear();
            _fileBytes = File.ReadAllBytes(HstFilePath);
            using var ms = new MemoryStream(_fileBytes);
            using var reader = new BinaryReader(ms);
            _highScores = new HighScores(reader);
            StatusText = "File loaded successfully.";
            RefreshEntries();
        }
        catch (Exception ex)
        {
            StatusText = $"Error: {ex.Message}";
            _highScores = null;
            _fileBytes = null;
        }
    }

    [RelayCommand]
    private void Update()
    {
        if (_fileBytes is null || _highScores is null)
        {
            StatusText = "No file loaded.";
            return;
        }

        if (_pendingDeletions.Count == 0)
        {
            StatusText = "No entries to update.";
            return;
        }

        try
        {
            foreach (long offset in _pendingDeletions)
            {
                HstWriter.ZeroEntry(_fileBytes, offset);
            }

            // Create a backup of the original file before overwriting
            string directory = Path.GetDirectoryName(HstFilePath) ?? string.Empty;
            string backupFileName = $"hst.{DateTime.Now:yyyyMMdd_HHmmss}.dat.backup";
            string backupPath = Path.Combine(directory, backupFileName);
            File.Copy(HstFilePath, backupPath);

            HstWriter.Save(HstFilePath, _fileBytes);

            int count = _pendingDeletions.Count;
            _pendingDeletions.Clear();

            // Reload to reflect changes
            using var ms = new MemoryStream(_fileBytes);
            using var reader = new BinaryReader(ms);
            _highScores = new HighScores(reader);

            StatusText = $"Updated {count} entry/entries. File saved.";
            RefreshEntries();
        }
        catch (Exception ex)
        {
            StatusText = $"Save error: {ex.Message}";
        }
    }

    private void RefreshEntries()
    {
        Entries.Clear();

        if (_highScores is null ||
            SelectedRallyIndex < 0 ||
            SelectedRallyIndex >= RallyBaseCodes.Length)
        {
            return;
        }

        int baseCode = RallyBaseCodes[SelectedRallyIndex];
        int targetCode = baseCode + StageIndex;

        var collection = GetCollection();
        if (collection is null)
        {
            return;
        }

        var stageEntry = collection.StageEntries
            .FirstOrDefault(se => se.StageCode == targetCode);

        if (stageEntry is null)
        {
            return;
        }

        for (int i = 0; i < stageEntry.TimeEntries.Length; i++)
        {
            var te = stageEntry.TimeEntries[i];
            bool pendingDelete = _pendingDeletions.Contains(te.CentisecondsOffset);
            Entries.Add(new ScoreEntryViewModel
            {
                Position = i + 1,
                Nat = RC2K.Parser.Utils.GetNat(te.Nat),
                DriverName = te.Name,
                Car = RC2K.Parser.Utils.GetCar(te.Car),
                Centiseconds = pendingDelete ? 0 : te.Centiseconds,
                CentisecondsOffset = te.CentisecondsOffset,
                IsDeleted = pendingDelete
            });
        }
    }

    private TimeEntriesCollection? GetCollection()
    {
        if (_highScores is null)
        {
            return null;
        }

        return (IsArcade, IsTimeTrial) switch
        {
            (false, false) => _highScores.TimesSimulationNormal,
            (false, true) => _highScores.TimesSimulationTimeTrial,
            (true, false) => _highScores.TimesArcadeNormal,
            (true, true) => _highScores.TimesArcadeTimeTrial,
        };
    }
}
