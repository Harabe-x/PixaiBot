using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PixaiBot.Data.Interfaces;

namespace PixaiBot.UI.Helpers;

internal class WindowHelper
{
    public static bool GetEnableClosing(DependencyObject obj)
    {
        return (bool)obj.GetValue(EnableClosingProperty);
    }

    public static void SetEnableClosing(DependencyObject obj, bool value)
    {
        obj.SetValue(EnableClosingProperty, value);
    }

    public static readonly DependencyProperty EnableClosingProperty =
        DependencyProperty.RegisterAttached("EnableClosing", typeof(bool), typeof(WindowHelper),
            new PropertyMetadata(false, OnEnableClosingChanged));

    private static void OnEnableClosingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Window window)
            window.Loaded += (s, e) =>
            {
                if (window.DataContext is IWindowHelper windowHelper)
                    windowHelper.Close += () => { window.Close(); };
            };
    }
}