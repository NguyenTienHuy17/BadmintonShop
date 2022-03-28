using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Common.Dtos;
using ERP.Dto;

namespace ERP.Common
{
    public interface ISizesAppService : IApplicationService
    {
        Task<PagedResultDto<GetSizeForViewDto>> GetAll(GetAllSizesInput input);

        Task<GetSizeForViewDto> GetSizeForView(long id);

        Task<GetSizeForEditOutput> GetSizeForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditSizeDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetSizesToExcel(GetAllSizesForExcelInput input);

    }
}