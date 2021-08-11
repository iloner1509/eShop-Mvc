using System.Collections.Generic;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications;
using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eShop_Mvc.Core.Services.Command.ProductCommand
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var tagRepository = _unitOfWork.Repository<Tag, string>();
            var productTagRepo = _unitOfWork.Repository<ProductTag, int>();

            if (!string.IsNullOrEmpty(request.Product.Tags))
            {
                string[] tags = request.Product.Tags.Split(",");

                // delete old product-tag
                var productTagSpecification = new ProductTagByProductIdSpecification(request.Product.Id);
                productTagRepo.DeleteRange(await productTagRepo.FindAllAsync(cancellationToken, productTagSpecification));
                foreach (var tag in tags)
                {
                    var tagId = TextHelper.ToUnsignString(tag);

                    var tagSpecification = new TagByIdSpecification(tagId);
                    if (!await tagRepository.ApplySpecification(tagSpecification).AnyAsync(cancellationToken))
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
            _unitOfWork.Repository<Product, int>().Update(request.Product);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;
        }
    }
}