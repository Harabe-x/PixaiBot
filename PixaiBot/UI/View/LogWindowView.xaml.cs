using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PixaiBot.UI.ViewModel;

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