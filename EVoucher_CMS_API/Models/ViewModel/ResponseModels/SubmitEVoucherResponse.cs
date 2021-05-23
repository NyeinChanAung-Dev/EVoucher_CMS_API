using EVoucher_CMS_API.Models.ViewModel.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVoucher_CMS_API.Models.ViewModel.ResponseModels
{
    public class SubmitEVoucherResponse : ResponseBase
    {
        public string EVoucherNo { get; set; }
    }
}
