using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    

    class GitOverwatchViewModel : GitOwerwatch
    {
        public GitOverwatchViewModel(IWatcherSerializer serializer) : base(serializer)
        {
            Repositories = new ObservableCollection<Repository>();
        }

        protected override Repository createRepository()
        {
            return new RepositoryViewModel();
        }
    }
}
