using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Services.Command.CategoryCommand
{
    public class UpdateCategoryOrderCommandHandler : IRequestHandler<UpdateCategoryOrderCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateCategoryOrderCommand request, CancellationToken cancellationToken)
        {
            var categoryRepo = _unitOfWork.Repository<ProductCategory, int>();
            var source = await categoryRepo.FindByIdAsync(request.SourceId);
            var target = await categoryRepo.FindByIdAsync(request.TargetId);

            // Swap
            int tempOrder = source.SortOrder;
            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            categoryRepo.Update(source);
            categoryRepo.Update(target);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;
        }
    }
}