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
    [AbpAuthorize(AppPermissions.Pages_Discounts)]
    public class DiscountsAppService : ERPAppServiceBase, IDiscountsAppService
    {
        private readonly IRepository<Discount, long> _discountRepository;
        private readonly IDiscountsExcelExporter _discountsExcelExporter;

        public DiscountsAppService(IRepository<Discount, long> discountRepository, IDiscountsExcelExporter discountsExcelExporter)
        {
            _discountRepository = discountRepository;
            _discountsExcelExporter = discountsExcelExporter;

        }

        public async Task<PagedResultDto<GetDiscountForViewDto>> GetAll(GetAllDiscountsInput input)
        {

            var filteredDiscounts = _discountRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DiscountCode.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DiscountCodeFilter), e => e.DiscountCode == input.DiscountCodeFilter)
                        .WhereIf(input.MinDiscountFilter != null, e => e.DiscountNum >= input.MinDiscountFilter)
                        .WhereIf(input.MaxDiscountFilter != null, e => e.DiscountNum <= input.MaxDiscountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var pagedAndFilteredDiscounts = filteredDiscounts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var discounts = from o in pagedAndFilteredDiscounts
                            select new
                            {

                                o.DiscountCode,
                                o.DiscountNum,
                                o.Description,
                                Id = o.Id
                            };

            var totalCount = await filteredDiscounts.CountAsync();

            var dbList = await discounts.ToListAsync();
            var results = new List<GetDiscountForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDiscountForViewDto()
                {
                    Discount = new DiscountDto
                    {

                        DiscountCode = o.DiscountCode,
                        DiscountNum = o.DiscountNum,
                        Description = o.Description,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDiscountForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetDiscountForViewDto> GetDiscountForView(long id)
        {
            var discount = await _discountRepository.GetAsync(id);

            var output = new GetDiscountForViewDto { Discount = ObjectMapper.Map<DiscountDto>(discount) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Discounts_Edit)]
        public async Task<GetDiscountForEditOutput> GetDiscountForEdit(EntityDto<long> input)
        {
            var discount = await _discountRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDiscountForEditOutput { Discount = ObjectMapper.Map<CreateOrEditDiscountDto>(discount) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDiscountDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Discounts_Create)]
        protected virtual async Task Create(CreateOrEditDiscountDto input)
        {
            var discount = ObjectMapper.Map<Discount>(input);

            if (AbpSession.TenantId != null)
            {
                discount.TenantId = (int?)AbpSession.TenantId;
            }

            await _discountRepository.InsertAsync(discount);

        }

        [AbpAuthorize(AppPermissions.Pages_Discounts_Edit)]
        protected virtual async Task Update(CreateOrEditDiscountDto input)
        {
            var discount = await _discountRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, discount);

        }

        [AbpAuthorize(AppPermissions.Pages_Discounts_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _discountRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDiscountsToExcel(GetAllDiscountsForExcelInput input)
        {

            var filteredDiscounts = _discountRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DiscountCode.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DiscountCodeFilter), e => e.DiscountCode == input.DiscountCodeFilter)
                        .WhereIf(input.MinDiscountFilter != null, e => e.DiscountNum >= input.MinDiscountFilter)
                        .WhereIf(input.MaxDiscountFilter != null, e => e.DiscountNum <= input.MaxDiscountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var query = (from o in filteredDiscounts
                         select new GetDiscountForViewDto()
                         {
                             Discount = new DiscountDto
                             {
                                 DiscountCode = o.DiscountCode,
                                 DiscountNum = o.DiscountNum,
                                 Description = o.Description,
                                 Id = o.Id
                             }
                         });

            var discountListDtos = await query.ToListAsync();

            return _discountsExcelExporter.ExportToFile(discountListDtos);
        }

    }
}