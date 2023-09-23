using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using PixaiBot.Data.Interfaces;

namespace PixaiBot.UI.Helpers;

internal class TrayIconHelper
{
    public static bool GetCanHideToTray(DependencyObject obj)
    {
        return (bool)obj.GetValue(CanHideToTrayProperty);
    }

    public static void SetCanHideToTray(DependencyObject obj, bool value)
    {
        obj.SetValue(CanHideToTrayProperty, value);
    }

    public static readonly DependencyProperty CanHideToTrayProperty =
        DependencyProperty.RegisterAttached("CanHideToTray", typeof(bool), typeof(TrayIconHelper),
            new PropertyMetadata(false, HideToTray));

    private static void HideToTray(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Window window)
            window.Loaded += (s, e) =>
            {
                if (window.DataContext is ITrayIconHelper trayIconHelper)
                    trayIconHelper.HideToTray += () =>
                    {
                        if (trayIconHelper.CanHideToTray())
                        {
                            window.Hide();
                            var notifyIcon = new NotifyIcon()
                            {
                                Icon = new Icon("Resources/images/PixaiAutoClaimerIcon.ico"),
                                Visible = true,
                                Text = "Pixai Auto Claimer"
                            };
                            notifyIcon.Click += (s, e) =>
                            {
                                window.Show();
                                notifyIcon.Visible = false;
                            };
                        }
                    };
            };
    }
}