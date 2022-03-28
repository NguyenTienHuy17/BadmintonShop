using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Entity.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Entity.Exporting
{
    public class BrandsExcelExporter : EpPlusExcelExporterBase, IBrandsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BrandsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBrandForViewDto> brands)
        {
            return CreateExcelPackage(
                "Brands.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Brands"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Country"),
                        L("Description"),
                        (L("Image")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, brands,
                        _ => _.Brand.Name,
                        _ => _.Brand.Country,
                        _ => _.Brand.Description,
                        _ => _.ImageName
                        );

                });
        }
    }
}