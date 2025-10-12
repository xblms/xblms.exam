﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using XBLMS.Configuration;

namespace XBLMS.Utils
{
    public static class TranslateUtils
    {
        public static T Get<T>(object value, T defaultValue = default)
        {
            switch (value)
            {
                case null:
                    return defaultValue;
                case T variable:
                    return variable;
                default:
                    try
                    {
                        return (T)Convert.ChangeType(value, typeof(T));
                    }
                    catch (InvalidCastException)
                    {
                        return defaultValue;
                    }
            }
        }

        public static T Get<T>(IDictionary<string, object> dict, string name, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(name)) return default;

            return dict.TryGetValue(name, out var extendValue) ? Get(extendValue, defaultValue) : default;
        }

        public static T ToEnum<T>(string value, T defaultValue) where T : struct
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return Enum.TryParse<T>(value, true, out var result) ? result : defaultValue;
        }

        public static (bool, T) ToEnum<T>(string value) where T : struct
        {
            if (string.IsNullOrEmpty(value))
            {
                return (false, default(T));
            }

            return Enum.TryParse<T>(value, true, out var result) ? (true, result) : (false, default(T));
        }

        public static int ToInt(string intStr, int defaultValue = 0)
        {
            if (!int.TryParse(intStr?.Trim().TrimStart('0'), out var i))
            {
                i = defaultValue;
            }
            if (i < 0)
            {
                i = defaultValue;
            }
            return i;
        }

        public static int ToIntWithNegative(string intStr, int defaultValue = 0)
        {
            if (!int.TryParse(intStr?.Trim(), out var i))
            {
                i = defaultValue;
            }
            return i;
        }

        public static decimal ToDecimal(string intStr, decimal defaultValue = 0)
        {
            if (!decimal.TryParse(intStr?.Trim(), out var i))
            {
                i = defaultValue;
            }
            if (i < 0)
            {
                i = defaultValue;
            }
            return i;
        }

        public static decimal ToDecimalWithNegative(string intStr, decimal defaultValue)
        {
            if (!decimal.TryParse(intStr?.Trim(), out var i))
            {
                i = defaultValue;
            }
            return i;
        }

        public static double ToDouble(string intStr, double defaultValue = 0)
        {
            if (!double.TryParse(intStr?.Trim(), out var i))
            {
                i = defaultValue;
            }
            if (i < 0)
            {
                i = defaultValue;
            }
            return i;
        }

        public static double ToDoubleWithNegative(string intStr, double defaultValue)
        {
            if (!double.TryParse(intStr?.Trim(), out var i))
            {
                i = defaultValue;
            }
            return i;
        }

        public static float ToFloat(string intStr, float defaultValue = 0)
        {
            if (!float.TryParse(intStr?.Trim(), out var i))
            {
                i = defaultValue;
            }
            if (i < 0)
            {
                i = defaultValue;
            }
            return i;
        }

        public static bool ToBool(string boolStr)
        {
            if (!bool.TryParse(boolStr?.Trim(), out var boolean))
            {
                boolean = false;
            }
            return boolean;
        }

        public static bool ToBool(string boolStr, bool defaultValue)
        {
            if (!bool.TryParse(boolStr?.Trim(), out var boolean))
            {
                boolean = defaultValue;
            }
            return boolean;
        }

        public static int Ceiling(int numerator, int denominator)
        {
            return Convert.ToInt32(Math.Ceiling((double)numerator / denominator));
        }
        public static string ToMinuteAndSecond(int second, bool isChinese = true)
        {
            if (second > 0)
            {
                if (second < 10)
                {
                    return isChinese ? $"0分{second}秒" : $"00:{second}";
                }
                else
                {
                    if (second > 60)
                    {
                        return isChinese ? $"{ToInt((second / 60).ToString())}分{second % 60}秒" : $"{ToInt((second / 60).ToString())}:{second % 60}";
                    }
                    else
                    {
                        return isChinese ? $"0分{second}秒" : $"0:{second}";
                    }
                }
            }
            else
            {
                return isChinese ? "0分0秒" : "00:00";
            }
        }
        public static string ToHour(int minute)
        {
            if (minute > 0)
            {
                if (minute < 60)
                {
                    return $"{minute}分钟";
                }
                else
                {
                    return $"{ToInt((minute / 60).ToString())}小时{minute % 60}分钟";
                }
            }
            else
            {
                return "0分钟";
            }
        }
        public static string ToAvg(double value, double sumValue)
        {
            if (value > 0 && sumValue > 0)
            {
                return (value / sumValue).ToString("0.00");
            }
            return "0.00";
        }
        public static string ToPercent(decimal a, decimal b)
        {
            if (a > 0 && b > 0)
            {
                if (a > b)
                {
                    return "100";
                }
                else
                {
                    decimal t = decimal.Parse((a / b).ToString("0.000")); //保留3位小数
                    var t1 = Math.Round(t, 2);  //四舍五入,精确2位
                    var t2 = t1 * 100;  //乘以100     x100结果%
                    return t2.ToString();
                }
            }
            return "0";
        }
        public static DateTime ToDateTime(string dateTimeStr)
        {
            var datetime = Constants.SqlMinValue;
            if (!string.IsNullOrEmpty(dateTimeStr))
            {
                if (!DateTime.TryParse(dateTimeStr.Trim(), out datetime))
                {
                    datetime = Constants.SqlMinValue;
                }
            }
            if (datetime < Constants.SqlMinValue)
            {
                datetime = Constants.SqlMinValue;
            }
            return datetime;
        }

        public static DateTime ToDateTime(string dateTimeStr, DateTime defaultValue)
        {
            var datetime = defaultValue;
            if (!string.IsNullOrEmpty(dateTimeStr))
            {
                if (!DateTime.TryParse(dateTimeStr.Trim(), out datetime))
                {
                    datetime = defaultValue;
                }
                return datetime;
            }
            return datetime;
        }

        public static Dictionary<string, string> ToDictionary(NameValueCollection attributes)
        {
            var dict = new Dictionary<string, string>();
            if (attributes != null)
            {
                dict = attributes.AllKeys.ToDictionary(k => k, k => attributes[k]);
            }
            return dict;
        }

        public static Dictionary<string, object> ToDictionary(object obj)
        {
            if (obj == null) return new Dictionary<string, object>();
            if (obj is Dictionary<string, object> objects) return objects;

            var props = obj.GetType().GetProperties();
            var pairDictionary = props.ToDictionary(x => x.Name, x => x.GetValue(obj, null));
            return pairDictionary;
        }

        public static string ToSqlInStringWithoutQuote(ICollection collection)
        {
            var builder = new StringBuilder();
            if (collection != null)
            {
                foreach (var obj in collection)
                {
                    builder.Append(obj).Append(",");
                }
                if (builder.Length != 0) builder.Remove(builder.Length - 1, 1);
            }
            return builder.Length == 0 ? "null" : builder.ToString();
        }

        public static NameValueCollection ToNameValueCollection(string separateString)
        {
            if (!string.IsNullOrEmpty(separateString))
            {
                separateString = separateString.Replace("/u0026", "&");
            }
            return ToNameValueCollection(separateString, '&');
        }

        public static NameValueCollection ToNameValueCollection(string separateString, char seperator)
        {
            var attributes = new NameValueCollection();
            if (!string.IsNullOrEmpty(separateString))
            {
                var pairs = separateString.Split(seperator);
                foreach (var pair in pairs)
                {
                    if (pair.IndexOf("=", StringComparison.Ordinal) != -1)
                    {
                        var name = StringUtils.ValueFromUrl(pair.Split('=')[0]);
                        var value = StringUtils.ValueFromUrl(pair.Split('=')[1]);
                        attributes.Add(name, value);
                    }
                }
            }
            return attributes;
        }

        public static string ToAttributesString(NameValueCollection attributes)
        {
            var builder = new StringBuilder();
            if (attributes != null && attributes.Count > 0)
            {
                foreach (string key in attributes.Keys)
                {
                    var value = attributes[key];
                    if (!string.IsNullOrEmpty(value))
                    {
                        value = value.Replace("\"", "'");
                    }
                    builder.Append($@"{key}=""{value}"" ");
                }
                builder.Length--;
            }
            return builder.ToString();
        }

        public static string ToAttributesString(Dictionary<string, string> attributes)
        {
            var builder = new StringBuilder();
            if (attributes != null && attributes.Count > 0)
            {
                foreach (var key in attributes.Keys)
                {
                    var value = attributes[key];

                    builder.Append(value == null ? $"{key} " : $@"{key}=""{value.Replace("\"", "'")}"" ");
                }
                builder.Length--;
            }
            return builder.ToString();
        }

        #region 汉字转拼音

        private static readonly int[] Pyvalue =
        {
            -20319, -20317, -20304, -20295, -20292, -20283, -20265, -20257, -20242, -20230, -20051, -20036, -20032,
            -20026, -20002, -19990, -19986, -19982, -19976, -19805, -19784, -19775, -19774, -19763, -19756, -19751,
            -19746, -19741, -19739, -19728,
            -19725, -19715, -19540, -19531, -19525, -19515, -19500, -19484, -19479, -19467, -19289, -19288, -19281,
            -19275, -19270, -19263,
            -19261, -19249, -19243, -19242, -19238, -19235, -19227, -19224, -19218, -19212, -19038, -19023, -19018,
            -19006, -19003, -18996,
            -18977, -18961, -18952, -18783, -18774, -18773, -18763, -18756, -18741, -18735, -18731, -18722, -18710,
            -18697, -18696, -18526,
            -18518, -18501, -18490, -18478, -18463, -18448, -18447, -18446, -18239, -18237, -18231, -18220, -18211,
            -18201, -18184, -18183,
            -18181, -18012, -17997, -17988, -17970, -17964, -17961, -17950, -17947, -17931, -17928, -17922, -17759,
            -17752, -17733, -17730,
            -17721, -17703, -17701, -17697, -17692, -17683, -17676, -17496, -17487, -17482, -17468, -17454, -17433,
            -17427, -17417, -17202,
            -17185, -16983, -16970, -16942, -16915, -16733, -16708, -16706, -16689, -16664, -16657, -16647, -16474,
            -16470, -16465, -16459,
            -16452, -16448, -16433, -16429, -16427, -16423, -16419, -16412, -16407, -16403, -16401, -16393, -16220,
            -16216, -16212, -16205,
            -16202, -16187, -16180, -16171, -16169, -16158, -16155, -15959, -15958, -15944, -15933, -15920, -15915,
            -15903, -15889, -15878,
            -15707, -15701, -15681, -15667, -15661, -15659, -15652, -15640, -15631, -15625, -15454, -15448, -15436,
            -15435, -15419, -15416,
            -15408, -15394, -15385, -15377, -15375, -15369, -15363, -15362, -15183, -15180, -15165, -15158, -15153,
            -15150, -15149, -15144,
            -15143, -15141, -15140, -15139, -15128, -15121, -15119, -15117, -15110, -15109, -14941, -14937, -14933,
            -14930, -14929, -14928,
            -14926, -14922, -14921, -14914, -14908, -14902, -14894, -14889, -14882, -14873, -14871, -14857, -14678,
            -14674, -14670, -14668,
            -14663, -14654, -14645, -14630, -14594, -14429, -14407, -14399, -14384, -14379, -14368, -14355, -14353,
            -14345, -14170, -14159,
            -14151, -14149, -14145, -14140, -14137, -14135, -14125, -14123, -14122, -14112, -14109, -14099, -14097,
            -14094, -14092, -14090,
            -14087, -14083, -13917, -13914, -13910, -13907, -13906, -13905, -13896, -13894, -13878, -13870, -13859,
            -13847, -13831, -13658,
            -13611, -13601, -13406, -13404, -13400, -13398, -13395, -13391, -13387, -13383, -13367, -13359, -13356,
            -13343, -13340, -13329,
            -13326, -13318, -13147, -13138, -13120, -13107, -13096, -13095, -13091, -13076, -13068, -13063, -13060,
            -12888, -12875, -12871,
            -12860, -12858, -12852, -12849, -12838, -12831, -12829, -12812, -12802, -12607, -12597, -12594, -12585,
            -12556, -12359, -12346,
            -12320, -12300, -12120, -12099, -12089, -12074, -12067, -12058, -12039, -11867, -11861, -11847, -11831,
            -11798, -11781, -11604,
            -11589, -11536, -11358, -11340, -11339, -11324, -11303, -11097, -11077, -11067, -11055, -11052, -11045,
            -11041, -11038, -11024,
            -11020, -11019, -11018, -11014, -10838, -10832, -10815, -10800, -10790, -10780, -10764, -10587, -10544,
            -10533, -10519, -10331,
            -10329, -10328, -10322, -10315, -10309, -10307, -10296, -10281, -10274, -10270, -10262, -10260, -10256,
            -10254
        };

        private static readonly string[] Pystr =
        {
            "a", "ai", "an", "ang", "ao", "ba", "bai", "ban", "bang", "bao", "bei", "ben", "beng", "bi", "bian", "biao",
            "bie", "bin", "bing", "bo", "bu", "ca", "cai", "can", "cang", "cao", "ce", "ceng", "cha", "chai", "chan",
            "chang", "chao", "che", "chen",
            "cheng", "chi", "chong", "chou", "chu", "chuai", "chuan", "chuang", "chui", "chun", "chuo", "ci", "cong",
            "cou", "cu", "cuan", "cui",
            "cun", "cuo", "da", "dai", "dan", "dang", "dao", "de", "deng", "di", "dian", "diao", "die", "ding", "diu",
            "dong", "dou", "du", "duan",
            "dui", "dun", "duo", "e", "en", "er", "fa", "fan", "fang", "fei", "fen", "feng", "fo", "fou", "fu", "ga",
            "gai", "gan", "gang", "gao",
            "ge", "gei", "gen", "geng", "gong", "gou", "gu", "gua", "guai", "guan", "guang", "gui", "gun", "guo", "ha",
            "hai", "han", "hang",
            "hao", "he", "hei", "hen", "heng", "hong", "hou", "hu", "hua", "huai", "huan", "huang", "hui", "hun", "huo",
            "ji", "jia", "jian",
            "jiang", "jiao", "jie", "jin", "jing", "jiong", "jiu", "ju", "juan", "jue", "jun", "ka", "kai", "kan",
            "kang", "kao", "ke", "ken",
            "keng", "kong", "kou", "ku", "kua", "kuai", "kuan", "kuang", "kui", "kun", "kuo", "la", "lai", "lan", "lang",
            "lao", "le", "lei",
            "leng", "li", "lia", "lian", "liang", "liao", "lie", "lin", "ling", "liu", "long", "lou", "lu", "lv", "luan",
            "lue", "lun", "luo",
            "ma", "mai", "man", "mang", "mao", "me", "mei", "men", "meng", "mi", "mian", "miao", "mie", "min", "ming",
            "miu", "mo", "mou", "mu",
            "na", "nai", "nan", "nang", "nao", "ne", "nei", "nen", "neng", "ni", "nian", "niang", "niao", "nie", "nin",
            "ning", "niu", "nong",
            "nu", "nv", "nuan", "nue", "nuo", "o", "ou", "pa", "pai", "pan", "pang", "pao", "pei", "pen", "peng", "pi",
            "pian", "piao", "pie",
            "pin", "ping", "po", "pu", "qi", "qia", "qian", "qiang", "qiao", "qie", "qin", "qing", "qiong", "qiu", "qu",
            "quan", "que", "qun",
            "ran", "rang", "rao", "re", "ren", "reng", "ri", "rong", "rou", "ru", "ruan", "rui", "run", "ruo", "sa",
            "sai", "san", "sang",
            "sao", "se", "sen", "seng", "sha", "shai", "shan", "shang", "shao", "she", "shen", "sheng", "shi", "shou",
            "shu", "shua",
            "shuai", "shuan", "shuang", "shui", "shun", "shuo", "si", "song", "sou", "su", "suan", "sui", "sun", "suo",
            "ta", "tai",
            "tan", "tang", "tao", "te", "teng", "ti", "tian", "tiao", "tie", "ting", "tong", "tou", "tu", "tuan", "tui",
            "tun", "tuo",
            "wa", "wai", "wan", "wang", "wei", "wen", "weng", "wo", "wu", "xi", "xia", "xian", "xiang", "xiao", "xie",
            "xin", "xing",
            "xiong", "xiu", "xu", "xuan", "xue", "xun", "ya", "yan", "yang", "yao", "ye", "yi", "yin", "ying", "yo",
            "yong", "you",
            "yu", "yuan", "yue", "yun", "za", "zai", "zan", "zang", "zao", "ze", "zei", "zen", "zeng", "zha", "zhai",
            "zhan", "zhang",
            "zhao", "zhe", "zhen", "zheng", "zhi", "zhong", "zhou", "zhu", "zhua", "zhuai", "zhuan", "zhuang", "zhui",
            "zhun", "zhuo",
            "zi", "zong", "zou", "zu", "zuan", "zui", "zun", "zuo"
        };

        public static string ToPinYin(string chrstr)
        {
            var returnstr = string.Empty;
            var nowchar = chrstr.ToCharArray();
            foreach (var t in nowchar)
            {
                var array = Encoding.Default.GetBytes(t.ToString());
                int i1 = array[0];
                int i2 = array[1];
                var chrasc = i1 * 256 + i2 - 65536;
                if (chrasc > 0 && chrasc < 160)
                {
                    returnstr += t;
                }
                else
                {
                    for (var i = (Pyvalue.Length - 1); i >= 0; i--)
                    {
                        if (Pyvalue[i] <= chrasc)
                        {
                            returnstr += Pystr[i];
                            break;
                        }
                    }
                }
            }
            return returnstr;
        }

        #endregion

        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = new List<JsonConverter>
            {
                new IsoDateTimeConverter {DateTimeFormat = "yyyy-MM-dd HH:mm:ss"}
            },
            Formatting = Formatting.Indented
        };

        public static string JsonSerialize(object obj)
        {
            if (obj == null) return string.Empty;

            try
            {
                return JsonConvert.SerializeObject(obj, JsonSettings);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static dynamic JsonDeserialize(string json)
        {
            if (string.IsNullOrEmpty(json)) return null;
            
            return JsonConvert.DeserializeObject<dynamic>(json);
        }

        public static T JsonDeserialize<T>(string json, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(json)) return defaultValue;

            try
            {
                return JsonConvert.DeserializeObject<T>(json, JsonSettings);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static Dictionary<string, object> ToDictionaryIgnoreCase(JObject json)
        {
            return new Dictionary<string, object>(json.ToObject<IDictionary<string, object>>(), StringComparer.OrdinalIgnoreCase);
        }

        public static Dictionary<string, object> ToDictionaryIgnoreCase(string json)
        {
            var defaultDict = new Dictionary<string, object>();
            return new Dictionary<string, object>(JsonDeserialize(json, defaultDict), StringComparer.OrdinalIgnoreCase);
        }

        public static NameValueCollection NewIgnoreCaseNameValueCollection()
        {
            var comparer = StringComparer.OrdinalIgnoreCase;
            var caseInsensitiveDictionary = new NameValueCollection(comparer);
            return caseInsensitiveDictionary;
        }

        public static void AddAttributeIfNotExists(NameValueCollection target, string attributeName, string attributeValue)
        {
            if (target == null || attributeName == null) return;

            if (string.IsNullOrEmpty(target[attributeName]))
            {
                target[attributeName] = attributeValue;
            }
        }

        public static void AddAttributesIfNotExists(NameValueCollection target, NameValueCollection attributes)
        {
            if (target == null || attributes == null) return;

            foreach (var attributeName in attributes.AllKeys)
            {
                if (string.IsNullOrEmpty(target[attributeName]))
                {
                    target[attributeName] = attributes[attributeName];
                }
            }
        }
    }
}
