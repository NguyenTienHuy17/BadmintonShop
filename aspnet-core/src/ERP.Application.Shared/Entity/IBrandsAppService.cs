using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Entity.Dtos;
using ERP.Dto;

namespace ERP.Entity
{
    public interface IBrandsAppService : IApplicationService
    {
        Task<PagedResultDto<GetBrandForViewDto>> GetAll(GetAllBrandsInput input);

        Task<GetBrandForViewDto> GetBrandForView(long id);

        Task<GetBrandForEditOutput> GetBrandForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditBrandDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetBrandsToExcel(GetAllBrandsForExcelInput input);

        Task<PagedResultDto<BrandImageLookupTableDto>> GetAllImageForLookupTable(GetAllForLookupTableInput input);

    }
}