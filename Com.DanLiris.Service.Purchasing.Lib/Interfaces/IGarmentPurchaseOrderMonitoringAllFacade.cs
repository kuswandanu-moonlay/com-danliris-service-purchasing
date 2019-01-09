using Com.DanLiris.Service.Purchasing.Lib.ViewModels.GarmentInternalPurchaseOrderViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Com.DanLiris.Service.Purchasing.Lib.Interfaces
{
    public interface IGarmentPurchaseOrderMonitoringAllFacade
    {
        Tuple<List<GarmentPurchaseOrderMonitoringAllViewModel>, int> GetReport(string unit, string category, string epoNo, string roNo, string prNo, string doNo, string supplier, string staff, DateTime? dateFrom, DateTime? dateTo, string status, int page, int size, string Order, int offset);
        MemoryStream GenerateExcel(string unit, string category, string epoNo, string roNo, string prNo, string doNo, string supplier, string staff, DateTime? dateFrom, DateTime? dateTo, string status, int offset);
    }
}
