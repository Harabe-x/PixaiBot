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
using PixaiBot.Bussines_Logic;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.ViewModel;

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