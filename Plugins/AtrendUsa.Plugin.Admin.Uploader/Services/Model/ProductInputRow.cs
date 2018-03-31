using System.Collections.Generic;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Model
{
    /// <summary>
    ///     Contains only customer required fields
    /// </summary>
    public class ProductInputRow
    {
        public ProductInputRow()
        {
            ProductSpecificationAttributeInput = new List<SpecificationAttributeInput>();
        }

        public string Sku { get; set; }
        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        public string VendorName { get; set; }

        public string ProductTags { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        public string MetaTitle { get; set; }

        public string SearchEngineString { get; set; }

        //public string AllowCustomerReviews { get; set; }

        //public string Published { get; set; }

        public string ManufacturerPartNumber { get; set; }

        public string IsShippingEnabled { get; set; }

        public string IsFreeShipping { get; set; }

        public string AdditionalShippingCharge { get; set; }

        public string TaxCategoryType { get; set; }

        public string ManageInventoryType { get; set; }

        public string StockQuantity { get; set; }

        public string DisplayStockAvailability { get; set; }

        public string DisplayStockQuantity { get; set; }

        public string MinStockQuantity { get; set; }

        public string OrderMinimumQuantity { get; set; }

        public string OrderMaximumQuantity { get; set; }

        public string Price { get; set; }

        public string ProductCost { get; set; }

        public string Weight { get; set; }

        public string Length { get; set; }

        public string Width { get; set; }

        public string Height { get; set; }

        public string Depth { get; set; }

        public string Categories { get; set; }

        public string Brands { get; set; }

        public string ImageFileNames { get; set; }

        public string HasSpecificationAttributes { get; set; }

        public List<SpecificationAttributeInput> ProductSpecificationAttributeInput { get; set; }

        public string UploadStatus { get; set; }

        public string UploadMessage { get; set; }

        public string UploadedProductId { get; set; }

        #region Metadata fields

        public bool Validated { get; set; }

        public bool Processed { get; set; }

        public int RowIndex { get; set; }

        #endregion

        //public List<AttributeInput> ProductAttributeInput { get; set; } 

        //entire list of properties
        //*
        //             "ProductTypeId",
        //             "ParentGroupedProductId",
        //             "VisibleIndividually",
        //             "Name",
        //             "ShortDescription",
        //             "FullDescription",
        //             "VendorId",
        //             "ProductTemplateId",
        //             "ShowOnHomePage",
        //             "MetaKeywords",
        //             "MetaDescription",
        //             "MetaTitle",
        //             "SeName",
        //             "AllowCustomerReviews",
        //             "Published",
        //             "SKU",
        //             "ManufacturerPartNumber",
        //             "Gtin",
        //             "IsGiftCard",
        //             "GiftCardTypeId",
        //             "RequireOtherProducts",
        //             "RequiredProductIds",
        //             "AutomaticallyAddRequiredProducts",
        //             "IsDownload",
        //             "DownloadId",
        //             "UnlimitedDownloads",
        //             "MaxNumberOfDownloads",
        //             "DownloadActivationTypeId",
        //             "HasSampleDownload",
        //             "SampleDownloadId",
        //             "HasUserAgreement",
        //             "UserAgreementText",
        //             "IsRecurring",
        //             "RecurringCycleLength",
        //             "RecurringCyclePeriodId",
        //             "RecurringTotalCycles",
        //             "IsShipEnabled",
        //             "IsFreeShipping",
        //             "AdditionalShippingCharge",
        //             "DeliveryDateId",
        //             "WarehouseId",
        //             "IsTaxExempt",
        //             "TaxCategoryId",
        //             "ManageInventoryMethodId",
        //             "StockQuantity",
        //             "DisplayStockAvailability",
        //             "DisplayStockQuantity",
        //             "MinStockQuantity",
        //             "LowStockActivityId",
        //             "NotifyAdminForQuantityBelow",
        //             "BackorderModeId",
        //             "AllowBackInStockSubscriptions",
        //             "OrderMinimumQuantity",
        //             "OrderMaximumQuantity",
        //             "AllowedQuantities",
        //             "AllowAddingOnlyExistingAttributeCombinations",
        //             "DisableBuyButton",
        //             "DisableWishlistButton",
        //             "AvailableForPreOrder",
        //             "PreOrderAvailabilityStartDateTimeUtc",
        //             "CallForPrice",
        //             "Price",
        //             "OldPrice",
        //             "ProductCost",
        //             "SpecialPrice",
        //             "SpecialPriceStartDateTimeUtc",
        //             "SpecialPriceEndDateTimeUtc",
        //             "CustomerEntersPrice",
        //             "MinimumCustomerEnteredPrice",
        //             "MaximumCustomerEnteredPrice",
        //             "Weight",
        //             "Length",
        //             "Width",
        //             "Height",
        //             "CreatedOnUtc",
        //             "CategoryIds",
        //             "ManufacturerIds",
        //             "Picture1",
        //             "Picture2",
        //             "Picture3",
        ////*
    }
}