using System;
using System.Collections.Generic;

#nullable disable

namespace EVoucher_CMS_API.Models
{
    public partial class GeneratedEvoucherTb
    {
        public int Id { get; set; }
        public string PromoCode { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string VoucherNo { get; set; }
        public string QrimagePath { get; set; }
        public string VoucherImagePath { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhone { get; set; }
        public decimal VoncherAmount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public short Status { get; set; }
    }
}
