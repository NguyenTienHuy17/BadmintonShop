using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Common.Dtos;
using ERP.Dto;

namespace ERP.Common
{
    public interface ICartsAppService : IApplicationService
    {
        Task<PagedResultDto<GetCartForViewDto>> GetAll(GetAllCartsInput input);

        Task<GetCartForViewDto> GetCartForView(long id);

        Task<GetCartForEditOutput> GetCartForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCartDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetCartsToExcel(GetAllCartsForExcelInput input);

    }
}