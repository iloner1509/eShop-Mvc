using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace eShop_Mvc.Core.Interfaces
{
    public interface IUploadService
    {
        string UploadImage();

        void UploadImageForCkEditor(IList<IFormFile> uploadFiles, string ckEditorFuncNum);
    }
}