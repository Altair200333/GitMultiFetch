namespace GitMulltyFetch.Model
{
    public enum RepoStatus
    {
        Unknown,
        Fetching,
        NotAGitRepo,
        UpToDate,
        Behind,
        Ahead
    }

    public class Repository
    {
        public string Name { get; set; } = "defaultName";
        public string FullPath { get; set; } = "C:/defaultName";
        public string Status => GetStatus();

        protected RepoStatus _status = RepoStatus.Unknown;

        private string GetStatus()
        {
            return _status.ToString();
        }

        public virtual void SetStatus(RepoStatus status)
        {
            _status = status;
        }
    }
}
