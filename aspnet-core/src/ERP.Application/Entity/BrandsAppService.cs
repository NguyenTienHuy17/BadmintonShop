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

        public BrandsAppService(IRepository<Brand, long> brandRepository, IBrandsExcelExporter brandsExcelExporter)
        {
            _brandRepository = brandRepository;
            _brandsExcelExporter = brandsExcelExporter;

        }

        public async Task<PagedResultDto<GetBrandForViewDto>> GetAll(GetAllBrandsInput input)
        {

            var filteredBrands = _brandRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Country.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryFilter), e => e.Country == input.CountryFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var pagedAndFilteredBrands = filteredBrands
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var brands = from o in pagedAndFilteredBrands
                         select new
                         {

                             o.Name,
                             o.Country,
                             o.Description,
                             o.ImageUrl,
                             Id = o.Id,
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
                        ImageUrl = o.ImageUrl,
                        Id = o.Id,
                    },
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

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Brands_Edit)]
        public async Task<GetBrandForEditOutput> GetBrandForEdit(EntityDto<long> input)
        {
            var brand = await _brandRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBrandForEditOutput { Brand = ObjectMapper.Map<CreateOrEditBrandDto>(brand) };

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
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Country.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryFilter), e => e.Country == input.CountryFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var query = (from o in filteredBrands

                         select new GetBrandForViewDto()
                         {
                             Brand = new BrandDto
                             {
                                 Name = o.Name,
                                 Country = o.Country,
                                 Description = o.Description,
                                 ImageUrl = o.ImageUrl,
                                 Id = o.Id
                             },
                         });

            var brandListDtos = await query.ToListAsync();

            return _brandsExcelExporter.ExportToFile(brandListDtos);
        }

        public async Task<List<Brand>> GetAllForProduct()
        {
            var listBrands = _brandRepository.GetAll();
            var dbList = await listBrands.ToListAsync();
            var results = new List<Brand>();
            foreach (var o in dbList)
            {
                results.Add(o);
            }
            return results;
        }

    }
}