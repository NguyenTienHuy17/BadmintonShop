using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Purchase.Dtos;
using ERP.Dto;

namespace ERP.Purchase
{
    public interface IReturnProdsAppService : IApplicationService
    {
        Task<PagedResultDto<GetReturnProdForViewDto>> GetAll(GetAllReturnProdsInput input);

        Task<GetReturnProdForViewDto> GetReturnProdForView(long id);

        Task<GetReturnProdForEditOutput> GetReturnProdForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditReturnProdDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetReturnProdsToExcel(GetAllReturnProdsForExcelInput input);

        Task<PagedResultDto<ReturnProdOrderLookupTableDto>> GetAllOrderForLookupTable(GetAllForLookupTableInput input);

    }
}