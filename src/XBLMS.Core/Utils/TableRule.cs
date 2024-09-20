using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Core.Utils
{
    [JsonConverter(typeof(StringEnumConverter))]
	public enum TableRule
	{
	    Choose,
	    HandWrite,
	    Create
    }
}
