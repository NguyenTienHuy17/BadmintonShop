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
    [AbpAuthorize(AppPermissions.Pages_Blogs)]
    public class BlogsAppService : ERPAppServiceBase, IBlogsAppService
    {
        private readonly IRepository<Blog, long> _blogRepository;
        private readonly IBlogsExcelExporter _blogsExcelExporter;

        public BlogsAppService(IRepository<Blog, long> blogRepository, IBlogsExcelExporter blogsExcelExporter)
        {
            _blogRepository = blogRepository;
            _blogsExcelExporter = blogsExcelExporter;

        }

        public async Task<PagedResultDto<GetBlogForViewDto>> GetAll(GetAllBlogsInput input)
        {

            var filteredBlogs = _blogRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.title.Contains(input.Filter) || e.content.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.titleFilter), e => e.title == input.titleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.contentFilter), e => e.content == input.contentFilter);

            var pagedAndFilteredBlogs = filteredBlogs
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var blogs = from o in pagedAndFilteredBlogs
                        select new
                        {

                            o.title,
                            o.content,
                            Id = o.Id
                        };

            var totalCount = await filteredBlogs.CountAsync();

            var dbList = await blogs.ToListAsync();
            var results = new List<GetBlogForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBlogForViewDto()
                {
                    Blog = new BlogDto
                    {

                        title = o.title,
                        content = o.content,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetBlogForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetBlogForViewDto> GetBlogForView(long id)
        {
            var blog = await _blogRepository.GetAsync(id);

            var output = new GetBlogForViewDto { Blog = ObjectMapper.Map<BlogDto>(blog) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Blogs_Edit)]
        public async Task<GetBlogForEditOutput> GetBlogForEdit(EntityDto<long> input)
        {
            var blog = await _blogRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBlogForEditOutput { Blog = ObjectMapper.Map<CreateOrEditBlogDto>(blog) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBlogDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Blogs_Create)]
        protected virtual async Task Create(CreateOrEditBlogDto input)
        {
            var blog = ObjectMapper.Map<Blog>(input);

            if (AbpSession.TenantId != null)
            {
                blog.TenantId = (int?)AbpSession.TenantId;
            }

            await _blogRepository.InsertAsync(blog);

        }

        [AbpAuthorize(AppPermissions.Pages_Blogs_Edit)]
        protected virtual async Task Update(CreateOrEditBlogDto input)
        {
            var blog = await _blogRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, blog);

        }

        [AbpAuthorize(AppPermissions.Pages_Blogs_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _blogRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetBlogsToExcel(GetAllBlogsForExcelInput input)
        {

            var filteredBlogs = _blogRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.title.Contains(input.Filter) || e.content.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.titleFilter), e => e.title == input.titleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.contentFilter), e => e.content == input.contentFilter);

            var query = (from o in filteredBlogs
                         select new GetBlogForViewDto()
                         {
                             Blog = new BlogDto
                             {
                                 title = o.title,
                                 content = o.content,
                                 Id = o.Id
                             }
                         });

            var blogListDtos = await query.ToListAsync();

            return _blogsExcelExporter.ExportToFile(blogListDtos);
        }

    }
}