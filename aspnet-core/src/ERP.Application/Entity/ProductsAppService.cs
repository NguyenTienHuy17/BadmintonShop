using ERP.Common;
using ERP.Entity;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Entity.Exporting;
using ERP.Entity.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;
using ERP.Common.Dtos;

namespace ERP.Entity
{
    public class ProductsAppService : ERPAppServiceBase, IProductsAppService
    {
        private readonly IRepository<Product, long> _productRepository;
        private readonly IRepository<ProductImage, long> _productImageRepository;
        private readonly IProductsExcelExporter _productsExcelExporter;
        private readonly IRepository<Brand, long> _lookup_brandRepository;
        private readonly IRepository<Category, long> _lookup_categoryRepository;
        private readonly IRepository<ProductImage, long> _lookup_productImageRepository;

        public ProductsAppService(IRepository<Product, long> productRepository, 
            IRepository<ProductImage, long> productImageRepository,
            IProductsExcelExporter productsExcelExporter, 
            IRepository<Brand, long> lookup_brandRepository, 
            IRepository<Category, long> lookup_categoryRepository,
            IRepository<ProductImage, long> lookup_productImageRepository)
        {
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _productsExcelExporter = productsExcelExporter;
            _lookup_brandRepository = lookup_brandRepository;
            _lookup_categoryRepository = lookup_categoryRepository;
            _lookup_productImageRepository = lookup_productImageRepository;

        }

        public async Task<PagedResultDto<GetProductForViewDto>> GetAll(GetAllProductsInput input)
        {

            var filteredProducts = _productRepository.GetAll()
                        .Include(e => e.BrandFk)
                        .Include(e => e.CategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.MadeIn.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Title.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MadeInFilter), e => e.MadeIn == input.MadeInFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(input.MinInStockFilter != null, e => e.InStock >= input.MinInStockFilter)
                        .WhereIf(input.MaxInStockFilter != null, e => e.InStock <= input.MaxInStockFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title == input.TitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BrandNameFilter), e => e.BrandFk != null && e.BrandFk.Name == input.BrandNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryNameFilter), e => e.CategoryFk != null && e.CategoryFk.Name == input.CategoryNameFilter);

            var pagedAndFilteredProducts = filteredProducts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var products = from o in pagedAndFilteredProducts

                           join o2 in _lookup_brandRepository.GetAll() on o.BrandId equals o2.Id into j2
                           from s2 in j2.DefaultIfEmpty()

                           join o3 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o3.Id into j3
                           from s3 in j3.DefaultIfEmpty()

                           select new
                           {

                               o.Name,
                               o.MadeIn,
                               o.Code,
                               o.Price,
                               o.InStock,
                               o.Description,
                               o.Title,
                               o.Color,
                               o.Size,
                               Id = o.Id,
                               BrandName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                               CategoryName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                           };

            var totalCount = await filteredProducts.CountAsync();

            var dbList = await products.ToListAsync();
            var results = new List<GetProductForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductForViewDto()
                {
                    Product = new ProductDto
                    {

                        Name = o.Name,
                        MadeIn = o.MadeIn,
                        Code = o.Code,
                        Price = o.Price,
                        InStock = o.InStock,
                        Description = o.Description,
                        Title = o.Title,
                        Id = o.Id,
                        Color = o.Color,
                        Size = o.Size
                    },
                    BrandName = o.BrandName,
                    CategoryName = o.CategoryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductForViewDto> GetProductForView(long id, string name)
        {
            var product = await _productRepository.GetAsync(id);

            var listProduct = _productRepository.GetAll().Where(x => x.Name.Equals(name));

            var listProductImg = _productImageRepository.GetAll().Where(x => x.ProductId == id);

            var output = new GetProductForViewDto { Product = ObjectMapper.Map<ProductDto>(product) };
            output.ProductImageUrl = new List<string>();
            output.ProductColor = new List<string>();
            output.ProductSize = new List<string>();

            if (output.Product.BrandId != null)
            {
                var _lookupBrand = await _lookup_brandRepository.FirstOrDefaultAsync((long)output.Product.BrandId);
                output.BrandName = _lookupBrand?.Name?.ToString();
            }

            if (output.Product.CategoryId != null)
            {
                var _lookupCategory = await _lookup_categoryRepository.FirstOrDefaultAsync((long)output.Product.CategoryId);
                output.CategoryName = _lookupCategory?.Name?.ToString();
            }
            foreach (var productImage in listProductImg)
            {
                //Each group has a key
                output.ProductImageUrl.Add(productImage.Url);
            }

            foreach (var prod in listProduct)
            {
                //Each group has a key
                if(product.Color != null && product.Color.ToString() != "null" && product.Color != "")
                {
                    output.ProductColor.Add(prod.Color);
                }
                if(prod.Size != null && product.Size.ToString() != "null")
                {
                    output.ProductSize.Add(prod.Size);
                }
            }

            if(output.ProductColor.Count > 0)
            {
                output.IsColor = true;
            }
            if (output.ProductSize.Count > 0)
            {
                output.IsSize = true;
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Products_Edit)]
        public async Task<GetProductForEditOutput> GetProductForEdit(EntityDto<long> input)
        {
            var product = await _productRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductForEditOutput { Product = ObjectMapper.Map<CreateOrEditProductDto>(product) };

            if (output.Product.BrandId != null)
            {
                var _lookupBrand = await _lookup_brandRepository.FirstOrDefaultAsync((long)output.Product.BrandId);
                output.BrandName = _lookupBrand?.Name?.ToString();
            }

            if (output.Product.CategoryId != null)
            {
                var _lookupCategory = await _lookup_categoryRepository.FirstOrDefaultAsync((long)output.Product.CategoryId);
                output.CategoryName = _lookupCategory?.Name?.ToString();
            }

            return output;
        }

        public async Task<long> CreateOrEdit(CreateOrEditProductDto input)
        {
            if (input.Id == null)
            {
                return await Create(input);
            }
            else
            {
                return await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Products_Create)]
        protected virtual async Task<long> Create(CreateOrEditProductDto input)
        {
            long productId = 0;
            try
            {
                var products = await _productRepository.GetAllListAsync();

                var tempName = products.FirstOrDefault(x => x.Name == input.Name && x.Color == input.Color && x.Size == input.Size);

                if (tempName != null)
                {
                    throw new UserFriendlyException(L("Identity.DuplicateProduct"));
                }

                var product = ObjectMapper.Map<Product>(input);

                if (AbpSession.TenantId != null)
                {
                    product.TenantId = (int?)AbpSession.TenantId;
                }

                productId = await _productRepository.InsertAndGetIdAsync(product);

            }
            catch (Exception ex)
            {

                throw;
            }
            return productId;
        }

        [AbpAuthorize(AppPermissions.Pages_Products_Edit)]
        protected virtual async Task<long> Update(CreateOrEditProductDto input)
        {
            var product = await _productRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, product);
            return (long)input.Id;
        }

        [AbpAuthorize(AppPermissions.Pages_Products_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductsToExcel(GetAllProductsForExcelInput input)
        {

            var filteredProducts = _productRepository.GetAll()
                        .Include(e => e.BrandFk)
                        .Include(e => e.CategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.MadeIn.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Title.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MadeInFilter), e => e.MadeIn == input.MadeInFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(input.MinInStockFilter != null, e => e.InStock >= input.MinInStockFilter)
                        .WhereIf(input.MaxInStockFilter != null, e => e.InStock <= input.MaxInStockFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title == input.TitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BrandNameFilter), e => e.BrandFk != null && e.BrandFk.Name == input.BrandNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryNameFilter), e => e.CategoryFk != null && e.CategoryFk.Name == input.CategoryNameFilter);

            var query = (from o in filteredProducts

                         join o2 in _lookup_brandRepository.GetAll() on o.BrandId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetProductForViewDto()
                         {
                             Product = new ProductDto
                             {
                                 Name = o.Name,
                                 MadeIn = o.MadeIn,
                                 Code = o.Code,
                                 Price = o.Price,
                                 InStock = o.InStock,
                                 Description = o.Description,
                                 Title = o.Title,
                                 Color = o.Color,
                                 Size = o.Size,
                                 Id = o.Id
                             },
                             BrandName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             CategoryName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var productListDtos = await query.ToListAsync();

            return _productsExcelExporter.ExportToFile(productListDtos);
        }

        //[AbpAuthorize(AppPermissions.Pages_Products)]
        //public async Task<PagedResultDto<ProductImageLookupTableDto>> GetAllImageForLookupTable(GetAllForLookupTableInput input)
        //{
        //    var query = _lookup_imageRepository.GetAll().WhereIf(
        //           !string.IsNullOrWhiteSpace(input.Filter),
        //          e => e.Name != null && e.Name.Contains(input.Filter)
        //       );

        //    var totalCount = await query.CountAsync();

        //    var imageList = await query
        //        .PageBy(input)
        //        .ToListAsync();

        //    var lookupTableDtoList = new List<ProductImageLookupTableDto>();
        //    foreach (var image in imageList)
        //    {
        //        lookupTableDtoList.Add(new ProductImageLookupTableDto
        //        {
        //            Id = image.Id,
        //            DisplayName = image.Name?.ToString()
        //        });
        //    }

        //    return new PagedResultDto<ProductImageLookupTableDto>(
        //        totalCount,
        //        lookupTableDtoList
        //    );
        //}

        //[AbpAuthorize(AppPermissions.Pages_Products)]
        //public async Task<PagedResultDto<ProductBrandLookupTableDto>> GetAllBrandForLookupTable(GetAllForLookupTableInput input)
        //{
        //    var query = _lookup_brandRepository.GetAll().WhereIf(
        //           !string.IsNullOrWhiteSpace(input.Filter),
        //          e => e.Name != null && e.Name.Contains(input.Filter)
        //       );

        //    var totalCount = await query.CountAsync();

        //    var brandList = await query
        //        .PageBy(input)
        //        .ToListAsync();

        //    var lookupTableDtoList = new List<ProductBrandLookupTableDto>();
        //    foreach (var brand in brandList)
        //    {
        //        lookupTableDtoList.Add(new ProductBrandLookupTableDto
        //        {
        //            Id = brand.Id,
        //            DisplayName = brand.Name?.ToString()
        //        });
        //    }

        //    return new PagedResultDto<ProductBrandLookupTableDto>(
        //        totalCount,
        //        lookupTableDtoList
        //    );
        //}

        //[AbpAuthorize(AppPermissions.Pages_Products)]
        //public async Task<PagedResultDto<ProductCategoryLookupTableDto>> GetAllCategoryForLookupTable(GetAllForLookupTableInput input)
        //{
        //    var query = _lookup_categoryRepository.GetAll().WhereIf(
        //           !string.IsNullOrWhiteSpace(input.Filter),
        //          e => e.Name != null && e.Name.Contains(input.Filter)
        //       );

        //    var totalCount = await query.CountAsync();

        //    var categoryList = await query
        //        .PageBy(input)
        //        .ToListAsync();

        //    var lookupTableDtoList = new List<ProductCategoryLookupTableDto>();
        //    foreach (var category in categoryList)
        //    {
        //        lookupTableDtoList.Add(new ProductCategoryLookupTableDto
        //        {
        //            Id = category.Id,
        //            DisplayName = category.Name?.ToString()
        //        });
        //    }

        //    return new PagedResultDto<ProductCategoryLookupTableDto>(
        //        totalCount,
        //        lookupTableDtoList
        //    );
        //}

        public async Task CreateProductImage(ProductImageUrl input)
        {
            for (int i = 0; i < input.ListImageUrl.Count; i++)
            {
                try
                {
                    var productImage = new ProductImage
                    {
                        ProductId = input.ProductId,
                        Url = input.ListImageUrl[i],
                    };

                    productImage.ProductId = input.ProductId;
                    productImage.Name = productImage.Url?.Substring(productImage.Url.LastIndexOf("\\") + 1);
                    productImage.Url = input.ListImageUrl[i].Replace(@"\", @"/");

                    await _productImageRepository.InsertAsync(productImage);
                }
                catch (Exception ex)
                {

                    throw;
                }
                
            }
        }

        public async Task<List<ProductDto>> GetProductForDashBoard()
        {

            try
            {
                var filteredProducts = _productRepository.GetAll();

                var pagedAndFilteredProducts = filteredProducts
                    .Take(9);

                var productImages = _lookup_productImageRepository.GetAll().GroupBy(x=> x.ProductId);

                var listProdImg = new List<ProductImage>();
                //iterate each group        
                foreach (var productImage in productImages)
                {
                    //Each group has a key
                    listProdImg.Add((ProductImage)productImage.FirstOrDefault());
                }

                var products = await (from o in pagedAndFilteredProducts

                                      join o2 in _lookup_brandRepository.GetAll() on o.BrandId equals o2.Id into j2
                                      from s2 in j2.DefaultIfEmpty()

                                      join o3 in listProdImg on o.Id equals o3.ProductId

                                      select new ProductDto
                                      {
                                          Name = o.Name,
                                          MadeIn = o.MadeIn,
                                          Code = o.Code,
                                          Price = o.Price,
                                          InStock = o.InStock,
                                          Description = o.Description,
                                          Title = o.Title,
                                          Id = o.Id,
                                          Color = o.Color,
                                          Size = o.Size,
                                          BrandName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                          ProductImageUrl = o3.Url.ToString()
                                      }).ToListAsync();
                return products;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<long> GetProductId(string name, string size, string color)
        {
            var listProductId = _productRepository.GetAll();
            if (size == null || size.ToString() == "null" || size.ToString() == "")
            {
                listProductId = listProductId.Where(x => x.Name.Equals(name) && x.Color.Equals(color));
            }
            else if ( color == null || color.ToString() == "null" || color.ToString() == "")
            {
                listProductId = listProductId.Where(x => x.Name.Equals(name) && x.Size.Equals(size));
            }
            else
            {
                listProductId = listProductId.Where(x => x.Name.Equals(name) && x.Size.Equals(size) && x.Color.Equals(color));
            }

            long id = 0;
            foreach(var prod in listProductId)
            {
                id = prod.Id;
            }
            return id;
        }

        public async Task<PagedResultDto<ProductDto>> GetAllProduct(GetAllProductsInput input)
        {

            try
            {
                var filteredProducts = _productRepository.GetAll()
                        .Include(e => e.BrandFk)
                        .Include(e => e.CategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.MadeIn.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Title.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MadeInFilter), e => e.MadeIn == input.MadeInFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(input.MinInStockFilter != null, e => e.InStock >= input.MinInStockFilter)
                        .WhereIf(input.MaxInStockFilter != null, e => e.InStock <= input.MaxInStockFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title == input.TitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BrandNameFilter), e => e.BrandFk != null && e.BrandFk.Name == input.BrandNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryNameFilter), e => e.CategoryFk != null && e.CategoryFk.Name == input.CategoryNameFilter);

                var pagedAndFilteredProducts = filteredProducts
                    .OrderBy(input.Sorting ?? "id asc");
                    //.PageBy(input);


                var productImages = _lookup_productImageRepository.GetAll().GroupBy(x => x.ProductId);

                var listProdImg = new List<ProductImage>();
                //iterate each group        
                foreach (var productImage in productImages)
                {
                    //Each group has a key
                    listProdImg.Add((ProductImage)productImage.FirstOrDefault());
                }
                var products = new List<ProductDto>();
                products = await (from o in pagedAndFilteredProducts

                                  join o2 in _lookup_brandRepository.GetAll() on o.BrandId equals o2.Id into j2
                                  from s2 in j2.DefaultIfEmpty()

                                  join o3 in listProdImg on o.Id equals o3.ProductId

                                  select new ProductDto
                                  {
                                      Name = o.Name,
                                      MadeIn = o.MadeIn,
                                      Code = o.Code,
                                      Price = o.Price,
                                      InStock = o.InStock,
                                      Description = o.Description,
                                      Title = o.Title,
                                      Id = o.Id,
                                      Color = o.Color,
                                      Size = o.Size,
                                      BrandName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                      ProductImageUrl = o3.Url.ToString()
                                  }).ToListAsync();

                var result = products.GroupBy(x => x.Name).Select(o => new ProductDto {
                    Name = o.FirstOrDefault().Name,
                    MadeIn = o.FirstOrDefault().MadeIn,
                    Code = o.FirstOrDefault().Code,
                    Price = o.FirstOrDefault().Price,
                    InStock = o.FirstOrDefault().InStock,
                    Description = o.FirstOrDefault().Description,
                    Title = o.FirstOrDefault().Title,
                    Id = o.FirstOrDefault().Id,
                    Color = o.FirstOrDefault().Color,
                    Size = o.FirstOrDefault().Size,
                    BrandName = o.FirstOrDefault().BrandName,
                    ProductImageUrl = o.FirstOrDefault().ProductImageUrl
                }).ToList().AsQueryable();



                var totalCount = result.Count();

                var output = result.PageBy(input).ToList();

                return new PagedResultDto<ProductDto>(
                    totalCount,
                    output
                );
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<PagedResultDto<ProductDto>> GetAllByBrandId(GetAllProductsInput input, long brandId)
        {

            try
            {
                var filteredProducts = _productRepository.GetAll()
                        .Where(x => x.BrandId == brandId)
                        .Include(e => e.BrandFk)
                        .Include(e => e.CategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.MadeIn.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Title.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MadeInFilter), e => e.MadeIn == input.MadeInFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(input.MinInStockFilter != null, e => e.InStock >= input.MinInStockFilter)
                        .WhereIf(input.MaxInStockFilter != null, e => e.InStock <= input.MaxInStockFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title == input.TitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BrandNameFilter), e => e.BrandFk != null && e.BrandFk.Name == input.BrandNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryNameFilter), e => e.CategoryFk != null && e.CategoryFk.Name == input.CategoryNameFilter);

                var pagedAndFilteredProducts = filteredProducts
                    .OrderBy(input.Sorting ?? "id asc");

                var productImages = _lookup_productImageRepository.GetAll().GroupBy(x => x.ProductId);

                var listProdImg = new List<ProductImage>();
                //iterate each group        
                foreach (var productImage in productImages)
                {
                    //Each group has a key
                    listProdImg.Add((ProductImage)productImage.FirstOrDefault());
                }
                var products = new List<ProductDto>();
                products = await (from o in pagedAndFilteredProducts

                                      join o2 in _lookup_brandRepository.GetAll() on o.BrandId equals o2.Id into j2
                                      from s2 in j2.DefaultIfEmpty()

                                      join o3 in listProdImg on o.Id equals o3.ProductId

                                      select new ProductDto
                                      {
                                          Name = o.Name,
                                          MadeIn = o.MadeIn,
                                          Code = o.Code,
                                          Price = o.Price,
                                          InStock = o.InStock,
                                          Description = o.Description,
                                          Title = o.Title,
                                          Id = o.Id,
                                          Color = o.Color,
                                          Size = o.Size,
                                          BrandName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                          ProductImageUrl = o3.Url.ToString()
                                      }).ToListAsync();


                var result = products.GroupBy(x => x.Name).Select(o => new ProductDto
                {
                    Name = o.FirstOrDefault().Name,
                    MadeIn = o.FirstOrDefault().MadeIn,
                    Code = o.FirstOrDefault().Code,
                    Price = o.FirstOrDefault().Price,
                    InStock = o.FirstOrDefault().InStock,
                    Description = o.FirstOrDefault().Description,
                    Title = o.FirstOrDefault().Title,
                    Id = o.FirstOrDefault().Id,
                    Color = o.FirstOrDefault().Color,
                    Size = o.FirstOrDefault().Size,
                    BrandName = o.FirstOrDefault().BrandName,
                    ProductImageUrl = o.FirstOrDefault().ProductImageUrl
                }).ToList().AsQueryable();



                var totalCount = result.Count();

                var output = result.PageBy(input).ToList();

                return new PagedResultDto<ProductDto>(
                    totalCount,
                    output
                );
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<PagedResultDto<ProductDto>> GetAllByCategoryId(GetAllProductsInput input, long categoryId)
        {

            try
            {
                var filteredProducts = _productRepository.GetAll()
                        .Where(x => x.CategoryId == categoryId)
                        .Include(e => e.BrandFk)
                        .Include(e => e.CategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.MadeIn.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Title.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MadeInFilter), e => e.MadeIn == input.MadeInFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(input.MinInStockFilter != null, e => e.InStock >= input.MinInStockFilter)
                        .WhereIf(input.MaxInStockFilter != null, e => e.InStock <= input.MaxInStockFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title == input.TitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BrandNameFilter), e => e.BrandFk != null && e.BrandFk.Name == input.BrandNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryNameFilter), e => e.CategoryFk != null && e.CategoryFk.Name == input.CategoryNameFilter);

                var pagedAndFilteredProducts = filteredProducts
                    .OrderBy(input.Sorting ?? "id asc");


                var productImages = _lookup_productImageRepository.GetAll().GroupBy(x => x.ProductId);

                var listProdImg = new List<ProductImage>();
                //iterate each group        
                foreach (var productImage in productImages)
                {
                    //Each group has a key
                    listProdImg.Add((ProductImage)productImage.FirstOrDefault());
                }
                var products = new List<ProductDto>();
                products = await (from o in pagedAndFilteredProducts

                                  join o2 in _lookup_brandRepository.GetAll() on o.BrandId equals o2.Id into j2
                                  from s2 in j2.DefaultIfEmpty()

                                  join o3 in listProdImg on o.Id equals o3.ProductId

                                  select new ProductDto
                                  {
                                      Name = o.Name,
                                      MadeIn = o.MadeIn,
                                      Code = o.Code,
                                      Price = o.Price,
                                      InStock = o.InStock,
                                      Description = o.Description,
                                      Title = o.Title,
                                      Id = o.Id,
                                      Color = o.Color,
                                      Size = o.Size,
                                      BrandName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                      ProductImageUrl = o3.Url.ToString()
                                  }).ToListAsync();


                var result = products.GroupBy(x => x.Name).Select(o => new ProductDto
                {
                    Name = o.FirstOrDefault().Name,
                    MadeIn = o.FirstOrDefault().MadeIn,
                    Code = o.FirstOrDefault().Code,
                    Price = o.FirstOrDefault().Price,
                    InStock = o.FirstOrDefault().InStock,
                    Description = o.FirstOrDefault().Description,
                    Title = o.FirstOrDefault().Title,
                    Id = o.FirstOrDefault().Id,
                    Color = o.FirstOrDefault().Color,
                    Size = o.FirstOrDefault().Size,
                    BrandName = o.FirstOrDefault().BrandName,
                    ProductImageUrl = o.FirstOrDefault().ProductImageUrl
                }).ToList().AsQueryable();



                var totalCount = result.Count();

                var output = result.PageBy(input).ToList();

                return new PagedResultDto<ProductDto>(
                    totalCount,
                    output
                );
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}