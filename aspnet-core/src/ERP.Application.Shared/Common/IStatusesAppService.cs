using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Common.Dtos;
using ERP.Dto;

namespace ERP.Common
{
    public interface IStatusesAppService : IApplicationService
    {
        Task<PagedResultDto<GetStatusForViewDto>> GetAll(GetAllStatusesInput input);

        Task<GetStatusForViewDto> GetStatusForView(long id);

        Task<GetStatusForEditOutput> GetStatusForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStatusDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStatusesToExcel(GetAllStatusesForExcelInput input);

    }
}