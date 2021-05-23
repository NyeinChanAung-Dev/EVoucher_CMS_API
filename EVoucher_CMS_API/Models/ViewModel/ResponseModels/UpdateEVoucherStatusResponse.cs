using EVoucher_CMS_API.Models.ViewModel.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVoucher_CMS_API.Models.ViewModel.ResponseModels
{
    public class UpdateEVoucherStatusResponse : ResponseBase
    {
        public bool Updated { get; set; }
        public string VoucherNo { get; set; }
    }
}
