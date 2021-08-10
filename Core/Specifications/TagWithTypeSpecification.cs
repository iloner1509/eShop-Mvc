using eShop_Mvc.Core.Entities;

namespace eShop_Mvc.Core.Specifications
{
    public class TagWithTypeSpecification : BaseSpecification<Tag>
    {
        public TagWithTypeSpecification(string type) : base(x => x.Type == type)
        {
            AddOrderBy(x => x.Name);
        }
    }
}