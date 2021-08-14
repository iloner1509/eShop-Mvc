using MediatR;

namespace eShop_Mvc.Core.Services.Query.BillQuery
{
    public class ExportBillToExcelFileQuery : IRequest<string>
    {
        public int BillId { get; set; }
    }
}