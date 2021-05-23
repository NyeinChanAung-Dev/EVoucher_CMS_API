using System;
using System.Collections.Generic;
using System.Text;

namespace EVoucher_CMS_API.Models.ViewModel.DTO
{
    public class SaveRefreshTokenDTO
    {
        public int ExpiryMinute { get; set; }
        public string RefreshToken { get; set; }
        public int UserId { get; set; }
    }
}
