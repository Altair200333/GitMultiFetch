using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitMultiFetch;

namespace GitMulltyFetch.Model
{
    public interface IWatcherSerializer
    {
        void Deserialize(GitOwerwatch watch);
        void Serialize(GitOwerwatch watch);
    }
}
