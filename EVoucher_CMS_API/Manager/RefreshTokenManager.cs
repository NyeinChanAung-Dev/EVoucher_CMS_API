using EVoucher_CMS_API.Helper;
using EVoucher_CMS_API.Interfaces;
using EVoucher_CMS_API.Models;
using EVoucher_CMS_API.Models.ViewModel.DTO;
using EVoucher_CMS_API.Models.ViewModel.RequestModels;
using EVoucher_CMS_API.Models.ViewModel.ResponseModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVoucher_CMS_API.Manager
{
    public class RefreshTokenManager
    {
        EVoucherSystemDBContext _dbContext;
        private readonly IConfiguration _configuration;
        IRepository<RefreshTokenTb> _rfrepo;

        public RefreshTokenManager(IConfiguration configuration)
        {
            _dbContext = new EVoucherSystemDBContext();
            _configuration = configuration;
            _rfrepo = new BaseRepository<RefreshTokenTb>(_dbContext);
        }

        public void SaveRefreshToken(SaveRefreshTokenDTO tokenDto)
        {
            RefreshTokenTb refreshTokenTb = new RefreshTokenTb
            {
                ExpiryDate = DateTime.Now.AddMinutes(tokenDto.ExpiryMinute),
                RefreshToken = tokenDto.RefreshToken,
                UserId = tokenDto.UserId
            };
            _rfrepo.Insert(refreshTokenTb);
            DeleteExpiryRefreshToken();
        }

        public void DeleteExpiryRefreshToken()
        {
            var deleteLoginToken = _rfrepo.Get.Where(a => a.ExpiryDate < DateTime.Now).ToList();
            _rfrepo.DeleteMultipleRecords(deleteLoginToken);
        }

        public void DeleteRefreshToken(string refreshToken)
        {
            var deleteLoginToken = _rfrepo.Get.Where(a => a.RefreshToken == refreshToken).FirstOrDefault();
            _rfrepo.Delete(deleteLoginToken);
        }

        public RefreshTokenResponse RefreshToken(RefreshTokenRequest _request, string token)
        {
            RefreshTokenResponse response = new RefreshTokenResponse();
            CheckValidateTokenDTO validateDto = new CheckValidateTokenDTO
            {
                Audience = _configuration["Audience"],
                Issuer = _configuration["Issuer"],
                PrivateKey = _configuration["RsaPrivateKey"],
                IsValidateExpiry = false,
                Token = token
            };

            var validatedToken = JwtHandler.CheckValidToken(validateDto);
            if (validatedToken.IsValid)
            {
                var tblRefreshToken = _rfrepo.Get.Where(a => a.RefreshToken == _request.RefreshToken && a.UserId == validatedToken.UserID && a.ExpiryDate > DateTime.Now).FirstOrDefault();

                if (tblRefreshToken != null && tblRefreshToken.RefreshToken != "")
                {
                    GetGenerateTokenDTO getGenerateToken = new GetGenerateTokenDTO
                    {
                        Audience = _configuration["Audience"],
                        Issuer = _configuration["Issuer"],
                        PrivateKey = _configuration["RsaPrivateKey"],
                        TokenExpiryMinute = Int32.Parse(_configuration["TokenExpiryMinute"]),
                        RefreshTokenExpiryMinute = Int32.Parse(_configuration["RefreshTokenExpiryMinute"]),
                        UserId = validatedToken.UserID,
                        UserName = validatedToken.UserName
                    };

                    var generatedToken = JwtHandler.GenerateToken(getGenerateToken);
                    if (generatedToken != null && string.IsNullOrEmpty(generatedToken.ErrorStatus))
                    {
                        response.AccessToken = generatedToken.AccessToken;
                        response.AccessTokenExpireMinutes = generatedToken.TokenExpiresMinute;
                        response.RefreshToken = generatedToken.RefreshToken;
                        response.RefreshTokenExpireMinutes = Int32.Parse(_configuration["RefreshTokenExpiryMinute"]);
                        
                        //Save newly generated token
                        SaveRefreshToken(new SaveRefreshTokenDTO
                        {
                            ExpiryMinute = generatedToken.RefreshTokenExpiresMinute,
                            RefreshToken = generatedToken.RefreshToken,
                            UserId = generatedToken.UserId
                        });

                        //Delete old token
                        DeleteRefreshToken(_request.RefreshToken);
                    }
                    else
                    {
                        response.StatusCode = 500;
                        response.ErrorType = "Token Generation Failed.";
                        response.ErrorMessage = "Unable to generate Access Token.";
                    }

                    //Delete Expiry Token
                    DeleteExpiryRefreshToken();
                }
                else
                {
                    response.StatusCode = 401;
                    response.ErrorType = "Unauthorized Request";
                    response.ErrorMessage = "Invalid or Expired Refresh Token.";
                }

            }
            else
            {
                response.StatusCode = 401;
                response.ErrorType = "Unauthorized Request";
                response.ErrorMessage = "Invalid or Expired Access Token.";
            }

            return response;
        }

    }
}
