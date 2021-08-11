using eShop_Mvc.Core.Entities;

namespace eShop_Mvc.Core.Specifications
{
    public class TagByIdSpecification : BaseSpecification<Tag>
    {
        public TagByIdSpecification(string tagId) : base(x => x.Id == tagId)
        {
        }
    }
}