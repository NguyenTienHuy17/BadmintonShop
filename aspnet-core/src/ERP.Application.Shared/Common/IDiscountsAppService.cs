using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Common.Dtos;
using ERP.Dto;

namespace ERP.Common
{
    public interface IDiscountsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDiscountForViewDto>> GetAll(GetAllDiscountsInput input);

        Task<GetDiscountForViewDto> GetDiscountForView(long id);

        Task<GetDiscountForEditOutput> GetDiscountForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDiscountDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetDiscountsToExcel(GetAllDiscountsForExcelInput input);

    }
}