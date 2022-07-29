using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using GitMultiFetch;

namespace GitMulltyFetch.Model
{
    interface IGitWatch
    {
        Collection<Repository> Repositories { get; set; }
        IWatcherSerializer serializer { get; set; }
    }


    public abstract class GitOwerwatch : IGitWatch
    {
        private string fetchCommand = "git remote update && git status -uno";
        private string isGitDirectory = "git rev-parse --is-inside-work-tree";

        readonly Regex _behindRegex = new Regex(@"Your branch is behind (.*) by ([\d]*)");
        readonly Regex _upToDateRegex = new Regex(@"Your branch is up to date with (.*)");
        readonly Regex _aheadRegex = new Regex(@"Your branch is ahead of");
        readonly Regex _notAGitRepoRegex = new Regex(@"not a git repository");
        readonly Regex _changesRegex = new Regex(@"modified:(.*)");

        private readonly Dictionary<Regex, RepoStatus> _messageToStatus;
        public Collection<Repository> Repositories { get; set; }
        public IWatcherSerializer serializer { get; set; }

        public GitOwerwatch(IWatcherSerializer serializer)
        {
            this.serializer = serializer;

            _messageToStatus = new Dictionary<Regex, RepoStatus>()
            {
                {_notAGitRepoRegex, RepoStatus.NotAGitRepo},
                {_upToDateRegex, RepoStatus.UpToDate},
                {_behindRegex, RepoStatus.Behind},
                {_aheadRegex, RepoStatus.Ahead},
            };
        }

        public void Save()
        {
            serializer.Serialize(this);
        }

        public void Load()
        {
            serializer.Deserialize(this);
        }

        public void Clear()
        {
            Repositories.Clear();
        }

        public void RemoveRepository(Repository repository)
        {
            Repositories.Remove(repository);
        }

        protected abstract Repository createRepository();

        public Repository TryAddRepository(string path)
        {
            if (Directory.Exists(path))
            {
                var directoryName = Path.GetFileName(path);
                var repository = createRepository();

                repository.Name = directoryName;
                repository.FullPath = path;

                Repositories.Add(repository);

                return repository;
            }

            return null;
        }

        private void UpdateStatus(Repository repository, CommandRunner.ExecutionResult result)
        {
            bool matched = false;

            repository.StatusReport = result.Output;

            var changesMatches = _changesRegex.Matches(result.Output);
            repository.Changes = 0;

            repository.ChangesReport = "";
            foreach (var changesMatch in changesMatches)
            {
                ++repository.Changes;
                repository.ChangesReport += changesMatch.ToString() + "\n";
            }
            if (repository.Changes == 0)
            {
                repository.ChangesReport = "No changes";
            }

            foreach (var tuple in _messageToStatus)
            {
                var match = tuple.Key.Match(result.Output);
                if (match.Success)
                {
                    repository.SetStatus(tuple.Value);
                    matched = true;
                }
            }

            if (!matched)
            {
                var notRepoMatches = _notAGitRepoRegex.Match(result.Output);
                if (notRepoMatches.Success)
                {
                    repository.SetStatus(RepoStatus.NotAGitRepo);
                }
                else
                {
                    repository.SetStatus(RepoStatus.UpToDate);
                }
            }
        }

        public void RefreshStatus(IEnumerable<Repository> repositories = null)
        {
            var collection = repositories ?? Repositories;

            foreach (var repository in collection)
            {
                RefreshStatus(repository);
            }
        }

        public void RefreshStatus(Repository repository)
        {
            repository.Changes = 0;
            repository.ChangesReport = "";
            repository.SetStatus(RepoStatus.Fetching);
            CommandRunner.RunCommand(SynchronizationContext.Current, fetchCommand,
                result => UpdateStatus(repository, result), repository.FullPath);
        }
    }
}