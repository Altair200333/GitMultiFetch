using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using GitMulltyFetch;
using GitMulltyFetch.Model;
using Button = System.Windows.Controls.Button;
using DataFormats = System.Windows.DataFormats;
using DragEventArgs = System.Windows.DragEventArgs;

namespace GitMultiFetch
{
    
    public partial class MainWindow : Window
    {
        public GitOwerwatch GitWatch { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            GitWatch = new GitOverwatchViewModel(new WatchTxtSerializer("folders.txt"));
            GitWatch.Load();
            GitWatch.RefreshStatus();

            RepoList.ItemsSource = GitWatch.Repositories;
        }

        private void AddRepoButtonClick(object sender, RoutedEventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                var repo = GitWatch.TryAddRepository(fbd.SelectedPath);

                GitWatch.RefreshStatus(repo);
            }

        }

        private void RefreshButtonClick(object sender, RoutedEventArgs e)
        {
            GitWatch.RefreshStatus();
        }

        private void ImagePanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files == null)
                {
                    return;
                }

                List<Repository> repositories = new List<Repository>();
                foreach (var path in files)
                {
                    var repo = GitWatch.TryAddRepository(path);
                    if (repo != null)
                    {
                        repositories.Add(repo);
                    }
                }

                GitWatch.Save();

                GitWatch.RefreshStatus(repositories);

            }

        }

        private void RemoveItemButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.DataContext is Repository repository)
                {
                    GitWatch.RemoveRepository(repository);
                }
            }

            GitWatch.Save();
        }
    }
}
