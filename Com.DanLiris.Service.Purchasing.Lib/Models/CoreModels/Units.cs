using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Purchasing.Lib.Models.CoreModels
{
    public partial class Units
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public string Name { get; set; }
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
        public string DivisionCode { get; set; }
    }
}
