using Datory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Core.Services
{
    public partial class ExamManager
    {
        public async Task<List<Cascade<int>>> GetExamTmTreeCascadesAsync(AdminAuth auth, bool isTotal = false)
        {
            var list = new List<Cascade<int>>();
            var trees = await _databaseManager.ExamTmTreeRepository.GetListAsync(auth);
            var items = trees.Where(p => p.ParentId == 0).ToList();

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    var total = 0;
                    var count = 0;
                    if (isTotal)
                    {
                        (count, total) = await _databaseManager.ExamTmRepository.GetTotalAndCountByTreeIdAsync(auth, item.Id);
                    }

                    var cascade = new Cascade<int>
                    {
                        Id = item.Id,
                        Popover = false,
                        Label = item.Name,
                        Value = item.Id,
                        Total = total,
                        SelfTotal = count,
                        Children = await GetExamTmCascadesAsync(trees, item.Id, auth, isTotal)
                    };
                    list.Add(cascade);
                }
            }
            return list;
        }
        private async Task<List<Cascade<int>>> GetExamTmCascadesAsync(List<ExamTmTree> all, int parentId, AdminAuth auth, bool isTotal = false)
        {
            var list = new List<Cascade<int>>();
            var items = all.Where(p => p.ParentId == parentId).ToList();

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    var total = 0;
                    var count = 0;
                    if (isTotal)
                    {
                        (count, total) = await _databaseManager.ExamTmRepository.GetTotalAndCountByTreeIdAsync(auth, item.Id);
                    }

                    var cascade = new Cascade<int>
                    {
                        Id = item.Id,
                        Popover = false,
                        Label = item.Name,
                        Value = item.Id,
                        SelfTotal = count,
                        Total = total,
                        Children = await GetExamTmCascadesAsync(all, item.Id, auth, isTotal)
                    };
                    list.Add(cascade);
                }
            }
            return list;
        }

        public async Task<List<Cascade<int>>> GetExamPaperTreeCascadesAsync(AdminAuth auth, bool isTotal = false)
        {
            var list = new List<Cascade<int>>();
            var trees = await _databaseManager.ExamPaperTreeRepository.GetListAsync(auth);
            var items = trees.Where(p => p.ParentId == 0).ToList();

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    var total = 0;
                    var count = 0;
                    if (isTotal)
                    {
                        (count, total) = await _databaseManager.ExamPaperRepository.GetTotalAndCountByTreeIdAsync(auth, item.Id);
                    }

                    var cascade = new Cascade<int>
                    {
                        Id = item.Id,
                        Popover = false,
                        Label = item.Name,
                        Value = item.Id,
                        Total = total,
                        SelfTotal = count,
                        Children = await GetExamPaperCascadesAsync(auth, trees, item.Id, isTotal)
                    };
                    list.Add(cascade);
                }
            }
            return list;
        }
        private async Task<List<Cascade<int>>> GetExamPaperCascadesAsync(AdminAuth auth, List<ExamPaperTree> all, int parentId, bool isTotal = false)
        {
            var list = new List<Cascade<int>>();
            var items = all.Where(p => p.ParentId == parentId).ToList();

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    var total = 0;
                    var count = 0;
                    if (isTotal)
                    {
                        (count, total) = await _databaseManager.ExamPaperRepository.GetTotalAndCountByTreeIdAsync(auth, item.Id);
                    }
                    var cascade = new Cascade<int>
                    {
                        Id = item.Id,
                        Popover = false,
                        Label = item.Name,
                        Value = item.Id,
                        SelfTotal = count,
                        Total = total,
                        Children = await GetExamPaperCascadesAsync(auth, all, item.Id, isTotal)
                    };
                    list.Add(cascade);
                }
            }
            return list;
        }



        public async Task<List<Cascade<int>>> GetKnowlegesTreeCascadesAsync(AdminAuth auth, bool isTotal = false)
        {
            var list = new List<Cascade<int>>();
            var trees = await _databaseManager.KnowlegesTreeRepository.GetListAsync(auth);
            var items = trees.Where(p => p.ParentId == 0).ToList();

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    var total = 0;
                    var count = 0;
                    if (isTotal)
                    {
                        (count, total) = await _databaseManager.KnowlegesRepository.GetTotalAndCountByTreeIdAsync(auth, item.Id);
                    }
                    var cascade = new Cascade<int>
                    {
                        Id = item.Id,
                        Popover = false,
                        Label = item.Name,
                        Value = item.Id,
                        Total = total,
                        SelfTotal = count,
                        Children = await GetKnowlegesTreeCascadesAsync(auth, trees, item.Id, isTotal)
                    };
                    list.Add(cascade);
                }
            }
            return list;
        }
        private async Task<List<Cascade<int>>> GetKnowlegesTreeCascadesAsync(AdminAuth auth, List<KnowledgesTree> all, int parentId, bool isTotal = false)
        {
            var list = new List<Cascade<int>>();
            var items = all.Where(p => p.ParentId == parentId).ToList();

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    var total = 0;
                    var count = 0;
                    if (isTotal)
                    {
                        (count, total) = await _databaseManager.KnowlegesRepository.GetTotalAndCountByTreeIdAsync(auth, item.Id);
                    }

                    var cascade = new Cascade<int>
                    {
                        Id = item.Id,
                        Popover = false,
                        Label = item.Name,
                        Value = item.Id,
                        SelfTotal = count,
                        Total = total,
                        Children = await GetKnowlegesTreeCascadesAsync(auth, all, item.Id, isTotal)
                    };
                    list.Add(cascade);
                }
            }
            return list;
        }
    }
}
