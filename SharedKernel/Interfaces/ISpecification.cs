﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace eShop_Mvc.SharedKernel.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDesc { get; }
    }
}