using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Common.Dtos;
using ERP.Dto;

namespace ERP.Common
{
    public interface IImagesAppService : IApplicationService
    {
        Task<PagedResultDto<GetImageForViewDto>> GetAll(GetAllImagesInput input);

        Task<GetImageForViewDto> GetImageForView(long id);

        Task<GetImageForEditOutput> GetImageForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditImageDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetImagesToExcel(GetAllImagesForExcelInput input);

    }
}