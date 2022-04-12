using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Purchase.Dtos;
using ERP.Dto;
using System.Collections.Generic;

namespace ERP.Purchase
{
    public interface IOrderItemsAppService : IApplicationService
    {
        Task<PagedResultDto<GetOrderItemForViewDto>> GetAll(GetAllOrderItemsInput input);

        Task<GetOrderItemForViewDto> GetOrderItemForView(long id);

        Task<GetOrderItemForEditOutput> GetOrderItemForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditOrderItemDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetOrderItemsToExcel(GetAllOrderItemsForExcelInput input);

        Task<List<GetOrderItemForViewDto>> GetOrderItemByOrderId(long orderId);
    }
}