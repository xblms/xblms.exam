using Datory.Caching;
using System.Collections.Generic;

namespace Datory.Utils
{
    internal class CompileInfo
    {
        public string Sql { get; set; }
        public Dictionary<string, object> NamedBindings { get; set; }
        public CachingCondition Caching { get; set; }
    }
}
