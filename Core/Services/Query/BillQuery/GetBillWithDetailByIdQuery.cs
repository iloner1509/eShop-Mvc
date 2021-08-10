using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.BillQuery
{
    public class GetBillWithDetailByIdQuery : IRequest<Bill>
    {
        public int BillId { get; set; }
    }
}