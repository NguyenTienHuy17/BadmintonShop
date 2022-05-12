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
using Abp.Runtime.Session;
using ERP.Entity.Dtos;

namespace ERP.Common
{
    public class CartsAppService : ERPAppServiceBase, ICartsAppService
    {
        private readonly IRepository<Cart, long> _cartRepository;
        private readonly ICartsExcelExporter _cartsExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IAbpSession _abpSession;
        private readonly IRepository<Product, long> _productRepository;
        private readonly IRepository<ProductImage, long> _lookup_productImageRepository;

        public CartsAppService(IRepository<Cart, long> cartRepository,
            ICartsExcelExporter cartsExcelExporter,
            IRepository<Product, long> lookup_productRepository,
            IAbpSession abpSession,
            IRepository<Product, long> productRepository,
            IRepository<ProductImage, long> lookup_productImageRepository)
        {
            _cartRepository = cartRepository;
            _cartsExcelExporter = cartsExcelExporter;
            _lookup_productRepository = lookup_productRepository;
            _abpSession = abpSession;
            _productRepository = productRepository;
            _lookup_productImageRepository = lookup_productImageRepository;
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
            var cartItem = await _cartRepository.FirstOrDefaultAsync(x => x.ProductId == input.ProductId);
            if (cartItem != null)
            {
                cartItem.quantity += input.quantity;
            }
            else if (input.Id == null && cartItem == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_User)]
        protected virtual async Task Create(CreateOrEditCartDto input)
        {
            var cart = ObjectMapper.Map<Cart>(input);

            if (AbpSession.TenantId != null)
            {
                cart.TenantId = (int?)AbpSession.TenantId;
            }

            await _cartRepository.InsertAsync(cart);

        }

        [AbpAuthorize(AppPermissions.Pages_User)]
        protected virtual async Task Update(CreateOrEditCartDto input)
        {
            var cart = await _cartRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, cart);

        }

        [AbpAuthorize(AppPermissions.Pages_User)]
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

        public async Task AddProductToCart(CreateOrEditCartDto input)
        {
            if (input.Id != null)
            {
                var cart = _cartRepository.GetAll().Where(x => x.ProductId == input.ProductId && x.CreatorUserId == _abpSession.GetUserId()).FirstOrDefault();
                var product = _productRepository.GetAll().Where(x => x.Id == input.ProductId).FirstOrDefault();
                var quantity = cart.quantity + input.quantity;
                if (cart != null && quantity <= product.InStock)
                {
                    cart.quantity += input.quantity;
                    return;
                }
            }
            if (input.Id == null)
            {
                var product = await _productRepository.FirstOrDefaultAsync(x => x.Id == input.ProductId);
                var cartItem = await _cartRepository.FirstOrDefaultAsync(x => x.ProductId == input.ProductId);
                if (input.quantity <= product.InStock && cartItem == null)
                {
                    await Create(input);
                }
                else if (cartItem != null && input.quantity + cartItem.quantity <= product.InStock)
                {
                    cartItem.quantity += input.quantity;
                }
                else if (input.quantity + cartItem.quantity > product.InStock)
                {
                    throw new UserFriendlyException(L("QuantityOverLimit"));
                }
            }
        }

        public async Task<List<GetCartForViewDto>> GetAllForCart()
        {
            try
            {
                var listCart = _cartRepository.GetAll().Where(x => x.CreatorUserId == _abpSession.GetUserId());
                var productImages = _lookup_productImageRepository.GetAll().GroupBy(x => x.ProductId);
                var products = await _productRepository.GetAll().ToListAsync();

                var listProdImg = new List<ProductImage>();
                //iterate each group        
                foreach (var productImage in productImages)
                {
                    //Each group has a key
                    listProdImg.Add((ProductImage)productImage.FirstOrDefault());
                }
                var listProdId = new List<long>();
                foreach (var cartItem in listCart)
                {
                    listProdId.Add(cartItem.ProductId);
                }
                var listProd = new List<Product>();
                //iterate each group        
                foreach (var product in products)
                {
                    foreach(var productId in listProdId)
                    {
                        if (product.Id == productId)
                        {
                            listProd.Add((Product)product);
                        }
                    }
                }
                var carts = from o in listCart

                            join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                            from s1 in j1.DefaultIfEmpty()

                            join o3 in listProdImg on o.ProductId equals o3.ProductId

                            join o4 in listProd on o.ProductId equals o4.Id

                            select new
                            {
                                o.quantity,
                                o.ProductId,
                                Id = o.Id,
                                ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                ProductImageUrl = o3.Url.ToString(),
                                ProductPrice = o4.Price
                            };
                var dbList = await carts.ToListAsync();
                var results = new List<GetCartForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetCartForViewDto()
                    {
                        Cart = new CartDto
                        {
                            ProductId = o.ProductId,
                            quantity = o.quantity,
                            Id = o.Id,
                        },
                        ProductName = o.ProductName,
                        ProductImageUrl = o.ProductImageUrl,
                        ProductPrice = o.ProductPrice,
                        Product = ObjectMapper.Map<ProductDto>(await _productRepository.FirstOrDefaultAsync(x => x.Id == o.ProductId))
                    };

                    results.Add(res);
                }

                return results;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}