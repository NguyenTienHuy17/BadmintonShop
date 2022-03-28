using ERP.Purchase;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Purchase.Exporting;
using ERP.Purchase.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;

namespace ERP.Purchase
{
    [AbpAuthorize(AppPermissions.Pages_ReturnProds)]
    public class ReturnProdsAppService : ERPAppServiceBase, IReturnProdsAppService
    {
        private readonly IRepository<ReturnProd, long> _returnProdRepository;
        private readonly IReturnProdsExcelExporter _returnProdsExcelExporter;
        private readonly IRepository<Order, long> _lookup_orderRepository;

        public ReturnProdsAppService(IRepository<ReturnProd, long> returnProdRepository, IReturnProdsExcelExporter returnProdsExcelExporter, IRepository<Order, long> lookup_orderRepository)
        {
            _returnProdRepository = returnProdRepository;
            _returnProdsExcelExporter = returnProdsExcelExporter;
            _lookup_orderRepository = lookup_orderRepository;

        }

        public async Task<PagedResultDto<GetReturnProdForViewDto>> GetAll(GetAllReturnProdsInput input)
        {

            var filteredReturnProds = _returnProdRepository.GetAll()
                        .Include(e => e.OrderFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Reason.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReasonFilter), e => e.Reason == input.ReasonFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderOrderCodeFilter), e => e.OrderFk != null && e.OrderFk.OrderCode == input.OrderOrderCodeFilter);

            var pagedAndFilteredReturnProds = filteredReturnProds
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var returnProds = from o in pagedAndFilteredReturnProds
                              join o1 in _lookup_orderRepository.GetAll() on o.OrderId equals o1.Id into j1
                              from s1 in j1.DefaultIfEmpty()

                              select new
                              {

                                  o.Reason,
                                  Id = o.Id,
                                  OrderOrderCode = s1 == null || s1.OrderCode == null ? "" : s1.OrderCode.ToString()
                              };

            var totalCount = await filteredReturnProds.CountAsync();

            var dbList = await returnProds.ToListAsync();
            var results = new List<GetReturnProdForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetReturnProdForViewDto()
                {
                    ReturnProd = new ReturnProdDto
                    {

                        Reason = o.Reason,
                        Id = o.Id,
                    },
                    OrderOrderCode = o.OrderOrderCode
                };

                results.Add(res);
            }

            return new PagedResultDto<GetReturnProdForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetReturnProdForViewDto> GetReturnProdForView(long id)
        {
            var returnProd = await _returnProdRepository.GetAsync(id);

            var output = new GetReturnProdForViewDto { ReturnProd = ObjectMapper.Map<ReturnProdDto>(returnProd) };

            if (output.ReturnProd.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((long)output.ReturnProd.OrderId);
                output.OrderOrderCode = _lookupOrder?.OrderCode?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ReturnProds_Edit)]
        public async Task<GetReturnProdForEditOutput> GetReturnProdForEdit(EntityDto<long> input)
        {
            var returnProd = await _returnProdRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetReturnProdForEditOutput { ReturnProd = ObjectMapper.Map<CreateOrEditReturnProdDto>(returnProd) };

            if (output.ReturnProd.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((long)output.ReturnProd.OrderId);
                output.OrderOrderCode = _lookupOrder?.OrderCode?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditReturnProdDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ReturnProds_Create)]
        protected virtual async Task Create(CreateOrEditReturnProdDto input)
        {
            var returnProd = ObjectMapper.Map<ReturnProd>(input);

            if (AbpSession.TenantId != null)
            {
                returnProd.TenantId = (int?)AbpSession.TenantId;
            }

            await _returnProdRepository.InsertAsync(returnProd);

        }

        [AbpAuthorize(AppPermissions.Pages_ReturnProds_Edit)]
        protected virtual async Task Update(CreateOrEditReturnProdDto input)
        {
            var returnProd = await _returnProdRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, returnProd);

        }

        [AbpAuthorize(AppPermissions.Pages_ReturnProds_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _returnProdRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetReturnProdsToExcel(GetAllReturnProdsForExcelInput input)
        {

            var filteredReturnProds = _returnProdRepository.GetAll()
                        .Include(e => e.OrderFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Reason.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReasonFilter), e => e.Reason == input.ReasonFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderOrderCodeFilter), e => e.OrderFk != null && e.OrderFk.OrderCode == input.OrderOrderCodeFilter);

            var query = (from o in filteredReturnProds
                         join o1 in _lookup_orderRepository.GetAll() on o.OrderId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetReturnProdForViewDto()
                         {
                             ReturnProd = new ReturnProdDto
                             {
                                 Reason = o.Reason,
                                 Id = o.Id
                             },
                             OrderOrderCode = s1 == null || s1.OrderCode == null ? "" : s1.OrderCode.ToString()
                         });

            var returnProdListDtos = await query.ToListAsync();

            return _returnProdsExcelExporter.ExportToFile(returnProdListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ReturnProds)]
        public async Task<PagedResultDto<ReturnProdOrderLookupTableDto>> GetAllOrderForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_orderRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.OrderCode != null && e.OrderCode.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var orderList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ReturnProdOrderLookupTableDto>();
            foreach (var order in orderList)
            {
                lookupTableDtoList.Add(new ReturnProdOrderLookupTableDto
                {
                    Id = order.Id,
                    DisplayName = order.OrderCode?.ToString()
                });
            }

            return new PagedResultDto<ReturnProdOrderLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}