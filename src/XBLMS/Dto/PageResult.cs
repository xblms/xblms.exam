using System.Collections.Generic;

namespace XBLMS.Dto
{
    public class PageResult<T> where T : class
    {
        public IEnumerable<T> Items { get; set; }

        public int Count { get; set; }
    }
}