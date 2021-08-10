using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications;
using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Services.Query.AnnouncementQuery
{
    public class GetAllAnnouncementQueryHandler : IRequestHandler<GetAllAnnouncementQuery, PagedResult<Announcement>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllAnnouncementQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<Announcement>> Handle(GetAllAnnouncementQuery request, CancellationToken cancellationToken)
        {
            var specification = new AnnouncementWithFilterSpecification(request.PagingParams);
            var totalRow = await _unitOfWork.Repository<Announcement, string>().CountAsync(cancellationToken, specification);
            var data = await _unitOfWork.Repository<Announcement, string>().FindAllAsync(cancellationToken, specification);

            return new PagedResult<Announcement>()
            {
                CurrentPage = request.PagingParams.PageIndex,
                RowCount = totalRow,
                Results = data,
                PageSize = request.PagingParams.PageSize,
                PageCount = (totalRow / request.PagingParams.PageSize) + 1
            };
        }
    }
}