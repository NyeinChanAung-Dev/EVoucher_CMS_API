using System;
using System.Collections.Generic;
using System.Text;

namespace EVoucher_CMS_API.Models.ViewModel.DTO
{
    public class ValidateTokenDTO
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
    }
}
