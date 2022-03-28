using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Common.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Common.Exporting
{
    public class ProductImagesExcelExporter : EpPlusExcelExporterBase, IProductImagesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductImagesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductImageForViewDto> productImages)
        {
            return CreateExcelPackage(
                "ProductImages.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ProductImages"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        (L("Product")) + L("Name"),
                        (L("Image")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, productImages,
                        _ => _.ProductName,
                        _ => _.ImageName
                        );

                });
        }
    }
}