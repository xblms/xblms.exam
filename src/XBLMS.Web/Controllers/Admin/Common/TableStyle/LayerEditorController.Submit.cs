using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Common.TableStyle
{
    public partial class LayerEditorController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] SubmitRequest request)
        {
            var admin = await _authManager.GetAdminAsync();

            var relatedIdentities = ListUtils.GetIntList(request.RelatedIdentities);
            var styleDatabase =
                await _tableStyleRepository.GetTableStyleAsync(request.TableName, request.AttributeName, relatedIdentities) ??
                new Models.TableStyle();

            bool isSuccess;
            string errorMessage;

            if (request.Id > 0)
            {
                (isSuccess, errorMessage) = await UpdateTableStyleAsync(styleDatabase, request);
            }
            else
            {
                (isSuccess, errorMessage) = await InsertTableStyleAsync(request);
            }

            if (!isSuccess)
            {
                return this.Error(errorMessage);
            }

            return new BoolResult
            {
                Value = true
            };
        }

        private async Task<(bool Success, string ErrorMessage)> InsertTableStyleAsync(SubmitRequest request)
        {
            var relatedIdentities = ListUtils.GetIntList(request.RelatedIdentities);
            var relatedIdentity = relatedIdentities[0];

            if (string.IsNullOrEmpty(request.AttributeName))
            {
                return (false, "字段名不能为空！");
            }

            if (await _tableStyleRepository.IsExistsAsync(request.TableName, request.AttributeName))
            {
                return (false, $@"字段名""{request.AttributeName}""已存在");
            }

            var style = await _databaseManager.IsAttributeNameExistsAsync(request.TableName, request.AttributeName) ? await _tableStyleRepository.GetTableStyleAsync(request.TableName, request.AttributeName, relatedIdentities) : new Models.TableStyle();

            style.RelatedIdentity = relatedIdentity;
            style.TableName = request.TableName;
            style.AttributeName = request.AttributeName;
            style.DisplayName = request.DisplayName;
            style.HelpText = request.HelpText;
            style.Taxis = request.Taxis;
            style.InputType = request.InputType;
            style.DefaultValue = request.DefaultValue;
            style.Horizontal = request.Horizontal;
            style.Locked = request.Locked;
            style.Valid = request.Valid;
            style.Items = new List<InputStyleItem>();

            if (request.InputType == InputType.CheckBox || request.InputType == InputType.Radio || request.InputType == InputType.SelectMultiple || request.InputType == InputType.SelectOne)
            {
                if (request.IsRapid)
                {
                    foreach (var rapidValue in ListUtils.GetStringListByReturnAndNewline(request.RapidValues))
                    {
                        var itemInfo = new InputStyleItem
                        {
                            Label = rapidValue,
                            Value = rapidValue,
                            Selected = false
                        };
                        style.Items.Add(itemInfo);
                    }
                }
                else
                {
                    var isHasSelected = false;
                    foreach (var styleItem in request.Items)
                    {
                        if (request.InputType != InputType.SelectMultiple && request.InputType != InputType.CheckBox && isHasSelected && styleItem.Selected)
                        {
                            return (false, "操作失败，只能有一个初始化时选定项！");
                        }
                        if (styleItem.Selected) isHasSelected = true;

                        var itemInfo = new InputStyleItem
                        {
                            Label = styleItem.Label,
                            Value = styleItem.Value,
                            Selected = styleItem.Selected
                        };
                        style.Items.Add(itemInfo);
                    }
                }
            }
            else if (request.InputType == InputType.TextArea || request.InputType == InputType.TextEditor)
            {
                style.Height = request.Height;
            }
            else if (request.InputType == InputType.Customize)
            {
                style.CustomizeCode = request.CustomizeCode;
            }
            style.Rules = new List<InputStyleRule>() { };
            if (style.Valid)
            {
                var validMsg = $"请输入{style.DisplayName}";
                if (style.InputType != InputType.Text)
                {
                    validMsg= $"请选择{style.DisplayName}";
                }
                style.Rules.Add(new InputStyleRule
                {
                    Message = validMsg,
                    Type = ValidateType.Required,
                });
            }
            await _tableStyleRepository.InsertAsync(relatedIdentities, style);

            return (true, string.Empty);
        }

        private async Task<(bool Success, string ErrorMessage)> UpdateTableStyleAsync(Models.TableStyle style, SubmitRequest request)
        {
            style.AttributeName = request.AttributeName;
            style.DisplayName = request.DisplayName;
            style.HelpText = request.HelpText;
            style.Taxis = request.Taxis;
            style.InputType = request.InputType;
            style.DefaultValue = request.DefaultValue;
            style.Horizontal = request.Horizontal;
            style.Locked = request.Locked;
            style.Valid = request.Valid;
            style.Items = new List<InputStyleItem>();

            if (request.InputType == InputType.CheckBox || request.InputType == InputType.Radio || request.InputType == InputType.SelectMultiple || request.InputType == InputType.SelectOne)
            {
                if (request.IsRapid)
                {
                    foreach (var rapidValue in ListUtils.GetStringListByReturnAndNewline(request.RapidValues))
                    {
                        var itemInfo = new InputStyleItem
                        {
                            Label = rapidValue,
                            Value = rapidValue,
                            Selected = false
                        };
                        style.Items.Add(itemInfo);
                    }
                }
                else
                {
                    var isHasSelected = false;
                    foreach (var styleItem in request.Items)
                    {
                        if (request.InputType != InputType.SelectMultiple && request.InputType != InputType.CheckBox && isHasSelected && styleItem.Selected)
                        {
                            return (false, "操作失败，只能有一个初始化时选定项！");
                        }
                        if (styleItem.Selected) isHasSelected = true;

                        var itemInfo = new InputStyleItem
                        {
                            Label = styleItem.Label,
                            Value = styleItem.Value,
                            Selected = styleItem.Selected
                        };
                        style.Items.Add(itemInfo);
                    }
                }
            }
            else if (request.InputType == InputType.TextArea || request.InputType == InputType.TextEditor)
            {
                style.Height = request.Height;
            }
            else if (request.InputType == InputType.Customize)
            {
                style.CustomizeCode = request.CustomizeCode;
            }
            style.Rules = new List<InputStyleRule>() { };
            if (style.Valid)
            {
                var validMsg = $"请输入{style.DisplayName}";
                if (style.InputType != InputType.Text)
                {
                    validMsg = $"请选择{style.DisplayName}";
                }
                style.Rules.Add(new InputStyleRule
                {
                    Message = validMsg,
                    Type = ValidateType.Required,
                });
            }
            await _tableStyleRepository.UpdateAsync(style);

            return (true, string.Empty);
        }
    }
}
