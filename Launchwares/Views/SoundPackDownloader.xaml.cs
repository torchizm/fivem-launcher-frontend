using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Application = System.Windows.Forms.Application;
using Path = System.IO.Path;

namespace Launchwares.Views
{
    /// <summary>
    /// SouncPackDownloader.xaml etkileşim mantığı
    /// </summary>
    public partial class SoundPackDownloader : Window
    {
        public SoundPackDownloader()
        => InitializeComponent();

        public static List<string> stringList;
        public static int downloadedFile = 0;

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateProgress.Content = $"{System.Windows.Application.Current.Resources["soundpackdownloader.preparingfordownload"]}";
            if (!Directory.Exists($@"{Path.GetDirectoryName(Application.ExecutablePath)}\Backups"))
                Directory.CreateDirectory($@"{Path.GetDirectoryName(Application.ExecutablePath)}\Backups");

            GetDownloadString();
        }

        public void Download()
        {
            stringList.RemoveAt(stringList.Count - 1);
            DownloadAsync(stringList[0]);
        }

        public void DownloadAsync(string uri)
        {
            if (uri != null && uri != "") {
                string[] uriSplit = uri.Split('/');
                Backup($@"{Properties.Settings.Default.Location_GtaV}\x64\audio\sfx\{uriSplit[uriSplit.Length - 1]}");
                using (WebClient wc = new WebClient()) {
                    wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                    wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
                    wc.DownloadFileAsync(
                        new Uri(uri),
                        $@"{Properties.Settings.Default.Location_GtaV}\x64\audio\sfx\{uriSplit[uriSplit.Length - 1]}"
                    );
                }
            }
        }

        public void Backup(string path)
        {
            string[] pathSplit = path.Split('\\');

            if (File.Exists($@"{path}")
                && !File.Exists($@"{Path.GetDirectoryName(Application.ExecutablePath)}\Backups\{pathSplit[pathSplit.Length - 1]}"))
                File.Move($@"{Properties.Settings.Default.Location_GtaV}\x64\audio\sfx\{pathSplit[pathSplit.Length - 1]}",
                $@"{Path.GetDirectoryName(Application.ExecutablePath)}\Backups\{pathSplit[pathSplit.Length - 1]}");
        }

        private void Wc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            downloadedFile++;
            //UpdateDetails.Content = $"(İndirilen {downloadedFile}/{stringList.Count})";
            UpdateDetails.Content = $"{System.Windows.Application.Current.Resources["soundpackdownloader.downloadingprogress"].ToString().Replace("$downloaded", $"{downloadedFile}").Replace("$total", $"{stringList.Count}")}";
            if (downloadedFile == stringList.Count) {
                string[] uriSplit = stringList[0].Split('/');
                UpdateProgress.Content = $"{System.Windows.Application.Current.Resources["soundpackdownloader.preparing"]}";
                MessageBox.Show($"{System.Windows.Application.Current.Resources["soundpackdownloader.ready"]}", $"{System.Windows.Application.Current.Resources["application.title"]}", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else {
                DownloadAsync(stringList[downloadedFile]);
            }
        }

        public void GetDownloadString()
        {
            using (WebClient web = new WebClient())
                stringList = web.DownloadString(new Uri("https://raw.githubusercontent.com/TORCHIZM/baphometupdater/master/soundstring")).Split('\n').ToList();
            //UpdateDetails.Content = $"(İndirilen {downloadedFile}/{stringList.Count - 1})";
            UpdateDetails.Content = $"{System.Windows.Application.Current.Resources["soundpackdownloader.downloadingprogress"].ToString().Replace("$downloaded", $"{downloadedFile}").Replace("$total", $"{stringList.Count - 1}")}";
            Download();
        }

        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        => UpdateProgress.Content = $"{System.Windows.Application.Current.Resources["soundpackdownloader.progress"].ToString().Replace("$progress", $"{e.ProgressPercentage}")}";
        //UpdateProgress.Content = $"%{e.ProgressPercentage} Güncelleniyor...";

        private void MainContainer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
        }
    }
}