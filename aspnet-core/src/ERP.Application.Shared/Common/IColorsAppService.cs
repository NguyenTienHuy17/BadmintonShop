using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Common.Dtos;
using ERP.Dto;

namespace ERP.Common
{
    public interface IColorsAppService : IApplicationService
    {
        Task<PagedResultDto<GetColorForViewDto>> GetAll(GetAllColorsInput input);

        Task<GetColorForViewDto> GetColorForView(long id);

        Task<GetColorForEditOutput> GetColorForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditColorDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetColorsToExcel(GetAllColorsForExcelInput input);

    }
}