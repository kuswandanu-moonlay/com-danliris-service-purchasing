using Com.DanLiris.Service.Purchasing.Lib;
using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentPurchaseRequestFacades;
using Com.DanLiris.Service.Purchasing.Lib.Models.CoreModels;
using Com.DanLiris.Service.Purchasing.Lib.Models.LocalGarmentMerchandiserModels;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.CoreDataUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.DanLiris.Service.Purchasing.Test.DataUtils.LocalGarmentMerchandiserDataUtils
{
    public class LocalGarmentMerchandiserDataUtil
    {
        private readonly LocalGarmentMerchandiserDbContext merchandiserDbContext;

        private readonly UnitDataUtil unitDataUtil;
        private readonly GarmentBuyerDataUtil garmentBuyerDataUtil;
        private readonly GarmentCategoryDataUtil garmentCategoryDataUtil;
        private readonly GarmentProductDataUtil garmentProductDataUtil;

        public LocalGarmentMerchandiserDataUtil(LocalGarmentMerchandiserDbContext merchandiserDbContext, CoreDbContext coreDbContext)
        {
            this.merchandiserDbContext = merchandiserDbContext;

            DivisionDataUtil divisionDataUtil = new DivisionDataUtil(coreDbContext);
            unitDataUtil = new UnitDataUtil(coreDbContext, divisionDataUtil);

            UnitOfMeasurementDataUtil unitOfMeasurementDataUtil = new UnitOfMeasurementDataUtil(coreDbContext);
            garmentCategoryDataUtil = new GarmentCategoryDataUtil(coreDbContext, unitOfMeasurementDataUtil);
            garmentProductDataUtil = new GarmentProductDataUtil(coreDbContext, unitOfMeasurementDataUtil);

            garmentBuyerDataUtil = new GarmentBuyerDataUtil(coreDbContext);
        }

        private (Units units, GarmentCategories garmentCategories, GarmentProducts garmentProducts, GarmentBuyers garmentBuyers) DataCore()
        {
            Units units = unitDataUtil.GetTestData();
            GarmentCategories garmentCategories = garmentCategoryDataUtil.GetTestData();
            GarmentProducts garmentProducts = garmentProductDataUtil.GetTestData();
            GarmentBuyers garmentBuyers = garmentBuyerDataUtil.GetTestData();

            return (units, garmentCategories, garmentProducts, garmentBuyers);
        }

        public (Porder Porder, Budget Budget) GetNewDataPorder()
        {
            var dataCore = DataCore();

            Guid guid = Guid.NewGuid();

            var porder = new Porder
            {
                IdPo = guid,
                Ro = string.Concat("Ro-", guid.ToString()),
                Po = string.Concat("Po-", guid.ToString()),
                Nopo = string.Concat("Nopo-", guid.ToString()),
                Buyer = dataCore.garmentBuyers.Code,
                Konf = dataCore.units.Code,
                Cat = dataCore.garmentCategories.Code,
                Kodeb = dataCore.garmentProducts.Code,
                Satb = dataCore.garmentProducts.UomUnit
            };

            var budget = new Budget
            {
                Po = porder.Nopo,
            };

            return (porder, budget);
        }

        public (Porder1 Porder1, Budget1 Budget1) GetNewDataPorder1()
        {
            Guid guid = Guid.NewGuid();

            var porder = new Porder1
            {
                Id = guid,
                Ro = string.Concat("Ro-", guid.ToString()),
                Po = string.Concat("Po-", guid.ToString()),
                Nopo = string.Concat("Nopo-", guid.ToString()),
            };

            var budget = new Budget1
            {
                Po = porder.Nopo,
            };

            return (porder, budget);
        }

        public (Porder Porder, Budget Budget) GetTestDataPorder()
        {
            var data = GetNewDataPorder();
            var porder = data.Porder;
            var budget = data.Budget;

            merchandiserDbContext.Set<Porder>().Add(porder);
            merchandiserDbContext.Set<Budget>().Add(budget);
            merchandiserDbContext.SaveChanges();

            return (porder, budget);
        }

        public (Porder1 Porder1, Budget1 Budget1) GetTestDataPorder1()
        {
            var data = GetNewDataPorder1();
            var porder = data.Porder1;
            var budget = data.Budget1;

            merchandiserDbContext.Set<Porder1>().Add(porder);
            merchandiserDbContext.Set<Budget1>().Add(budget);
            merchandiserDbContext.SaveChanges();

            return (porder, budget);
        }
    }
}
