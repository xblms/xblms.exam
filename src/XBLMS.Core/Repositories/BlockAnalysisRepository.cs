using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datory;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class BlockAnalysisRepository : IBlockAnalysisRepository
    {
        private readonly Repository<BlockAnalysis> _repository;

        public BlockAnalysisRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<BlockAnalysis>(settingsManager.Database);
        }
        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;
        private static class Attr
        {

            public const string BlockDate = nameof(BlockAnalysis.BlockDate);

            public const string BlockCount = nameof(BlockAnalysis.BlockCount);
            public const string BlockType = nameof(BlockAnalysis.BlockType);
        }

        public async Task AddBlockAsync(int blockType = 1)
        {
            var now = GetNow();
            var exists = await _repository.ExistsAsync(Q
                .Where(Attr.BlockDate, now)
                .Where(Attr.BlockType, blockType)
            );

            if (exists)
            {
                await _repository.IncrementAsync(Attr.BlockCount, Q
                    .Where(Attr.BlockDate, now)
                );
            }
            else
            {
                await _repository.InsertAsync(new BlockAnalysis
                {
                    BlockDate = now,
                    BlockCount = 1,
                    BlockType = blockType
                });
            }
        }

        private static DateTime GetNow()
        {
            var now = DateTime.Now;
            return new DateTime(now.Year, now.Month, now.Day);
        }

        public async Task<List<KeyValuePair<string, int>>> GetMonthlyBlockedListAsync(int blockType = 1)
        {
            var now = GetNow();
            var blockInfoList = await _repository.GetAllAsync(Q
                .WhereBetween(Attr.BlockDate, now.AddDays(-30), now.AddDays(1))
                .Where(Attr.BlockType, blockType)
            );

            var blockedList = new List<KeyValuePair<string, int>>();
            for (var i = 30; i >= 0; i--)
            {
                var date = now.AddDays(-i).ToString("M-d");
                var blockInfo = blockInfoList.FirstOrDefault(x => x.BlockDate.ToString("M-d") == date);
                blockedList.Add(new KeyValuePair<string, int>(date, blockInfo?.BlockCount ?? 0));
            }

            return blockedList;
        }

    }
}
