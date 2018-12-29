using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Purchasing.Lib.Models.CoreModels
{
    public partial class UnitOfMeasurements
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public string Uid { get; set; }
        public string Unit { get; set; }
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
    }
}
