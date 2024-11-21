using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NPOI.XWPF.UserModel;

namespace XBLMS.Core.Utils.Office.Word2Html
{
    public class PicturesConvert
    {
        public event Func<byte[], string, string> uploadImgUrlyDelegate;

        private string OnUploadImgUrl(byte[] imgByte, string PicType)
        {
            if (uploadImgUrlyDelegate != null) return uploadImgUrlyDelegate(imgByte, PicType);

            return $"data:{PicType};base64,{Convert.ToBase64String(imgByte)}";
        }

        /// <summary>
        ///     图片处理
        /// </summary>
        /// <param name="myDocx"></param>
        /// <returns></returns>
        public async Task<List<PicInfo>> PicturesHandleAsync(XWPFDocument myDocx)
        {
            var picInfoList = new List<PicInfo>();
            var picturesList = myDocx.AllPictures;
            foreach (var pictures in picturesList)
            {
                var pData = pictures.Data;
                var picPackagePart = pictures.GetPackagePart();

#pragma warning disable
                var picPackageRelationship = pictures.GetPackageRelationship();
#pragma warning restore

                var picInfo = new PicInfo
                {
                    Id = picPackageRelationship.Id,
                    PicType = picPackagePart.ContentType
                };


                try
                {
                    picInfo.Url = OnUploadImgUrl(pData, picInfo.PicType);
                }
                catch (Exception)
                {
                    // ignored
                }

                if (string.IsNullOrWhiteSpace(picInfo.Url))
                    picInfo.Url = $"data:{picInfo.PicType};base64,{Convert.ToBase64String(pData)}";

                picInfoList.Add(picInfo);
            }

            return await Task.FromResult(picInfoList);
        }
    }
}
