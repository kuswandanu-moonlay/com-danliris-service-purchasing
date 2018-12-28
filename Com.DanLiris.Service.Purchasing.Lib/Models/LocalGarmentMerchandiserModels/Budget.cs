using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Purchasing.Lib.Models.LocalGarmentMerchandiserModels
{
    public partial class Budget
    {
        public Guid Id { get; set; }
        public string Urut { get; set; }
        public string Ro { get; set; }
        public string Konf { get; set; }
        public string Seksi { get; set; }
        public string Codeby { get; set; }
        public string Article { get; set; }
        public string Cat { get; set; }
        public string Series { get; set; }
        public string Kodeb { get; set; }
        public string Kodec { get; set; }
        public string Po { get; set; }
        public string Ketr { get; set; }
        public decimal? Qty { get; set; }
        public string Satb { get; set; }
        public decimal? Harga { get; set; }
        public decimal? Jumlah { get; set; }
        public string Comp { get; set; }
        public string Const { get; set; }
        public string Yarn { get; set; }
        public string Width { get; set; }
        public string DtlsPo { get; set; }
        public string ExMaster { get; set; }
        public string ExPo { get; set; }
        public string Userin { get; set; }
        public DateTime? Tglin { get; set; }
        public string Jamin { get; set; }
        public string Usered { get; set; }
        public DateTime? Tgled { get; set; }
        public string Jamed { get; set; }
    }
}
