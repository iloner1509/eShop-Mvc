using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.Core.Services.Command.BillCommand;
using eShop_Mvc.Core.Services.Query.BillQuery;
using eShop_Mvc.Models.Common;
using eShop_Mvc.Models.ProductViewModels;
using eShop_Mvc.SharedKernel.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShop_Mvc.Core.Specifications;

namespace eShop_Mvc.Areas.Admin.Controllers
{
    public class BillController : BaseController
    {
        private readonly IBillService _billService;
        private readonly IMapper _mapper;
        private readonly ILogger<BillController> _logger;
        private readonly IMediator _mediator;

        public BillController(IBillService billService, IMapper mapper, ILogger<BillController> logger, IMediator mediator)
        {
            _billService = billService;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int billId)
        {
            // normal approach
            //var model = await _billService.GetDetailAsync(billId);

            // using CQRS with mediator
            var model = await _mediator.Send(new GetBillWithDetailByIdQuery()
            {
                BillId = billId
            });
            return new OkObjectResult(_mapper.Map<Bill, BillViewModel>(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaging(PagingParams pagingParams)
        {
            //return new OkObjectResult(await _billService.GetAllPagingAsync(startDate, endDate, keyword, page, pageSize));
            return new OkObjectResult(await _mediator.Send(new GetAllBillPagingQuery()
            {
                PagingParams = pagingParams
            }));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int billId, BillStatus status)
        {
            //await _billService.UpdateStatusAsync(billId, status);
            await _mediator.Send(new UpdateBillStatusCommand()
            {
                BillId = billId,
                Status = status
            });
            _logger.LogInformation($"Order {billId} status had been changed to {status}!");
            return new OkResult();
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(BillViewModel billViewModel)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            if (billViewModel.Id == 0)
            {
                //await _billService.CreateAsync(_mapper.Map<BillViewModel, Bill>(billViewModel));
                await _mediator.Send(new CreateBillCommand()
                {
                    Bill = _mapper.Map<BillViewModel, Bill>(billViewModel)
                });
                _logger.LogInformation("New order is created successfully!");
            }
            else
            {
                //await _billService.UpdateAsync(_mapper.Map<BillViewModel, Bill>(billViewModel));
                await _mediator.Send(new UpdateBillCommand()
                {
                    Bill = _mapper.Map<BillViewModel, Bill>(billViewModel)
                });
                _logger.LogInformation($"order {billViewModel.Id} had been modified !");
            }
            //_billService.Save();
            return new OkObjectResult(billViewModel);
        }

        [HttpGet]
        public IActionResult GetBillStatus()
        {
            List<EnumModel> enums = ((BillStatus[])Enum.GetValues(typeof(BillStatus))).Select(c => new EnumModel()
            {
                Value = (int)c,
                Name = c.ToString()
            }).ToList();
            return new OkObjectResult(enums);
        }

        [HttpGet]
        public IActionResult GetPaymentMethod()
        {
            List<EnumModel> enums = ((PaymentMethod[])Enum.GetValues(typeof(PaymentMethod))).Select(c => new EnumModel()
            {
                Value = (int)c,
                Name = c.ToString()
            }).ToList();
            return new OkObjectResult(enums);
        }

        [HttpPost]
        public async Task<IActionResult> ExportExcel(int billId)
        {
            return new OkObjectResult(await _billService.ExportExcel(billId));
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}