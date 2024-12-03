using Datory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class UserMenuRepository : IUserMenuRepository
    {
        private readonly Repository<UserMenu> _repository;

        public UserMenuRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<UserMenu>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        private string CacheKey => CacheUtils.GetListKey(_repository.TableName);

        public async Task<int> InsertAsync(UserMenu userMenu)
        {
            return await _repository.InsertAsync(userMenu, Q.CachingRemove(CacheKey));
        }

        public async Task UpdateAsync(UserMenu userMenu)
        {
            await _repository.UpdateAsync(userMenu, Q.CachingRemove(CacheKey));
        }

        public async Task DeleteAsync(int menuId)
        {
            await _repository.DeleteAsync(menuId, Q.CachingRemove(CacheKey));
        }

        public async Task<List<UserMenu>> GetUserMenusAsync()
        {
            var infoList = await _repository.GetAllAsync(Q.CachingGet(CacheKey));
            var list = infoList.ToList();

            return list.OrderBy(userMenu => userMenu.Taxis == 0 ? int.MaxValue : userMenu.Taxis).ToList();
        }

        public async Task<UserMenu> GetAsync(int id)
        {
            var infoList = await _repository.GetAllAsync(Q.CachingGet(CacheKey));
            return infoList.FirstOrDefault(x => x.Id == id);
        }

        public async Task ResetAsync()
        {
            await _repository.DeleteAsync(Q.CachingRemove(CacheKey));

            var parentId = await InsertAsync(new UserMenu
            {
                Name = "index",
                Text = "首页",
                IconClass = "el-icon-s-home",
                Link = "/home/dashboard/",
                Taxis = 1,
                
            });

            parentId = await InsertAsync(new UserMenu
            {
                Name = "examPaper",
                Text = "考试中心",
                Link = "/home/exam/examPaper",
                IconClass = "el-icon-platform-eleme",
                Taxis = 2
            });

            parentId = await InsertAsync(new UserMenu
            {
                Name = "examPaperMoni",
                Text = "模拟自测",
                Link = "/home/exam/examPaperMoni",
                IconClass = "el-icon-eleme",
                Taxis = 3
            });

            parentId = await InsertAsync(new UserMenu
            {
                Name = "examQuestionnaire",
                Text = "调查问卷",
                Link = "/home/exam/examQuestionnaire",
                IconClass = "el-icon-question",
                Taxis = 4
            });

            parentId = await InsertAsync(new UserMenu
            {
                Name = "examAssessment",
                Text = "测评中心",
                Link = "/home/exam/examAssessment",
                IconClass = "el-icon-magic-stick",
                Taxis = 5
            });

            parentId = await InsertAsync(new UserMenu
            {
                Name = "examPk",
                Text = "答题竞赛",
                Link = "/home/exam/examPk",
                IconClass = "el-icon-trophy",
                Taxis = 6
            });

            parentId = await InsertAsync(new UserMenu
            {
                Name = "examPractice",
                Text = "刷题练习",
                Link = "/home/exam/examPractice",
                IconClass = "el-icon-s-order",
                Taxis = 7
            });

            parentId = await InsertAsync(new UserMenu
            {
                Name = "examPaperScore",
                Text = "考试成绩",
                Link = "/home/exam/examPaperScore",
                IconClass = "el-icon-notebook-2",
                Taxis = 8
            });
            parentId = await InsertAsync(new UserMenu
            {
                Name = "examPaperCer",
                Text = "获得证书",
                Link = "/home/exam/examPaperCer",
                IconClass = "el-icon-medal",
                Taxis = 9
            });

            parentId = await InsertAsync(new UserMenu
            {
                Name = "logout",
                Text = "退出系统",
                IconClass = "el-icon-switch-button",
                Link = "/home/logout/",
                Taxis = 10
            });

        }
    }
}
