using eShop_Mvc.Core.Entities;

namespace eShop_Mvc.Core.Specifications
{
    public class BillDetailsIncludeProductByBillIdSpecification : BaseSpecification<BillDetail>
    {
        public BillDetailsIncludeProductByBillIdSpecification(int billId) : base(x => x.BillId == billId)
        {
            AddInclude(x => x.Product);
        }
    }
}