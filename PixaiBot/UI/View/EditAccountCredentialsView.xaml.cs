﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PixaiBot.UI.View;

/// <summary>
/// Logika interakcji dla klasy EditAccountCredentialsView.xaml
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