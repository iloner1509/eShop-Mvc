using System.Collections.Generic;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;

namespace eShop_Mvc.Core.Interfaces
{
    public interface IBillService
    {
        Task CreateAsync(Bill bill);

        Task UpdateAsync(Bill bill);

        Task<Bill> GetDetailAsync(int billId);

        Task<BillDetail> CreateBillDetailAsync(BillDetail billDetail);

        Task DeleteDetailAsync(int productId, int billId);

        Task UpdateStatusAsync(int orderId, BillStatus status);

        Task<IReadOnlyList<BillDetail>> GetListBillDetailAsync(int billId);

        Task<PagedResult<Bill>> GetAllPagingAsync(string startDate, string endDate, string keyword, int page, int pageSize);

        Task<string> ExportExcel(int billId);

        void Save();
    }
}