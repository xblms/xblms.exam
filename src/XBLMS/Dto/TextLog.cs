using System;
using XBLMS.Utils;

namespace XBLMS.Dto
{
    public class TextLog
    {
        public DateTime DateTime { private get; set; }
        public string Detail { private get; set; }
        public Exception Exception { private get; set; }

        public override string ToString()
        {
            return TranslateUtils.JsonSerialize(new
            {
                DateTime,
                Detail,
                Exception?.Message,
                Exception?.StackTrace
            });
        }
    }
}
