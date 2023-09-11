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
using PixaiBot.UI.ViewModel;

namespace PixaiBot.UI.View
{
    /// <summary>
    /// Logika interakcji dla klasy AddAccountWindowView.xaml
    /// </summary>
    public partial class AddAccountWindowView : Window
    {
        public AddAccountWindowView()
        {
            InitializeComponent();
            this.DataContext = new AddAccountWindowViewModel();
        }

        private void Border_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

    }
}
