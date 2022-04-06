using ERP.Entity;

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
    [AbpAuthorize(AppPermissions.Pages_Carts)]
    public class CartsAppService : ERPAppServiceBase, ICartsAppService
    {
        private readonly IRepository<Cart, long> _cartRepository;
        private readonly ICartsExcelExporter _cartsExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;

        public CartsAppService(IRepository<Cart, long> cartRepository, ICartsExcelExporter cartsExcelExporter, IRepository<Product, long> lookup_productRepository)
        {
            _cartRepository = cartRepository;
            _cartsExcelExporter = cartsExcelExporter;
            _lookup_productRepository = lookup_productRepository;

        }

        public async Task<PagedResultDto<GetCartForViewDto>> GetAll(GetAllCartsInput input)
        {

            var filteredCarts = _cartRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinquantityFilter != null, e => e.quantity >= input.MinquantityFilter)
                        .WhereIf(input.MaxquantityFilter != null, e => e.quantity <= input.MaxquantityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var pagedAndFilteredCarts = filteredCarts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var carts = from o in pagedAndFilteredCarts
                        join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                        from s1 in j1.DefaultIfEmpty()

                        select new
                        {

                            o.quantity,
                            Id = o.Id,
                            ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                        };

            var totalCount = await filteredCarts.CountAsync();

            var dbList = await carts.ToListAsync();
            var results = new List<GetCartForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCartForViewDto()
                {
                    Cart = new CartDto
                    {

                        quantity = o.quantity,
                        Id = o.Id,
                    },
                    ProductName = o.ProductName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCartForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCartForViewDto> GetCartForView(long id)
        {
            var cart = await _cartRepository.GetAsync(id);

            var output = new GetCartForViewDto { Cart = ObjectMapper.Map<CartDto>(cart) };

            if (output.Cart.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.Cart.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Carts_Edit)]
        public async Task<GetCartForEditOutput> GetCartForEdit(EntityDto<long> input)
        {
            var cart = await _cartRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCartForEditOutput { Cart = ObjectMapper.Map<CreateOrEditCartDto>(cart) };

            if (output.Cart.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.Cart.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCartDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Carts_Create)]
        protected virtual async Task Create(CreateOrEditCartDto input)
        {
            var cart = ObjectMapper.Map<Cart>(input);

            if (AbpSession.TenantId != null)
            {
                cart.TenantId = (int?)AbpSession.TenantId;
            }

            await _cartRepository.InsertAsync(cart);

        }

        [AbpAuthorize(AppPermissions.Pages_Carts_Edit)]
        protected virtual async Task Update(CreateOrEditCartDto input)
        {
            var cart = await _cartRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, cart);

        }

        [AbpAuthorize(AppPermissions.Pages_Carts_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _cartRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCartsToExcel(GetAllCartsForExcelInput input)
        {

            var filteredCarts = _cartRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinquantityFilter != null, e => e.quantity >= input.MinquantityFilter)
                        .WhereIf(input.MaxquantityFilter != null, e => e.quantity <= input.MaxquantityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var query = (from o in filteredCarts
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetCartForViewDto()
                         {
                             Cart = new CartDto
                             {
                                 quantity = o.quantity,
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var cartListDtos = await query.ToListAsync();

            return _cartsExcelExporter.ExportToFile(cartListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Carts)]
        public async Task<PagedResultDto<CartProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CartProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new CartProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<CartProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task AddProductToCart(CreateOrEditCartDto input)
        {
            var listCart = _cartRepository.GetAll().Where(x => x.ProductId == input.ProductId).FirstOrDefault();
            if (listCart != null)
            {
                listCart.quantity += input.quantity;
                return;
            }
            if (input.Id == null)
            {
                await Create(input);
            }
        }
    }
}