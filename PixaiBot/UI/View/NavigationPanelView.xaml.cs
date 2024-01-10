using System.Windows.Input;
using System.Windows;

namespace PixaiBot.UI.View;


public partial class NavigationPanelView : Window
{
    public NavigationPanelView()
    {
        InitializeComponent();
    }
    private void Border_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left) DragMove();
        
    }
}