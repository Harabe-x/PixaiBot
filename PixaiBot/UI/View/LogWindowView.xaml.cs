using System.Windows;
using System.Windows.Controls;

namespace PixaiBot.UI.View;

public partial class LogWindowView : Window
{
    public LogWindowView()
    {
        InitializeComponent();
    }


    private void ScrollViewer_OnScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        ScrollViewer.ScrollToEnd();
    }
}