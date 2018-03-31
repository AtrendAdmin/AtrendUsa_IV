using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using AtrendUsa.Plugin.Admin.Uploader.Services.Extensions;
using AtrendUsa.Plugin.Admin.Uploader.Services.Interfaces;
using AtrendUsa.Plugin.Admin.Uploader.Services.Model;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Seo;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Implementation
{
    public class ProductResolver : IProductResolver
    {
        #region Private Variables

        private readonly ISpecificationAttributeService _attributeService;
        private readonly ICategoryService _categoryService;
        private readonly string _imageStore = ConfigurationManager.AppSettings["ProductImageStore"];
        private readonly IManufacturerService _manufacturerService;
        private readonly IPictureService _pictureService;
        private readonly IProductService _productService;
        private readonly IProductTagService _tagService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly ILogger _logger;
        #endregion

        //product service
        //image service
        //specification attribute service
        //category service ?
        //manufacturers services ?
        //ILocalizationService
        //ICustomerActivityService
        //Tag Services

        public ProductResolver(IProductService productService, IProductTagService tagService,
            ISpecificationAttributeService attributeService,
            ICategoryService categoryService, IManufacturerService manufacturerService, IPictureService pictureService,
            IUrlRecordService urlRecordService, ILogger logger)
        {
            if (productService == null) throw new ArgumentNullException("productService");
            if (pictureService == null) throw new ArgumentNullException("pictureService");
            if (tagService == null) throw new ArgumentNullException("tagService");
            if (attributeService == null) throw new ArgumentNullException("attributeService");
            if (categoryService == null) throw new ArgumentNullException("categoryService");
            if (manufacturerService == null) throw new ArgumentNullException("manufacturerService");
            if (urlRecordService == null) throw new ArgumentNullException("urlRecordService");

            _productService = productService;
            _tagService = tagService;
            _attributeService = attributeService;
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
            _pictureService = pictureService;
            _urlRecordService = urlRecordService;
            _logger = logger;
        }

        #region Implementation of IResolver<in ProductResolveInput,out Product>

        //todo: Add Localization!!!
        public Product Resolve(ProductResolveInput input)
        {
            if (input == null) throw new ArgumentNullException("input");

            Product product = ResolveProduct(input);

            ResolveProductCategories(input, product);

            ResolveProductBrand(input, product);

            ResolveProductSpecificationAttributes(input, product);

            ResolveProductTags(input, product);

            ResolveProductPirctures(input, product);


            _productService.UpdateHasTierPricesProperty(product);
            _productService.UpdateHasDiscountsApplied(product);

            return product;
        }

        #endregion

        #region Private Methods

        private Product ResolveProduct(ProductResolveInput input)
        {
            //check if product exists
            Product product = _productService.GetProductBySku(input.ProductInputRow.Sku);
            bool isNew = false;
            if (product == null)
            {
                product = new Product();
                isNew = true;
            }

            product.Sku = input.ProductInputRow.Sku;
            product.Name = input.ProductInputRow.Name;
            product.ShortDescription = input.ProductInputRow.ShortDescription;
            product.FullDescription = input.ProductInputRow.FullDescription;
            product.VendorId = 0; //todo: resolve
            product.ProductTemplateId = 1;
            product.ShowOnHomePage = false;
            product.MetaKeywords = input.ProductInputRow.MetaKeywords;
            product.MetaDescription = input.ProductInputRow.MetaDescription;
            product.MetaTitle = input.ProductInputRow.MetaTitle;
            product.AllowCustomerReviews = true;
            product.ManufacturerPartNumber = input.ProductInputRow.ManufacturerPartNumber;
            product.IsShipEnabled = input.ProductInputRow.IsShippingEnabled.InvariantEquals("yes");
            product.IsFreeShipping = input.ProductInputRow.IsFreeShipping.InvariantEquals("yes");
            product.AdditionalShippingCharge = Convert.ToDecimal(input.ProductInputRow.AdditionalShippingCharge);
            product.ManageInventoryMethod = ManageInventoryMethod.DontManageStock; // todo correct, add resolver
            product.TaxCategoryId = 2; //electronics and software todo: add resolver
            product.StockQuantity = int.Parse(input.ProductInputRow.StockQuantity);
            product.DisplayStockAvailability = input.ProductInputRow.DisplayStockAvailability.InvariantEquals("yes");
            product.DisplayStockQuantity = input.ProductInputRow.DisplayStockQuantity.InvariantEquals("yes");
            product.MinStockQuantity = int.Parse(input.ProductInputRow.MinStockQuantity);
            product.OrderMinimumQuantity = int.Parse(input.ProductInputRow.OrderMinimumQuantity);
            product.OrderMaximumQuantity = int.Parse(input.ProductInputRow.OrderMaximumQuantity);
            product.Price = decimal.Parse(input.ProductInputRow.Price);
            product.ProductCost = decimal.Parse(input.ProductInputRow.ProductCost);
            product.Weight = decimal.Parse(input.ProductInputRow.Weight);
            product.Width = decimal.Parse(input.ProductInputRow.Width);
            product.Height = decimal.Parse(input.ProductInputRow.Height);
            product.Length = decimal.Parse(input.ProductInputRow.Depth);
            product.ProductTypeId = (int) ProductType.SimpleProduct;
            product.ParentGroupedProductId = 0; //?? todo: support multiple levels
            product.VisibleIndividually = true; // todo: add to template
            product.Published = true;


            if (isNew)
            {
                product.CreatedOnUtc = DateTime.UtcNow;
                product.UpdatedOnUtc = DateTime.UtcNow;
                _productService.InsertProduct(product);
            }
            else
            {
                product.UpdatedOnUtc = DateTime.UtcNow;
                _productService.UpdateProduct(product);
            }

            _urlRecordService.SaveSlug(product,
                product.ValidateSeName(input.ProductInputRow.SearchEngineString, product.Name, true), 0);
            return product;
        }

        private void ResolveProductPirctures(ProductResolveInput input, Product product)
        {
            if (!string.IsNullOrWhiteSpace(input.ProductInputRow.ImageFileNames))
            {
                string[] productImages = input.ProductInputRow.ImageFileNames.Split('|');

                for (int i = 0; i < productImages.Length; i++)
                {
                    byte[] fileData = ReadFile(productImages[i]);

                    if (fileData != null)
                    {
                        string mimeType = GetMimeTypeFromFilePath(productImages[i]);

                        if (!_pictureService.GetPicturesByProductId(product.Id).Any(x => _pictureService.LoadPictureBinary(x).SequenceEqual(fileData)))
                        {
                            var newPicture = _pictureService.InsertPicture(fileData, mimeType, _pictureService.GetPictureSeName(product.Name), true);

                            product.ProductPictures.Add(new ProductPicture
                            {
                                Picture = newPicture,
                                DisplayOrder = i + 1
                            });
                        }
                    }
                    else // if no pictures found move product to drafts
                    {
                        _logger.Warning(string.Format("Image: {0} not found for: {1}", productImages[i], product.Sku));
                    }
                }

                product.Published = product.ProductPictures.Count > 0;
                _productService.UpdateProduct(product);
            }
        }

        public byte[] ReadFile(string url)
        {
            byte[] result = null;

            try
            {
                using (var client = new WebClient())
                {
                    result = client.DownloadData(url);
                }
            }
            catch
            {
            }

            return result;
        }

        private string GetMimeTypeFromFilePath(string filePath)
        {
            string mimeType = MimeMapping.GetMimeMapping(filePath);

            //little hack here because MimeMapping does not contain all mappings (e.g. PNG)
            if (mimeType == "application/octet-stream")
                mimeType = "image/jpeg";

            return mimeType;
        }

        private void ResolveProductTags(ProductResolveInput input, Product product)
        {
            //resolve tags
            //todo: prep for update, currently supports only addition
            string[] tags = input.ProductInputRow.ProductTags.Split('|');
            foreach (string tag in tags)
            {
                ProductTag productTag = _tagService.GetProductTagByName(tag.Trim());
                if (productTag == null)
                {
                    productTag = new ProductTag
                    {
                        Name = tag.Trim()
                    };
                    _tagService.InsertProductTag(productTag);
                }
                if (product.ProductTags.All(x => x.Id != productTag.Id))
                {
                    product.ProductTags.Add(productTag);
                    _productService.UpdateProduct(product);
                }

            }
        }

        private void ResolveProductSpecificationAttributes(ProductResolveInput input, Product product)
        {
            IPagedList<SpecificationAttribute> existingAttributes = _attributeService.GetSpecificationAttributes();
            IList<ProductSpecificationAttribute> existingProductSpecificationAttributes =
                _attributeService.GetProductSpecificationAttributesByProductId(product.Id);
            foreach (SpecificationAttributeInput attr in input.ProductInputRow.ProductSpecificationAttributeInput)
            {
                SpecificationAttribute spesAttr = existingAttributes.FirstOrDefault(x => x.Name == attr.Name);
                SpecificationAttributeOption spesAttrOption;
                if (spesAttr != null)
                {
                    //check options
                    spesAttrOption = spesAttr.SpecificationAttributeOptions.FirstOrDefault(x => x.Name == attr.Value);
                    if (spesAttrOption == null)
                    {
                        spesAttrOption = new SpecificationAttributeOption
                        {
                            Name = attr.Value,
                            DisplayOrder = 1,
                            SpecificationAttribute = spesAttr,
                            SpecificationAttributeId = spesAttr.Id
                        };
                        _attributeService.InsertSpecificationAttributeOption(spesAttrOption);
                    }
                }
                else
                {
                    spesAttr = new SpecificationAttribute
                    {
                        Name = attr.Name,
                        DisplayOrder = 1
                    };
                    _attributeService.InsertSpecificationAttribute(spesAttr);

                    spesAttrOption = new SpecificationAttributeOption
                    {
                        Name = attr.Value,
                        DisplayOrder = 1,
                        SpecificationAttribute = spesAttr,
                        SpecificationAttributeId = spesAttr.Id
                    };
                    _attributeService.InsertSpecificationAttributeOption(spesAttrOption);
                }

                if (
                    existingProductSpecificationAttributes.FirstOrDefault(
                        x => x.SpecificationAttributeOptionId == spesAttrOption.Id) == null)
                {
                    _attributeService.InsertProductSpecificationAttribute(new ProductSpecificationAttribute
                    {
                        AllowFiltering = attr.IncludeInFilter.InvariantEquals("yes"),
                        ShowOnProductPage = attr.ShowOnProductPage.InvariantEquals("yes"),
                        DisplayOrder = 1,
                        Product = product,
                        ProductId = product.Id,
                        SpecificationAttributeOption = spesAttrOption,
                        SpecificationAttributeOptionId = spesAttrOption.Id
                    });
                }
            }
        }

        private void ResolveProductBrand(ProductResolveInput input, Product product)
        {
            Manufacturer brand = ResolveBrand(input.ProductInputRow.Brands.Split('|')[0].Trim());

            if (product.ProductManufacturers.FirstOrDefault(x => x.ManufacturerId == brand.Id) == null)
            {
                var productManufacturer = new ProductManufacturer
                {
                    ProductId = product.Id,
                    ManufacturerId = brand.Id,
                    IsFeaturedProduct = false,
                    DisplayOrder = 1
                };
                _manufacturerService.InsertProductManufacturer(productManufacturer);
            }
        }

        private Manufacturer ResolveBrand(string brandName)
        {
            Manufacturer brand = _manufacturerService.GetAllManufacturers(brandName).FirstOrDefault();
            if (brand != null) return brand;
            brand = new Manufacturer
            {
                Name = brandName,
                AllowCustomersToSelectPageSize = true,
                //Description = brandName,
                MetaDescription = brandName,
                MetaKeywords = brandName,
                MetaTitle = brandName,
                ManufacturerTemplateId = 1,
                LimitedToStores = false,
                DisplayOrder = 1,
                PageSize = 8,
                PageSizeOptions = "4,8,12",
                SubjectToAcl = false,
                Published = true,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            _manufacturerService.InsertManufacturer(brand);
            _urlRecordService.SaveSlug(brand, brand.ValidateSeName(brandName, brandName, true), 0);

            return _manufacturerService.GetAllManufacturers(brandName).First();
        }


        private void ResolveProductCategories(ProductResolveInput input, Product product)
        {
            List<Category> categories = ResolveCategories(input);
            IEnumerable<Category> filteredCategories = categories.Any(x => x.ParentCategoryId != 0)
                ? categories.Where(x => x.ParentCategoryId != 0)
                : categories;

            foreach (Category category in filteredCategories)
            {
                if (product.ProductCategories.FirstOrDefault(x => x.CategoryId == category.Id) == null)
                {
                    var productCategory = new ProductCategory
                    {
                        ProductId = product.Id,
                        CategoryId = category.Id,
                        IsFeaturedProduct = false,
                        DisplayOrder = 1,
                    };
                    _categoryService.InsertProductCategory(productCategory);
                }
            }
        }

        private List<Category> ResolveCategories(ProductResolveInput input)
        {
            var productCategories = new List<Category>();
            int i = 0;

            foreach (string category in input.ProductInputRow.Categories.Split('|'))
            {
                productCategories.Add(ResolveCategory(category.Trim(),
                    productCategories.Any() ? productCategories[i - 1] : null));
                i++;
            }
            return productCategories;
        }

        private Category ResolveCategory(string categoryName, Category parentCategory)
        {
            Category currentCategory = _categoryService.GetAllCategories(categoryName).FirstOrDefault();
            if (currentCategory != null) return currentCategory;
            currentCategory = new Category
            {
                Name = categoryName,
                MetaTitle = categoryName,
                MetaDescription = categoryName,
                MetaKeywords = categoryName,
                AllowCustomersToSelectPageSize = true,
                HasDiscountsApplied = false,
                CategoryTemplateId = 1,
                //Description = categoryName,
                DisplayOrder = 1,
                IncludeInTopMenu = false,
                PageSize = 8,
                PageSizeOptions = "12,8,4",
                ParentCategoryId = parentCategory != null ? parentCategory.Id : 0,
                ShowOnHomePage = false,
                SubjectToAcl = false,
                LimitedToStores = false,
                Published = true,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            _categoryService.InsertCategory(currentCategory);
            _urlRecordService.SaveSlug(currentCategory, currentCategory.ValidateSeName(categoryName, categoryName, true),
                0);

            return _categoryService.GetAllCategories(categoryName).First();
        }

        #endregion
    }
}