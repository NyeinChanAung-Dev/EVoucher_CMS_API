using EVoucher_CMS_API.Helper;
using EVoucher_CMS_API.Interfaces;
using EVoucher_CMS_API.Models;
using EVoucher_CMS_API.Models.ViewModel.DTO;
using EVoucher_CMS_API.Models.ViewModel.RequestModels;
using EVoucher_CMS_API.Models.ViewModel.ResponseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVoucher_CMS_API.Manager
{
    public class UserAuthManager
    {
        EVoucherSystemDBContext _dbContext;
        private readonly IConfiguration _configuration;
        IRepository<UsersTb> _userrepo;

        public UserAuthManager(IConfiguration configuration)
        {
            _dbContext = new EVoucherSystemDBContext();
            _configuration = configuration;
            _userrepo = new BaseRepository<UsersTb>(_dbContext);
        }

        public LoginResponse Login(LoginRequest _request)
        {
            try
            {
                LoginResponse response = new LoginResponse();
                UsersTb user = _userrepo.Get.Where(u => u.LoginId == _request.LoginId && u.Password == _request.Password).FirstOrDefault();
                if (user != null)
                {
                    GetGenerateTokenDTO getGenerateToken = new GetGenerateTokenDTO
                    {
                        Audience = _configuration["Audience"],
                        Issuer = _configuration["Issuer"],
                        PrivateKey = _configuration["RsaPrivateKey"],
                        TokenExpiryMinute = Int32.Parse(_configuration["TokenExpiryMinute"]),
                        RefreshTokenExpiryMinute = Int32.Parse(_configuration["RefreshTokenExpiryMinute"]),
                        UserId = user.UserId,
                        UserName = user.UserName
                    };
                    TokenGeneratedDTO generatedToken = JwtHandler.GenerateToken(getGenerateToken);
                    if (String.IsNullOrEmpty(generatedToken.ErrorStatus))
                    {
                        response.AccessToken = generatedToken.AccessToken;
                        response.AccessTokenExpireMinutes = generatedToken.TokenExpiresMinute;
                        response.RefreshToken = generatedToken.RefreshToken;
                        response.RefreshTokenExpireMinutes = Int32.Parse(_configuration["RefreshTokenExpiryMinute"]);
                        response.UserId = user.UserId;
                    }
                    else
                    {
                        response.ErrorStatus = generatedToken.ErrorStatus;
                    }
                }
                else
                {
                    response.ErrorStatus = "Invalid Login Id or password!";
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
