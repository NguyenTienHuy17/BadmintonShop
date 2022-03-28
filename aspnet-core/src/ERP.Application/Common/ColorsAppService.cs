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
    [AbpAuthorize(AppPermissions.Pages_Colors)]
    public class ColorsAppService : ERPAppServiceBase, IColorsAppService
    {
        private readonly IRepository<Color, long> _colorRepository;
        private readonly IColorsExcelExporter _colorsExcelExporter;

        public ColorsAppService(IRepository<Color, long> colorRepository, IColorsExcelExporter colorsExcelExporter)
        {
            _colorRepository = colorRepository;
            _colorsExcelExporter = colorsExcelExporter;

        }

        public async Task<PagedResultDto<GetColorForViewDto>> GetAll(GetAllColorsInput input)
        {

            var filteredColors = _colorRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ColorName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ColorNameFilter), e => e.ColorName == input.ColorNameFilter)
                        .WhereIf(input.MinColorCodeFilter != null, e => e.ColorCode >= input.MinColorCodeFilter)
                        .WhereIf(input.MaxColorCodeFilter != null, e => e.ColorCode <= input.MaxColorCodeFilter);

            var pagedAndFilteredColors = filteredColors
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var colors = from o in pagedAndFilteredColors
                         select new
                         {

                             o.ColorName,
                             o.ColorCode,
                             Id = o.Id
                         };

            var totalCount = await filteredColors.CountAsync();

            var dbList = await colors.ToListAsync();
            var results = new List<GetColorForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetColorForViewDto()
                {
                    Color = new ColorDto
                    {

                        ColorName = o.ColorName,
                        ColorCode = o.ColorCode,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetColorForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetColorForViewDto> GetColorForView(long id)
        {
            var color = await _colorRepository.GetAsync(id);

            var output = new GetColorForViewDto { Color = ObjectMapper.Map<ColorDto>(color) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Colors_Edit)]
        public async Task<GetColorForEditOutput> GetColorForEdit(EntityDto<long> input)
        {
            var color = await _colorRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetColorForEditOutput { Color = ObjectMapper.Map<CreateOrEditColorDto>(color) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditColorDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Colors_Create)]
        protected virtual async Task Create(CreateOrEditColorDto input)
        {
            var color = ObjectMapper.Map<Color>(input);

            if (AbpSession.TenantId != null)
            {
                color.TenantId = (int?)AbpSession.TenantId;
            }

            await _colorRepository.InsertAsync(color);

        }

        [AbpAuthorize(AppPermissions.Pages_Colors_Edit)]
        protected virtual async Task Update(CreateOrEditColorDto input)
        {
            var color = await _colorRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, color);

        }

        [AbpAuthorize(AppPermissions.Pages_Colors_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _colorRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetColorsToExcel(GetAllColorsForExcelInput input)
        {

            var filteredColors = _colorRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ColorName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ColorNameFilter), e => e.ColorName == input.ColorNameFilter)
                        .WhereIf(input.MinColorCodeFilter != null, e => e.ColorCode >= input.MinColorCodeFilter)
                        .WhereIf(input.MaxColorCodeFilter != null, e => e.ColorCode <= input.MaxColorCodeFilter);

            var query = (from o in filteredColors
                         select new GetColorForViewDto()
                         {
                             Color = new ColorDto
                             {
                                 ColorName = o.ColorName,
                                 ColorCode = o.ColorCode,
                                 Id = o.Id
                             }
                         });

            var colorListDtos = await query.ToListAsync();

            return _colorsExcelExporter.ExportToFile(colorListDtos);
        }

    }
}