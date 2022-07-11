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
        public int Changes { get; set; } = 0;
        public string ChangesReport { get; set; } = "No changes";

        public string Status => GetStatus();
        public string StatusReport { get; set; }

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
