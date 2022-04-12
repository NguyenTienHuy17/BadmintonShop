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
    [AbpAuthorize(AppPermissions.Pages_ProductImages)]
    public class ProductImagesAppService : ERPAppServiceBase, IProductImagesAppService
    {
        private readonly IRepository<ProductImage, long> _productImageRepository;
        private readonly IProductImagesExcelExporter _productImagesExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;

        public ProductImagesAppService(IRepository<ProductImage, long> productImageRepository, IProductImagesExcelExporter productImagesExcelExporter, IRepository<Product, long> lookup_productRepository)
        {
            _productImageRepository = productImageRepository;
            _productImagesExcelExporter = productImagesExcelExporter;
            _lookup_productRepository = lookup_productRepository;

        }

        public async Task<PagedResultDto<GetProductImageForViewDto>> GetAll(GetAllProductImagesInput input)
        {

            var filteredProductImages = _productImageRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var pagedAndFilteredProductImages = filteredProductImages
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productImages = from o in pagedAndFilteredProductImages
                                join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                select new
                                {

                                    Id = o.Id,
                                    ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                };

            var totalCount = await filteredProductImages.CountAsync();

            var dbList = await productImages.ToListAsync();
            var results = new List<GetProductImageForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductImageForViewDto()
                {
                    ProductImage = new ProductImageDto
                    {

                        Id = o.Id,
                    },
                    ProductName = o.ProductName,
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductImageForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductImageForViewDto> GetProductImageForView(long id)
        {
            var productImage = await _productImageRepository.GetAsync(id);

            var output = new GetProductImageForViewDto { ProductImage = ObjectMapper.Map<ProductImageDto>(productImage) };

            if (output.ProductImage.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductImage.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductImages_Edit)]
        public async Task<GetProductImageForEditOutput> GetProductImageForEdit(EntityDto<long> input)
        {
            var productImage = await _productImageRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductImageForEditOutput { ProductImage = ObjectMapper.Map<CreateOrEditProductImageDto>(productImage) };

            if (output.ProductImage.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductImage.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductImageDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductImages_Create)]
        protected virtual async Task Create(CreateOrEditProductImageDto input)
        {
            var productImage = ObjectMapper.Map<ProductImage>(input);

            if (AbpSession.TenantId != null)
            {
                productImage.TenantId = (int?)AbpSession.TenantId;
            }

            await _productImageRepository.InsertAsync(productImage);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductImages_Edit)]
        protected virtual async Task Update(CreateOrEditProductImageDto input)
        {
            var productImage = await _productImageRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productImage);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductImages_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productImageRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductImagesToExcel(GetAllProductImagesForExcelInput input)
        {

            var filteredProductImages = _productImageRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var query = (from o in filteredProductImages
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetProductImageForViewDto()
                         {
                             ProductImage = new ProductImageDto
                             {
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         });

            var productImageListDtos = await query.ToListAsync();

            return _productImagesExcelExporter.ExportToFile(productImageListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductImages)]
        public async Task<PagedResultDto<ProductImageProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductImageProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductImageProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductImageProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<List<ProductImage>> GetListProductImageUrlByProductId(long productId)
        {
            return await _productImageRepository.GetAllListAsync(x => x.ProductId == productId);
        }

        public async Task DeleteProductImage(long id)
        {
            if(id == null || id == 0)
            {
                return;
            }
            else
            {
                await _productImageRepository.DeleteAsync(id);
            }
        }
    }
}