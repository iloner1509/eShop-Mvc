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
            new SelectListItem(){Value = "latest",Text = "Mới nhất"},
            new SelectListItem(){Value = "price",Text = "Giá"},
            new SelectListItem(){Value = "name",Text = "Tên"}
        };

        public List<SelectListItem> PageSizes { get; } = new List<SelectListItem>()
        {
            new SelectListItem(){Value = "12",Text = "12"},
            new SelectListItem(){Value = "16",Text = "16"},
            new SelectListItem(){Value = "30",Text = "30"}
        };
    }
}