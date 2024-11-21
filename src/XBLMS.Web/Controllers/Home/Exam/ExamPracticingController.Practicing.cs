namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPracticingController
    {
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="request">0 错题练习 -1 收藏练习</param>
        ///// <returns></returns>
        //[HttpGet, Route(RoutePricticingTmIds)]
        //public async Task<ActionResult<GetPracticingIdsResult>> GetTmIds([FromQuery] GetPracticingRequest request)
        //{
        //    var user = await _authManager.GetAdminAsync();
        //    if (user == null)
        //    {
        //        return Unauthorized();
        //    }
        //    else
        //    {
        //        var lmsUtils = new LmsUtils(_databaseManager);


        //        var title = "";
        //        var companyId = user.CompanyId;
        //        var organId = user.OrganId;
        //        var zsds = new List<string>();
        //        var resultIds = new List<int>();
        //        if (request.Id > 0)
        //        {
        //            var info = await _examPracticeRepository.GetAsync(request.Id);
        //            var creator = await _adminRepository.GetByUserIdAsync(info.CreatedUserId);
        //            var purviewParams = await lmsUtils.GetPurviewParamsAsync(creator);

        //            title = $"题库练习：{info.Title}";
        //            //zsds = await _examTmRepository.GetKnowledgeList(organId, info.TkIds);
        //            zsds = info.Zsds;
        //            resultIds = await _examTmRepository.GetTmIdsForUserPracticeAsync(purviewParams, organId, info.TkIds, info.Zsds, info.TxIds);
        //        }
        //        else if (request.Id == 0)
        //        {
        //            title = $"错题练习：{request.Zsd}";
        //            zsds.Add(request.Zsd);
        //            resultIds = await _examPracticeErrorRepository.GetTmIdsForUserAsync(user.Id, request.Zsd);
        //        }
        //        else if (request.Id == -1)
        //        {
        //            title = $"收藏练习：{request.Zsd}";
        //            zsds.Add(request.Zsd);
        //            resultIds = await _examPracticeCollectionRepository.GetTmIdsForUserAsync(user.Id, request.Zsd);
        //        }
        //        else if (request.Id == -2)
        //        {
        //            title = $"综合练习";
        //            var infoList = await _examPracticeRepository.GetListAsync(organId, "");

        //            if (infoList != null && infoList.Count > 0)
        //            {

        //                var tmIds = new List<int>();
        //                foreach (var info in infoList)
        //                {
        //                    var creator = await _adminRepository.GetByUserIdAsync(info.CreatedUserId);
        //                    var purviewParams = await lmsUtils.GetPurviewParamsAsync(creator);

        //                    var tkids = new List<int>();
        //                    var txids = new List<int>();
        //                    if (info.Range)
        //                    {
        //                        var zsd = info.Zsds;
        //                        if (info.Zsds == null || info.Zsds.Count < 1)
        //                        {
        //                            zsd = await _examTmRepository.GetKnowledgeList(purviewParams, organId, info.TkIds);
        //                        }
        //                        zsds.AddRange(zsd);
        //                        tkids.AddRange(info.TkIds);
        //                        txids.AddRange(info.TxIds);
        //                    }
        //                    else
        //                    {
        //                        if (ListUtils.Contains(info.RangeCompanys, user.CompanyId) || ListUtils.Contains(info.RangeDepartments, user.DepartmentId) || ListUtils.Contains(info.RangePosts, user.PostId))
        //                        {
        //                            var zsd = info.Zsds;
        //                            if (info.Zsds == null || info.Zsds.Count < 1)
        //                            {
        //                                zsd = await _examTmRepository.GetKnowledgeList(purviewParams, organId, info.TkIds);
        //                            }
        //                            zsds.AddRange(zsd);
        //                            tkids.AddRange(info.TkIds);
        //                            txids.AddRange(info.TxIds);
        //                        }
        //                    }

        //                    var resultTmIds = await _examTmRepository.GetTmIdsForUserPracticeAsync(purviewParams, organId, tkids, zsds, txids);
        //                    tmIds.AddRange(resultTmIds);
        //                }
        //                resultIds = tmIds;
        //            }
        //        }

        //        resultIds = resultIds.OrderBy(i => StringUtils.GetShortGuid()).ToList();
        //        var total = resultIds.Count;
        //        if (total > 0)
        //        {
        //            var practiceUserId = await _examPracticeUserRepository.InsertAsync(new ExamPracticeUser
        //            {
        //                PracticeId = request.Id,
        //                UserId = user.Id,
        //                Zsds = zsds,
        //                TmCount = total,
        //                CompanyId = companyId,
        //                OrganId = organId,
        //                CreatedUserId = user.Id,
        //                CreatedUserName = user.UserName,
        //                DepartmentId = user.DepartmentId
        //            });
        //            return new GetPracticingIdsResult
        //            {
        //                PracticeUserId = practiceUserId,
        //                TmIdList = resultIds,
        //                Total = total,
        //                Title = title
        //            };
        //        }
        //        else
        //        {
        //            return this.Error("没有练习题");
        //        }
        //    }
        //}

        //[HttpGet, Route(RoutePricticingTmInfo)]
        //public async Task<ActionResult<GetPracticingResultTmInfo>> GetTmInfo([FromQuery] IdRequest request)
        //{
        //    var user = await _authManager.GetAdminAsync();
        //    if (user == null)
        //    {
        //        return Unauthorized();
        //    }
        //    else
        //    {
        //        var lmsUtils = new LmsUtils(_configRepository);

        //        var organId = user.OrganId;
        //        var tm = await _examTmRepository.GetAsync(request.Id);
        //        if (tm == null) return NotFound();

        //        var smallTmList = new List<GetPracticingResultTmInfo>();

        //        var txInfo = await _examTxRepository.GetAsync(tm.TxId, organId);
        //        if (txInfo == null) return NotFound();
        //        var tmTitle = await lmsUtils.ParseFileContentUrl(tm.Title);
        //        var tmTitleList = new List<KeyValuePair<int, string>>();
        //        if (txInfo.TxType == Enums.ExamTxType.Completion && tmTitle.Contains("___"))
        //        {
        //            string newTitle = StringUtils.Replace(tmTitle, "___", "|___|");
        //            newTitle = StringUtils.Replace(newTitle, "<p>", "");
        //            newTitle = StringUtils.Replace(newTitle, "</p>", "");
        //            var tmTitleSplitList = ListUtils.GetStringList(newTitle, '|');
        //            var inputIndex = 0;
        //            for (int i = 0; i < tmTitleSplitList.Count; i++)
        //            {
        //                if (tmTitleSplitList[i].Contains("___"))
        //                {
        //                    tmTitleList.Add(new KeyValuePair<int, string>(inputIndex, tmTitleSplitList[i]));
        //                    inputIndex++;
        //                }
        //                else
        //                {
        //                    tmTitleList.Add(new KeyValuePair<int, string>(i, tmTitleSplitList[i]));
        //                }
        //            }
        //        }
        //        if (txInfo.TxType == Enums.ExamTxType.Group)
        //        {
        //            var smallList = await _examTmRepository.GetSmallListForParentIdAsync(tm.Id);
        //            foreach (var small in smallList)
        //            {
        //                var smalltmTitleList = new List<KeyValuePair<int, string>>();
        //                var smallTx = await _examTxRepository.GetAsync(small.TxId, organId);
        //                var smallTitle = await lmsUtils.ParseFileContentUrl(small.Title);
        //                if (smallTx.TxType == Enums.ExamTxType.Completion && small.Title.Contains("___"))
        //                {
        //                    string newTitle = StringUtils.Replace(smallTitle, "___", "|___|");
        //                    newTitle = StringUtils.Replace(newTitle, "<p>", "");
        //                    newTitle = StringUtils.Replace(newTitle, "</p>", "");
        //                    var tmTitleSplitList = ListUtils.GetStringList(newTitle, '|');
        //                    var inputIndex = 0;
        //                    for (int i = 0; i < tmTitleSplitList.Count; i++)
        //                    {
        //                        if (tmTitleSplitList[i].Contains("___"))
        //                        {
        //                            smalltmTitleList.Add(new KeyValuePair<int, string>(inputIndex, tmTitleSplitList[i]));
        //                            inputIndex++;
        //                        }
        //                        else
        //                        {
        //                            smalltmTitleList.Add(new KeyValuePair<int, string>(i, tmTitleSplitList[i]));
        //                        }
        //                    }
        //                }
        //                smallTmList.Add(new GetPracticingResultTmInfo
        //                {
        //                    Id = small.Id,
        //                    Tm = smallTitle,
        //                    TmTitle = smalltmTitleList,
        //                    Tx = smallTx.Name,
        //                    TxType = smallTx.TxType.ToString(),
        //                    Options = ListUtils.ToList(small.Get("options")),
        //                    OptionsValue = new List<string>(),
        //                    ParentId = small.ParentId,
        //                    Zsd = small.Knowledge,
        //                    IsCollection = await _examPracticeCollectionRepository.IsCollectionAsync(user.Id, small.Id),
        //                    IsError = await _examPracticeErrorRepository.ExsitAsync(user.Id, small.Id),

        //                });

        //            }
        //        }
        //        var txtm = new GetPracticingResultTmInfo
        //        {
        //            Id = tm.Id,
        //            Tm = tmTitle,
        //            TmTitle = tmTitleList,
        //            Tx = txInfo.Name,
        //            TxType = txInfo.TxType.ToString(),
        //            Options = ListUtils.ToList(tm.Get("options")),
        //            OptionsValue = new List<string>(),
        //            ParentId = tm.ParentId,
        //            Zsd = tm.Knowledge,
        //            IsCollection = await _examPracticeCollectionRepository.IsCollectionAsync(user.Id, tm.Id),
        //            IsError = await _examPracticeErrorRepository.ExsitAsync(user.Id, tm.Id),
        //            SmallList = smallTmList,
        //        };

        //        return txtm;

        //    }

        //}

        //[RequestSizeLimit(long.MaxValue)]
        //[HttpPost, Route(RoutePricticingSubmit)]
        //public async Task<ActionResult<GetPracticingResultTmInfo>> SubmitPrictice([FromBody] GetSubmitPracticingRequest request)
        //{
        //    var user = await _authManager.GetAdminAsync();
        //    if (user == null)
        //    {
        //        return Unauthorized();
        //    }
        //    else
        //    {
        //        var organId = user.OrganId;
        //        var companyId = user.CompanyId;

        //        var requestTm = request.Tm;
        //        var tm = await _examTmRepository.GetAsync(requestTm.Id);
        //        var smallList = requestTm.SmallList;
        //        if (smallList != null && smallList.Count > 0)
        //        {
        //            var allRight = true;
        //            foreach (var item in smallList)
        //            {
        //                var smallTm = await _examTmRepository.GetAsync(item.Id);
        //                var isRight = smallTm.Answer == item.Answer;
        //                //if (!isRight)
        //                //{
        //                //    allRight = false;
        //                //    await _examPracticeErrorRepository.InsertAsync(new ExamPracticeError
        //                //    {
        //                //        UserId = user.Id,
        //                //        TmId = smallTm.Id,
        //                //        MyAnswer = item.Answer,
        //                //        TmAnswer = smallTm.Answer,
        //                //        Source = "题库练习",
        //                //        Zsd = smallTm.Knowledge,
        //                //        CompanyId = companyId,
        //                //        CreatedUserId = user.Id,
        //                //        CreatedUserName = user.CreatedUserName,
        //                //        IsBig = false,
        //                //        OrganId = organId
        //                //    });
        //                //}
        //                await _examPracticeUserAnswerRepository.InsertAsync(new ExamPracticeUserAnswer
        //                {
        //                    PracticeId = request.PracticeId,
        //                    PracticeUserId = request.PracticeUserId,
        //                    UserId = user.Id,
        //                    IsRight = isRight,
        //                    MyAnswer = item.Answer,
        //                    TmAnswer = smallTm.Answer,
        //                    TmId = item.Id,
        //                    CompanyId = companyId,
        //                    CreatedUserId = user.Id,
        //                    CreatedUserName = user.UserName,
        //                    OrganId = organId,
        //                    DepartmentId = user.DepartmentId

        //                });
        //                item.IsRight = isRight;
        //                item.IsSubmit = true;
        //                item.Analysis = smallTm.Analysis;
        //                item.RightAnswer = smallTm.Answer;
        //            }
        //            var bigIsRight = allRight;
        //            await _examPracticeUserAnswerRepository.InsertAsync(new ExamPracticeUserAnswer
        //            {
        //                PracticeId = request.PracticeId,
        //                PracticeUserId = request.PracticeUserId,
        //                UserId = user.Id,
        //                IsRight = bigIsRight,
        //                TmId = requestTm.Id,
        //                CompanyId = companyId,
        //                CreatedUserId = user.Id,
        //                CreatedUserName = user.UserName,
        //                OrganId = organId,
        //                DepartmentId = user.DepartmentId

        //            });
        //            requestTm.SmallList = smallList;
        //            requestTm.Analysis = tm.Analysis;
        //            requestTm.IsRight = bigIsRight;
        //            requestTm.IsSubmit = true;
        //            requestTm.RightAnswer = tm.Answer;

        //            if (!bigIsRight)
        //            {
        //                await _examPracticeErrorRepository.InsertAsync(new ExamPracticeError
        //                {
        //                    UserId = user.Id,
        //                    TmId = tm.Id,
        //                    MyAnswer = requestTm.Answer,
        //                    TmAnswer = tm.Answer,
        //                    Source = "题库练习",
        //                    Zsd = tm.Knowledge,
        //                    CompanyId = companyId,
        //                    CreatedUserId = user.Id,
        //                    CreatedUserName = user.CreatedUserName,
        //                    IsBig = true,
        //                    OrganId = organId,
        //                    DepartmentId = user.DepartmentId
        //                });
        //            }
        //            else
        //            {
        //                await _examPracticeUserRepository.UpdateRightCountAsync(request.PracticeUserId);
        //            }
        //        }
        //        else
        //        {
        //            var isRight = requestTm.Answer == tm.Answer;
        //            await _examPracticeUserAnswerRepository.InsertAsync(new ExamPracticeUserAnswer
        //            {
        //                PracticeId = request.PracticeId,
        //                PracticeUserId = request.PracticeUserId,
        //                UserId = user.Id,
        //                IsRight = isRight,
        //                MyAnswer = requestTm.Answer,
        //                TmAnswer = tm.Answer,
        //                TmId = requestTm.Id,
        //                CompanyId = companyId,
        //                CreatedUserId = user.Id,
        //                CreatedUserName = user.UserName,
        //                OrganId = organId,
        //                DepartmentId = user.DepartmentId

        //            });
        //            requestTm.Analysis = tm.Analysis;
        //            requestTm.IsRight = isRight;
        //            requestTm.IsSubmit = true;
        //            requestTm.RightAnswer = tm.Answer;

        //            if (!isRight)
        //            {
        //                await _examPracticeErrorRepository.InsertAsync(new ExamPracticeError
        //                {
        //                    UserId = user.Id,
        //                    TmId = tm.Id,
        //                    MyAnswer = requestTm.Answer,
        //                    TmAnswer = tm.Answer,
        //                    Source = "题库练习",
        //                    Zsd = tm.Knowledge,
        //                    CompanyId = companyId,
        //                    CreatedUserId = user.Id,
        //                    CreatedUserName = user.UserName,
        //                    IsBig = false,
        //                    OrganId = organId,
        //                    DepartmentId = user.DepartmentId
        //                });
        //            }
        //            else
        //            {
        //                await _examPracticeUserRepository.UpdateRightCountAsync(request.PracticeUserId);
        //            }
        //        }
        //        await _examPracticeUserRepository.UpdateAnswerCountAsync(request.PracticeUserId);
        //        var errorCount = await _examPracticeErrorRepository.CountAsync(user.Id, tm.Id);

        //        await _pointsLogRepository.XInsertAsync(user.OrganId, user.Id, Enums.PointsType.Practice, user.Id);

        //        return new GetPracticingResultTmInfo
        //        {
        //            SmallList = smallList,
        //            IsRight = requestTm.IsRight,
        //            IsSubmit = true,
        //            RightAnswer = requestTm.RightAnswer,
        //            Analysis = requestTm.Analysis,
        //            ErrorTotal = errorCount,
        //        };
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="request">0 错题练习 -1 收藏练习</param>
        ///// <returns></returns>
        //[HttpGet, Route(RoutePricticing)]
        //public async Task<ActionResult<GetPracticingResult>> Get([FromQuery] GetPracticingRequest request)
        //{
        //    var user = await _authManager.GetAdminAsync();
        //    if (user == null)
        //    {
        //        return Unauthorized();
        //    }
        //    else
        //    {
        //        var title = "";
        //        var organId = user.OrganId;
        //        var companyId = user.CompanyId;

        //        var zsds = new List<string>();
        //        var tmList = new List<ExamTm>();
        //        if (request.Id > 0)
        //        {
        //            var info = await _examPracticeRepository.GetAsync(request.Id);
        //            title = $"题库练习：{info.Title}";
        //            zsds = await _examTmRepository.GetKnowledgeList(organId, info.TkIds);
        //            tmList = await _examTmRepository.GetTmsForUserPracticeAsync(organId, info.TkIds, info.Zsds, info.TxIds);
        //        }
        //        else if (request.Id == 0)
        //        {
        //            title = $"错题练习：{request.Zsd}";
        //            zsds.Add(request.Zsd);
        //            var tmIds = new List<int>();
        //            var errorTmList = await _examPracticeErrorRepository.GetTmListForUserAsync(user.Id, request.Zsd);
        //            foreach (var item in errorTmList)
        //            {
        //                if (!tmIds.Contains(item.TmId))
        //                {
        //                    tmIds.Add(item.TmId);
        //                    if (item.IsBig)
        //                    {
        //                        var smallIds = await _examTmRepository.GetSmallIdsForParentIdAsync(item.TmId);
        //                        foreach (var smallId in smallIds)
        //                        {
        //                            if (!tmIds.Contains(smallId))
        //                            {
        //                                tmIds.Add(smallId);
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            tmList = await _examTmRepository.GetListAsync(tmIds);
        //        }
        //        else if (request.Id == -1)
        //        {
        //            title = $"收藏练习：{request.Zsd}";
        //            zsds.Add(request.Zsd);
        //            var tmIds = new List<int>();
        //            var collectionTmList = await _examPracticeCollectionRepository.GetTmListForUserAsync(user.Id, request.Zsd);
        //            foreach (var item in collectionTmList)
        //            {
        //                if (!tmIds.Contains(item.TmId))
        //                {
        //                    tmIds.Add(item.TmId);
        //                    if (item.IsBig)
        //                    {
        //                        tmIds.AddRange(await _examTmRepository.GetSmallIdsForParentIdAsync(item.TmId));
        //                    }
        //                }
        //            }
        //            tmList = await _examTmRepository.GetListAsync(tmIds);
        //        }
        //        else if (request.Id == -2)
        //        {
        //            title = $"综合练习";
        //            var infoList = await _examPracticeRepository.GetListAsync(organId, "", 1, int.MaxValue);
        //            if (infoList.total > 0)
        //            {
        //                var tmIds = new List<int>();
        //                foreach (var info in infoList.list)
        //                {
        //                    zsds.AddRange(await _examTmRepository.GetKnowledgeList(organId, info.TkIds));
        //                    tmList = await _examTmRepository.GetTmsForUserPracticeAsync(organId, info.TkIds, info.Zsds, info.TxIds);
        //                    foreach (var tm in tmList)
        //                    {
        //                        if (!tmIds.Contains(tm.Id))
        //                        {
        //                            tmIds.Add(tm.Id);
        //                        }
        //                    }
        //                }
        //                tmList = await _examTmRepository.GetListAsync(tmIds);
        //            }
        //        }

        //        var lmsUtils = new LmsUtils(_configRepository);

        //        var smallTmList = new List<GetPracticingResultTmInfo>();
        //        var parentList = new List<GetPracticingResultTmInfo>();
        //        var tmListForParent = tmList.FindAll(ftm => ftm.ParentId == 0);
        //        foreach (var tm in tmListForParent)
        //        {
        //            var txInfo = await _examTxRepository.GetAsync(tm.TxId, organId);
        //            var tmTitle = await lmsUtils.ParseFileContentUrl(tm.Title);
        //            var tmTitleList = new List<KeyValuePair<int, string>>();
        //            if (txInfo.TxType == Enums.ExamTxType.Completion && tmTitle.Contains("___"))
        //            {
        //                string newTitle = StringUtils.Replace(tmTitle, "___", "|___|");
        //                newTitle = StringUtils.Replace(newTitle, "<p>", "");
        //                newTitle = StringUtils.Replace(newTitle, "</p>", "");
        //                var tmTitleSplitList = ListUtils.GetStringList(newTitle, '|');
        //                var inputIndex = 0;
        //                for (int i = 0; i < tmTitleSplitList.Count; i++)
        //                {
        //                    if (tmTitleSplitList[i].Contains("___"))
        //                    {
        //                        tmTitleList.Add(new KeyValuePair<int, string>(inputIndex, tmTitleSplitList[i]));
        //                        inputIndex++;
        //                    }
        //                    else
        //                    {
        //                        tmTitleList.Add(new KeyValuePair<int, string>(i, tmTitleSplitList[i]));
        //                    }
        //                }
        //            }
        //            var txtm = new GetPracticingResultTmInfo
        //            {
        //                Id = tm.Id,
        //                Tm = tmTitle,
        //                TmTitle = tmTitleList,
        //                Tx = txInfo.Name,
        //                TxType = txInfo.TxType.ToString(),
        //                Options = ListUtils.ToList(tm.Get("options")),
        //                OptionsValue = new List<string>(),
        //                ParentId = tm.ParentId,
        //                Zsd = tm.Knowledge,
        //                IsCollection = await _examPracticeCollectionRepository.IsCollectionAsync(user.Id, tm.Id),
        //                IsError = await _examPracticeErrorRepository.ExsitAsync(user.Id, tm.Id)
        //            };
        //            parentList.Add(txtm);
        //        }


        //        var txSmallTmList = tmList.FindAll(ftm => ftm.ParentId != 0);
        //        foreach (var tm in txSmallTmList)
        //        {
        //            var txInfo = await _examTxRepository.GetAsync(tm.TxId, user.CompanyId);
        //            var tmTitle = await lmsUtils.ParseFileContentUrl(tm.Title);
        //            var tmTitleList = new List<KeyValuePair<int, string>>();
        //            if (txInfo.TxType == Enums.ExamTxType.Completion && tmTitle.Contains("___"))
        //            {
        //                string newTitle = StringUtils.Replace(tmTitle, "___", "|___|");
        //                newTitle = StringUtils.Replace(newTitle, "<p>", "");
        //                newTitle = StringUtils.Replace(newTitle, "</p>", "");
        //                var tmTitleSplitList = ListUtils.GetStringList(newTitle, '|');
        //                var inputIndex = 0;
        //                for (int i = 0; i < tmTitleSplitList.Count; i++)
        //                {
        //                    if (tmTitleSplitList[i].Contains("___"))
        //                    {
        //                        tmTitleList.Add(new KeyValuePair<int, string>(inputIndex, tmTitleSplitList[i]));
        //                        inputIndex++;
        //                    }
        //                    else
        //                    {
        //                        tmTitleList.Add(new KeyValuePair<int, string>(i, tmTitleSplitList[i]));
        //                    }
        //                }
        //            }
        //            var txtm = new GetPracticingResultTmInfo
        //            {
        //                Id = tm.Id,
        //                Tm = tmTitle,
        //                TmTitle = tmTitleList,
        //                Tx = txInfo.Name,
        //                TxType = txInfo.TxType.ToString(),
        //                Options = ListUtils.ToList(tm.Get("options")),
        //                OptionsValue = new List<string>(),
        //                ParentId = tm.ParentId,
        //                Zsd = tm.Knowledge
        //            };
        //            smallTmList.Add(txtm);
        //        }

        //        var newtmList = new List<GetPracticingResultTmInfo>();
        //        foreach (var tm in parentList)
        //        {
        //            var smallTms = new List<GetPracticingResultTmInfo>();
        //            foreach (var smallTm in smallTmList)
        //            {
        //                if (smallTm.ParentId == tm.Id)
        //                {
        //                    smallTms.Add(smallTm);
        //                }
        //            }
        //            tm.SmallList = smallTms;
        //            newtmList.Add(tm);
        //        }

        //        var total = newtmList.Count;
        //        if (total > 0)
        //        {
        //            var practiceUserId = await _examPracticeUserRepository.InsertAsync(new ExamPracticeUser
        //            {
        //                PracticeId = request.Id,
        //                UserId = user.Id,
        //                Zsds = zsds,
        //                TmCount = newtmList.Count,
        //                CompanyId = companyId,
        //                CreatedUserId = user.Id,
        //                CreatedUserName = user.UserName,
        //                OrganId = organId
        //            });
        //            return new GetPracticingResult
        //            {
        //                PracticeUserId = practiceUserId,
        //                List = newtmList,
        //                Total = newtmList.Count,
        //                Title = title
        //            };
        //        }
        //        else
        //        {
        //            return this.Error("没有练习题");
        //        }

        //    }

        //}

    }
}



