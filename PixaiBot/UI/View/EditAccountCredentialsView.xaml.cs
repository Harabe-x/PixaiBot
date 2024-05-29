using System.Windows;
using System.Windows.Input;

namespace PixaiBot.UI.View;

/// <summary>
///     Logika interakcji dla klasy EditAccountCredentialsView.xaml
/// </summary>
public partial class EditAccountCredentialsView : Window
{
    public EditAccountCredentialsView()
    {
        InitializeComponent();
    }

    private void Border_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed) DragMove();
    }
}