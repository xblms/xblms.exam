using Aspose.Pdf;
using Aspose.Pdf.Devices;
using XBLMS.Core.Utils;

namespace XLMS.Core.Utils.Office
{
    public static class AsposePdfObject
    {
        public static int GetFirstImg(string pdfPath, string imagePath)
        {
            try
            {
                Document pdfDocument = new Document(pdfPath);

                // ����һ��ͼƬ��ȡ��
                JpegDevice jpg = new JpegDevice(600, 800);
                jpg.Process(pdfDocument.Pages[1], imagePath);

                ImageUtils.Crop(imagePath, 100, 100, 450, 650);

                return pdfDocument.Pages.Count;

            }
            catch
            {
                imagePath = "";
                return 0;
            }

        }
    }
}
