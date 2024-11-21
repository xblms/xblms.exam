using System;
using System.Collections.Generic;
using XBLMS.Services;

namespace XBLMS.Core.Utils
{
    public class ColumnsManager
    {
        private readonly IDatabaseManager _databaseManager;
        private readonly IPathManager _pathManager;

        public ColumnsManager(IDatabaseManager databaseManager, IPathManager pathManager)
        {
            _databaseManager = databaseManager;
            _pathManager = pathManager;
        }

        public const string PageContent = nameof(PageContent);
        public const string NavigationUrl = nameof(NavigationUrl);
        public const string CheckState = nameof(CheckState);
        public const string CheckAdminId = nameof(CheckAdminId);                    //审核人
        public const string CheckDate = nameof(CheckDate);                          //审核时间
        public const string CheckReasons = nameof(CheckReasons);                    //审核意见

        public const string Sequence = nameof(Sequence);                            //序号
        private const string ChannelName = nameof(ChannelName);
        private const string AdminName = nameof(AdminName);
        private const string AdminGuid = nameof(AdminGuid);
        private const string LastEditAdminName = nameof(LastEditAdminName);
        private const string LastEditAdminGuid = nameof(LastEditAdminGuid);
        private const string UserName = nameof(UserName);
        private const string UserGuid = nameof(UserGuid);
        private const string CheckAdminName = nameof(CheckAdminName);
        private const string CheckAdminGuid = nameof(CheckAdminGuid);
        private const string SourceName = nameof(SourceName);
        private const string TemplateName = nameof(TemplateName);
        private const string State = nameof(State);

        public static string GetFormatStringAttributeName(string attributeName)
        {
            return attributeName + "FormatString";
        }

        public static string GetCountName(string attributeName)
        {
            return $"{attributeName}Count";
        }

        public static string GetExtendName(string attributeName, int n)
        {
            return n > 0 ? $"{attributeName}{n}" : attributeName;
        }

        public static string GetColumnWidthName(string attributeName)
        {
            return $"{attributeName}ColumnWidth";
        }

        public static readonly Lazy<List<string>> MetadataAttributes = new Lazy<List<string>>(() => new List<string>
        {
            "HitsByDay",
            "HitsByWeek",
            "HitsByMonth",
            "LastHitsDate",
            "ExtendValues",
        });


        public static readonly Lazy<List<string>> DropAttributes = new Lazy<List<string>>(() => new List<string>
        {
            "WritingUserName",
            "ConsumePoint",
            "Comments",
            "Reply",
            "CheckTaskDate",
            "UnCheckTaskDate",
            "Photos",
            "Teleplays",
            "MemberName",
            "GroupNameCollection",
            "Tags",
            "IsChecked",
            "SettingsXml",
            "IsTop",
            "IsRecommend",
            "IsHot",
            "IsColor",
            "AddUserName",
            "LastEditUserName",
            "Content",
            "LastEditDate",
            "HitsByDay",
            "HitsByWeek",
            "HitsByMonth",
            "LastHitsDate"
        });

        private static readonly List<string> CalculatedAttributes = new List<string>
        {
            Sequence,
            CheckAdminId
        };

        private static readonly List<string> UnSearchableAttributes = new List<string>
        {
            Sequence,
            CheckAdminId,
            CheckDate,
            CheckReasons,
        };

        public enum PageType
        {
            Contents,
            SearchContents,
            CheckContents,
            RecycleContents,
            Export
        }

    }
}
