﻿using eShop_Mvc.SharedKernel.Enums;
using MediatR;

namespace eShop_Mvc.Core.Services.Command.BillCommand
{
    public class UpdateBillStatusCommand : IRequest
    {
        public int BillId { get; set; }
        public BillStatus Status { get; set; }
    }
}