using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Entity.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Entity.Exporting
{
    public class ProductsExcelExporter : EpPlusExcelExporterBase, IProductsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductForViewDto> products)
        {
            return CreateExcelPackage(
                "Products.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Products"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("MadeIn"),
                        L("Code"),
                        L("Price"),
                        L("InStock"),
                        L("Description"),
                        L("Title"),
                        (L("Image")) + L("Name"),
                        (L("Brand")) + L("Name"),
                        (L("Category")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, products,
                        _ => _.Product.Name,
                        _ => _.Product.MadeIn,
                        _ => _.Product.Code,
                        _ => _.Product.Price,
                        _ => _.Product.InStock,
                        _ => _.Product.Description,
                        _ => _.Product.Title,
                        _ => _.ImageName,
                        _ => _.BrandName,
                        _ => _.CategoryName
                        );

                });
        }
    }
}