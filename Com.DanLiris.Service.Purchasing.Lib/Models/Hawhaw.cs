using Com.DanLiris.Service.Purchasing.Lib.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Com.DanLiris.Service.Purchasing.Lib.Models
{
    public class Hawhaw : BaseModel
    {
        [MaxLength(10)]
        public string Nomor { get; set; }
    }
}
