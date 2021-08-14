using System.Reflection.Metadata.Ecma335;
using MediatR;

namespace eShop_Mvc.Core.Services.Command.ProductCommand
{
    public class ImportProductByExcelFileCommand : IRequest
    {
        public string FilePath { get; set; }
        public int CategoryId { get; set; }
    }
}