using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Common.Dtos;
using ERP.Dto;

namespace ERP.Common
{
    public interface ICategoriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCategoryForViewDto>> GetAll(GetAllCategoriesInput input);

        Task<GetCategoryForViewDto> GetCategoryForView(long id);

        Task<GetCategoryForEditOutput> GetCategoryForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCategoryDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetCategoriesToExcel(GetAllCategoriesForExcelInput input);

        Task<PagedResultDto<CategoryImageLookupTableDto>> GetAllImageForLookupTable(GetAllForLookupTableInput input);

    }
}