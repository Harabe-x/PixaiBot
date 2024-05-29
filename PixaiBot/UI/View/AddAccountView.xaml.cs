using System.Windows;
using System.Windows.Input;

namespace PixaiBot.UI.View;

public partial class AddAccountView : Window
{
    public AddAccountView()
    {
        InitializeComponent();
    }

    private void Border_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left) DragMove();
    }
}