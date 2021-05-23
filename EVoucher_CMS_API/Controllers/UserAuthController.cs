using EVoucher_CMS_API.Manager;
using EVoucher_CMS_API.Models.ViewModel.DTO;
using EVoucher_CMS_API.Models.ViewModel.RequestModels;
using EVoucher_CMS_API.Models.ViewModel.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVoucher_CMS_API.Controllers
{
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private UserAuthManager _usermgr;
        private RefreshTokenManager _refreshtokenmgr;

        public UserAuthController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _usermgr = new UserAuthManager(_configuration);
            _refreshtokenmgr = new RefreshTokenManager(_configuration);
        }

        [HttpPost]
        [Route("api/userauth/login")]
        public IActionResult Login(LoginRequest _request)
        {
            try
            {
                var response = _usermgr.Login(_request);
                if (String.IsNullOrEmpty(response.ErrorStatus))
                {
                    _refreshtokenmgr.SaveRefreshToken(new SaveRefreshTokenDTO
                    {
                        ExpiryMinute = response.RefreshTokenExpireMinutes,
                        RefreshToken = response.RefreshToken,
                        UserId = response.UserId
                    });
                    return Ok(response);
                }
                else
                {
                    return NotFound(new Error("Unauthorized", response.ErrorStatus));
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new Error("internal_error", e.Message));
            }
        }

        [Route("api/userauth/refreshtoken")]
        [HttpPost]
        public IActionResult RefreshToken(RefreshTokenRequest _request)
        {
            try
            {
                var requestHeader = _httpContextAccessor.HttpContext.Request.Headers;
                string accessToken = requestHeader["Authorization"];
                var response = _refreshtokenmgr.RefreshToken(_request, accessToken);

                if (response.StatusCode == 200)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(response.StatusCode, response.GetError());
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new Error("internal_error", e.Message));
            }
        }

    }
}
