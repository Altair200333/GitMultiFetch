using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMulltyFetch.Model
{
    class WatchTxtSerializer: IWatcherSerializer
    {
        private string _path;

        public WatchTxtSerializer(string path)
        {
            this._path = path;
        }

        public void Deserialize(GitOwerwatch watch)
        {
            if(File.Exists(_path))
            {
                foreach (string line in File.ReadLines(_path))
                {
                    watch.TryAddRepository(line);
                }
            }
        }

        public void Serialize(GitOwerwatch watch)
        {
            using StreamWriter file = new(_path);

            foreach (var repository in watch.Repositories)
            {
                file.WriteLine(repository.FullPath);
            }
        }
    }
}
