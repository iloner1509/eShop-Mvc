using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Enums;

namespace eShop_Mvc.Core.Specifications.FunctionSpecification
{
    public class FunctionWithFilterSpecification : BaseSpecification<Function>
    {
        public FunctionWithFilterSpecification(string keyword) : base(x => x.Status == Status.Active && (string.IsNullOrEmpty(keyword) || x.Name.ToLower().Contains(keyword)))
        {
            AddOrderBy(x => x.ParentId);
        }
    }
}