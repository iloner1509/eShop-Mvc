using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications;
using eShop_Mvc.Core.Specifications.AnnouncementSpecification;
using eShop_Mvc.SharedKernel;
using MediatR;

namespace eShop_Mvc.Core.Services.Query.AnnouncementQuery
{
    public class GetAllAnnouncementPagingQuery : IRequest<PagedResult<Announcement>>
    {
        public AnnouncementPagingParams PagingParams { get; set; }
    }
}