using CommunityToolkit.Mvvm.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace RC2K.Presentation.HstEraser.ViewModels;

public partial class ScoreEntryViewModel : ObservableObject
{
    [ObservableProperty] 
    private int _position;

    [ObservableProperty] 
    private string _nat = string.Empty;

    [ObservableProperty] 
    private string _driverName = string.Empty;

    [ObservableProperty]
    private string _car = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Time))]
    private int _centiseconds;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Time))]
    private bool _isDeleted;

    public string Time => RC2K.Utils.Utils.CentisecondsToTimeOnly(Centiseconds).ToString("m\\:ss\\.ff");

    public long ByteOffset { get; set; }
}
