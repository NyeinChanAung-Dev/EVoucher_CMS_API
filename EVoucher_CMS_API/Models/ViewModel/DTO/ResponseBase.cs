﻿using EVoucher_CMS_API.Models.ViewModel.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVoucher_CMS_API.Models.ViewModel.DTO
{
    public class ResponseBase
    {
        public ResponseBase()
        {
            StatusCode = 200;
        }

        public ResponseBase(int statusCode, string errorType, string errorMessage)
        {
            StatusCode = statusCode;
            ErrorType = errorType;
            ErrorMessage = errorMessage;
        }

        public int StatusCode;
        public string ErrorType;
        public string ErrorMessage;

        public Error GetError()
        {
            return new Error(ErrorType, ErrorMessage);
        }
    }
}
