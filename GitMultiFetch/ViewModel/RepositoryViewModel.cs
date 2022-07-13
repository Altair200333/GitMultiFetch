using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using GitMulltyFetch.Annotations;
using GitMulltyFetch.Model;
using GitMultiFetch;

namespace GitMulltyFetch
{
    public class RepositoryViewModel : Repository, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static Dictionary<RepoStatus, Brush> _statusToBrush = new Dictionary<RepoStatus, Brush>()
        {
            {RepoStatus.Unknown, new SolidColorBrush(Colors.Black)},
            {RepoStatus.Fetching, new SolidColorBrush(Colors.Orange)},
            {RepoStatus.NotAGitRepo, new SolidColorBrush(Colors.Red)},
            {RepoStatus.UpToDate, new SolidColorBrush(Colors.Green)},
            {RepoStatus.Behind, new SolidColorBrush(Colors.OrangeRed)},
            {RepoStatus.Ahead, new SolidColorBrush(Colors.GreenYellow)},
        };

        public Brush StatusColor => GetStatusColor();
        public string ChangesText => GetChangesText();
        public Brush ChangesColor => GetChangesColor();

        public int WarningChangesCount = 5;

        private Brush GetChangesColor()
        {
            if (Changes == 0)
                return new SolidColorBrush(Colors.Green);
            if (Changes < WarningChangesCount)
                return new SolidColorBrush(Colors.Orange);
            return new SolidColorBrush(Colors.Red);
        }

        private string GetChangesText()
        {
            return "Changes " + Changes;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override void SetStatus(RepoStatus status)
        {
            base.SetStatus(status);
            
            //Status
            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(StatusColor));
            OnPropertyChanged(nameof(StatusReport));

            //Changes
            OnPropertyChanged(nameof(Changes));
            OnPropertyChanged(nameof(ChangesText));
            OnPropertyChanged(nameof(ChangesReport));
            OnPropertyChanged(nameof(ChangesColor));
        }

        private Brush GetStatusColor()
        {
            return _statusToBrush[_status];
        }
    }
}
