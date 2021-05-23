using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVoucher_CMS_API.Models.ViewModel.RequestModels
{
    public class RefreshTokenRequest 
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
