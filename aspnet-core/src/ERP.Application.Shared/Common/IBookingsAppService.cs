using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Common.Dtos;
using ERP.Dto;

namespace ERP.Common
{
    public interface IBookingsAppService : IApplicationService
    {
        Task<PagedResultDto<GetBookingForViewDto>> GetAll(GetAllBookingsInput input);

        Task<GetBookingForViewDto> GetBookingForView(long id);

        Task<GetBookingForEditOutput> GetBookingForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditBookingDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetBookingsToExcel(GetAllBookingsForExcelInput input);

    }
}