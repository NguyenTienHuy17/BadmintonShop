using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Common.Dtos;
using ERP.Dto;

namespace ERP.Common
{
    public interface IColorItemsAppService : IApplicationService
    {
        Task<PagedResultDto<GetColorItemForViewDto>> GetAll(GetAllColorItemsInput input);

        Task<GetColorItemForViewDto> GetColorItemForView(long id);

        Task<GetColorItemForEditOutput> GetColorItemForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditColorItemDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetColorItemsToExcel(GetAllColorItemsForExcelInput input);

        Task<PagedResultDto<ColorItemProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ColorItemColorLookupTableDto>> GetAllColorForLookupTable(GetAllForLookupTableInput input);

    }
}