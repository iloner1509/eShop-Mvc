using System.Collections.Generic;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;

namespace eShop_Mvc.Core.Interfaces
{
    public interface IBillService
    {
        Task<string> ExportExcel(int billId);
    }
}