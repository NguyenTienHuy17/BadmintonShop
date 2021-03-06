using ERP.Entity;
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
using ERP.Entity.Dtos;

namespace ERP.Purchase
{
    public class OrderItemsAppService : ERPAppServiceBase, IOrderItemsAppService
    {
        private readonly IRepository<OrderItem, long> _orderItemRepository;
        private readonly IOrderItemsExcelExporter _orderItemsExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<Order, long> _lookup_orderRepository;
        private readonly IRepository<Product, long> _productRepository;

        public OrderItemsAppService(IRepository<OrderItem, long> orderItemRepository, 
            IOrderItemsExcelExporter orderItemsExcelExporter, 
            IRepository<Product, long> lookup_productRepository, 
            IRepository<Order, long> lookup_orderRepository,
            IRepository<Product, long> productRepository)
        {
            _orderItemRepository = orderItemRepository;
            _orderItemsExcelExporter = orderItemsExcelExporter;
            _lookup_productRepository = lookup_productRepository;
            _lookup_orderRepository = lookup_orderRepository;
            _productRepository = productRepository;
        }

        public async Task<PagedResultDto<GetOrderItemForViewDto>> GetAll(GetAllOrderItemsInput input)
        {

            var filteredOrderItems = _orderItemRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.OrderFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderOrderCodeFilter), e => e.OrderFk != null && e.OrderFk.OrderCode == input.OrderOrderCodeFilter);

            var pagedAndFilteredOrderItems = filteredOrderItems
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var orderItems = from o in pagedAndFilteredOrderItems
                             join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             join o2 in _lookup_orderRepository.GetAll() on o.OrderId equals o2.Id into j2
                             from s2 in j2.DefaultIfEmpty()

                             select new
                             {

                                 o.Quantity,
                                 Id = o.Id,
                                 ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                 OrderOrderCode = s2 == null || s2.OrderCode == null ? "" : s2.OrderCode.ToString()
                             };

            var totalCount = await filteredOrderItems.CountAsync();

            var dbList = await orderItems.ToListAsync();
            var results = new List<GetOrderItemForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOrderItemForViewDto()
                {
                    OrderItem = new OrderItemDto
                    {

                        Quantity = o.Quantity,
                        Id = o.Id,
                    },
                    ProductName = o.ProductName,
                    OrderOrderCode = o.OrderOrderCode
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOrderItemForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOrderItemForViewDto> GetOrderItemForView(long id)
        {
            var orderItem = await _orderItemRepository.GetAsync(id);

            var output = new GetOrderItemForViewDto { OrderItem = ObjectMapper.Map<OrderItemDto>(orderItem) };

            if (output.OrderItem.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.OrderItem.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.OrderItem.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((long)output.OrderItem.OrderId);
                output.OrderOrderCode = _lookupOrder?.OrderCode?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_OrderItems_Edit)]
        public async Task<GetOrderItemForEditOutput> GetOrderItemForEdit(EntityDto<long> input)
        {
            var orderItem = await _orderItemRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOrderItemForEditOutput { OrderItem = ObjectMapper.Map<CreateOrEditOrderItemDto>(orderItem) };

            if (output.OrderItem.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.OrderItem.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.OrderItem.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((long)output.OrderItem.OrderId);
                output.OrderOrderCode = _lookupOrder?.OrderCode?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOrderItemDto input)
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
        protected virtual async Task Create(CreateOrEditOrderItemDto input)
        {
            var orderItem = ObjectMapper.Map<OrderItem>(input);

            if (AbpSession.TenantId != null)
            {
                orderItem.TenantId = (int?)AbpSession.TenantId;
            }

            await _orderItemRepository.InsertAsync(orderItem);

            var product = await _productRepository.GetAsync(input.ProductId);

            product.InStock -= int.Parse(input.Quantity.ToString());
        }

        [AbpAuthorize(AppPermissions.Pages_OrderItems_Edit)]
        protected virtual async Task Update(CreateOrEditOrderItemDto input)
        {
            var orderItem = await _orderItemRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, orderItem);

            await _orderItemRepository.InsertAsync(orderItem);

            var product = await _productRepository.GetAsync(input.ProductId);

            product.InStock -= int.Parse(input.Quantity.ToString());
        }

        [AbpAuthorize(AppPermissions.Pages_OrderItems_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _orderItemRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetOrderItemsToExcel(GetAllOrderItemsForExcelInput input)
        {

            var filteredOrderItems = _orderItemRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.OrderFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderOrderCodeFilter), e => e.OrderFk != null && e.OrderFk.OrderCode == input.OrderOrderCodeFilter);

            var query = (from o in filteredOrderItems
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_orderRepository.GetAll() on o.OrderId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetOrderItemForViewDto()
                         {
                             OrderItem = new OrderItemDto
                             {
                                 Quantity = o.Quantity,
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             OrderOrderCode = s2 == null || s2.OrderCode == null ? "" : s2.OrderCode.ToString()
                         });

            var orderItemListDtos = await query.ToListAsync();

            return _orderItemsExcelExporter.ExportToFile(orderItemListDtos);
        }

        public async Task<List<GetOrderItemForViewDto>> GetOrderItemByOrderId(long orderId)
        {
            var listOrderItem = _orderItemRepository.GetAll().Where(x => x.OrderId == orderId);

            var output = new List<GetOrderItemForViewDto>();

            foreach (var item in listOrderItem)
            {
                var orderItem = new GetOrderItemForViewDto { OrderItem = ObjectMapper.Map<OrderItemDto>(item) };
                var product = _productRepository.GetAll().Where(x => x.Id == item.ProductId).FirstOrDefault();
                orderItem.Product = ObjectMapper.Map<ProductDto>(product);
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((long)item.OrderId);
                orderItem.OrderOrderCode = _lookupOrder?.OrderCode?.ToString();
                output.Add(orderItem);
            }

            return output;
        }
    }
}