using Com.DanLiris.Service.Purchasing.Lib;
using Com.DanLiris.Service.Purchasing.Lib.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.DanLiris.Service.Purchasing.Test.DataUtils.CoreDataUtils
{
    public class GarmentProductDataUtil
    {
        private readonly CoreDbContext coreDbContext;
        private readonly UnitOfMeasurementDataUtil unitOfMeasurementDataUtil;

        public GarmentProductDataUtil(CoreDbContext coreDbContext, UnitOfMeasurementDataUtil unitOfMeasurementDataUtil)
        {
            this.coreDbContext = coreDbContext;
            this.unitOfMeasurementDataUtil = unitOfMeasurementDataUtil;
        }

        public GarmentProducts GetNewData()
        {
            UnitOfMeasurements uom = unitOfMeasurementDataUtil.GetTestData();

            DateTime dateTime = DateTime.Now;
            long ticks = dateTime.Ticks;

            return new GarmentProducts
            {
                Uid = string.Concat("Uid-", ticks),

                Active = true,
                CreatedAgent = string.Concat("CreatedAgent-", ticks),
                CreatedBy = string.Concat("CreatedBy-", ticks),
                CreatedUtc = dateTime,
                DeletedAgent = "",
                DeletedBy = "",
                DeletedUtc = DateTime.MinValue,
                IsDeleted = false,
                LastModifiedAgent = "",
                LastModifiedBy = "",
                LastModifiedUtc = DateTime.MinValue,

                Code = string.Concat("Code-", ticks),
                Name = string.Concat("Name-", ticks),

                UomId = uom.Id,
                UomUnit = uom.Unit,

                Composition = string.Concat("Composition-", ticks),
                Const = string.Concat("Const-", ticks),
                ProductType = string.Concat("ProductType-", ticks),
                Remark = string.Concat("Remark-", ticks),
                Tags = string.Concat("Tags-", ticks),
                Width = string.Concat("Width-", ticks),
                Yarn = string.Concat("Yarn-", ticks),
            };
        }

        public GarmentProducts GetTestData()
        {
            var data = GetNewData();

            coreDbContext.Set<GarmentProducts>().Add(data);
            coreDbContext.SaveChanges();

            return data;
        }
    }
}
