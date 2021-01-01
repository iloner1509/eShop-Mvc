using System.ComponentModel.DataAnnotations;
using eShop_Mvc.Core.Enums;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.SharedKernel;

namespace eShop_Mvc.Core.Entities
{
    public class Page : BaseEntity<int>, ISwitchable
    {
        public Page()
        {
        }

        public Page(int id, string name, string alias, string content, Status status)
        {
            Id = id;
            Name = name;
            Alias = alias;
            Content = content;
            Status = status;
        }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        [StringLength(150)]
        public string Alias { get; set; }

        public string Content { get; set; }

        [Required]
        public Status Status { get; set; }
    }
}