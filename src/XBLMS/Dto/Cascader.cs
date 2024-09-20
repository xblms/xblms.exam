using System.Collections.Generic;
using XBLMS.Utils;

namespace XBLMS.Dto
{
    public class Cascade<T> : Dictionary<string, object>
    {
        public T Value
        {
            get => TranslateUtils.Get(this, nameof(Value), default(T));
            set => this[nameof(Value)] = value;
        }

        public int Id
        {
            get => TranslateUtils.Get<int>(this, nameof(Id));
            set => this[nameof(Id)] = value;
        }

        public string Label {
            get => TranslateUtils.Get<string>(this, nameof(Label));
            set => this[nameof(Label)] = value;
        }

        public bool Popover
        {
            get => TranslateUtils.Get<bool>(this, nameof(Popover));
            set => this[nameof(Popover)] = value;
        }

        public int Total
        {
            get => TranslateUtils.Get<int>(this, nameof(Total));
            set => this[nameof(Total)] = value;
        }

        public int SelfTotal
        {
            get => TranslateUtils.Get<int>(this, nameof(SelfTotal));
            set => this[nameof(SelfTotal)] = value;
        }

        public List<Cascade<T>> Children
        {
            get => TranslateUtils.Get<List<Cascade<T>>>(this, nameof(Children), null);
            set => this[nameof(Children)] = value;
        }
    }
}
