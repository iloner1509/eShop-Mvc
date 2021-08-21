using System.Collections.Generic;
using eShop_Mvc.SharedKernel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eShop_Mvc.Models.ProductViewModels
{
    public class CatalogViewModel
    {
        public PagedResult<ProductViewModel> Data { get; set; }
        public ProductCategoryViewModel Category { get; set; }
        public string SortType { get; set; }
        public int? PageSize { get; set; }

        public List<SelectListItem> SortTypes { get; } = new List<SelectListItem>()
        {
            new SelectListItem(){Value = "",Text = "Mới nhất"},
            new SelectListItem(){Value = $"{SharedKernel.Enums.SortTypes.DateCreatedDesc}",Text = "Cũ nhất"},
            new SelectListItem(){Value = $"{SharedKernel.Enums.SortTypes.PriceAsc}",Text = "Giá từ thấp lên cao"},
            new SelectListItem(){Value = $"{SharedKernel.Enums.SortTypes.PriceDesc}",Text = "Giá từ cao xuống thấp"},
            new SelectListItem(){Value = $"{SharedKernel.Enums.SortTypes.NameAsc}",Text = "Tên A-Z"},
            new SelectListItem(){Value = $"{SharedKernel.Enums.SortTypes.NameDesc}",Text = "Tên Z-A"}
        };

        public List<SelectListItem> PageSizes { get; } = new List<SelectListItem>()
        {
            new SelectListItem(){Value = "12",Text = "12"},
            new SelectListItem(){Value = "16",Text = "16"},
            new SelectListItem(){Value = "30",Text = "30"}
        };

        public List<string> ListCategory { get; set; }
        public List<string> ListTag { get; set; }
    }
}