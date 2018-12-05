﻿using Com.DanLiris.Service.Purchasing.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.DanLiris.Service.Purchasing.Lib.Models.BankExpenditureNoteModel
{
    public class BankExpenditureNoteModel : BaseModel
    {
        #region Bank
        [MaxLength(255)]
        public string BankId { get; set; }
        [MaxLength(255)]
        public string BankCode { get; set; }
        [MaxLength(255)]
        public string BankAccountName { get; set; }
        [MaxLength(255)]
        public string BankAccountCOA { get; set; }
        [MaxLength(255)]
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        [MaxLength(255)]
        public string BankCurrencyId { get; set; }
        [MaxLength(255)]
        public string BankCurrencyCode { get; set; }
        [MaxLength(255)]
        public string BankCurrencyRate { get; set; }
        #endregion
        [MaxLength(255)]
        public string BGCheckNumber { get; set; }
        public DateTimeOffset Date { get; set; }
        public ICollection<BankExpenditureNoteDetailModel> Details { get; set; }
        [MaxLength(255)]
        public string DocumentNo { get; set; }
        public double GrandTotal { get; set; }
        #region Supplier
        [MaxLength(255)]
        public string SupplierId { get; set; }
        [MaxLength(255)]
        public string SupplierCode { get; set; }
        [MaxLength(255)]
        public string SupplierName { get; set; }
        public bool SupplierImport { get; set; }
        #endregion
    }
}
