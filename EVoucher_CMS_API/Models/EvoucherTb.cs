using System;
using System.Collections.Generic;

#nullable disable

namespace EVoucher_CMS_API.Models
{
    public partial class EvoucherTb
    {
        public int Id { get; set; }
        public string VoucherNo { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string ImagePath { get; set; }
        public string BuyType { get; set; }
        public decimal VoucherAmount { get; set; }
        public string PaymentMethod { get; set; }
        public decimal SellingPrice { get; set; }
        public short? SellingDiscount { get; set; }
        public int Quantity { get; set; }
        public int MaxLimit { get; set; }
        public int GiftPerUserLimit { get; set; }
        public short Status { get; set; }
    }
}
