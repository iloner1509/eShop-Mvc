using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Command.BillCommand
{
    public class CreateBillCommand : IRequest
    {
        public Bill Bill { get; set; }
    }
}