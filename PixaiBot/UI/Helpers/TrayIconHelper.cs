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

public class TrayIconHelper
{



    public static bool GetCanShowWindow(DependencyObject obj)
    {
        return (bool)obj.GetValue(CanShowWindowProperty);
    }

    public static void SetCanShowWindow(DependencyObject obj, bool value)
    {
        obj.SetValue(CanShowWindowProperty, value);
    }

    // Using a DependencyProperty as the backing store for CanShowWindow.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CanShowWindowProperty =
        DependencyProperty.RegisterAttached("CanShowWindow", typeof(bool), typeof(TrayIconHelper), new PropertyMetadata(false,ShowWindow));

    private static void ShowWindow(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Window window)
        {
            window.Loaded += (s, e) =>
            {
                if (window.DataContext is ITrayIconHelper trayIconHelper)
                {
                    trayIconHelper.ShowWindow += () =>
                    {
                        if (trayIconHelper.CanHideToTray())
                        {
                            window.Show();
                        }
                    };
                }
            };
        }
    }


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
                        if (!trayIconHelper.CanHideToTray()) return;
                        
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
                    };
            };
    }
}