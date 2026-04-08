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
    private class LoadedData
    {
        public string FilePath { get; }
        public HighScores HighScores { get; }
        public Dictionary<long, TimeEntry> Offset2te { get; }

        public LoadedData(string filePath)
        {
            FilePath = filePath;
            
            var bytes = File.ReadAllBytes(FilePath);
            using var ms = new MemoryStream(bytes);
            using var reader = new BinaryReader(ms);
            HighScores = new HighScores(reader);

            Offset2te = HighScores.GetAll()
                .ToDictionary(x => x.ByteOffset, x => x);
        }
    }

    private static readonly int[] RallyBaseCodes = [41, 61, 31, 71, 51, 21];

    private readonly HashSet<long> _pendingDeletions = [];

    private LoadedData? _loadedData;

    private HstWriter _hstWriter;

    public ObservableCollection<string> Rallies { get; } = [];
    public ObservableCollection<ScoreEntryViewModel> Entries { get; } = [];

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

    [ObservableProperty]
    private string _selectionText = string.Empty;

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

        DataGenerator gen = new();
        _hstWriter = new HstWriter(
            zeroCar: gen.GenerateCar,
            zeroNat: gen.GenerateNationality,
            zeroName: gen.GenerateDriverName);

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
        if (entry is null)
        {
            return;
        }

        entry.IsDeleted = true;
        _pendingDeletions.Add(entry.ByteOffset);
        UpdateSelectionText();
    }

    [RelayCommand]
    private void RestoreEntry(ScoreEntryViewModel? entry)
    {
        if (entry is null)
        {
            return;
        }

        entry.IsDeleted = false;
        _pendingDeletions.Remove(entry.ByteOffset);
        UpdateSelectionText();
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
            UpdateSelectionText();
            _loadedData = new LoadedData(HstFilePath);
            StatusText = "File loaded successfully.";
            RefreshEntries();
        }
        catch (Exception ex)
        {
            StatusText = $"Error: {ex.Message}";
            _loadedData = null;

        }
    }

    private void UpdateSelectionText()
    {
        SelectionText = $"Selected time entries to delete: {_pendingDeletions.Count}";
    }

    [RelayCommand]
    private void Update()
    {
        if (_loadedData is null)
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
            CreateCurrentHstBackup();

            var timeEntriesToDelete = _pendingDeletions.Select(x => _loadedData.Offset2te[x]);
            using var file = File.OpenWrite(_loadedData.FilePath);
            _hstWriter.ShredTimeEntries(file, timeEntriesToDelete);

            int count = _pendingDeletions.Count;
            _pendingDeletions.Clear();
            UpdateSelectionText();

            _loadedData = new LoadedData(_loadedData.FilePath); // reload
            StatusText = $"Updated {count} entry/entries. File saved.";
            RefreshEntries();
        }
        catch (Exception ex)
        {
            StatusText = $"Save error: {ex.Message}";
        }
    }

    private void CreateCurrentHstBackup()
    {
        if (_loadedData is null)
        {
            return;
        }

        string directory = Path.GetDirectoryName(_loadedData.FilePath) ?? string.Empty;
        string backupFileName = $"hst_{DateTime.Now:yyyyMMdd_HHmmss}.dat.backup";
        string backupPath = Path.Combine(directory, backupFileName);
        File.Copy(_loadedData.FilePath, backupPath);
    }

    private void RefreshEntries()
    {
        Entries.Clear();

        if (_loadedData is null ||
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
            bool pendingDelete = _pendingDeletions.Contains(te.ByteOffset);
            Entries.Add(new ScoreEntryViewModel
            {
                Position = i + 1,
                Nat = RC2K.Parser.Utils.GetNat(te.Nat),
                DriverName = te.Name,
                Car = RC2K.Parser.Utils.GetCar(te.Car),
                Centiseconds = te.Centiseconds,
                IsDeleted = pendingDelete,
                ByteOffset = te.ByteOffset
            });
        }
    }

    private TimeEntriesCollection? GetCollection()
    {
        if (_loadedData is null)
        {
            return null;
        }

        return (IsArcade, IsTimeTrial) switch
        {
            (false, false) => _loadedData.HighScores.TimesSimulationNormal,
            (false, true) => _loadedData.HighScores.TimesSimulationTimeTrial,
            (true, false) => _loadedData.HighScores.TimesArcadeNormal,
            (true, true) => _loadedData.HighScores.TimesArcadeTimeTrial,
        };
    }
}
