using Com.DanLiris.Service.Purchasing.Lib;
using Com.DanLiris.Service.Purchasing.Lib.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.DanLiris.Service.Purchasing.Test.DataUtils.CoreDataUtils
{
    public class UnitOfMeasurementDataUtil
    {
        private readonly CoreDbContext coreDbContext;

        public UnitOfMeasurementDataUtil(CoreDbContext coreDbContext)
        {
            this.coreDbContext = coreDbContext;
        }

        public UnitOfMeasurements GetNewData()
        {
            DateTime dateTime = DateTime.Now;
            long ticks = dateTime.Ticks;

            return new UnitOfMeasurements
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

                Unit = string.Concat("Unit-", ticks),
            };
        }

        public UnitOfMeasurements GetTestData()
        {
            var data = GetNewData();

            coreDbContext.Set<UnitOfMeasurements>().Add(data);
            coreDbContext.SaveChanges();

            return data;
        }
    }
}
