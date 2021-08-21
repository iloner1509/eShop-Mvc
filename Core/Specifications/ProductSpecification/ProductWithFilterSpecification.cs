using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Enums;

namespace eShop_Mvc.Core.Specifications.ProductSpecification
{
    public class ProductWithFilterSpecification : BaseSpecification<Product>
    {
        public ProductWithFilterSpecification(ProductPagingParams pagingParams) : base(x => (string.IsNullOrEmpty(pagingParams.SearchKeyword)
                 || x.Name.ToLower().Contains(pagingParams.SearchKeyword) || x.Description.ToLower().Contains(pagingParams.SearchKeyword))
                 && x.Status == Status.Active && (pagingParams.CategoryId.HasValue == false || x.CategoryId == pagingParams.CategoryId.Value))
        {
            AddInclude(x => x.ProductCategory);
            AddOrderByDesc(x => x.DateCreated);
            ApplyPaging(pagingParams.PageSize, pagingParams.PageSize * (pagingParams.PageIndex - 1));
            switch (pagingParams.Sort)
            {
                case SortTypes.PriceAsc:
                    AddOrderBy(x => x.Price > x.PromotionPrice ? x.PromotionPrice : x.Price);
                    break;

                case SortTypes.PriceDesc:
                    AddOrderByDesc(x => x.Price > x.PromotionPrice ? x.PromotionPrice : x.Price);
                    break;

                case SortTypes.NameAsc:
                    AddOrderBy(x => x.Name);
                    break;

                case SortTypes.NameDesc:
                    AddOrderByDesc(x => x.Name);
                    break;

                case SortTypes.DateCreatedDesc:
                    AddOrderByDesc(x => x.DateCreated);
                    break;

                default:
                    AddOrderBy(x => x.DateCreated);
                    break;
            }
        }
    }
}