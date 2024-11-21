using System;
using System.IO;
using System.Threading.Tasks;
using XBLMS.Core.Utils.Office.Word2Html;
using XBLMS.Utils;

namespace XBLMS.Core.Utils.Office
{
    public class WordManager
    {
        private string ImageDirectoryPath { get; set; }
        private string ImageDirectoryUrl { get; set; }
        private string DocsFilePath { get; set; }

        public WordManager(string docsFilePath,string imageDirectoryPath,string imageDirectoryUrl)
        {
            DocsFilePath = docsFilePath;
            ImageDirectoryPath = imageDirectoryPath;
            ImageDirectoryUrl = imageDirectoryUrl;
        }


        public async Task<string> ParseAsync()
        {
            var wordContent = string.Empty;
            try
            {
                wordContent = await ConvertToHtmlAsync();

            }
            catch
            {

            }

            XBLMS.Utils.FileUtils.DeleteFileIfExists(DocsFilePath);

            return wordContent;
        }

        private async Task<string> ConvertToHtmlAsync()
        {
            FileStream stream = new FileStream(DocsFilePath, FileMode.Open, FileAccess.Read);
            var npoiDoc = new NpoiDoc();
            return await npoiDoc.NpoiDocx(stream, UploadImageUrlDelegate);
        }

        private string UploadImageUrlDelegate(byte[] imgByte, string picType)
        {
            var extension = StringUtils.ToLower(picType.Split('/')[1]);
            var imageFileName = StringUtils.GetShortGuid(false) + "." + extension;

            var imageFilePath = PathUtils.Combine(ImageDirectoryPath, imageFileName);
            try
            {
                ImageUtils.Save(imgByte, imageFilePath);
                ImageUtils.ResizeImageIfExceeding(imageFilePath,100);

                var imgSrc = PageUtils.Combine(ImageDirectoryUrl, imageFileName);

                return imgSrc;
            }
            catch
            {

            }

            return $"data:{picType};base64,{Convert.ToBase64String(imgByte)}";
        }

        private string ConvertToHtml()
        {
            return string.Empty;

            //var fi = new FileInfo(DocsFilePath);

            //var byteArray = File.ReadAllBytes(fi.FullName);
            //using (var memoryStream = new MemoryStream())
            //{
            //    memoryStream.Write(byteArray, 0, byteArray.Length);

            //    using (var wDoc = WordprocessingDocument.Open(memoryStream, true))
            //    {
            //        var htmlSettings = new HtmlConverterSettings
            //        {

            //            FabricateCssClasses = true,
            //            CssClassPrefix = "pt-",
            //            RestrictToSupportedLanguages = false,
            //            RestrictToSupportedNumberingFormats = false,
            //            ImageHandler = imageInfo =>
            //            {
            //                DirectoryUtils.CreateDirectoryIfNotExists(ImageDirectoryPath);

            //                var extension = StringUtils.ToLower(imageInfo.ContentType.Split('/')[1]);
            //                ImageFormat imageFormat = null;
            //                if (extension == "png")
            //                    imageFormat = ImageFormat.Png;
            //                else if (extension == "gif")
            //                    imageFormat = ImageFormat.Gif;
            //                else if (extension == "bmp")
            //                    imageFormat = ImageFormat.Bmp;
            //                else if (extension == "jpeg")
            //                    imageFormat = ImageFormat.Jpeg;
            //                else if (extension == "tiff")
            //                {
            //                    // Convert tiff to gif.
            //                    extension = "gif";
            //                    imageFormat = ImageFormat.Gif;
            //                }
            //                else if (extension == "x-wmf")
            //                {
            //                    extension = "wmf";
            //                    imageFormat = ImageFormat.Wmf;
            //                }

            //                // If the image format isn't one that we expect, ignore it,
            //                // and don't return markup for the link.
            //                if (imageFormat == null)
            //                    return null;

            //                var imageFileName = StringUtils.GetShortGuid(false) + "." + extension;

            //                var imageFilePath = PathUtils.Combine(ImageDirectoryPath, imageFileName);
            //                try
            //                {
            //                    imageInfo.Bitmap.Save(imageFilePath, imageFormat);

            //                    ImageUtils.ResizeImageIfExceeding(imageFilePath, 100);
            //                }
            //                catch (System.Runtime.InteropServices.ExternalException)
            //                {
            //                    return null;
            //                }
            //                var imageSource = PageUtils.Combine(ImageDirectoryUrl, imageFileName);

            //                var img = new XElement(
            //                  Xhtml.img,
            //                  new XAttribute(NoNamespace.src, imageSource),
            //                  // imageInfo.ImgStyleAttribute,
            //                  imageInfo.AltText != null ? new XAttribute(NoNamespace.alt, imageInfo.AltText) : null
            //                );
            //                return img;
            //            }
            //        };
            //        var htmlElement = HtmlConverter.ConvertToHtml(wDoc, htmlSettings);

            //        // Produce HTML document with <!DOCTYPE html > declaration to tell the browser
            //        // we are using HTML5.
            //        var html = new XDocument(
            //            new XDocumentType("html", null, null, null),
            //            htmlElement);

            //        // Note: the xhtml returned by ConvertToHtmlTransform contains objects of type
            //        // XEntity.  PtOpenXmlUtil.cs define the XEntity class.  See
            //        // http://blogs.msdn.com/ericwhite/archive/2010/01/21/writing-entity-references-using-linq-to-xml.aspx
            //        // for detailed explanation.
            //        //
            //        // If you further transform the XML tree returned by ConvertToHtmlTransform, you
            //        // must do it correctly, or entities will not be serialized properly.

            //        var htmlString = html.ToString(SaveOptions.DisableFormatting);
            //        var htmlDoc = new HtmlDocument();
            //        htmlDoc.LoadHtml(htmlString);
            //        var body = htmlDoc.DocumentNode.SelectSingleNode("//body").InnerHtml;

            //        return body;
            //    }
            //}
        }
    }
}
