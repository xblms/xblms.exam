using System;
using XBLMS.Core.Utils;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class PathManager
    {
        public void AddImageMarkForCertificateReviewAsync(string imagePath, string waterMarkImagePath, int x, int y, int width, int height)
        {
            try
            {
                var fileExtName = PathUtils.GetExtension(imagePath);
                var waterFileExtName = PathUtils.GetExtension(waterMarkImagePath);
                if (FileUtils.IsImage(fileExtName) && FileUtils.IsImage(waterFileExtName))
                {
                    var now = DateTime.Now;
                    ImageUtils.AddImageWaterMark(imagePath, waterMarkImagePath, x, y, 10, width, height);
                }
            }
            catch
            {
                // ignored
            }
        }
        public void AddWaterMarkForCertificateReviewAsync(string imagePath, string text, int fontSize, int x, int y)
        {
            try
            {
                var fileExtName = PathUtils.GetExtension(imagePath);
                if (FileUtils.IsImage(fileExtName))
                {
                    var now = DateTime.Now;
                    ImageUtils.AddTextWaterMark(imagePath, text, "SimHei", fontSize, x, y, 0, 0, 0);
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}
