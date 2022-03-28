using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Common.Exporting;
using ERP.Common.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;

namespace ERP.Common
{
    [AbpAuthorize(AppPermissions.Pages_Sizes)]
    public class SizesAppService : ERPAppServiceBase, ISizesAppService
    {
        private readonly IRepository<Size, long> _sizeRepository;
        private readonly ISizesExcelExporter _sizesExcelExporter;

        public SizesAppService(IRepository<Size, long> sizeRepository, ISizesExcelExporter sizesExcelExporter)
        {
            _sizeRepository = sizeRepository;
            _sizesExcelExporter = sizesExcelExporter;

        }

        public async Task<PagedResultDto<GetSizeForViewDto>> GetAll(GetAllSizesInput input)
        {

            var filteredSizes = _sizeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DisplayName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DisplayNameFilter), e => e.DisplayName == input.DisplayNameFilter)
                        .WhereIf(input.MinSizeNumFilter != null, e => e.SizeNum >= input.MinSizeNumFilter)
                        .WhereIf(input.MaxSizeNumFilter != null, e => e.SizeNum <= input.MaxSizeNumFilter);

            var pagedAndFilteredSizes = filteredSizes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var sizes = from o in pagedAndFilteredSizes
                        select new
                        {

                            o.DisplayName,
                            o.SizeNum,
                            Id = o.Id
                        };

            var totalCount = await filteredSizes.CountAsync();

            var dbList = await sizes.ToListAsync();
            var results = new List<GetSizeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSizeForViewDto()
                {
                    Size = new SizeDto
                    {

                        DisplayName = o.DisplayName,
                        SizeNum = o.SizeNum,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetSizeForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetSizeForViewDto> GetSizeForView(long id)
        {
            var size = await _sizeRepository.GetAsync(id);

            var output = new GetSizeForViewDto { Size = ObjectMapper.Map<SizeDto>(size) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Sizes_Edit)]
        public async Task<GetSizeForEditOutput> GetSizeForEdit(EntityDto<long> input)
        {
            var size = await _sizeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSizeForEditOutput { Size = ObjectMapper.Map<CreateOrEditSizeDto>(size) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSizeDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Sizes_Create)]
        protected virtual async Task Create(CreateOrEditSizeDto input)
        {
            var size = ObjectMapper.Map<Size>(input);

            if (AbpSession.TenantId != null)
            {
                size.TenantId = (int?)AbpSession.TenantId;
            }

            await _sizeRepository.InsertAsync(size);

        }

        [AbpAuthorize(AppPermissions.Pages_Sizes_Edit)]
        protected virtual async Task Update(CreateOrEditSizeDto input)
        {
            var size = await _sizeRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, size);

        }

        [AbpAuthorize(AppPermissions.Pages_Sizes_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _sizeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetSizesToExcel(GetAllSizesForExcelInput input)
        {

            var filteredSizes = _sizeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DisplayName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DisplayNameFilter), e => e.DisplayName == input.DisplayNameFilter)
                        .WhereIf(input.MinSizeNumFilter != null, e => e.SizeNum >= input.MinSizeNumFilter)
                        .WhereIf(input.MaxSizeNumFilter != null, e => e.SizeNum <= input.MaxSizeNumFilter);

            var query = (from o in filteredSizes
                         select new GetSizeForViewDto()
                         {
                             Size = new SizeDto
                             {
                                 DisplayName = o.DisplayName,
                                 SizeNum = o.SizeNum,
                                 Id = o.Id
                             }
                         });

            var sizeListDtos = await query.ToListAsync();

            return _sizesExcelExporter.ExportToFile(sizeListDtos);
        }

    }
}