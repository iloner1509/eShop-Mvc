using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShop_Mvc.Areas.Admin.Controllers
{
    public class UploadController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UploadController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public IActionResult UploadImage()
        {
            ;
            var files = Request.Form.Files;
            if (files.Count == 0)
            {
                return new BadRequestObjectResult(files);
            }

            var file = files[0];
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var imgFolder = $@"\uploaded\img";
            string folder = _hostingEnvironment.WebRootPath + imgFolder;
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string filePath = Path.Combine(folder, fileName);
            using FileStream fs = System.IO.File.Create(filePath);
            file.CopyTo(fs);
            fs.Flush();
            return new OkObjectResult(Path.Combine(imgFolder, fileName).Replace(@"\", @"/"));
        }

        //[HttpPost]
        //public async Task UploadImageForCkEditor(IList<IFormFile> uploadFiles, string CKEditorFuncNum, string CKEditor,
        //    string langCode)
        //{
        //    if (uploadFiles.Count == 0)
        //    {
        //        await HttpContext.Response.WriteAsync("Nhập ảnh");
        //    }
        //    else
        //    {
        //        var file = uploadFiles[0];
        //        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        //        var imgFolder = $@"\uploaded\img\contentImg";
        //        string folder = _hostingEnvironment.WebRootPath + imgFolder;
        //        if (!Directory.Exists(folder))
        //        {
        //            Directory.CreateDirectory(folder);
        //        }

        //        string filePath = Path.Combine(folder, fileName);
        //        await using FileStream fs = System.IO.File.Create(filePath);
        //        await file.CopyToAsync(fs);
        //        fs.Flush();
        //        await HttpContext.Response.WriteAsync("<script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", '" + Path.Combine(imgFolder, fileName).Replace(@"\", @"/") + "');</script>");
        //    }
        //}
    }
}