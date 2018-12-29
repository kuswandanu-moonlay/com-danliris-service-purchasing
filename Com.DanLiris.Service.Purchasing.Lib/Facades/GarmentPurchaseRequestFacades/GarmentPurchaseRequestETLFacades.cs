using Com.DanLiris.Service.Purchasing.Lib.Interfaces;
using Com.DanLiris.Service.Purchasing.Lib.Models.CoreModels;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentPurchaseRequestModel;
using Com.DanLiris.Service.Purchasing.Lib.Models.LocalGarmentMerchandiserModels;
using Com.DanLiris.Service.Purchasing.Lib.Services;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.NewIntegrationViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentPurchaseRequestFacades
{
    public class GarmentPurchaseRequestETLFacade : IGarmentPurchaseRequestETLFacade
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IdentityService identityService;

        private readonly LocalGarmentMerchandiserDbContext merchandiserDbContext;
        private readonly CoreDbContext coreDbContext;

        private readonly DbSet<Porder> porderDbSet;
        private readonly DbSet<Budget> budgetDbSet;
        private readonly DbSet<Porder1> porder1DbSet;
        private readonly DbSet<Budget1> budget1DbSet;

        public readonly DbSet<Divisions> divisionsDbSet;
        public readonly DbSet<GarmentBuyers> garmentBuyersDbSet;
        public readonly DbSet<GarmentCategories> garmentCategoriesDbSet;
        public readonly DbSet<GarmentProducts> garmentProductsDbSet;
        public readonly DbSet<UnitOfMeasurements> unitOfMeasurementsDbSet;
        public readonly DbSet<Units> unitsDbSet;


        private readonly IGarmentPurchaseRequestFacade purchaseRequestFacade;

        public GarmentPurchaseRequestETLFacade(IServiceProvider serviceProvider, LocalGarmentMerchandiserDbContext merchandiserDbContext, CoreDbContext coreDbContext)
        {
            this.serviceProvider = serviceProvider;
            identityService = (IdentityService)serviceProvider.GetService(typeof(IdentityService));

            this.merchandiserDbContext = merchandiserDbContext;
            this.coreDbContext = coreDbContext;

            porderDbSet = this.merchandiserDbContext.Set<Porder>();
            budgetDbSet = this.merchandiserDbContext.Set<Budget>();
            porder1DbSet = this.merchandiserDbContext.Set<Porder1>();
            budget1DbSet = this.merchandiserDbContext.Set<Budget1>();

            divisionsDbSet = this.coreDbContext.Set<Divisions>();
            garmentBuyersDbSet = this.coreDbContext.Set<GarmentBuyers>();
            garmentCategoriesDbSet = this.coreDbContext.Set<GarmentCategories>();
            garmentProductsDbSet = this.coreDbContext.Set<GarmentProducts>();
            unitOfMeasurementsDbSet = this.coreDbContext.Set<UnitOfMeasurements>();
            unitsDbSet = this.coreDbContext.Set<Units>();

            purchaseRequestFacade = (IGarmentPurchaseRequestFacade)serviceProvider.GetService(typeof(IGarmentPurchaseRequestFacade));
        }

        public async Task<int> Run(string tables = "", string month = "", string year = "", BuyerViewModel buyer = null)
        {
            try
            {
                IQueryable<ExtractedDataPOrder> extract = Extract(tables, month, year, buyer == null ? "" : buyer.Code);
                List<GarmentPurchaseRequest> transform = Transform(extract);
                int load = await Load(transform);

                return load;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private IQueryable<ExtractedDataPOrder> Extract(string tables, string month, string year, string buyer)
        {
            DateTime dateFilter;
            if (!DateTime.TryParseExact(string.Concat(month, " ", year), "MMMM yyyy", new CultureInfo("en-EN"), DateTimeStyles.None, out dateFilter))
                dateFilter = DateTime.MaxValue;

            //dateFilter = dateFilter.AddDays(26);

            IQueryable<ExtractedDataPOrder> Query = null;

            if (tables.Trim() == "Budget & POrder")
            {
                Query = from porder in porderDbSet
                        join budget in budgetDbSet on porder.Nopo equals budget.Po
                        let Ro = porder.Ro ?? ""
                        //where int.Parse(Ro.Substring(0, Ro.Length > 2 ? 2 : Ro.Length)) > 17
                        //    && ((porder.Post ?? "").Trim().ToUpper() == "Y" || (porder.Post ?? "").Trim().ToUpper() == "M")
                        //    && porder.TgValid.GetValueOrDefault() >= dateFilter
                        //    && porder.Harga == 0
                        //    && (porder.Nopo ?? "").Trim() != ""
                        //    && (porder.CodeSpl ?? "").Trim() == ""
                        //    && porder.Qty > 0
                        //    && (string.IsNullOrWhiteSpace(buyer) ? true : porder.Buyer == buyer)
                        select new ExtractedDataPOrder
                        {
                            UId = porder.IdPo.ToString(),
                            CreatedBy = porder.Userin,
                            CreatedUtc = porder.Tglin.GetValueOrDefault() + TimeSpan.Parse(string.IsNullOrWhiteSpace(porder.Jamin) ? "00:00:00" : porder.Jamin),
                            LastModifiedBy = porder.Usered,
                            LastModifiedUtc = porder.Tgled.GetValueOrDefault() + TimeSpan.Parse(string.IsNullOrWhiteSpace(porder.Jamed) ? "00:00:00" : porder.Jamed),

                            Article = porder.Art,
                            BuyerCode = porder.Buyer,
                            Date = porder.TgValid,
                            RONo = porder.Ro,
                            ShipmentDate = porder.Shipment,
                            UnitCode = porder.Konf == "K.1" ? "C2A" :
                                        porder.Konf == "K.2" ? "C2B" :
                                        porder.Konf == "K.3" ? "C2C" :
                                        porder.Konf == "K.4" ? "C1A" :
                                        porder.Konf == "K.5" ? "C1B" :
                                        porder.Konf,

                            BudgetPrice = budget.Harga,
                            CategoryCode = porder.Cat,
                            PO_SerialNumber = porder.Nopo,
                            ProductCode = (porder.Kodeb ?? "").Trim() == (porder.Cat ?? "").Trim() ? string.Concat((porder.Kodeb ?? "").Trim(), "001") : (porder.Kodeb ?? "").Trim(),
                            ProductRemark = (porder.Ketr + Environment.NewLine + porder.Kett + Environment.NewLine + porder.Kett2 + Environment.NewLine + porder.Kett3 + Environment.NewLine + porder.Kett4 + Environment.NewLine + porder.Kett5),
                            Quantity = porder.Qty,
                            UomUnit = (porder.Satb ?? "").Trim(),
                        };
            }
            else if (tables.Trim() == "Budget1 & POrder1")
            {
                Query = from porder in porder1DbSet
                        join budget in budget1DbSet on porder.Nopo equals budget.Po
                        let Ro = porder.Ro ?? ""
                        where int.Parse(Ro.Substring(0, Ro.Length > 2 ? 2 : Ro.Length)) > 17
                            && ((porder.Post ?? "").Trim().ToUpper() == "Y" || (porder.Post ?? "").Trim().ToUpper() == "M")
                            && porder.TgValid.GetValueOrDefault() >= dateFilter
                            && porder.Harga == 0
                            && (porder.Nopo ?? "").Trim() != ""
                            && (porder.CodeSpl ?? "").Trim() == ""
                            && porder.Qty > 0
                            && (string.IsNullOrWhiteSpace(buyer) ? true : porder.Buyer == buyer)
                        select new ExtractedDataPOrder
                        {
                            UId = porder.Id.ToString(),
                            CreatedBy = porder.Userin,
                            CreatedUtc = porder.Tglin.GetValueOrDefault() + TimeSpan.Parse(string.IsNullOrWhiteSpace(porder.Jamin) ? "00:00:00" : porder.Jamin),
                            LastModifiedBy = porder.Usered,
                            LastModifiedUtc = porder.Tgled.GetValueOrDefault() + TimeSpan.Parse(string.IsNullOrWhiteSpace(porder.Jamed) ? "00:00:00" : porder.Jamed),

                            Article = porder.Art,
                            BuyerCode = porder.Buyer,
                            Date = porder.TgValid,
                            RONo = porder.Ro,
                            ShipmentDate = porder.Shipment,
                            UnitCode = porder.Konf == "K.1" ? "C2A" :
                                        porder.Konf == "K.2" ? "C2B" :
                                        porder.Konf == "K.3" ? "C2C" :
                                        porder.Konf == "K.4" ? "C1A" :
                                        porder.Konf == "K.5" ? "C1B" :
                                        porder.Konf,

                            BudgetPrice = budget.Harga,
                            CategoryCode = porder.Cat,
                            PO_SerialNumber = porder.Nopo,
                            ProductCode = (porder.Kodeb ?? "").Trim() == (porder.Cat ?? "").Trim() ? string.Concat((porder.Kodeb ?? "").Trim(), "001") : (porder.Kodeb ?? "").Trim(),
                            ProductRemark = (porder.Ketr + Environment.NewLine + porder.Kett + Environment.NewLine + porder.Kett2 + Environment.NewLine + porder.Kett3 + Environment.NewLine + porder.Kett4 + Environment.NewLine + porder.Kett5),
                            Quantity = porder.Qty,
                            UomUnit = (porder.Satb ?? "").Trim(),
                        };
            }

            return Query;
        }

        private List<GarmentPurchaseRequest> Transform(IQueryable<ExtractedDataPOrder> extractedDatas)
        {
            var GroupQuery = from query in extractedDatas
                             group query by query.RONo into groupQuery
                             let data = groupQuery.OrderBy(x => x.CreatedUtc).First()
                             select new GarmentPurchaseRequest
                             {
                                 Active = true,
                                 CreatedAgent = "manager",
                                 CreatedBy = data.CreatedBy,
                                 CreatedUtc = data.CreatedUtc,
                                 DeletedAgent = "",
                                 DeletedBy = "",
                                 DeletedUtc = DateTime.MinValue,
                                 IsDeleted = false,
                                 LastModifiedAgent = "manager",
                                 LastModifiedBy = data.LastModifiedBy,
                                 LastModifiedUtc = data.LastModifiedUtc,
                                 IsUsed = false,

                                 PRNo = string.Concat("PR", groupQuery.Key),
                                 Article = data.Article,
                                 BuyerCode = data.BuyerCode,
                                 BuyerId = garmentBuyersDbSet.Where(x => x.Code == data.BuyerCode).Select(x => x.Id).FirstOrDefault().ToString(),
                                 BuyerName = garmentBuyersDbSet.Where(x => x.Code == data.BuyerCode).Select(x => x.Name).FirstOrDefault(),
                                 Date = new DateTimeOffset(data.Date.GetValueOrDefault(), new TimeSpan(7, 0, 0)),
                                 IsPosted = true,
                                 RONo = groupQuery.Key,
                                 ShipmentDate = new DateTimeOffset(data.ShipmentDate.GetValueOrDefault(), new TimeSpan(7, 0, 0)),
                                 UnitCode = data.UnitCode,
                                 UnitId = unitsDbSet.Where(x => x.Code == data.UnitCode).Select(x => x.Id).FirstOrDefault().ToString(),
                                 UnitName = unitsDbSet.Where(x => x.Code == data.UnitCode).Select(x => x.Name).FirstOrDefault(),

                                 Items = groupQuery.Select(item => new GarmentPurchaseRequestItem
                                 {
                                     UId = item.UId,
                                     Active = true,
                                     CreatedAgent = "manager",
                                     CreatedBy = item.CreatedBy,
                                     CreatedUtc = item.CreatedUtc,
                                     DeletedAgent = "",
                                     DeletedBy = "",
                                     DeletedUtc = DateTime.MinValue,
                                     IsDeleted = false,
                                     LastModifiedAgent = "manager",
                                     LastModifiedBy = item.LastModifiedBy,
                                     LastModifiedUtc = item.LastModifiedUtc,
                                     IsUsed = false,

                                     BudgetPrice = (long)item.BudgetPrice,
                                     //CategoryCode = b.CategoryCode,
                                     CategoryId = garmentCategoriesDbSet.Where(x => x.Code == data.CategoryCode).Select(x => x.Id).FirstOrDefault().ToString(),
                                     CategoryName = garmentCategoriesDbSet.Where(x => x.Code == data.CategoryCode).Select(x => x.Name).FirstOrDefault(),
                                     PO_SerialNumber = item.PO_SerialNumber,
                                     ProductCode = item.ProductCode,
                                     ProductId = garmentProductsDbSet.Where(x => x.Code == item.ProductCode).Select(x => x.Id).FirstOrDefault().ToString(),
                                     ProductName = garmentProductsDbSet.Where(x => x.Code == item.ProductCode).Select(x => x.Name).FirstOrDefault(),
                                     ProductRemark = item.ProductRemark,
                                     Quantity = (long)item.Quantity,
                                     Status = "Belum diterima Pembelian",
                                     UomUnit = item.UomUnit,
                                     UomId = unitOfMeasurementsDbSet.Where(x => x.Unit == item.UomUnit).Select(x => x.Id).FirstOrDefault().ToString(),
                                 })
                                .ToList()
                             };

            List<GarmentPurchaseRequest> transformedDatas = new List<GarmentPurchaseRequest>();
            transformedDatas.AddRange(GroupQuery);

            return transformedDatas;
        }

        private async Task<int> Load(List<GarmentPurchaseRequest> transformedDatas)
        {
            foreach (var data in transformedDatas)
            {
                var existingDataByRONo = purchaseRequestFacade.ReadByRONo(data.RONo);
                if (existingDataByRONo == null)
                {
                    await purchaseRequestFacade.Create(data, identityService.Username);
                }
                else
                {
                    foreach (var item in data.Items)
                    {
                        var existingItem = existingDataByRONo.Items.FirstOrDefault(x => x.UId == item.UId);
                        if (existingItem == null)
                        {
                            existingDataByRONo.Items.Add(item);
                        }
                    }
                    await purchaseRequestFacade.Update((int)existingDataByRONo.Id, existingDataByRONo, identityService.Username);
                }
            }

            return 0;
        }
    }

    public class ExtractedDataPOrder
    {
        public string UId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedUtc { get; set; }
        public string Article { get; set; }
        public string BuyerCode { get; set; }
        public DateTime? Date { get; set; }
        public string RONo { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public string UnitCode { get; set; }
        public decimal? BudgetPrice { get; set; }
        public string CategoryCode { get; set; }
        public string PO_SerialNumber { get; set; }
        public string ProductCode { get; set; }
        public string ProductRemark { get; set; }
        public decimal? Quantity { get; set; }
        public string UomUnit { get; set; }
    }
}
