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
    [AbpAuthorize(AppPermissions.Pages_Images)]
    public class ImagesAppService : ERPAppServiceBase, IImagesAppService
    {
        private readonly IRepository<Image, long> _imageRepository;
        private readonly IImagesExcelExporter _imagesExcelExporter;

        public ImagesAppService(IRepository<Image, long> imageRepository, IImagesExcelExporter imagesExcelExporter)
        {
            _imageRepository = imageRepository;
            _imagesExcelExporter = imagesExcelExporter;

        }

        public async Task<PagedResultDto<GetImageForViewDto>> GetAll(GetAllImagesInput input)
        {

            var filteredImages = _imageRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Url.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UrlFilter), e => e.Url == input.UrlFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var pagedAndFilteredImages = filteredImages
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var images = from o in pagedAndFilteredImages
                         select new
                         {

                             o.Name,
                             o.Url,
                             o.Description,
                             Id = o.Id
                         };

            var totalCount = await filteredImages.CountAsync();

            var dbList = await images.ToListAsync();
            var results = new List<GetImageForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetImageForViewDto()
                {
                    Image = new ImageDto
                    {

                        Name = o.Name,
                        Url = o.Url,
                        Description = o.Description,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetImageForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetImageForViewDto> GetImageForView(long id)
        {
            var image = await _imageRepository.GetAsync(id);

            var output = new GetImageForViewDto { Image = ObjectMapper.Map<ImageDto>(image) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Images_Edit)]
        public async Task<GetImageForEditOutput> GetImageForEdit(EntityDto<long> input)
        {
            var image = await _imageRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetImageForEditOutput { Image = ObjectMapper.Map<CreateOrEditImageDto>(image) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditImageDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Images_Create)]
        protected virtual async Task Create(CreateOrEditImageDto input)
        {
            var image = ObjectMapper.Map<Image>(input);

            if (AbpSession.TenantId != null)
            {
                image.TenantId = (int?)AbpSession.TenantId;
            }

            await _imageRepository.InsertAsync(image);

        }

        [AbpAuthorize(AppPermissions.Pages_Images_Edit)]
        protected virtual async Task Update(CreateOrEditImageDto input)
        {
            var image = await _imageRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, image);

        }

        [AbpAuthorize(AppPermissions.Pages_Images_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _imageRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetImagesToExcel(GetAllImagesForExcelInput input)
        {

            var filteredImages = _imageRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Url.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UrlFilter), e => e.Url == input.UrlFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var query = (from o in filteredImages
                         select new GetImageForViewDto()
                         {
                             Image = new ImageDto
                             {
                                 Name = o.Name,
                                 Url = o.Url,
                                 Description = o.Description,
                                 Id = o.Id
                             }
                         });

            var imageListDtos = await query.ToListAsync();

            return _imagesExcelExporter.ExportToFile(imageListDtos);
        }

    }
}