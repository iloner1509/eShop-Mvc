using eShop_Mvc.SharedKernel.Enums;

namespace eShop_Mvc.Models.Common
{
    public class SlideViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public string Url { get; set; }

        public int? DisplayOrder { get; set; }

        public Status Status { get; set; }

        public string Content { get; set; }

        public string GroupAlias { get; set; }
    }
}