﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ModernWpf.Controls.Primitives
{
    public static class WindowHelper
    {
        #region UseModernWindowStyle

        private const string DefaultWindowStyleKey = "DefaultWindowStyle";

        public static readonly DependencyProperty UseModernWindowStyleProperty =
            DependencyProperty.RegisterAttached(
                "UseModernWindowStyle",
                typeof(bool),
                typeof(WindowHelper),
                new PropertyMetadata(OnUseModernWindowStyleChanged));

        public static bool GetUseModernWindowStyle(Window window)
        {
            return (bool)window.GetValue(UseModernWindowStyleProperty);
        }

        public static void SetUseModernWindowStyle(Window window, bool value)
        {
            window.SetValue(UseModernWindowStyleProperty, value);
        }

        private static void OnUseModernWindowStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool newValue = (bool)e.NewValue;

            if (DesignerProperties.GetIsInDesignMode(d))
            {
                if (d is Control control)
                {
                    if (newValue)
                    {
                        if (control.TryFindResource(DefaultWindowStyleKey) is Style style)
                        {
                            var dStyle = new Style();

                            foreach (Setter setter in style.Setters)
                            {
                                if (setter.Property == Control.BackgroundProperty ||
                                    setter.Property == Control.ForegroundProperty)
                                {
                                    dStyle.Setters.Add(setter);
                                }
                            }

                            control.Style = dStyle;
                        }
                    }
                    else
                    {
                        control.ClearValue(FrameworkElement.StyleProperty);
                    }
                }
            }
            else
            {
                var window = (Window)d;
                if (newValue)
                {
                    window.SetResourceReference(FrameworkElement.StyleProperty, DefaultWindowStyleKey);
                }
                else
                {
                    window.ClearValue(FrameworkElement.StyleProperty);
                }
            }
        }

        #endregion

        #region IsAutoPaddingEnabled

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly DependencyProperty IsAutoPaddingEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsAutoPaddingEnabled",
                typeof(bool),
                typeof(WindowHelper),
                new PropertyMetadata(false, OnIsAutoPaddingEnabledChanged));

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool GetIsAutoPaddingEnabled(Window window)
        {
            return (bool)window.GetValue(IsAutoPaddingEnabledProperty);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetIsAutoPaddingEnabled(Window window, bool value)
        {
            window.SetValue(IsAutoPaddingEnabledProperty, value);
        }

        private static void OnIsAutoPaddingEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window)
            {
                if ((bool)e.NewValue)
                {
                    MaximizedWindowFixer.SetMaximizedWindowFixer(window, new MaximizedWindowFixer());
                }
                else
                {
                    window.ClearValue(MaximizedWindowFixer.MaximizedWindowFixerProperty);
                }
            }
        }

        #endregion
    }
}
