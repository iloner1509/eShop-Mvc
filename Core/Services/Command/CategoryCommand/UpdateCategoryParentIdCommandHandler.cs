using MediatR;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications;
using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Core.Services.Command.CategoryCommand
{
    public class UpdateCategoryParentIdCommandHandler : IRequestHandler<UpdateCategoryParentIdCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryParentIdCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateCategoryParentIdCommand request, CancellationToken cancellationToken)
        {
            var categoryRepo = _unitOfWork.Repository<ProductCategory, int>();
            var sourceCategory = await categoryRepo.FindByIdAsync(request.SourceId);
            sourceCategory.ParentId = request.TargetId;
            categoryRepo.Update(sourceCategory);

            var specification = new CategoryByListCategoryIdSpecification(request.SubCategoryData.Keys);
            var sibling = await categoryRepo.FindAllAsync(cancellationToken, specification);
            foreach (var item in sibling)
            {
                item.SortOrder = request.SubCategoryData[item.Id];
                categoryRepo.Update(item);
            }
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;
        }
    }
}