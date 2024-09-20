using MaxMind.GeoIP2;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public class BlockManager : IBlockManager
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IBlockAnalysisRepository _analysisRepository;
        private readonly IBlockRuleRepository _ruleRepository;

        private static DatabaseReader _reader;
        private static List<BlockArea> _areas;

        public BlockManager(ISettingsManager settingsManager, IBlockAnalysisRepository analysisRepository, IBlockRuleRepository ruleRepository)
        {
            _settingsManager = settingsManager;
            _analysisRepository = analysisRepository;
            _ruleRepository = ruleRepository;


            if (_areas == null)
            {
                _areas = new List<BlockArea>();

                var locationsEn =
                    PathUtils.Combine(_settingsManager.WebRootPath,
                        "sitefiles/assets/js/block/GeoLite2-Country-CSV_20190423/GeoLite2-Country-Locations-en.csv");
                var locationsCn =
                    PathUtils.Combine(_settingsManager.WebRootPath,
                        "sitefiles/assets/js/block/GeoLite2-Country-CSV_20190423/GeoLite2-Country-Locations-zh-CN.csv");
                var enCsv = File.ReadAllLines(locationsEn);
                var cnCsv = File.ReadAllLines(locationsCn);

                for (var i = 0; i < enCsv.Length; i++)
                {
                    if (i == 0) continue;

                    var enSplits = enCsv[i].Split(',');
                    var cnSplits = cnCsv[i].Split(',');

                    var geoNameIdEn = TranslateUtils.ToInt(enSplits[0]);
                    var areaEn = enSplits[5].Trim('"');
                    var geoNameIdCn = TranslateUtils.ToInt(cnSplits[0]);
                    var areaCn = cnSplits[5].Trim('"');

                    if (geoNameIdEn == geoNameIdCn && !string.IsNullOrEmpty(areaEn) && !string.IsNullOrEmpty(areaCn))
                    {
                        _areas.Add(new BlockArea
                        {
                            GeoNameId = geoNameIdEn,
                            AreaEn = areaEn,
                            AreaCn = areaCn
                        });
                    }
                }

                _areas = _areas.OrderBy(x => x.AreaEn).ToList();

                _areas.Insert(0, new BlockArea
                {
                    GeoNameId = LocalGeoNameId,
                    AreaEn = LocalAreaEn,
                    AreaCn = LocalAreaCn
                });
            }

            if (_reader == null)
            {
                var filePath = PathUtils.Combine(_settingsManager.WebRootPath,
                    "sitefiles/assets/js/block/GeoLite2-Country_20190423/GeoLite2-Country.mmdb");
                _reader = new DatabaseReader(filePath);
            }
        }

        public List<IdName> GetAreas()
        {
            return _areas.Select(x => new IdName
            {
                Id = x.GeoNameId,
                Name = $"{x.AreaEn}({x.AreaCn})"
            })
                .ToList();
        }

        public BlockArea GetArea(int geoNameId)
        {
            return _areas.FirstOrDefault(x => x.GeoNameId == geoNameId);
        }

        public int GetGeoNameId(string ipAddress)
        {
            if (IsLocalIp(ipAddress)) return LocalGeoNameId;
            return _reader.TryCountry(ipAddress, out var response) ? (int)response.Country.GeoNameId : 0;
        }

        private static bool IsLocalIp(string ipAddress)
        {
            return ipAddress == "127.0.0.1" || Regex.IsMatch(ipAddress,
                @"(^192\.168\.([0-9]|[0-9][0-9]|[0-2][0-5][0-5])\.([0-9]|[0-9][0-9]|[0-2][0-5][0-5])$)|(^172\.([1][6-9]|[2][0-9]|[3][0-1])\.([0-9]|[0-9][0-9]|[0-2][0-5][0-5])\.([0-9]|[0-9][0-9]|[0-2][0-5][0-5])$)|(^10\.([0-9]|[0-9][0-9]|[0-2][0-5][0-5])\.([0-9]|[0-9][0-9]|[0-2][0-5][0-5])\.([0-9]|[0-9][0-9]|[0-2][0-5][0-5])$)");
        }

        private static void AddRestrictionToIpList(IpList list, string restriction)
        {
            if (string.IsNullOrEmpty(restriction)) return;

            if (StringUtils.Contains(restriction, "-"))
            {
                restriction = restriction.Trim(' ', '-');
                var arr = restriction.Split('-');
                list.AddRange(arr[0].Trim(), arr[1].Trim());
            }
            else if (StringUtils.Contains(restriction, "*"))
            {
                var ipPrefix = restriction.Substring(0, restriction.IndexOf('*'));
                ipPrefix = ipPrefix.Trim(' ', '.');
                var dotNum = StringUtils.GetCount(".", ipPrefix);

                string ipNumber;
                string mask;
                if (dotNum == 0)
                {
                    ipNumber = ipPrefix + ".0.0.0";
                    mask = "255.0.0.0";
                }
                else if (dotNum == 1)
                {
                    ipNumber = ipPrefix + ".0.0";
                    mask = "255.255.0.0";
                }
                else
                {
                    ipNumber = ipPrefix + ".0";
                    mask = "255.255.255.0";
                }
                list.Add(ipNumber, mask);
            }
            else
            {
                list.Add(restriction);
            }
        }

        private static bool IsAllowed(string ipAddress, List<string> blockList, List<string> allowList)
        {
            var isAllowed = true;

            if (blockList != null && blockList.Count > 0)
            {
                var list = new IpList();
                foreach (var restriction in blockList)
                {
                    AddRestrictionToIpList(list, restriction);
                }
                if (list.CheckNumber(ipAddress))
                {
                    isAllowed = false;
                }
            }
            else if (allowList != null && allowList.Count > 0)
            {
                isAllowed = false;
                var list = new IpList();
                foreach (var restriction in allowList)
                {
                    AddRestrictionToIpList(list, restriction);
                }
                if (list.CheckNumber(ipAddress))
                {
                    isAllowed = true;
                }
            }

            return isAllowed;
        }

        public async Task<(bool, BlockRule)> IsBlockedAsync(string ipAddress, string sessionId,int blockType=1)
        {
            var rules = await _ruleRepository.GetAllAsync(blockType);
            if (rules == null || rules.Count == 0) return (false, null);

            var geoNameId = GetGeoNameId(ipAddress);
            var area = GetArea(geoNameId);

            foreach (var rule in rules)
            {
                if (rule.BlockMethod == BlockMethod.Password && !string.IsNullOrEmpty(sessionId))
                {
                    if (rule.Password == _settingsManager.Decrypt(sessionId))
                    {
                        continue;
                    }
                }

                var isMatch = false;
                if (area != null)
                {
                    if (rule.BlockAreas != null && rule.BlockAreas.Contains(area.GeoNameId))
                    {
                        isMatch = true;
                    }
                }

                var isBlocked = false;
                if (rule.AreaType == AreaType.Includes)
                {
                    isBlocked = isMatch;
                }
                else if (rule.AreaType == AreaType.Excludes)
                {
                    isBlocked = !isMatch;
                }

                if (!isBlocked)
                {
                    isBlocked = !IsAllowed(ipAddress, rule.BlockList, rule.AllowList);
                }

                if (isBlocked)
                {
                    await _analysisRepository.AddBlockAsync(blockType);
                    return (true, rule);
                }
            }

            return (false, null);
        }

        public const int LocalGeoNameId = 10000;

        public const string LocalAreaEn = "Local IP";

        public const string LocalAreaCn = "内网地址";

        private static bool IsIpAddress(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }
}
