using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Datory;
using Datory.Annotations;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Newtonsoft.Json.Converters;
using NPOI.POIFS.Properties;
using NPOI.SS.Formula.Functions;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class ExamManager
    {
        public async Task<List<Cascade<int>>> GetExamTmTreeCascadesAsync(bool isTotal = false)
        {
            var list = new List<Cascade<int>>();
            var trees = await _examTmTreeRepository.GetListAsync();
            var items = trees.Where(p => p.ParentId == 0).ToList();

            if (items != null && items.Any())
            {
                foreach (var item in items)
                {
                    var tmTotal = 0;
                    var tmCount = 0;
                    if (isTotal)
                    {
                        var treeIds = await _examTmTreeRepository.GetIdsAsync(item.Id);
                        tmTotal = await _examTmRepository.GetCountByTreeIdsAsync(treeIds);
                        tmCount = await _examTmRepository.GetCountByTreeIdAsync(item.Id);
                    }


                    var cascade = new Cascade<int>
                    {
                        Id = item.Id,
                        Popover = false,
                        Label = item.Name,
                        Value = item.Id,
                        Total = tmTotal,
                        SelfTotal = tmCount,
                        Children = await GetExamTmCascadesAsync(trees, item.Id, isTotal)
                    };
                    list.Add(cascade);
                }
            }
            return list;
        }
        private async Task<List<Cascade<int>>> GetExamTmCascadesAsync(List<ExamTmTree> all, int parentId, bool isTotal = false)
        {
            var list = new List<Cascade<int>>();
            var items = all.Where(p => p.ParentId == parentId).ToList();

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    var tmTotal = 0;
                    var tmCount = 0;
                    if (isTotal)
                    {
                        var treeIds = await _examTmTreeRepository.GetIdsAsync(item.Id);
                        tmTotal = await _examTmRepository.GetCountByTreeIdsAsync(treeIds);
                        tmCount = await _examTmRepository.GetCountByTreeIdAsync(item.Id);
                    }
                    var cascade = new Cascade<int>
                    {
                        Id = item.Id,
                        Popover = false,
                        Label = item.Name,
                        Value = item.Id,
                        SelfTotal = tmCount,
                        Total = tmTotal,
                        Children = await GetExamTmCascadesAsync(all, item.Id, isTotal)
                    };
                    list.Add(cascade);
                }
            }
            return list;
        }
        public async Task<List<Cascade<int>>> GetExamPaperTreeCascadesAsync(bool isTotal = false)
        {
            var list = new List<Cascade<int>>();
            var trees = await _examPaperTreeRepository.GetListAsync();
            var items = trees.Where(p => p.ParentId == 0).ToList();

            if (items != null && items.Any())
            {
                foreach (var item in items)
                {
                    var total = 0;
                    var count = 0;
                    if (isTotal)
                    {
                        var treeIds = await _examPaperTreeRepository.GetIdsAsync(item.Id);
                        
                        total = await _examPaperRepository.GetCountAsync(treeIds);
                        count = await _examPaperRepository.GetCountAsync(new List<int>() { item.Id });
                    }


                    var cascade = new Cascade<int>
                    {
                        Id = item.Id,
                        Popover = false,
                        Label = item.Name,
                        Value = item.Id,
                        Total = total,
                        SelfTotal = count,
                        Children = await GetExamPaperCascadesAsync(trees, item.Id, isTotal)
                    };
                    list.Add(cascade);
                }
            }
            return list;
        }
        private async Task<List<Cascade<int>>> GetExamPaperCascadesAsync(List<ExamPaperTree> all, int parentId, bool isTotal = false)
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
                        var treeIds = await _examPaperTreeRepository.GetIdsAsync(item.Id);
                        total = await _examPaperRepository.GetCountAsync(treeIds);
                        count = await _examPaperRepository.GetCountAsync(new List<int>() { item.Id });
                    }
                    var cascade = new Cascade<int>
                    {
                        Id = item.Id,
                        Popover = false,
                        Label = item.Name,
                        Value = item.Id,
                        SelfTotal = count,
                        Total = total,
                        Children = await GetExamPaperCascadesAsync(all, item.Id, isTotal)
                    };
                    list.Add(cascade);
                }
            }
            return list;
        }
    }
}
