using ERP.Common;
using ERP.Common;

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
using Abp.Runtime.Session;

namespace ERP.Purchase
{
    public class OrdersAppService : ERPAppServiceBase, IOrdersAppService
    {
        private readonly IRepository<Order, long> _orderRepository;
        private readonly IOrdersExcelExporter _ordersExcelExporter;
        private readonly IRepository<Status, long> _lookup_statusRepository;
        private readonly IRepository<Discount, long> _lookup_discountRepository;
        private readonly IAbpSession _abpSession;
        public OrdersAppService(IRepository<Order, long> orderRepository, 
            IOrdersExcelExporter ordersExcelExporter, 
            IRepository<Status, long> lookup_statusRepository, 
            IRepository<Discount, long> lookup_discountRepository,
            IAbpSession abpSession)
        {
            _orderRepository = orderRepository;
            _ordersExcelExporter = ordersExcelExporter;
            _lookup_statusRepository = lookup_statusRepository;
            _lookup_discountRepository = lookup_discountRepository;
            _abpSession = abpSession;
        }

        public async Task<PagedResultDto<GetOrderForViewDto>> GetAll(GetAllOrdersInput input)
        {

            var filteredOrders = _orderRepository.GetAll()
                        .Include(e => e.StatusFk)
                        .Include(e => e.DiscountFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.OrderCode.Contains(input.Filter) || e.TotalPrice.Contains(input.Filter) || e.ShippingAddress.Contains(input.Filter) || e.ShippingNumber.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderCodeFilter), e => e.OrderCode == input.OrderCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TotalPriceFilter), e => e.TotalPrice == input.TotalPriceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ShippingAddressFilter), e => e.ShippingAddress == input.ShippingAddressFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ShippingNumberFilter), e => e.ShippingNumber == input.ShippingNumberFilter)
                        .WhereIf(input.MinDiscountAmountFilter != null, e => e.DiscountAmount >= input.MinDiscountAmountFilter)
                        .WhereIf(input.MaxDiscountAmountFilter != null, e => e.DiscountAmount <= input.MaxDiscountAmountFilter)
                        .WhereIf(input.MinActualPriceFilter != null, e => e.ActualPrice >= input.MinActualPriceFilter)
                        .WhereIf(input.MaxActualPriceFilter != null, e => e.ActualPrice <= input.MaxActualPriceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusNameFilter), e => e.StatusFk != null && e.StatusFk.Name == input.StatusNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DiscountDiscountCodeFilter), e => e.DiscountFk != null && e.DiscountFk.DiscountCode == input.DiscountDiscountCodeFilter);

            var pagedAndFilteredOrders = filteredOrders
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var orders = from o in pagedAndFilteredOrders
                         join o1 in _lookup_statusRepository.GetAll() on o.StatusId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_discountRepository.GetAll() on o.DiscountId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new
                         {

                             o.OrderCode,
                             o.TotalPrice,
                             o.ShippingAddress,
                             o.ShippingNumber,
                             o.DiscountAmount,
                             o.ActualPrice,
                             Id = o.Id,
                             StatusName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             DiscountDiscountCode = s2 == null || s2.DiscountCode == null ? "" : s2.DiscountCode.ToString()
                         };

            var totalCount = await filteredOrders.CountAsync();

            var dbList = await orders.ToListAsync();
            var results = new List<GetOrderForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOrderForViewDto()
                {
                    Order = new OrderDto
                    {

                        OrderCode = o.OrderCode,
                        TotalPrice = o.TotalPrice,
                        ShippingAddress = o.ShippingAddress,
                        ShippingNumber = o.ShippingNumber,
                        DiscountAmount = o.DiscountAmount,
                        ActualPrice = o.ActualPrice,
                        Id = o.Id,
                    },
                    StatusName = o.StatusName,
                    DiscountDiscountCode = o.DiscountDiscountCode
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOrderForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOrderForViewDto> GetOrderForView(long id)
        {
            var order = await _orderRepository.GetAsync(id);

            var output = new GetOrderForViewDto { Order = ObjectMapper.Map<OrderDto>(order) };

            if (output.Order.StatusId != null)
            {
                var _lookupStatus = await _lookup_statusRepository.FirstOrDefaultAsync((long)output.Order.StatusId);
                output.StatusName = _lookupStatus?.Name?.ToString();
            }

            if (output.Order.DiscountId != null)
            {
                var _lookupDiscount = await _lookup_discountRepository.FirstOrDefaultAsync((long)output.Order.DiscountId);
                output.DiscountDiscountCode = _lookupDiscount?.DiscountCode?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Orders_Edit)]
        public async Task<GetOrderForEditOutput> GetOrderForEdit(EntityDto<long> input)
        {
            var order = await _orderRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOrderForEditOutput { Order = ObjectMapper.Map<CreateOrEditOrderDto>(order) };

            if (output.Order.StatusId != null)
            {
                var _lookupStatus = await _lookup_statusRepository.FirstOrDefaultAsync((long)output.Order.StatusId);
                output.StatusName = _lookupStatus?.Name?.ToString();
            }

            if (output.Order.DiscountId != null)
            {
                var _lookupDiscount = await _lookup_discountRepository.FirstOrDefaultAsync((long)output.Order.DiscountId);
                output.DiscountDiscountCode = _lookupDiscount?.DiscountCode?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOrderDto input)
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

        [AbpAuthorize(AppPermissions.Pages_User)]
        protected virtual async Task Create(CreateOrEditOrderDto input)
        {
            var order = ObjectMapper.Map<Order>(input);

            if (AbpSession.TenantId != null)
            {
                order.TenantId = (int?)AbpSession.TenantId;
            }

            await _orderRepository.InsertAsync(order);

        }

        [AbpAuthorize(AppPermissions.Pages_User)]
        protected virtual async Task Update(CreateOrEditOrderDto input)
        {
            var order = await _orderRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, order);

        }

        [AbpAuthorize(AppPermissions.Pages_Orders_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _orderRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetOrdersToExcel(GetAllOrdersForExcelInput input)
        {

            var filteredOrders = _orderRepository.GetAll()
                        .Include(e => e.StatusFk)
                        .Include(e => e.DiscountFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.OrderCode.Contains(input.Filter) || e.TotalPrice.Contains(input.Filter) || e.ShippingAddress.Contains(input.Filter) || e.ShippingNumber.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderCodeFilter), e => e.OrderCode == input.OrderCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TotalPriceFilter), e => e.TotalPrice == input.TotalPriceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ShippingAddressFilter), e => e.ShippingAddress == input.ShippingAddressFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ShippingNumberFilter), e => e.ShippingNumber == input.ShippingNumberFilter)
                        .WhereIf(input.MinDiscountAmountFilter != null, e => e.DiscountAmount >= input.MinDiscountAmountFilter)
                        .WhereIf(input.MaxDiscountAmountFilter != null, e => e.DiscountAmount <= input.MaxDiscountAmountFilter)
                        .WhereIf(input.MinActualPriceFilter != null, e => e.ActualPrice >= input.MinActualPriceFilter)
                        .WhereIf(input.MaxActualPriceFilter != null, e => e.ActualPrice <= input.MaxActualPriceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusNameFilter), e => e.StatusFk != null && e.StatusFk.Name == input.StatusNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DiscountDiscountCodeFilter), e => e.DiscountFk != null && e.DiscountFk.DiscountCode == input.DiscountDiscountCodeFilter);

            var query = (from o in filteredOrders
                         join o1 in _lookup_statusRepository.GetAll() on o.StatusId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_discountRepository.GetAll() on o.DiscountId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetOrderForViewDto()
                         {
                             Order = new OrderDto
                             {
                                 OrderCode = o.OrderCode,
                                 TotalPrice = o.TotalPrice,
                                 ShippingAddress = o.ShippingAddress,
                                 ShippingNumber = o.ShippingNumber,
                                 DiscountAmount = o.DiscountAmount,
                                 ActualPrice = o.ActualPrice,
                                 Id = o.Id
                             },
                             StatusName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             DiscountDiscountCode = s2 == null || s2.DiscountCode == null ? "" : s2.DiscountCode.ToString()
                         });

            var orderListDtos = await query.ToListAsync();

            return _ordersExcelExporter.ExportToFile(orderListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Orders)]
        public async Task<PagedResultDto<OrderStatusLookupTableDto>> GetAllStatusForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_statusRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var statusList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderStatusLookupTableDto>();
            foreach (var status in statusList)
            {
                lookupTableDtoList.Add(new OrderStatusLookupTableDto
                {
                    Id = status.Id,
                    DisplayName = status.Name?.ToString()
                });
            }

            return new PagedResultDto<OrderStatusLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Orders)]
        public async Task<PagedResultDto<OrderDiscountLookupTableDto>> GetAllDiscountForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_discountRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.DiscountCode != null && e.DiscountCode.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var discountList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderDiscountLookupTableDto>();
            foreach (var discount in discountList)
            {
                lookupTableDtoList.Add(new OrderDiscountLookupTableDto
                {
                    Id = discount.Id,
                    DisplayName = discount.DiscountCode?.ToString()
                });
            }

            return new PagedResultDto<OrderDiscountLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }


        public async Task<long> CreateAndGetId(CreateOrEditOrderDto input)
        {
            try
            {
                var order = ObjectMapper.Map<Order>(input);

                if (AbpSession.TenantId != null)
                {
                    order.TenantId = (int?)AbpSession.TenantId;
                }
                long id = 0;
                id = await _orderRepository.InsertAndGetIdAsync(order);
                return id;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<GetOrderForViewDto>> GetAllUserOrder()
        {

            var filteredOrders = _orderRepository.GetAll().Where(x => x.CreatorUserId == _abpSession.UserId);
            var pagedAndFilteredOrders = filteredOrders
                .OrderBy(x=> x.CreationTime);

            var orders = from o in pagedAndFilteredOrders
                         join o1 in _lookup_statusRepository.GetAll() on o.StatusId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_discountRepository.GetAll() on o.DiscountId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new
                         {

                             o.OrderCode,
                             o.TotalPrice,
                             o.ShippingAddress,
                             o.ShippingNumber,
                             o.DiscountAmount,
                             o.ActualPrice,
                             Id = o.Id,
                             StatusName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             DiscountDiscountCode = s2 == null || s2.DiscountCode == null ? "" : s2.DiscountCode.ToString()
                         };

            var dbList = await orders.ToListAsync();
            var results = new List<GetOrderForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOrderForViewDto()
                {
                    Order = new OrderDto
                    {

                        OrderCode = o.OrderCode,
                        TotalPrice = o.TotalPrice,
                        ShippingAddress = o.ShippingAddress,
                        ShippingNumber = o.ShippingNumber,
                        DiscountAmount = o.DiscountAmount,
                        ActualPrice = o.ActualPrice,
                        Id = o.Id,
                    },
                    StatusName = o.StatusName,
                    DiscountDiscountCode = o.DiscountDiscountCode
                };

                results.Add(res);
            }

            return results;

        }
    }
}