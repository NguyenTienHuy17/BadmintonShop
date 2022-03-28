using ERP.Common;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Entity.Exporting;
using ERP.Entity.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;

namespace ERP.Entity
{
    [AbpAuthorize(AppPermissions.Pages_Brands)]
    public class BrandsAppService : ERPAppServiceBase, IBrandsAppService
    {
        private readonly IRepository<Brand, long> _brandRepository;
        private readonly IBrandsExcelExporter _brandsExcelExporter;
        private readonly IRepository<Image, long> _lookup_imageRepository;

        public BrandsAppService(IRepository<Brand, long> brandRepository, IBrandsExcelExporter brandsExcelExporter, IRepository<Image, long> lookup_imageRepository)
        {
            _brandRepository = brandRepository;
            _brandsExcelExporter = brandsExcelExporter;
            _lookup_imageRepository = lookup_imageRepository;

        }

        public async Task<PagedResultDto<GetBrandForViewDto>> GetAll(GetAllBrandsInput input)
        {

            var filteredBrands = _brandRepository.GetAll()
                        .Include(e => e.ImageFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Country.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryFilter), e => e.Country == input.CountryFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ImageNameFilter), e => e.ImageFk != null && e.ImageFk.Name == input.ImageNameFilter);

            var pagedAndFilteredBrands = filteredBrands
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var brands = from o in pagedAndFilteredBrands
                         join o1 in _lookup_imageRepository.GetAll() on o.ImageId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new
                         {

                             o.Name,
                             o.Country,
                             o.Description,
                             Id = o.Id,
                             ImageName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         };

            var totalCount = await filteredBrands.CountAsync();

            var dbList = await brands.ToListAsync();
            var results = new List<GetBrandForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBrandForViewDto()
                {
                    Brand = new BrandDto
                    {

                        Name = o.Name,
                        Country = o.Country,
                        Description = o.Description,
                        Id = o.Id,
                    },
                    ImageName = o.ImageName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetBrandForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetBrandForViewDto> GetBrandForView(long id)
        {
            var brand = await _brandRepository.GetAsync(id);

            var output = new GetBrandForViewDto { Brand = ObjectMapper.Map<BrandDto>(brand) };

            if (output.Brand.ImageId != null)
            {
                var _lookupImage = await _lookup_imageRepository.FirstOrDefaultAsync((long)output.Brand.ImageId);
                output.ImageName = _lookupImage?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Brands_Edit)]
        public async Task<GetBrandForEditOutput> GetBrandForEdit(EntityDto<long> input)
        {
            var brand = await _brandRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBrandForEditOutput { Brand = ObjectMapper.Map<CreateOrEditBrandDto>(brand) };

            if (output.Brand.ImageId != null)
            {
                var _lookupImage = await _lookup_imageRepository.FirstOrDefaultAsync((long)output.Brand.ImageId);
                output.ImageName = _lookupImage?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBrandDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Brands_Create)]
        protected virtual async Task Create(CreateOrEditBrandDto input)
        {
            var brand = ObjectMapper.Map<Brand>(input);

            if (AbpSession.TenantId != null)
            {
                brand.TenantId = (int?)AbpSession.TenantId;
            }

            await _brandRepository.InsertAsync(brand);

        }

        [AbpAuthorize(AppPermissions.Pages_Brands_Edit)]
        protected virtual async Task Update(CreateOrEditBrandDto input)
        {
            var brand = await _brandRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, brand);

        }

        [AbpAuthorize(AppPermissions.Pages_Brands_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _brandRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetBrandsToExcel(GetAllBrandsForExcelInput input)
        {

            var filteredBrands = _brandRepository.GetAll()
                        .Include(e => e.ImageFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Country.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryFilter), e => e.Country == input.CountryFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ImageNameFilter), e => e.ImageFk != null && e.ImageFk.Name == input.ImageNameFilter);

            var query = (from o in filteredBrands
                         join o1 in _lookup_imageRepository.GetAll() on o.ImageId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetBrandForViewDto()
                         {
                             Brand = new BrandDto
                             {
                                 Name = o.Name,
                                 Country = o.Country,
                                 Description = o.Description,
                                 Id = o.Id
                             },
                             ImageName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var brandListDtos = await query.ToListAsync();

            return _brandsExcelExporter.ExportToFile(brandListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Brands)]
        public async Task<PagedResultDto<BrandImageLookupTableDto>> GetAllImageForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_imageRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var imageList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BrandImageLookupTableDto>();
            foreach (var image in imageList)
            {
                lookupTableDtoList.Add(new BrandImageLookupTableDto
                {
                    Id = image.Id,
                    DisplayName = image.Name?.ToString()
                });
            }

            return new PagedResultDto<BrandImageLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}