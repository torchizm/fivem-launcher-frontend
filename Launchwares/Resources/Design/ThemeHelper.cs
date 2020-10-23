using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Purple,
            Black,
            Blue,
            Red,
            Orange,
            DarkBlue
        }

        internal static Dictionary<Theme, string> ThemeDict = new Dictionary<Theme, string>() {
            { Theme.Purple, "Purple" },
            { Theme.Black, "Black" },
            { Theme.Blue, "Blue" },
            { Theme.Red, "Red" },
            { Theme.Orange, "Orange" },
            { Theme.DarkBlue, "DarkBlue" }
        };

        internal static void SetTheme(Theme theme)
        {
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
