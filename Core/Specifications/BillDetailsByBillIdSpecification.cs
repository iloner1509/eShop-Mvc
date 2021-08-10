using eShop_Mvc.Core.Entities;

namespace eShop_Mvc.Core.Specifications
{
    public class BillDetailsByBillIdSpecification : BaseSpecification<BillDetail>
    {
        public BillDetailsByBillIdSpecification(int billId) : base(x => x.BillId == billId)
        {
        }
    }
}