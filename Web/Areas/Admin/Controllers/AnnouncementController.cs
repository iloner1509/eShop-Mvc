using System.Threading.Tasks;
using AutoMapper;
using eShop_Mvc.Core.Services.Query.AnnouncementQuery;
using eShop_Mvc.Core.Specifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace eShop_Mvc.Areas.Admin.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public AnnouncementController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaging(PagingParams pagingParams)
        {
            return new OkObjectResult(await _mediator.Send(new GetAllAnnouncementQuery()
            {
                PagingParams = pagingParams
            }));
        }
    }
}