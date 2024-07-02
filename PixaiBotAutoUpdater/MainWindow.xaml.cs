using System.Windows;
using System.Windows.Input;
using PixaiBot.AutoUpdater;

namespace PixaiBotAutoUpdater;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ApplicationAutoUpdater _autoUpdater;

    public MainWindow()
    {
        InitializeComponent();

        _autoUpdater = new ApplicationAutoUpdater();
        CheckForUpdates();
    }


    private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left) DragMove();
    }


    private void CheckForUpdates()
    {
        Task.Run(async () =>
        {
            if (!_autoUpdater.IsUpdateAvailable() && _autoUpdater.DoesApplicationDirectoryExist())
            {
                Application.Current.Dispatcher.Invoke(() => { label.Text = "Application up to date"; });
                _autoUpdater.CallApplication();
                _autoUpdater.CloseApplication();
                return;
            }

            Application.Current.Dispatcher.Invoke(() => { label.Text = "Downloading update"; });
            var applicationUpdate = await _autoUpdater.DownloadUpdate();

            if (applicationUpdate == null) _autoUpdater.CallApplication();

            Application.Current.Dispatcher.Invoke(() => { label.Text = "Installing update"; });

            await _autoUpdater.InstallUpdate(applicationUpdate);

            _autoUpdater.CloseApplication();
        });
    }
}