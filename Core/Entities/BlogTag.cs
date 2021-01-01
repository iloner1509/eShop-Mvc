using eShop_Mvc.SharedKernel;

namespace eShop_Mvc.Core.Entities
{
    public class BlogTag : BaseEntity<int>
    {
        public int BlogId { get; set; }
        public string TagId { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual Tag Tag { get; set; }
    }
}