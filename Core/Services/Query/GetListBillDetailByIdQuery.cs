using System.Collections.Generic;
using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Query
{
    public class GetListBillDetailByIdQuery : IRequest<IReadOnlyList<BillDetail>>
    {
        public int BillId { get; set; }
    }
}