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

        public GarmentProductDataUtil(CoreDbContext coreDbContext)
        {
            this.coreDbContext = coreDbContext;
        }

        public GarmentProducts GetNewData()
        {
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
