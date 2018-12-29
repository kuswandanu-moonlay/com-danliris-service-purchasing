using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Purchasing.Lib.Models.CoreModels
{
    public partial class GarmentProducts
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public string Code { get; set; }
        public string Const { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public string Tags { get; set; }
        public string Uid { get; set; }
        public int? UomId { get; set; }
        public string UomUnit { get; set; }
        public string Width { get; set; }
        public string Yarn { get; set; }
        public string CreatedAgent { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string DeletedAgent { get; set; }
        public string DeletedBy { get; set; }
        public DateTime DeletedUtc { get; set; }
        public bool IsDeleted { get; set; }
        public string LastModifiedAgent { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedUtc { get; set; }
        public string ProductType { get; set; }
        public string Composition { get; set; }
    }
}
