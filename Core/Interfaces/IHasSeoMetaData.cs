﻿namespace eShop_Mvc.Core.Interfaces
{
    public interface IHasSeoMetaData
    {
        string SeoTitle { get; set; }
        string SeoAlias { get; set; }
        string SeoKeywords { get; set; }
        string SeoDescription { get; set; }
    }
}