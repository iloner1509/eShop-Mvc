using System.ComponentModel.DataAnnotations;
using eShop_Mvc.Core.Enums;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.SharedKernel;

namespace eShop_Mvc.Core.Entities
{
    public class Function : BaseEntity<string>, ISwitchable, ISortable
    {
        public Function()
        {
        }

        public Function(string name, string url, string parentId, string iconCss, int sortOrder)
        {
            Name = name;
            URL = url;
            ParentId = parentId;
            IconCss = iconCss;
            Status = Status.Active;
            SortOrder = sortOrder;
        }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        [StringLength(150)]
        public string URL { get; set; }

        public string ParentId { get; set; }
        public string IconCss { get; set; }

        [Required]
        public Status Status { get; set; }

        public int SortOrder { get; set; }
    }
}