﻿using eShop_Mvc.Core.Entities;
using MediatR;

namespace eShop_Mvc.Core.Services.Command.ProductCommand
{
    public class CreateProductCommand : IRequest
    {
        public Product Product { get; set; }
    }
}