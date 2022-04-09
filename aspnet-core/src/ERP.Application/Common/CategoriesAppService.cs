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
    public class CategoriesAppService : ERPAppServiceBase, ICategoriesAppService
    {
        private readonly IRepository<Category, long> _categoryRepository;
        private readonly ICategoriesExcelExporter _categoriesExcelExporter;

        public CategoriesAppService(IRepository<Category, long> categoryRepository, ICategoriesExcelExporter categoriesExcelExporter)
        {
            _categoryRepository = categoryRepository;
            _categoriesExcelExporter = categoriesExcelExporter;

        }

        public async Task<PagedResultDto<GetCategoryForViewDto>> GetAll(GetAllCategoriesInput input)
        {

            var filteredCategories = _categoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var pagedAndFilteredCategories = filteredCategories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var categories = from o in pagedAndFilteredCategories

                             select new
                             {

                                 o.Name,
                                 o.Description,
                                 Id = o.Id,
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

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Categories_Edit)]
        public async Task<GetCategoryForEditOutput> GetCategoryForEdit(EntityDto<long> input)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCategoryForEditOutput { Category = ObjectMapper.Map<CreateOrEditCategoryDto>(category) };

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
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var query = (from o in filteredCategories

                         select new GetCategoryForViewDto()
                         {
                             Category = new CategoryDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Id = o.Id
                             },
                         });

            var categoryListDtos = await query.ToListAsync();

            return _categoriesExcelExporter.ExportToFile(categoryListDtos);
        }

        public async Task<List<Category>> GetAllForProduct()
        {

            var listCategories = _categoryRepository.GetAll();
            var dbList = await listCategories.ToListAsync();
            var results = new List<Category>();

            foreach (var o in dbList)
            {
                results.Add(o);
            }

            return results;

        }

    }
}