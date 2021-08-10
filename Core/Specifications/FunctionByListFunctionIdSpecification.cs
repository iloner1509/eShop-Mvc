using System.Collections.Generic;
using System.Linq;
using eShop_Mvc.Core.Entities;

namespace eShop_Mvc.Core.Specifications
{
    public class FunctionByListFunctionIdSpecification : BaseSpecification<Function>
    {
        public FunctionByListFunctionIdSpecification(IEnumerable<string> functionId) : base(x => functionId.Contains(x.Id))
        {
        }
    }
}