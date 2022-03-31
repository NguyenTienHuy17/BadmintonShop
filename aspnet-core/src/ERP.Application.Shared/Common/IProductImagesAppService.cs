using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Common.Dtos;
using ERP.Dto;

namespace ERP.Common
{
    public interface IProductImagesAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductImageForViewDto>> GetAll(GetAllProductImagesInput input);

        Task<GetProductImageForViewDto> GetProductImageForView(long id);

        Task<GetProductImageForEditOutput> GetProductImageForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductImageDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductImagesToExcel(GetAllProductImagesForExcelInput input);

        Task<PagedResultDto<ProductImageProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

    }
}