using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using RC2K.Presentation.HstEraser.ViewModels;

namespace RC2K.Presentation.HstEraser.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void BrowseButton_Click(object? sender, RoutedEventArgs e)
    {
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select HST file",
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType("DAT files") { Patterns = ["*.dat"] }
            ]
        });

        if (files.Count > 0 && DataContext is MainViewModel vm)
        {
            vm.HstFilePath = files[0].Path.LocalPath;
        }
    }
}
