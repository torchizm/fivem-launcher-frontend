using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Launchwares.Resources.Design
{
    internal static class ThemeHelper
    {
        internal static Style SelectedPageStyle { get; set; }
        internal static Style MenuItemStyle { get; set; }
        internal static Style LanguageOptionStyle { get; set; }
        internal static Style LanguageOptionSelectedStyle { get; set; }

        internal static ResourceDictionary CurrentThemeDictionary { get; set; }
        internal static Theme CurrentTheme { get; set; }

        internal enum Theme
        {
            Vlast,
            Purple,
            Black,
            Blue,
            Red,
            Orange,
            DarkBlue,
            Green
        }

        internal static Dictionary<Theme, string> ThemeDict = new Dictionary<Theme, string>() {
            { Theme.Vlast, "Vlast" },
            { Theme.Purple, "Purple" },
            { Theme.Black, "Black" },
            { Theme.Blue, "Blue" },
            { Theme.Red, "Red" },
            { Theme.Orange, "Orange" },
            { Theme.DarkBlue, "DarkBlue" },
            { Theme.Green, "Green" }
        };

        internal static void SetTheme(Theme theme, bool fromUser = true)
        {
            if (fromUser == true) {
                Properties.Settings.Default.ThemeIndex = (int)theme;
                Properties.Settings.Default.UserThemeIndex = true;
                Properties.Settings.Default.Save();
            }

            var dict = new ResourceDictionary();
            var ThemePath = ThemeDict.Where(x => x.Key == theme).Select(x => x.Value).FirstOrDefault();

            if (ThemePath == "") {
                ThemePath = "Black";
                theme = Theme.Black;
            }

            dict.Source = new Uri($@"..\Resources\Design\XamlThemes\{ThemePath}.xaml", UriKind.Relative);
            CurrentTheme = theme;
            CurrentThemeDictionary = dict;
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }
}
