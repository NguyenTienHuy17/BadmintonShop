using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Purchase.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Purchase.Exporting
{
    public class ReturnProdsExcelExporter : EpPlusExcelExporterBase, IReturnProdsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ReturnProdsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetReturnProdForViewDto> returnProds)
        {
            return CreateExcelPackage(
                "ReturnProds.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ReturnProds"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Reason"),
                        (L("Order")) + L("OrderCode")
                        );

                    AddObjects(
                        sheet, 2, returnProds,
                        _ => _.ReturnProd.Reason,
                        _ => _.OrderOrderCode
                        );

                });
        }
    }
}