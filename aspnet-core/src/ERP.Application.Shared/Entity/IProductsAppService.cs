using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Entity.Dtos;
using ERP.Dto;

namespace ERP.Entity
{
    public interface IProductsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductForViewDto>> GetAll(GetAllProductsInput input);

        Task<GetProductForViewDto> GetProductForView(long id);

        Task<GetProductForEditOutput> GetProductForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductsToExcel(GetAllProductsForExcelInput input);

        Task<PagedResultDto<ProductImageLookupTableDto>> GetAllImageForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductBrandLookupTableDto>> GetAllBrandForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductCategoryLookupTableDto>> GetAllCategoryForLookupTable(GetAllForLookupTableInput input);

    }
}