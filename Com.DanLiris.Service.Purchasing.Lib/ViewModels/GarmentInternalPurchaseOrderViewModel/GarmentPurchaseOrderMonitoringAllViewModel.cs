using Com.DanLiris.Service.Purchasing.Lib.Utilities;
using System;

using System.Collections.Generic;
using System.Text;

namespace Com.DanLiris.Service.Purchasing.Lib.ViewModels.GarmentInternalPurchaseOrderViewModel
{
    public class GarmentPurchaseOrderMonitoringAllViewModel : BaseViewModel
    {
        public string prNo { get; set; }
        public DateTimeOffset prDate { get; set; }
        public string unitCode { get; set; }
        public string unitName { get; set; }
        public string refPo { get; set; }
        public string RO_Number { get; set; }
        public string BuyerCode { get; set; }
        public string BuyerName { get; set; }
        public DateTimeOffset ShipmentDate { get; set; }
        public string Article { get; set; }
        public string category { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public string remark { get; set; }
        public double quantity { get; set; }
        public string uom { get; set; }
        public double? budgetprice { get; set; }
        public double? pricePerDealUnit { get; set; }
        public double? priceTotal { get; set; }
        public string kurs { get; set; }
        public double? rate { get; set; }
        public double? priceTotalIDR { get; set; }
        public string supplierCode { get; set; }
        public string supplierName { get; set; }
        public DateTimeOffset receivedDatePO { get; set; }

        //EPO
        public DateTimeOffset epoDate { get; set; }
        public DateTimeOffset epoExpectedDeliveryDate { get; set; }
        public string epoNo { get; set; }
        public string useVat { get; set; }
        public string useIncomeTax { get; set; }
        public string IncomeTaxDesc { get; set; }
        
        //DO
        public DateTimeOffset doDate { get; set; }
        public DateTimeOffset doDeliveryDate { get; set; }
        public string doNo { get; set; }
        public string useCustoms { get; set; }
        public double doQuantity { get; set; }
        public string doUom { get; set; }
        public double? remainingQuantity { get; set; }
        public long doDetailId { get; set; }

        //CUSTOMS
        public DateTimeOffset customsDate { get; set; }
        public string customsNo { get; set; }

        //URN
        public DateTimeOffset urnDate { get; set; }
        public string urnNo { get; set; }
        public decimal urnQuantity { get; set; }
        public string urnUom { get; set; }
       
        //INVOICE
        public DateTimeOffset invoiceDate { get; set; }
        public string invoiceNo { get; set; }
        
        //PPN
        public DateTimeOffset vatDate { get; set; }
        public string vatNo { get; set; }
        public double vatValue { get; set; }

        //PPH
        public DateTimeOffset? incomeTaxDate { get; set; }
        public string incomeTaxNo { get; set; }
        public string incomeTaxName { get; set; }
        public double? incomeTaxRate { get; set; }
        public double incomeTaxValue { get; set; }

        //NI
        public DateTimeOffset upoDate { get; set; }
        public string upoNo { get; set; }
        public double upoPriceTotal { get; set; }
        public DateTimeOffset dueDate { get; set; }

        //CORRECTION
        public DateTimeOffset correctionDate { get; set; }
        public string correctionNo { get; set; }
        public string correctionType { get; set; }
        public decimal priceBefore { get; set; }
        public decimal priceAfter { get; set; }
        public decimal priceTotalAfter { get; set; }
        public decimal priceTotalBefore { get; set; }
        public decimal qtyCorrection { get; set; }
        public decimal valueCorrection { get; set; }
        public string correctionRemark { get; set; }
        public string correctionDates { get; set; }
        public string correctionQtys { get; set; }
        public string status { get; set; }
        public string staff { get; set; }
        public long LastModifiedUtc { get; set; }
        public string useInternalPO { get; set; }
    }
}
