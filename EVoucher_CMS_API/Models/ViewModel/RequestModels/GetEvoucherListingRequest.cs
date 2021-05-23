using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVoucher_CMS_API.Models.ViewModel.RequestModels
{
    public class GetEVoucherListingRequest
    {
        public short? Status { get; set; }
        [Required]
        [Range(1,1000)]
        public int PageNumber { get; set; }
        [Required]
        [Range(1, 1000)]
        public int PageSize { get; set; }
    }
}
