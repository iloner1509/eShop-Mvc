using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications.TagSpecification;
using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Services.Command.ProductCommand
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var tagRepository = _unitOfWork.Repository<Tag, string>();
            var productTagRepo = _unitOfWork.Repository<ProductTag, int>();
            if (!string.IsNullOrEmpty(request.Product.Tags))
            {
                string[] tags = request.Product.Tags.Split(",");
                foreach (var tag in tags)
                {
                    var tagId = TextHelper.ToUnsignString(tag);

                    var specification = new TagByIdSpecification(tagId);
                    if (!await tagRepository.ApplySpecification(specification).AnyAsync(cancellationToken))
                    {
                        Tag t = new Tag
                        {
                            Id = tagId,
                            Name = tag,
                            Type = "Product"
                        };
                        await tagRepository.AddAsync(t, cancellationToken);
                    }
                    ProductTag productTag = new ProductTag
                    {
                        ProductId = request.Product.Id,
                        TagId = tagId
                    };
                    await productTagRepo.AddAsync(productTag, cancellationToken);
                }
            }

            await _unitOfWork.Repository<Product, int>().AddAsync(request.Product, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;
        }
    }
}