using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AtrendUsa.Plugin.Admin.Uploader.Services.Extensions;
using AtrendUsa.Plugin.Admin.Uploader.Services.Model;
using NPOI.SS.UserModel;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Helpers
{
    internal static class Converter
    {
        private static readonly Dictionary<int, string> InputRowMap = new Dictionary<int, string>
        {
            {0, "Sku"},
            {1, "Name"},
            {2, "ShortDescription"},
            {3, "FullDescription"},
            {4, "VendorName"},
            {5, "ProductTags"},
            {6, "MetaKeywords"},
            {7, "MetaDescription"},
            {8, "MetaTitle"},
            {9, "SearchEngineString"},
            {10, "ManufacturerPartNumber"},
            {11, "IsShippingEnabled"},
            {12, "IsFreeShipping"},
            {13, "AdditionalShippingCharge"},
            {14, "TaxCategoryType"},
            {15, "ManageInventoryType"},
            {16, "StockQuantity"},
            {17, "DisplayStockAvailability"},
            {18, "DisplayStockQuantity"},
            {19, "MinStockQuantity"},
            {20, "OrderMinimumQuantity"},
            {21, "OrderMaximumQuantity"},
            {22, "Price"},
            {23, "ProductCost"},
            {24, "Weight"},
            {25, "Width"},
            {26, "Height"},
            {27, "Depth"},
            {28, "Categories"},
            {29, "Brands"},
            {30, "ImageFileNames"},
            {31, "HasSpecificationAttributes"},
            {32, "Width"},
            {33, "IncludeInFilter|ShowOnProducts"},
            {34, "Height"},
            {35, "IncludeInFilter|ShowOnProducts"},
            {36, "Depth"},
            {37, "IncludeInFilter|ShowOnProducts"},
            {38, "Vent Width"},
            {39, "IncludeInFilter|ShowOnProducts"},
            {40, "Vent Height"},
            {41, "IncludeInFilter|ShowOnProducts"},
            {42, "Vent Depth"},
            {43, "IncludeInFilter|ShowOnProducts"},
            {44, "Mounting Depth"},
            {45, "IncludeInFilter|ShowOnProducts"},
            {46, "Gross Cubic Feet"},
            {47, "IncludeInFilter|ShowOnProducts"},
            {48, "Net Cubic Feet"},
            {49, "IncludeInFilter|ShowOnProducts"},
            {50, "HZ"},
            {51, "IncludeInFilter|ShowOnProducts"},
            {52, "Excursion"},
            {53, "IncludeInFilter|ShowOnProducts"},
            {54, "Speaker OEM"},
            {55, "IncludeInFilter|ShowOnProducts"},
            {56, "Speaker Size"},
            {57, "IncludeInFilter|ShowOnProducts"},
            {58, "Speaker Model"},
            {59, "IncludeInFilter|ShowOnProducts"},
            {60, "Vented"},
            {61, "IncludeInFilter|ShowOnProducts"},
            {62, "Sealed"},
            {63, "IncludeInFilter|ShowOnProducts"},
            {64, "Enclosure Type"},
            {65, "IncludeInFilter|ShowOnProducts"},
            {66, "Car Make"},
            {67, "IncludeInFilter|ShowOnProducts"},
            {68, "Cab Type"},
            {69, "IncludeInFilter|ShowOnProducts"},
            {70, "Cab Model"},
            {71, "IncludeInFilter|ShowOnProducts"},
            {72, "UploadStatus"},
            {73, "UploadMessage"},
            {74, "UploadedProductId"}
        };

        /// <summary>
        ///     Converts Excell Row to POCO object. This is index based, depends on order of cells ion the file and properties in
        ///     object
        /// </summary>
        /// <param name="source"></param>
        /// <param name="rowIndex"></param>
        /// /// <param name="header"></param>
        /// <returns></returns>
        public static ProductInputRow Convert(IRow source, int rowIndex, IRow header)
        {
            if (source == null) return null;
            var inputRow = new ProductInputRow();
            List<PropertyInfo> props = inputRow.GetProperties();
            const int specificationAtrributesIdexStart = 32;
            int specificationAtrributesIdexEnd = header.LastCellNum - 4;

            //process product props
            ProcessProductBase(source, specificationAtrributesIdexStart, props, inputRow);

            //process product specification attributes props
            ProcessSpecificationAttributes(source, inputRow, specificationAtrributesIdexStart,
                specificationAtrributesIdexEnd, header);

            inputRow.RowIndex = rowIndex;

            return inputRow;
        }

        private static void ProcessProductBase(IRow source, int specificationAtrributesIdexStart,
            List<PropertyInfo> props,
            ProductInputRow inputRow)
        {
            for (int i = 0; i < specificationAtrributesIdexStart; i++)
            {
                ICell cell = source.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellType(CellType.String);
                string cellValue = cell.StringCellValue.Trim();
                PropertyInfo prop = props.First(x => x.Name == InputRowMap[i]);
                prop.SetValue(inputRow, cellValue, null);
            }
        }

        private static void ProcessSpecificationAttributes(IRow source, ProductInputRow inputRow,
            int specificationAtrributesIdexStart, int specificationAtrributesIdexEnd, IRow header)
        {
            if (!inputRow.HasSpecificationAttributes.InvariantEquals("yes")) return;

            for (int i = specificationAtrributesIdexStart; i <= specificationAtrributesIdexEnd; i += 2)
            {
                ICell attrNameCell = header.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                attrNameCell.SetCellType(CellType.String);

                string attrName = attrNameCell.StringCellValue.Trim();

                //var mapAttr = InputRowMap.FirstOrDefault(p => p.Value == attrName);

                //if (!attrName.InvariantEquals(mapAttr.Value)) continue;

                ICell attrCell = source.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                attrCell.SetCellType(CellType.String);
                string attrValue = attrCell.StringCellValue.Trim();

                if (attrValue.InvariantEquals("n/a")) continue;

                ICell settingsCell = source.GetCell(i + 1, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                settingsCell.SetCellType(CellType.String);
                string[] settingsValue = settingsCell.StringCellValue.Trim().Split('|');

                foreach (string attrVal in attrValue.Split('|'))
                {
                    inputRow.ProductSpecificationAttributeInput.Add(new SpecificationAttributeInput
                    {
                        Name = attrName.Trim(),
                        IncludeInFilter = settingsValue[0],
                        ShowOnProductPage = settingsValue[1],
                        Value = attrVal.Trim()
                    });
                }
            }
        }
    }
}