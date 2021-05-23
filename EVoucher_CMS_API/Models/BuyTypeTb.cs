using System;
using System.Collections.Generic;

#nullable disable

namespace EVoucher_CMS_API.Models
{
    public partial class BuyTypeTb
    {
        public int Id { get; set; }
        public string BuyType { get; set; }
        public string Description { get; set; }
        public short? MaxLimit { get; set; }
        public short Status { get; set; }
    }
}
