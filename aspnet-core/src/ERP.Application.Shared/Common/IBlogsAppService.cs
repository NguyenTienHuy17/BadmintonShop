using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Common.Dtos;
using ERP.Dto;

namespace ERP.Common
{
    public interface IBlogsAppService : IApplicationService
    {
        Task<PagedResultDto<GetBlogForViewDto>> GetAll(GetAllBlogsInput input);

        Task<GetBlogForViewDto> GetBlogForView(long id);

        Task<GetBlogForEditOutput> GetBlogForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditBlogDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetBlogsToExcel(GetAllBlogsForExcelInput input);

    }
}