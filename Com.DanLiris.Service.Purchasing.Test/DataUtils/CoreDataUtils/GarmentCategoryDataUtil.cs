using Com.DanLiris.Service.Purchasing.Lib;
using Com.DanLiris.Service.Purchasing.Lib.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.DanLiris.Service.Purchasing.Test.DataUtils.CoreDataUtils
{
    public class GarmentCategoryDataUtil
    {
        private readonly CoreDbContext coreDbContext;
        private readonly UnitOfMeasurementDataUtil unitOfMeasurementDataUtil;

        public GarmentCategoryDataUtil(CoreDbContext coreDbContext, UnitOfMeasurementDataUtil unitOfMeasurementDataUtil)
        {
            this.coreDbContext = coreDbContext;
            this.unitOfMeasurementDataUtil = unitOfMeasurementDataUtil;
        }

        public GarmentCategories GetNewData()
        {
            UnitOfMeasurements uom = unitOfMeasurementDataUtil.GetTestData();

            DateTime dateTime = DateTime.Now;
            long ticks = dateTime.Ticks;

            return new GarmentCategories
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
                
                CodeRequirement = string.Concat("CodeRequirement-", ticks),
            };
        }

        public GarmentCategories GetTestData()
        {
            var data = GetNewData();

            coreDbContext.Set<GarmentCategories>().Add(data);
            coreDbContext.SaveChanges();

            return data;
        }
    }
}
