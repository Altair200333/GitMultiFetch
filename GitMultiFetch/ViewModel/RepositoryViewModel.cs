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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override void SetStatus(RepoStatus status)
        {
            base.SetStatus(status);

            OnPropertyChanged("Status");
            OnPropertyChanged("StatusColor");
            OnPropertyChanged("StatusReport");
        }

        private Brush GetStatusColor()
        {
            return _statusToBrush[_status];
        }
    }
}
