using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Database
{
    public partial class BackupController
    {
        [HttpPost, Route(RouteBackupUpload)]
        public async Task<ActionResult<BoolResult>> Upload([FromForm] GetUploadFormRequest request, [FromForm] IFormFile file)
        {
            if (_settingsManager.SecurityKey != request.SecurityKey)
            {
                return this.Error("SecurityKey 输入错误！");
            }

            var admin = await _authManager.GetAdminAsync();

            var fileName = request.Name;
            //当前分块序号
            var index = request.Chunk;
            //所有块数
            var maxChunk = request.MaxChunk;
            var guid = request.Guid;
            //临时保存分块的目录
            var dir = PathUtils.Combine(_settingsManager.WebRootPath, "sitefiles", "dbbackup", "upload", guid);
            DirectoryUtils.CreateDirectoryIfNotExists(dir);
            //分块文件名为索引名，更严谨一些可以加上是否存在的判断，防止多线程时并发冲突
            var filePath = PathUtils.Combine(dir, index.ToString());
            var filePathWithFileName = string.Concat(filePath, fileName);
            using (var stream = new FileStream(filePathWithFileName, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            bool completed = false;
            string saveName = string.Empty;
            string tempPath = string.Empty;

            if (index == maxChunk)
            {
                //获取文件格式
                var fileFormat = PathUtils.GetExtension(fileName);
                var yearMonth = DateTime.Now.ToString("yyyyMMdd");
                //最终保存路径
                var finalDir = PathUtils.Combine(_settingsManager.WebRootPath, "sitefiles", "dbbackup", "upload", yearMonth);
                DirectoryUtils.CreateDirectoryIfNotExists(finalDir);

                //随机生成一个文件名
                var trustedFileNameForFileStorage = Path.GetRandomFileName();
                saveName = yearMonth + "_" + trustedFileNameForFileStorage + fileFormat;
                var finalPath = PathUtils.Combine(finalDir, saveName);
                await MergeFileAsync(dir, finalPath, guid);

                var backuppathdate = $"{DateTime.Now:yyyy-MM-dd-hh-mm-ss}";
                _pathManager.ExtractZip(finalPath, PathUtils.Combine(_settingsManager.WebRootPath, "sitefiles", "dbbackup", backuppathdate));
                await _dbBackupRepository.InsertAsync(new DbBackup
                {
                    CompanyId = admin.CompanyId,
                    CreatorId = admin.Id,
                    Status = 1,
                    BeginTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    FilePath = $"sitefiles/dbbackup/{backuppathdate}",
                    DataSize = FileUtils.GetFileSizeByFilePath(finalPath),
                    ErrorLog = "导入备份"
                });

                DirectoryUtils.DeleteDirectoryIfExists(finalDir);
                completed = true;
            }

            return new BoolResult { Value = completed };
        }
        public static async Task MergeFileAsync(string chunkpath, string fileName, string guid)
        {
            //临时文件夹
            var dir = chunkpath;
            //获得下面的所有文件
            var files = Directory.GetFiles(dir);


            var finalPath = fileName;//Path.Combine(finalDir, saveName);
            using (var fs = new FileStream(finalPath, FileMode.Create))
            {
                //排一下序，保证从0-N Write
                var fileParts = files.OrderBy(x => x.Length).ThenBy(x => x);
                foreach (var part in fileParts)
                {
                    var bytes = await System.IO.File.ReadAllBytesAsync(part);
                    await fs.WriteAsync(bytes, 0, bytes.Length);
                    bytes = null;
                    //删除分块
                    System.IO.File.Delete(part);
                }
                await fs.FlushAsync();
                fs.Close();
                //删除临时文件夹和分片文件
                Directory.Delete(dir);
            }
        }
    }
}
