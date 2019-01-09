using AutoMapper;
using Com.DanLiris.Service.Purchasing.Lib.Helpers;
using Com.DanLiris.Service.Purchasing.Lib.Interfaces;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentInternalPurchaseOrderModel;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.GarmentInternalPurchaseOrderViewModel;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.IO;
using System.Data;
using System.Globalization;


namespace Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentInternalPurchaseOrderFacades
{
    public class GarmentPurchaseOrderMonitoringAllFacade : IGarmentPurchaseOrderMonitoringAllFacade
    {
        private readonly PurchasingDbContext dbContext;
        private readonly DbSet<GarmentInternalPurchaseOrder> dbSet;

        public GarmentPurchaseOrderMonitoringAllFacade(PurchasingDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<GarmentInternalPurchaseOrder>();
        }
        
        public IQueryable<GarmentPurchaseOrderMonitoringAllViewModel> GetReportQuery(string unit, string category, string epoNo, string roNo, string prNo, string doNo, string supplier, string staff, DateTime? dateFrom, DateTime? dateTo, string status, int offset)
        {

            DateTime DateFrom = dateFrom == null ? new DateTime(1970, 1, 1) : (DateTime)dateFrom;
            DateTime DateTo = dateTo == null ? DateTime.Now : (DateTime)dateTo;
            //             //IPO
            var Query = (from a in dbContext.GarmentInternalPurchaseOrders
                         join b in dbContext.GarmentInternalPurchaseOrderItems on a.Id equals b.GPOId
                         //PR
                         join d in dbContext.GarmentPurchaseRequestItems on b.GPRItemId equals d.Id
                         join c in dbContext.GarmentPurchaseRequests on d.GarmentPRId equals c.Id
                         //EPO
                         join e in dbContext.GarmentExternalPurchaseOrderItems on b.GPOId equals e.POId into f
                         from epoItem in f.DefaultIfEmpty()
                         join g in dbContext.GarmentExternalPurchaseOrders on epoItem.GarmentEPOId equals g.Id into h
                         from epo in h.DefaultIfEmpty()
                          //DO
                         join yy in dbContext.GarmentDeliveryOrderDetails on epoItem.POId equals yy.POId into zz
                         from doDetail in zz.DefaultIfEmpty()
                         join m in dbContext.GarmentDeliveryOrderItems on doDetail.GarmentDOItemId equals m.Id into n
                         from doItem in n.DefaultIfEmpty()
                         join l in dbContext.GarmentDeliveryOrders on doItem.GarmentDOId equals l.Id into p
                         from DO in p.DefaultIfEmpty()
                             //BC
                         join i in dbContext.GarmentBeacukaiItems on DO.Id equals i.GarmentDOId into j
                         from bcItem in j.DefaultIfEmpty()
                         join dd in dbContext.GarmentBeacukais on bcItem.BeacukaiId equals dd.Id into k
                         from bc in k.DefaultIfEmpty()
                             //INVOICE
                         join yy in dbContext.GarmentInvoiceDetails on epo.Id equals yy.EPOId into ee
                         from invDetail in ee.DefaultIfEmpty()
                         join ff in dbContext.GarmentInvoiceItems on DO.Id equals ff.DeliveryOrderId into gg
                         from invItem in gg.DefaultIfEmpty()
                         join hh in dbContext.GarmentInvoices on invItem.InvoiceId equals hh.Id into ii
                         from invoice in ii.DefaultIfEmpty()
                             //URN
                         join q in dbContext.GarmentUnitReceiptNotes on DO.Id equals q.DOId into r
                         from urn in r.DefaultIfEmpty()
                         join s in dbContext.GarmentUnitReceiptNoteItems on doDetail.Id equals s.DODetailId into t
                         from urnItem in t.DefaultIfEmpty()
                             //NI
                         join u in dbContext.GarmentInternNoteItems on invoice.Id equals u.InvoiceId into v
                         from upoItem in v.DefaultIfEmpty()
                         join w in dbContext.GarmentInternNotes on upoItem.GarmentINId equals w.Id into x
                         from upo in x.DefaultIfEmpty()
                         join y in dbContext.GarmentInternNoteDetails on DO.Id equals y.DOId into z
                         from upoDetail in z.DefaultIfEmpty()
                             //CORRECTION
                         join cc in dbContext.GarmentCorrectionNoteItems on epo.Id equals cc.EPOId into dd
                         from corrItem in dd.DefaultIfEmpty()
                         join aa in dbContext.GarmentCorrectionNotes on corrItem.GCorrectionId equals aa.Id into bb
                         from corr in bb.DefaultIfEmpty()

                         where a.IsDeleted == false && b.IsDeleted == false
                               && c.IsDeleted == false && d.IsDeleted == false
                               && epo.IsDeleted == false && epoItem.IsDeleted == false
                               && DO.IsDeleted == false && doItem.IsDeleted == false && doDetail.IsDeleted == false
                               && urn.IsDeleted == false && urnItem.IsDeleted == false
                               && upo.IsDeleted == false && upoItem.IsDeleted == false && upoDetail.IsDeleted == false
                               && corr.IsDeleted == false && corrItem.IsDeleted == false
                               && a.PRDate.AddHours(offset).Date >= DateFrom.Date
                               && a.PRDate.AddHours(offset).Date <= DateTo.Date
                               && a.UnitCode == (string.IsNullOrWhiteSpace(unit) ? a.UnitCode : unit)
                               && b.CategoryName == (string.IsNullOrWhiteSpace(category) ? b.CategoryName : category)
                               && epo.EPONo == (string.IsNullOrWhiteSpace(epoNo) ? epo.EPONo : epoNo)
                               && a.RONo == (string.IsNullOrWhiteSpace(roNo) ? a.RONo : roNo)
                               && b.PO_SerialNumber == (string.IsNullOrWhiteSpace(prNo) ? b.PO_SerialNumber : prNo)
                               && DO.DONo == (string.IsNullOrWhiteSpace(doNo) ? DO.DONo : doNo)
                               && epo.SupplierCode == (string.IsNullOrWhiteSpace(supplier) ? epo.SupplierCode : supplier)
                               && b.Status == (string.IsNullOrWhiteSpace(status) ? b.Status : status)
                               && a.CreatedBy == (string.IsNullOrWhiteSpace(staff) ? a.CreatedBy : staff)
                               && b.Quantity > 0

                         select new GarmentPurchaseOrderMonitoringAllViewModel
                         {
                             prNo = a.PRNo,
                             prDate = a.PRDate,
                             unitName = a.UnitName,
                             refPo = b.PO_SerialNumber,
                             RO_Number = a.RONo,
                             BuyerCode = a.BuyerCode,
                             BuyerName = a.BuyerName,
                             ShipmentDate = a.ShipmentDate,
                             Article = a.Article,
                             category = b.CategoryName,
                             productCode = b.ProductCode,
                             productName = b.ProductName,
                             remark = epoItem != null ? epo.IsPosted == true ? epoItem.Remark : "" : "",
                             quantity = epoItem != null ? epoItem.DefaultQuantity : 0 ,
                             uom = epoItem != null  ? epoItem.DealUomUnit : "-" ,
                             budgetprice = epoItem != null ? epo.IsPosted == true ? epoItem.BudgetPrice : 0 : 0,
                             pricePerDealUnit = epoItem != null ? epo.IsPosted==true? epoItem.PricePerDealUnit : 0 : 0,
                             priceTotal = epoItem != null ? epo.IsPosted == true ? epoItem.DealQuantity * epoItem.PricePerDealUnit : 0 :0,
                             kurs = epo.CurrencyCode,
                             rate = epo.CurrencyRate,
                             priceTotalIDR = epoItem != null ? epo.IsPosted == true ? epoItem.DealQuantity * epoItem.PricePerDealUnit * epo.CurrencyRate : 0 : 0,
                             supplierCode = epo!=null ? epo.IsPosted == true?epo.SupplierCode : "-" : "-",
                             supplierName = epo != null ? epo.IsPosted == true ? epo.SupplierName : "-" : "-",
                             receivedDatePO = a.CreatedUtc,
                             epoNo = epo != null ? epo.IsPosted == true ? epo.EPONo : "-" : "-",
                             epoDate = epo != null ? epo.IsPosted == true ? epo.OrderDate : new DateTime(1970, 1, 1) : new DateTime(1970, 1, 1) ,
                             epoExpectedDeliveryDate = epo.DeliveryDate,
                             useVat = epo.IsUseVat == true ? "Ya" : "TIdak",
                             useIncomeTax = epo.IsIncomeTax == true ? "Ya" : "TIdak",
                             IncomeTaxDesc = epo.IsIncomeTax == true ? epo.IncomeTaxName : "-", 
                             doNo = DO.DONo ?? "-",
                             doDate = DO == null ? new DateTime(1970, 1, 1) : DO.DODate,
                             doDeliveryDate = DO == null ? new DateTime(1970, 1, 1) : DO.ArrivalDate,
                             useCustoms = DO.IsCustoms == true ? "Ya" : "TIdak",
                             doQuantity = doDetail != null ? doDetail.DOQuantity : 0,
                             doUom = doDetail != null ? doDetail.UomUnit : "-",
                             remainingQuantity = doDetail != null ? doDetail.DOQuantity - doDetail.ReceiptQuantity : doDetail.DOQuantity - doDetail.ReceiptQuantity,
                             doDetailId = corrItem == null ? 0 : upoDetail.Id,
                             customsNo = bc.BeacukaiNo ?? "-",
                             customsDate = bc == null ? new DateTime(1970, 1, 1) : bc.BeacukaiDate,
                             urnDate = urn == null ? new DateTime(1970, 1, 1) : urn.ReceiptDate,
                             urnNo = urn.URNNo ?? "-",
                             urnQuantity = urnItem == null ? 0 : urnItem.ReceiptQuantity,
                             urnUom = urnItem.UomUnit ?? "-",
                             invoiceDate = invoice == null ? new DateTime(1970, 1, 1) : invoice.InvoiceDate,
                             invoiceNo = invoice.InvoiceNo ?? "-",
                             vatDate = invoice != null ? invoice.UseVat ? invoice.VatDate : new DateTime(1970, 1, 1) : new DateTime(1970, 1, 1),
                             vatNo = invoice.VatNo ?? "-",
                             vatValue = invoice != null && invoice != null ? invoice.UseVat ? 0.1 * invoice.TotalAmount : 0 : 0,
                             incomeTaxDate = invoice == null ? new DateTime(1970, 1, 1) : invoice.IncomeTaxDate,
                             incomeTaxNo = invoice.IncomeTaxNo ?? null,
                             incomeTaxName = invoice.UseIncomeTax == true ? invoice.IncomeTaxName : "-",
                             incomeTaxRate = invoice.UseIncomeTax == true ? invoice.IncomeTaxRate : 0,
                             incomeTaxValue = invItem != null && invoice != null ? invoice.IsPayTax ? (invoice.IncomeTaxRate * invoice.TotalAmount / 100) : 0 : 0,                            
                             upoDate = upo == null ? new DateTime(1970, 1, 1) : upo.INDate,
                             upoNo = upo.INNo ?? "-",
                             upoPriceTotal = upoDetail == null ? 0 : upoDetail.PriceTotal,
                             dueDate = upoDetail == null ? new DateTime(1970, 1, 1) : upoDetail.PaymentDueDate,
                             correctionDate = corr == null ? new DateTime(1970, 1, 1) : corr.CorrectionDate,
                             correctionNo = corr.CorrectionNo ?? null,
                             correctionType = corr.CorrectionType ?? null,
                             valueCorrection = corrItem == null ? 0 : corr.CorrectionType == "Harga Total" ? corrItem.PriceTotalAfter - corrItem.PriceTotalBefore : corr.CorrectionType == "Harga Satuan" ? (corrItem.PricePerDealUnitAfter - corrItem.PricePerDealUnitBefore) * corrItem.Quantity : corr.CorrectionType == "Jumlah" ? corrItem.PriceTotalAfter * -1 : 0,
                             priceAfter = corrItem == null ? 0 : corrItem.PricePerDealUnitAfter,
                             priceBefore = corrItem == null ? 0 : corrItem.PricePerDealUnitBefore,
                             priceTotalAfter = corrItem == null ? 0 : corrItem.PriceTotalAfter,
                             priceTotalBefore = corrItem == null ? 0 : corrItem.PricePerDealUnitBefore,
                             qtyCorrection = corrItem == null ? 0 : corrItem.Quantity,
                             status = b.Status,
                             staff = a.CreatedBy,
                             useInternalPO = d.IsUsed == true ? "Sudah" : "Belum",
                         }
                         );
            

            Dictionary<long, string> qry = new Dictionary<long, string>();
            Dictionary<long, string> qryDate = new Dictionary<long, string>();
            Dictionary<long, string> qryQty = new Dictionary<long, string>();
            Dictionary<long, string> qryType = new Dictionary<long, string>();

            List<GarmentPurchaseOrderMonitoringAllViewModel> listData = new List<GarmentPurchaseOrderMonitoringAllViewModel>();

            var index = 0;
            var iop = Query.ToList();
            foreach (GarmentPurchaseOrderMonitoringAllViewModel data in iop)
            {
                string value;
                if (data.doDetailId != 0)
                {
                    string correctionDate = data.correctionDate == new DateTime(1970, 1, 1) ? "-" : data.correctionDate.ToOffset(new TimeSpan(offset, 0, 0)).ToString("dd MMM yyyy", new CultureInfo("id-ID"));
                    if (data.correctionNo != null)
                    {
                        if (qry.TryGetValue(data.doDetailId, out value))
                        {
                            qry[data.doDetailId] += (index).ToString() + ". " + data.correctionNo + "\n";
                            qryType[data.doDetailId] += (index).ToString() + ". " + data.correctionType + "\n";
                            qryDate[data.doDetailId] += (index).ToString() + ". " + correctionDate + "\n";
                            qryQty[data.doDetailId] += (index).ToString() + ". " + String.Format("{0:N2}", data.valueCorrection) + "\n";
                            index++;
                        }
                        else
                        {
                            index = 1;
                            qry[data.doDetailId] = (index).ToString() + ". " + data.correctionNo + "\n";
                            qryType[data.doDetailId] = (index).ToString() + ". " + data.correctionType + "\n";
                            qryDate[data.doDetailId] = (index).ToString() + ". " + correctionDate + "\n";
                            qryQty[data.doDetailId] = (index).ToString() + ". " + String.Format("{0:N2}", data.valueCorrection) + "\n";
                            index++;
                        }
                    }
                }
                else
                {
                    listData.Add(data);
                }

            }
            foreach(var corrections in qry)
            {
                foreach(GarmentPurchaseOrderMonitoringAllViewModel data in Query.ToList())
                {
                    if( corrections.Key==data.doDetailId)
                    {
                        data.correctionNo = qry[data.doDetailId];
                        data.correctionType= qryType[data.doDetailId];
                        data.correctionQtys = qryQty[data.doDetailId];
                        data.correctionDates = qryDate[data.doDetailId];
                        listData.Add(data);
                        break;
                    }
                }
            }

            var op = qry;
            return Query=listData.AsQueryable();
            //return Query;

        }
        
        public Tuple<List<GarmentPurchaseOrderMonitoringAllViewModel>, int> GetReport(string unit, string category, string epoNo, string roNo, string prNo, string doNo, string supplier, string staff, DateTime? dateFrom, DateTime? dateTo, string status, int page, int size, string Order, int offset)
        {
            var Query = GetReportQuery(unit, category, epoNo, roNo, prNo, doNo, supplier, staff, dateFrom, dateTo, status, offset);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            if (OrderDictionary.Count.Equals(0))
            {
                Query = Query.OrderByDescending(b => b.LastModifiedUtc);
            }
           
            Pageable<GarmentPurchaseOrderMonitoringAllViewModel> pageable = new Pageable<GarmentPurchaseOrderMonitoringAllViewModel>(Query, page - 1, size);
            List<GarmentPurchaseOrderMonitoringAllViewModel> Data = pageable.Data.ToList<GarmentPurchaseOrderMonitoringAllViewModel>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData);
        }

        public MemoryStream GenerateExcel(string unit, string category, string epoNo, string roNo, string prNo, string doNo, string supplier, string staff, DateTime? dateFrom, DateTime? dateTo, string status, int offset)
        {
            var Query = GetReportQuery(unit, category, epoNo, roNo, prNo, doNo, supplier, staff, dateFrom, dateTo, status, offset);
            Query = Query.OrderByDescending(b => b.LastModifiedUtc);
            DataTable result = new DataTable();
            result.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Purchase Request", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "No Purchase Request", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Unit", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "No Ref Purchase Request", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Dibuat PO Internal", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "No RO", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Buyer", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Nama Buyer", DataType = typeof(String) });

            result.Columns.Add(new DataColumn() { ColumnName = "Shipment Garment", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Artikel", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Kategori", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Kode Barang", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Nama Barang", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Keterangan Barang", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Jumlah Barang", DataType = typeof(double) });
            result.Columns.Add(new DataColumn() { ColumnName = "Satuan Barang", DataType = typeof(String) });

            result.Columns.Add(new DataColumn() { ColumnName = "Harga Budget", DataType = typeof(double) });
            result.Columns.Add(new DataColumn() { ColumnName = "Harga Satuan Beli", DataType = typeof(double) });
            result.Columns.Add(new DataColumn() { ColumnName = "Harga Total", DataType = typeof(double) });
            result.Columns.Add(new DataColumn() { ColumnName = "Mata Uang", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Kurs", DataType = typeof(double) });
            result.Columns.Add(new DataColumn() { ColumnName = "Harga Total IDR", DataType = typeof(double) });
            result.Columns.Add(new DataColumn() { ColumnName = "Kode Supplier", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Nama Supplier", DataType = typeof(String) });

            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Terima PO Internal", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "No PO Eksternal", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal PO Eksternal", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Target Datang", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Dikenakan PPN", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Dikenakan PPH", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Nama PPH", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "No Surat Jalan", DataType = typeof(String) });

            result.Columns.Add(new DataColumn() { ColumnName = "Dikenakan Bea Cukai", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Surat Jalan", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Datang Barang", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Jumlah Barang Datang", DataType = typeof(double) });
            result.Columns.Add(new DataColumn() { ColumnName = "Satuan", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Jumlah Barang Sisa", DataType = typeof(double) });
            result.Columns.Add(new DataColumn() { ColumnName = "No Bea Cukai", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Bea Cukai", DataType = typeof(String) });

            result.Columns.Add(new DataColumn() { ColumnName = "No Bon Terima Unit", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Bon Terima Unit", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Jumlah Barang Diterima", DataType = typeof(double) });
            result.Columns.Add(new DataColumn() { ColumnName = "Satuan Barang Diterima", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "No Invoice", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Invoice", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "No PPN", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal PPN", DataType = typeof(String) });

            result.Columns.Add(new DataColumn() { ColumnName = "Nilai PPN", DataType = typeof(double) });
            result.Columns.Add(new DataColumn() { ColumnName = "No PPH", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal PPH", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Nama PPH", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Rate PPH", DataType = typeof(double) });
            result.Columns.Add(new DataColumn() { ColumnName = "Nilai PPH", DataType = typeof(double) });
            result.Columns.Add(new DataColumn() { ColumnName = "No Nota Intern", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Nota Intern", DataType = typeof(String) });

            result.Columns.Add(new DataColumn() { ColumnName = "Nilai Nota Intern", DataType = typeof(double) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Jatuh Tempo", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "No Koreksi", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Koreksi", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Nilai Koreksi", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Keterangan Koreksi", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Status", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Staff Pembelian", DataType = typeof(String) });

            if (Query.ToArray().Count() == 0)
                result.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, "", 0, 0, 0, "", "", "", 0, 0, "", "", "", "", "", "", "", "", "", "", "", 0, "", 0, "", "", "", "", 0, "", "", "", "", "", 0, "", "", "", 0, 0, "", "", 0, "", "", "", "", "", "", ""); // to allow column name to be generated properly for empty data as template
            else
            {
                int index = 0;
                foreach (var item in Query)
                {
                    index++;
                    result.Rows.Add(index, item.prNo, item.prDate, item.unitName, item.refPo, item.useInternalPO, item.RO_Number, item.BuyerCode, item.BuyerName,
                                    item.ShipmentDate, item.Article, item.category, item.productCode, item.productName, item.remark, item.quantity, item.uom,
                                    item.budgetprice, item.pricePerDealUnit, item.priceTotal, item.kurs, item.rate, item.priceTotalIDR, item.supplierCode, item.supplierName,
                                    item.receivedDatePO, item.epoNo, item.epoDate, item.epoExpectedDeliveryDate, item.useVat, item.useIncomeTax, item.IncomeTaxDesc, item.doNo, 
                                    item.doDate, item.doDeliveryDate, item.useCustoms, item.doQuantity, item.doUom, item.remainingQuantity, item.customsNo, item.customsDate, 
                                    item.urnNo, item.urnDate, item.urnQuantity, item.urnUom, item.invoiceNo, item.invoiceDate, item.vatNo, item.vatDate, 
                                    item.vatValue, item.incomeTaxNo, item.incomeTaxDate, item.incomeTaxName, item.incomeTaxRate, item.incomeTaxValue, item.upoNo, item.upoDate,                                     
                                    item.upoPriceTotal, item.dueDate, item.correctionNo, item.correctionDate, item.correctionQtys, item.correctionType, item.status, item.staff);
                }
            }

            return Excel.CreateExcel(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(result, "Territory") }, true);
        }
    }
}
