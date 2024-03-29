﻿using System.Collections.Generic;
using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.BillQuery
{
    public class GetListBillDetailByBillIdQuery : IRequest<IReadOnlyList<BillDetail>>
    {
        public int BillId { get; set; }
    }
}