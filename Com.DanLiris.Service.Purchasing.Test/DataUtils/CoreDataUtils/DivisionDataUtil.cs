using Com.DanLiris.Service.Purchasing.Lib;
using Com.DanLiris.Service.Purchasing.Lib.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.DanLiris.Service.Purchasing.Test.DataUtils.CoreDataUtils
{
    public class DivisionDataUtil
    {
        private readonly CoreDbContext coreDbContext;

        public DivisionDataUtil(CoreDbContext coreDbContext)
        {
            this.coreDbContext = coreDbContext;
        }

        public Divisions GetNewData()
        {
            DateTime dateTime = DateTime.Now;
            long ticks = dateTime.Ticks;

            return new Divisions
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
                Description = string.Concat("Description-", ticks),
                Name = string.Concat("Name-", ticks),
            };
        }

        public Divisions GetTestData()
        {
            var data = GetNewData();

            coreDbContext.Set<Divisions>().Add(data);
            coreDbContext.SaveChanges();

            return data;
        }
    }
}
