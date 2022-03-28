using ERP.Entity;
using ERP.Common;

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
    [AbpAuthorize(AppPermissions.Pages_ColorItems)]
    public class ColorItemsAppService : ERPAppServiceBase, IColorItemsAppService
    {
        private readonly IRepository<ColorItem, long> _colorItemRepository;
        private readonly IColorItemsExcelExporter _colorItemsExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<Color, long> _lookup_colorRepository;

        public ColorItemsAppService(IRepository<ColorItem, long> colorItemRepository, IColorItemsExcelExporter colorItemsExcelExporter, IRepository<Product, long> lookup_productRepository, IRepository<Color, long> lookup_colorRepository)
        {
            _colorItemRepository = colorItemRepository;
            _colorItemsExcelExporter = colorItemsExcelExporter;
            _lookup_productRepository = lookup_productRepository;
            _lookup_colorRepository = lookup_colorRepository;

        }

        public async Task<PagedResultDto<GetColorItemForViewDto>> GetAll(GetAllColorItemsInput input)
        {

            var filteredColorItems = _colorItemRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.ColorFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ColorColorNameFilter), e => e.ColorFk != null && e.ColorFk.ColorName == input.ColorColorNameFilter);

            var pagedAndFilteredColorItems = filteredColorItems
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var colorItems = from o in pagedAndFilteredColorItems
                             join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             join o2 in _lookup_colorRepository.GetAll() on o.ColorId equals o2.Id into j2
                             from s2 in j2.DefaultIfEmpty()

                             select new
                             {

                                 Id = o.Id,
                                 ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                 ColorColorName = s2 == null || s2.ColorName == null ? "" : s2.ColorName.ToString()
                             };

            var totalCount = await filteredColorItems.CountAsync();

            var dbList = await colorItems.ToListAsync();
            var results = new List<GetColorItemForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetColorItemForViewDto()
                {
                    ColorItem = new ColorItemDto
                    {

                        Id = o.Id,
                    },
                    ProductName = o.ProductName,
                    ColorColorName = o.ColorColorName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetColorItemForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetColorItemForViewDto> GetColorItemForView(long id)
        {
            var colorItem = await _colorItemRepository.GetAsync(id);

            var output = new GetColorItemForViewDto { ColorItem = ObjectMapper.Map<ColorItemDto>(colorItem) };

            if (output.ColorItem.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ColorItem.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ColorItem.ColorId != null)
            {
                var _lookupColor = await _lookup_colorRepository.FirstOrDefaultAsync((long)output.ColorItem.ColorId);
                output.ColorColorName = _lookupColor?.ColorName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ColorItems_Edit)]
        public async Task<GetColorItemForEditOutput> GetColorItemForEdit(EntityDto<long> input)
        {
            var colorItem = await _colorItemRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetColorItemForEditOutput { ColorItem = ObjectMapper.Map<CreateOrEditColorItemDto>(colorItem) };

            if (output.ColorItem.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ColorItem.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ColorItem.ColorId != null)
            {
                var _lookupColor = await _lookup_colorRepository.FirstOrDefaultAsync((long)output.ColorItem.ColorId);
                output.ColorColorName = _lookupColor?.ColorName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditColorItemDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ColorItems_Create)]
        protected virtual async Task Create(CreateOrEditColorItemDto input)
        {
            var colorItem = ObjectMapper.Map<ColorItem>(input);

            if (AbpSession.TenantId != null)
            {
                colorItem.TenantId = (int?)AbpSession.TenantId;
            }

            await _colorItemRepository.InsertAsync(colorItem);

        }

        [AbpAuthorize(AppPermissions.Pages_ColorItems_Edit)]
        protected virtual async Task Update(CreateOrEditColorItemDto input)
        {
            var colorItem = await _colorItemRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, colorItem);

        }

        [AbpAuthorize(AppPermissions.Pages_ColorItems_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _colorItemRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetColorItemsToExcel(GetAllColorItemsForExcelInput input)
        {

            var filteredColorItems = _colorItemRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.ColorFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ColorColorNameFilter), e => e.ColorFk != null && e.ColorFk.ColorName == input.ColorColorNameFilter);

            var query = (from o in filteredColorItems
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_colorRepository.GetAll() on o.ColorId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetColorItemForViewDto()
                         {
                             ColorItem = new ColorItemDto
                             {
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ColorColorName = s2 == null || s2.ColorName == null ? "" : s2.ColorName.ToString()
                         });

            var colorItemListDtos = await query.ToListAsync();

            return _colorItemsExcelExporter.ExportToFile(colorItemListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ColorItems)]
        public async Task<PagedResultDto<ColorItemProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ColorItemProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ColorItemProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ColorItemProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ColorItems)]
        public async Task<PagedResultDto<ColorItemColorLookupTableDto>> GetAllColorForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_colorRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.ColorName != null && e.ColorName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var colorList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ColorItemColorLookupTableDto>();
            foreach (var color in colorList)
            {
                lookupTableDtoList.Add(new ColorItemColorLookupTableDto
                {
                    Id = color.Id,
                    DisplayName = color.ColorName?.ToString()
                });
            }

            return new PagedResultDto<ColorItemColorLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}