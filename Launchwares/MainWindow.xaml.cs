using Launchwares.AnimationHelpers;
using Launchwares.Core;
using Launchwares.Helpers;
using Launchwares.Resources.Design;
using Launchwares.Views;
using Launchwares.LanguageHelper;
using LaunchwaresCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Application = System.Windows.Application;

namespace Launchwares
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static bool isLoaded = false;

        internal readonly NotifyIcon SmallIcon = null;
        internal static MainWindow main { get; set; }
        internal static List<Window> ActiveProgress { get; set; }
        internal static Window Main { get; private set; }
        internal static Page SelectedPage { get; set; }
        internal static CustomElements.MenuItem SelectedStackpanel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            main = this;

            // Create URI Scheme
            //RegistryKey scheme = Registry.ClassesRoot.CreateSubKey("vlastdev");
            //scheme.SetValue("URL Protocol", "");

            // Create URI Scheme's action
            //var command = scheme.CreateSubKey("shell").CreateSubKey("open").CreateSubKey("command");

            //command.SetValue("", $"\"{Assembly.GetExecutingAssembly().Location}\" \"%1\"");

            //if (WindowsIdentity.GetCurrent().Owner.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid)) {
            //    System.Windows.MessageBox.Show("Program yönetici olarak çalıştırılamaz.");
            //    Application.Current.Shutdown(0);
            //}

            if (Properties.Settings.Default.IsFirst) {
                Properties.Settings.Default.Location_FiveM = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\FiveM";
                Properties.Settings.Default.IsFirst = false;
                Properties.Settings.Default.Save();
            }

            var dict = new ResourceDictionary();
            var data = Properties.Settings.Default.Language;

            try {
                if (data == null && data == "") {
                    var Language = Thread.CurrentThread.CurrentCulture.ToString();

                    if (!Languages.LanguageList.Contains(Language))
                        Language = "tr-TR";

                    dict.Source = new Uri($@"..\Resources\Locales\Locale.{Language}.xaml", UriKind.Relative);
                    Properties.Settings.Default.Language = Language;
                    Properties.Settings.Default.Save();
                } else {
                    if (String.IsNullOrWhiteSpace(data)) {
                        data = "tr-TR";
                        Properties.Settings.Default.Language = data;
                        Properties.Settings.Default.Save();
                    }

                    dict.Source = new Uri($@"..\Resources\Locales\Locale.{data}.xaml", UriKind.Relative);
                }
            }
            catch (Exception) {
                dict.Source = new Uri($@"..\Resources\Locales\Locale.en-EN.xaml", UriKind.Relative);
                data = dict.Source.ToString().Split('.')[3];
                Properties.Settings.Default.Language = data;
                Properties.Settings.Default.Save();
            }

            Utils.Language = dict;
            Application.Current.Resources.MergedDictionaries.Add(Utils.Language);

            if (Properties.Settings.Default.LogoPath != "") LoadingImage.ImageSource = ImageHelper.ConvertPhoto(Properties.Settings.Default.LogoPath);

            ThemeHelper.SetTheme((ThemeHelper.Theme)Properties.Settings.Default.ThemeIndex, false);

            SmallIcon = new NotifyIcon();
            SmallIcon.DoubleClick += new EventHandler(SmallIcon_DoubleClick);
            SmallIcon.Text = $"{Application.Current.Resources["application.title"]}";
            SmallIcon.Icon = Properties.Resources.Logo;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ThemeHelper.SelectedPageStyle = Application.Current.FindResource("SelectedPage") as Style;
            ThemeHelper.MenuItemStyle = Application.Current.FindResource("MenuItem") as Style;
            ThemeHelper.LanguageOptionStyle = Application.Current.FindResource("LanguageOption") as Style;
            ThemeHelper.LanguageOptionSelectedStyle = Application.Current.FindResource("LanguageOptionSelected") as Style;
            BlurHelper.MakeBlur(this);
            SmallIcon.Visible = true;
            Loading.CheckVersion();
        }

        internal void CreateView(Page Content, CustomElements.MenuItem ClickedItem, bool Check = true)
        {
            if ((SelectedPage != null && SelectedPage.Title == Content.Title) && Check) return;
            SelectedStackpanel.Style = ThemeHelper.MenuItemStyle;
            SelectedStackpanel = ClickedItem;
            SelectedStackpanel.Style = ThemeHelper.SelectedPageStyle;
            SelectedPage = Content;
            MainFrame.Navigate(Content);
        }

        internal void CreateViewWithoutAnimation(Page Content)
        {
            if (SelectedPage != null && SelectedPage.Title == Content.Title) return;
            SelectedStackpanel.Style = ThemeHelper.MenuItemStyle;
            SelectedPage = Content;
            MainFrame.Navigate(Content);
        }

        internal async void LauncherLoaded()
        {
            if (Properties.Settings.Default.CreateShortcut) {
                try {
                    string ProgramDirector = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    string ShortcutterPath = $@"{ProgramDirector}\LaunchwaresShortcutter.exe";
                    ProcessStartInfo ShortCutter = new ProcessStartInfo(ShortcutterPath) {
                        Arguments = $"{Application.Current.Resources["application.title"]},{Properties.Settings.Default.LogoPath},{Properties.Settings.Default.OldName}",
                        Verb = "runas"
                    };
                    Process.Start(ShortCutter);
                    Properties.Settings.Default.OldName = Application.Current.Resources["application.title"].ToString();
                    Properties.Settings.Default.Save();
                    SmallIcon.Text = $"{Application.Current.Resources["application.title"]}";
                    string OutputPath = $@"{ProgramDirector}\Resources\{Application.Current.Resources["application.title"]}.ico";
                    SmallIcon.Icon = new Icon(OutputPath);
                }
                catch (Exception) { }
            }

            isLoaded = true;

            if (Utils.UserType == Models.UserType.None)
                Badge.Visibility = Visibility.Collapsed;
            else
                Badge.Kind = Utils.GetBadge(Utils.UserType);

            //System.Windows.Forms.MessageBox.Show(((int)Utils.UserType).ToString());
            AdministrationArea.Visibility = (Utils.UserType >= Models.UserType.Guider) ? Visibility.Visible : Visibility.Hidden;
            Utils.Server = await API.client.Get<Models.Server>($"server/{API.client.Token.slug}");
            ThemeHelper.SetTheme((ThemeHelper.Theme)Utils.Server.ThemeIndex, false);

            SelectedStackpanel = HomepageButton;
            this.Dispatcher.Invoke(delegate
            {
                LoadingContainer.Visibility = Visibility.Hidden;
                ProgramContainer.Visibility = Visibility.Visible;
                CreateView(new Views.MainMenu(), HomepageButton);
            });

            Core.Anticheat.Library.StartHackTimer();
            Core.Anticheat.Library.CheckGameFiles();
        }

        void SmallIcon_DoubleClick(object sender, EventArgs e)
        {
            Visibility = Visibility.Visible;
            WindowState = WindowState.Normal;
            ShowInTaskbar = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => Utils.ClosePrograms();

        private void Drag_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void ExitProgram_MouseDown(object sender, MouseButtonEventArgs e) => Close();

        private void UpdatesButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            CreateView(new Views.Updates(), UpdatesButton);
        }

        private void HomepageButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            CreateView(new Views.MainMenu(), HomepageButton);
        }

        private void PostsButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            CreateView(new Views.Posts(), PostsButton);
        }

        private void ServerStatsButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            CreateView(new Views.Moderation.Statistics(Views.Moderation.TimeScale.Daily), ServerStatsButton);
        }

        private void PlayersButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            CreateView(new Views.Moderation.Players(), PlayersButton);
        }

        private void AnticheatButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            CreateView(new Views.Moderation.Anticheat(), AnticheatButton);
        }

        private void OptionsButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            CreateViewWithoutAnimation(new Views.Options());
        }

        private void Minimize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            this.WindowState = WindowState.Minimized;
        }

        private void MyProfileButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            CreateViewWithoutAnimation(new Profile(Utils.Uid));
        }

        private void PackIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            Process.Start("https://api.vlastcommunity.net/where-can-i-find-my-discord-uid");
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (UidTextbox.Text.Trim() != "" &&
                NameTextbox.Text.Trim() != "" &&
                NameTextbox.Text.Trim().Length <= 40 &&
                UidTextbox.Text.Trim().Length == 18 &&
                IsNumber(UidTextbox.Text)) {
                var User = await API.client.Get<Models.ErrorMessage>($"player/launcher/check?uid={UidTextbox.Text}", true, false);

                if (User.Message != "Ok") {
                    if (ErrorGrid.Visibility == Visibility.Hidden)
                        ErrorGrid.Visibility = Visibility.Visible;

                    ErrorText.Text = $"{Application.Current.Resources["register.uidistaken"]}";
                    return;
                }
                else if (User.Message == "Ok") {
                    Properties.Settings.Default.PlayerUID = ulong.Parse(UidTextbox.Text);
                    Properties.Settings.Default.PlayerUsername = NameTextbox.Text;
                    Properties.Settings.Default.PlayerProfilephotoPath = "http://api.vlastcommunity.net/img/vlast.png";
                    Properties.Settings.Default.ManuallyCreated = true;
                    Properties.Settings.Default.Save();
                    Utils.Uid = Properties.Settings.Default.PlayerUID;
                    Utils.Username = Properties.Settings.Default.PlayerUsername;
                    Utils.ProfilePhoto = Properties.Settings.Default.PlayerProfilephotoPath;

                    RegisterContainer.Visibility = Visibility.Hidden;
                    LoadingContainer.Visibility = Visibility.Visible;
                    main.SmallIcon.Visible = true;
                    API.GetToken();
                }

                return;
            }
            else {
                if (ErrorGrid.Visibility == Visibility.Hidden) ErrorGrid.Visibility = Visibility.Visible;

                ErrorText.Text = $"{Application.Current.Resources["register.pleasefilleverything"]}";
                return;
            }
        }

        private void UidTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (UidTextbox.Text.Length != 18) {
                if (ErrorGrid.Visibility == Visibility.Hidden) ErrorGrid.Visibility = Visibility.Visible;

                ErrorText.Text = $"{Application.Current.Resources["register.uidmustbe18"]}";
            }
            else if (UidTextbox.Text.Length == 18) {
                if (ErrorText.Text == $"{Application.Current.Resources["register.uidmustbe18"]}") {
                    ErrorGrid.Visibility = Visibility.Hidden;
                }
            }

            if (!IsNumber(UidTextbox.Text)) {
                if (ErrorGrid.Visibility == Visibility.Hidden)
                    ErrorGrid.Visibility = Visibility.Visible;

                ErrorText.Text = $"{Application.Current.Resources["register.uidmustbenumber"]}";
            }
            else if (IsNumber(UidTextbox.Text)) {
                if (ErrorText.Text == $"{Application.Current.Resources["register.uidmustbenumber"]}") {
                    ErrorGrid.Visibility = Visibility.Hidden;
                }
            }
        }

        private void NameTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NameTextbox.Text.Length > 40) {
                if (ErrorGrid.Visibility == Visibility.Hidden)
                    ErrorGrid.Visibility = Visibility.Visible;

                ErrorText.Text = $"{Application.Current.Resources["register.namelessthan40"]}";
            }
            else if (NameTextbox.Text.Length <= 40) {
                if (ErrorText.Text == $"{Application.Current.Resources["register.namelessthan40"]}") {
                    ErrorGrid.Visibility = Visibility.Hidden;
                }
            }
        }

        internal static bool IsNumber(string s) => s.All(char.IsDigit);

        //private void LiveServerInfoButton_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    CreateView(new Views.Moderation.ServerInformation(), LiveServerInfoButton);
        //}
    }
}
