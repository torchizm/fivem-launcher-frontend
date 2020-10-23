using Launchwares.Core;
using Launchwares.CustomElements;
using Launchwares.Resources.Design;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Launchwares.Views
{
    /// <summary>
    /// Options.xaml etkileşim mantığı
    /// </summary>
    public partial class Options : Page
    {
        public Options()
        {
            InitializeComponent();
            LanguageContainer.Children.OfType<LanguageOption>().Where(x => x.LanguageKey == Properties.Settings.Default.Language).FirstOrDefault().IsChecked = true;
            CreateShortcutViewer.IsChecked = Properties.Settings.Default.CreateShortcut;
        }

        internal void SetLanguage(string LanguageKey)
        {
            var SelectedLanguage = LanguageContainer.Children.OfType<LanguageOption>().Where(x => x.LanguageKey == LanguageKey).FirstOrDefault();
            SelectedLanguage.IsChecked = true;

            var dict = new ResourceDictionary();

            if (LanguageKey == null || LanguageKey == "") LanguageKey = "en-US";
            dict.Source = new Uri($@"..\Resources\Locales\Locale.{LanguageKey}.xaml", UriKind.Relative);

            Application.Current.Resources.MergedDictionaries.Add(dict);
            Utils.Language = dict;

            Properties.Settings.Default.Language = LanguageKey;
            Properties.Settings.Default.Save();
        }

        private void LanguageOption_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            var ClickedOption = e.Source as LanguageOption;
            if (ClickedOption.IsChecked == true) return;
            LanguageContainer.Children.OfType<LanguageOption>().ToList().ForEach(x => x.IsChecked = false);
            SetLanguage(ClickedOption.LanguageKey);
        }

        private void Purple_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ThemeHelper.SetTheme(ThemeHelper.Theme.Purple);
        }

        private void Black_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ThemeHelper.SetTheme(ThemeHelper.Theme.Black);
        }

        private void Blue_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ThemeHelper.SetTheme(ThemeHelper.Theme.Blue);
        }

        private void Red_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ThemeHelper.SetTheme(ThemeHelper.Theme.Red);
        }

        private void Orange_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ThemeHelper.SetTheme(ThemeHelper.Theme.Orange);
        }

        private void DarkBlue_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ThemeHelper.SetTheme(ThemeHelper.Theme.DarkBlue);
        }

        private void CreateShortcut_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Properties.Settings.Default.CreateShortcut = !Properties.Settings.Default.CreateShortcut;
            Properties.Settings.Default.Save();
        }
    }
}
