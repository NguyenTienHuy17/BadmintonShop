using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Purchase.Dtos;
using ERP.Dto;

namespace ERP.Purchase
{
    public interface IOrdersAppService : IApplicationService
    {
        Task<PagedResultDto<GetOrderForViewDto>> GetAll(GetAllOrdersInput input);

        Task<GetOrderForViewDto> GetOrderForView(long id);

        Task<GetOrderForEditOutput> GetOrderForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditOrderDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetOrdersToExcel(GetAllOrdersForExcelInput input);

        Task<PagedResultDto<OrderStatusLookupTableDto>> GetAllStatusForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<OrderDiscountLookupTableDto>> GetAllDiscountForLookupTable(GetAllForLookupTableInput input);

    }
}