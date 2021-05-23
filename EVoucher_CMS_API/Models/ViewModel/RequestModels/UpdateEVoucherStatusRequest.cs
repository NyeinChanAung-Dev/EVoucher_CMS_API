using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVoucher_CMS_API.Models.ViewModel.RequestModels
{
    public class UpdateEVoucherStatusRequest
    {
        [Required]
        public string VoucherNo { get; set; }
        [Required]
        public short Status { get; set; }
    }
}
