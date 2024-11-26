using HtmlAgilityPack;
using OpenXmlPowerTools;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using XBLMS.Core.Utils.Office.Word2Html;
using XBLMS.Utils;
using System.Linq;

namespace XBLMS.Core.Utils.Office
{
    public class WordManager
    {
        private string ImageDirectoryPath { get; set; }
        private string ImageDirectoryUrl { get; set; }
        private string DocsFilePath { get; set; }

        public WordManager(string docsFilePath, string imageDirectoryPath, string imageDirectoryUrl)
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
                ImageUtils.ResizeImageIfExceeding(imageFilePath, 100);

                var imgSrc = PageUtils.Combine(ImageDirectoryUrl, imageFileName);

                return imgSrc;
            }
            catch
            {

            }

            return $"data:{picType};base64,{Convert.ToBase64String(imgByte)}";
        }

        public static string HtmlToWord(string fileHtmlPath, string fileWordPath)
        {
            var sourceHtmlFi = new FileInfo(fileHtmlPath);
            var destDocxFi = new FileInfo(fileWordPath);

            var html = ReadAsXElement(sourceHtmlFi);

            var usedAuthorCss = HtmlToWmlConverter.CleanUpCss((string)html.Descendants().FirstOrDefault(d => StringUtils.ToLower(d.Name.LocalName) == "style"));

            var settings = HtmlToWmlConverter.GetDefaultSettings();

            settings.BaseUriForImages = destDocxFi.DirectoryName;
            settings.DefaultFontSize = 10.5d;

            var doc = HtmlToWmlConverter.ConvertHtmlToWml(DefaultCss, usedAuthorCss, UserCss, html, settings, null, null);
            doc.SaveAs(destDocxFi.FullName);
            return fileWordPath;
        }

        private static XElement ReadAsXElement(FileInfo sourceHtmlFi)
        {
            var htmlString = File.ReadAllText(sourceHtmlFi.FullName);
            var hdoc = new HtmlDocument();
            hdoc.LoadHtml(htmlString);
            hdoc.OptionOutputAsXml = true;
            var sbxml = new StringBuilder();
            using (var writer = new StringWriter(sbxml))
            {
                hdoc.Save(writer);
            }
            XElement html;
            try
            {
                html = XElement.Parse(hdoc.DocumentNode.OuterHtml);
            }
            catch (XmlException)
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.Load(sourceHtmlFi.FullName, Encoding.Default);
                htmlDoc.OptionOutputAsXml = true;
                htmlDoc.Save(sourceHtmlFi.FullName, Encoding.Default);
                var sb = new StringBuilder(File.ReadAllText(sourceHtmlFi.FullName, Encoding.Default));
                sb.Replace("&amp;", "&");
                sb.Replace("&nbsp;", "\xA0");
                sb.Replace("&quot;", "\"");
                sb.Replace("&lt;", "~lt;");
                sb.Replace("&gt;", "~gt;");
                sb.Replace("&#", "~#");
                sb.Replace("&", "&amp;");
                sb.Replace("~lt;", "&lt;");
                sb.Replace("~gt;", "&gt;");
                sb.Replace("~#", "&#");
                File.WriteAllText(sourceHtmlFi.FullName, sb.ToString(), Encoding.Default);
                html = XElement.Parse(sb.ToString());
            }
            // HtmlToWmlConverter expects the HTML elements to be in no namespace, so convert all elements to no namespace.
            html = (XElement)ConvertToNoNamespace(html);
            return html;
        }

        private static object ConvertToNoNamespace(XNode node)
        {
            var element = node as XElement;
            if (element != null)
            {
                return new XElement(element.Name.LocalName,
                    element.Attributes().Where(a => !a.IsNamespaceDeclaration),
                    element.Nodes().Select(n => ConvertToNoNamespace(n)));
            }
            return node;
        }

        private const string DefaultCss =
            @"html, address,
blockquote,
body, dd, div,
dl, dt, fieldset, form,
frame, frameset,
h1, h2, h3, h4,
h5, h6, noframes,
ol, p, ul, center,
dir, hr, menu, pre { display: block; unicode-bidi: embed }
li { display: list-item }
head { display: none }
table { display: table }
tr { display: table-row }
thead { display: table-header-group }
tbody { display: table-row-group }
tfoot { display: table-footer-group }
col { display: table-column }
colgroup { display: table-column-group }
td, th { display: table-cell }
caption { display: table-caption }
th { font-weight: bolder; text-align: center }
caption { text-align: center }
body { margin: auto; }
h1 { font-size: 2em; margin: auto; }
h2 { font-size: 1.5em; margin: auto; }
h3 { font-size: 1.17em; margin: auto; }
h4, p,
blockquote, ul,
fieldset, form,
ol, dl, dir,
menu { margin: auto }
a { color: blue; }
h5 { font-size: .83em; margin: auto }
h6 { font-size: .75em; margin: auto }
h1, h2, h3, h4,
h5, h6, b,
strong { font-weight: bolder }
blockquote { margin-left: 40px; margin-right: 40px }
i, cite, em,
var, address { font-style: italic }
pre, tt, code,
kbd, samp { font-family: monospace }
pre { white-space: pre }
button, textarea,
input, select { display: inline-block }
big { font-size: 1.17em }
small, sub, sup { font-size: .83em }
sub { vertical-align: sub }
sup { vertical-align: super }
table { border-spacing: 2px; }
thead, tbody,
tfoot { vertical-align: middle }
td, th, tr { vertical-align: inherit }
s, strike, del { text-decoration: line-through }
hr { border: 1px inset }
ol, ul, dir,
menu, dd { margin-left: 40px }
ol { list-style-type: decimal }
ol ul, ul ol,
ul ul, ol ol { margin-top: 0; margin-bottom: 0 }
u, ins { text-decoration: underline }
br:before { content: ""\A""; white-space: pre-line }
center { text-align: center }
:link, :visited { text-decoration: underline }
:focus { outline: thin dotted invert }
/* Begin bidirectionality settings (do not change) */
BDO[DIR=""ltr""] { direction: ltr; unicode-bidi: bidi-override }
BDO[DIR=""rtl""] { direction: rtl; unicode-bidi: bidi-override }
*[DIR=""ltr""] { direction: ltr; unicode-bidi: embed }
*[DIR=""rtl""] { direction: rtl; unicode-bidi: embed }
";

        private const string UserCss = @"";

    }
}
