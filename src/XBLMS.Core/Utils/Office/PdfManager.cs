using Aspose.Pdf;
using Aspose.Pdf.Devices;

namespace XBLMS.Core.Utils.Office
{
    public class PdfManager
    {
        public static void GetFirstImg(string pdfPath, string imagePath)
        {
            Document pdfDocument = new Document(pdfPath);

            // 创建一个图片提取器

            JpegDevice jpg = new JpegDevice(600, 800);
            jpg.Process(pdfDocument.Pages[1], imagePath);

            ImageUtils.Crop(imagePath, 100, 100, 450, 650);
        }
    }
}
