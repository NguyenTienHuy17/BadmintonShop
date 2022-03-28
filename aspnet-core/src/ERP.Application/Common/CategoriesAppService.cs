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
    [AbpAuthorize(AppPermissions.Pages_Categories)]
    public class CategoriesAppService : ERPAppServiceBase, ICategoriesAppService
    {
        private readonly IRepository<Category, long> _categoryRepository;
        private readonly ICategoriesExcelExporter _categoriesExcelExporter;
        private readonly IRepository<Image, long> _lookup_imageRepository;

        public CategoriesAppService(IRepository<Category, long> categoryRepository, ICategoriesExcelExporter categoriesExcelExporter, IRepository<Image, long> lookup_imageRepository)
        {
            _categoryRepository = categoryRepository;
            _categoriesExcelExporter = categoriesExcelExporter;
            _lookup_imageRepository = lookup_imageRepository;

        }

        public async Task<PagedResultDto<GetCategoryForViewDto>> GetAll(GetAllCategoriesInput input)
        {

            var filteredCategories = _categoryRepository.GetAll()
                        .Include(e => e.ImageFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ImageNameFilter), e => e.ImageFk != null && e.ImageFk.Name == input.ImageNameFilter);

            var pagedAndFilteredCategories = filteredCategories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var categories = from o in pagedAndFilteredCategories
                             join o1 in _lookup_imageRepository.GetAll() on o.ImageId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             select new
                             {

                                 o.Name,
                                 o.Description,
                                 Id = o.Id,
                                 ImageName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                             };

            var totalCount = await filteredCategories.CountAsync();

            var dbList = await categories.ToListAsync();
            var results = new List<GetCategoryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCategoryForViewDto()
                {
                    Category = new CategoryDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Id = o.Id,
                    },
                    ImageName = o.ImageName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCategoryForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCategoryForViewDto> GetCategoryForView(long id)
        {
            var category = await _categoryRepository.GetAsync(id);

            var output = new GetCategoryForViewDto { Category = ObjectMapper.Map<CategoryDto>(category) };

            if (output.Category.ImageId != null)
            {
                var _lookupImage = await _lookup_imageRepository.FirstOrDefaultAsync((long)output.Category.ImageId);
                output.ImageName = _lookupImage?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Categories_Edit)]
        public async Task<GetCategoryForEditOutput> GetCategoryForEdit(EntityDto<long> input)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCategoryForEditOutput { Category = ObjectMapper.Map<CreateOrEditCategoryDto>(category) };

            if (output.Category.ImageId != null)
            {
                var _lookupImage = await _lookup_imageRepository.FirstOrDefaultAsync((long)output.Category.ImageId);
                output.ImageName = _lookupImage?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCategoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Categories_Create)]
        protected virtual async Task Create(CreateOrEditCategoryDto input)
        {
            var category = ObjectMapper.Map<Category>(input);

            if (AbpSession.TenantId != null)
            {
                category.TenantId = (int?)AbpSession.TenantId;
            }

            await _categoryRepository.InsertAsync(category);

        }

        [AbpAuthorize(AppPermissions.Pages_Categories_Edit)]
        protected virtual async Task Update(CreateOrEditCategoryDto input)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, category);

        }

        [AbpAuthorize(AppPermissions.Pages_Categories_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _categoryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCategoriesToExcel(GetAllCategoriesForExcelInput input)
        {

            var filteredCategories = _categoryRepository.GetAll()
                        .Include(e => e.ImageFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ImageNameFilter), e => e.ImageFk != null && e.ImageFk.Name == input.ImageNameFilter);

            var query = (from o in filteredCategories
                         join o1 in _lookup_imageRepository.GetAll() on o.ImageId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetCategoryForViewDto()
                         {
                             Category = new CategoryDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Id = o.Id
                             },
                             ImageName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var categoryListDtos = await query.ToListAsync();

            return _categoriesExcelExporter.ExportToFile(categoryListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Categories)]
        public async Task<PagedResultDto<CategoryImageLookupTableDto>> GetAllImageForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_imageRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var imageList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CategoryImageLookupTableDto>();
            foreach (var image in imageList)
            {
                lookupTableDtoList.Add(new CategoryImageLookupTableDto
                {
                    Id = image.Id,
                    DisplayName = image.Name?.ToString()
                });
            }

            return new PagedResultDto<CategoryImageLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}