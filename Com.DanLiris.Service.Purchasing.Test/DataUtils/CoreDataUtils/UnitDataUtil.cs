using Com.DanLiris.Service.Purchasing.Lib;
using Com.DanLiris.Service.Purchasing.Lib.Models.CoreModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.DanLiris.Service.Purchasing.Test.DataUtils.CoreDataUtils
{
    public class UnitDataUtil
    {
        private readonly CoreDbContext coreDbContext;
        private readonly DivisionDataUtil divisionDataUtil;

        public UnitDataUtil(CoreDbContext coreDbContext, DivisionDataUtil divisionDataUtil)
        {
            this.coreDbContext = coreDbContext;
            this.divisionDataUtil = divisionDataUtil;
        }

        public Units GetNewData()
        {
            Divisions division = divisionDataUtil.GetTestData();

            DateTime dateTime = DateTime.Now;
            long ticks = dateTime.Ticks;

            return new Units
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

                Description = string.Concat("Description-", ticks),

                DivisionId = division.Id,
                DivisionName = division.Name,
                DivisionCode = division.Code,
            };
        }

        public Units GetTestData()
        {
            var data = GetNewData();



            return data;
        }
    }
}
