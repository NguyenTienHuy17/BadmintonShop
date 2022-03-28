using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Common.Dtos;
using ERP.Dto;

namespace ERP.Common
{
    public interface ISizeItemsAppService : IApplicationService
    {
        Task<PagedResultDto<GetSizeItemForViewDto>> GetAll(GetAllSizeItemsInput input);

        Task<GetSizeItemForViewDto> GetSizeItemForView(long id);

        Task<GetSizeItemForEditOutput> GetSizeItemForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditSizeItemDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetSizeItemsToExcel(GetAllSizeItemsForExcelInput input);

        Task<PagedResultDto<SizeItemProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<SizeItemSizeLookupTableDto>> GetAllSizeForLookupTable(GetAllForLookupTableInput input);

    }
}