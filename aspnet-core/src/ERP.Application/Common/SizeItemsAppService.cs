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
    [AbpAuthorize(AppPermissions.Pages_SizeItems)]
    public class SizeItemsAppService : ERPAppServiceBase, ISizeItemsAppService
    {
        private readonly IRepository<SizeItem, long> _sizeItemRepository;
        private readonly ISizeItemsExcelExporter _sizeItemsExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<Size, long> _lookup_sizeRepository;

        public SizeItemsAppService(IRepository<SizeItem, long> sizeItemRepository, ISizeItemsExcelExporter sizeItemsExcelExporter, IRepository<Product, long> lookup_productRepository, IRepository<Size, long> lookup_sizeRepository)
        {
            _sizeItemRepository = sizeItemRepository;
            _sizeItemsExcelExporter = sizeItemsExcelExporter;
            _lookup_productRepository = lookup_productRepository;
            _lookup_sizeRepository = lookup_sizeRepository;

        }

        public async Task<PagedResultDto<GetSizeItemForViewDto>> GetAll(GetAllSizeItemsInput input)
        {

            var filteredSizeItems = _sizeItemRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.SizeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SizeDisplayNameFilter), e => e.SizeFk != null && e.SizeFk.DisplayName == input.SizeDisplayNameFilter);

            var pagedAndFilteredSizeItems = filteredSizeItems
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var sizeItems = from o in pagedAndFilteredSizeItems
                            join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                            from s1 in j1.DefaultIfEmpty()

                            join o2 in _lookup_sizeRepository.GetAll() on o.SizeId equals o2.Id into j2
                            from s2 in j2.DefaultIfEmpty()

                            select new
                            {

                                Id = o.Id,
                                ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                SizeDisplayName = s2 == null || s2.DisplayName == null ? "" : s2.DisplayName.ToString()
                            };

            var totalCount = await filteredSizeItems.CountAsync();

            var dbList = await sizeItems.ToListAsync();
            var results = new List<GetSizeItemForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSizeItemForViewDto()
                {
                    SizeItem = new SizeItemDto
                    {

                        Id = o.Id,
                    },
                    ProductName = o.ProductName,
                    SizeDisplayName = o.SizeDisplayName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetSizeItemForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetSizeItemForViewDto> GetSizeItemForView(long id)
        {
            var sizeItem = await _sizeItemRepository.GetAsync(id);

            var output = new GetSizeItemForViewDto { SizeItem = ObjectMapper.Map<SizeItemDto>(sizeItem) };

            if (output.SizeItem.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.SizeItem.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.SizeItem.SizeId != null)
            {
                var _lookupSize = await _lookup_sizeRepository.FirstOrDefaultAsync((long)output.SizeItem.SizeId);
                output.SizeDisplayName = _lookupSize?.DisplayName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SizeItems_Edit)]
        public async Task<GetSizeItemForEditOutput> GetSizeItemForEdit(EntityDto<long> input)
        {
            var sizeItem = await _sizeItemRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSizeItemForEditOutput { SizeItem = ObjectMapper.Map<CreateOrEditSizeItemDto>(sizeItem) };

            if (output.SizeItem.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.SizeItem.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.SizeItem.SizeId != null)
            {
                var _lookupSize = await _lookup_sizeRepository.FirstOrDefaultAsync((long)output.SizeItem.SizeId);
                output.SizeDisplayName = _lookupSize?.DisplayName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSizeItemDto input)
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

        [AbpAuthorize(AppPermissions.Pages_SizeItems_Create)]
        protected virtual async Task Create(CreateOrEditSizeItemDto input)
        {
            var sizeItem = ObjectMapper.Map<SizeItem>(input);

            if (AbpSession.TenantId != null)
            {
                sizeItem.TenantId = (int?)AbpSession.TenantId;
            }

            await _sizeItemRepository.InsertAsync(sizeItem);

        }

        [AbpAuthorize(AppPermissions.Pages_SizeItems_Edit)]
        protected virtual async Task Update(CreateOrEditSizeItemDto input)
        {
            var sizeItem = await _sizeItemRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, sizeItem);

        }

        [AbpAuthorize(AppPermissions.Pages_SizeItems_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _sizeItemRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetSizeItemsToExcel(GetAllSizeItemsForExcelInput input)
        {

            var filteredSizeItems = _sizeItemRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.SizeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SizeDisplayNameFilter), e => e.SizeFk != null && e.SizeFk.DisplayName == input.SizeDisplayNameFilter);

            var query = (from o in filteredSizeItems
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_sizeRepository.GetAll() on o.SizeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetSizeItemForViewDto()
                         {
                             SizeItem = new SizeItemDto
                             {
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             SizeDisplayName = s2 == null || s2.DisplayName == null ? "" : s2.DisplayName.ToString()
                         });

            var sizeItemListDtos = await query.ToListAsync();

            return _sizeItemsExcelExporter.ExportToFile(sizeItemListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_SizeItems)]
        public async Task<PagedResultDto<SizeItemProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<SizeItemProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new SizeItemProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<SizeItemProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_SizeItems)]
        public async Task<PagedResultDto<SizeItemSizeLookupTableDto>> GetAllSizeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_sizeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.DisplayName != null && e.DisplayName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var sizeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<SizeItemSizeLookupTableDto>();
            foreach (var size in sizeList)
            {
                lookupTableDtoList.Add(new SizeItemSizeLookupTableDto
                {
                    Id = size.Id,
                    DisplayName = size.DisplayName?.ToString()
                });
            }

            return new PagedResultDto<SizeItemSizeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}