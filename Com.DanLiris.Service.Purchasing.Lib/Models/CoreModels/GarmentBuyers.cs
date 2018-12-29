using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Purchasing.Lib.Models.CoreModels
{
    public partial class GarmentBuyers
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Code { get; set; }
        public string Contact { get; set; }
        public string Country { get; set; }
        public string Npwp { get; set; }
        public string Name { get; set; }
        public int? Tempo { get; set; }
        public string Type { get; set; }
        public string Uid { get; set; }
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
