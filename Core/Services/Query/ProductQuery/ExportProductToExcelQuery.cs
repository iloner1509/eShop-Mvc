using MediatR;
using Microsoft.AspNetCore.Http;

namespace eShop_Mvc.Core.Services.Query.ProductQuery
{
    public class ExportProductToExcelQuery : IRequest<string>
    {
        public string WebRootPath { get; set; }
        public HttpRequest HttpRequest { get; set; }
    }
}