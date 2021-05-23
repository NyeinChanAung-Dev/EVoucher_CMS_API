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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EVoucher_CMS_API.Manager
{
    public class EVoucherManager
    {
        private readonly IConfiguration _configuration;
        EVoucherSystemDBContext _dbContext;
        IRepository<EvoucherTb> _evoucherepo;
        IRepository<PaymentMethodTb> _paymentmethodrepo;

        public EVoucherManager(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbContext = new EVoucherSystemDBContext();
            _evoucherepo = new BaseRepository<EvoucherTb>(_dbContext);
            _paymentmethodrepo = new BaseRepository<PaymentMethodTb>(_dbContext);
        }

        public SubmitEVoucherResponse UpsertEvoucher(SubmitEVoucherRequest _request)
        {
            SubmitEVoucherResponse response = new SubmitEVoucherResponse();
            _request.BuyType = "";


            if (string.IsNullOrEmpty(_request.VoucherNo))
            {
                //New
                var newResponse = NewEVoucher(_request);
                return newResponse;

            }
            else
            {
                //Update
                var updateResponse = UpdateEVoucher(_request);
                return updateResponse;
            }
        }

        public SubmitEVoucherResponse NewEVoucher(SubmitEVoucherRequest _request)
        {
            SubmitEVoucherResponse response = new SubmitEVoucherResponse();
            var evList = _evoucherepo.Get.Select(e => e.Id).ToList();

            int maxNo = 1;
            if (evList != null && evList.Count > 0)
            {
                maxNo = evList.Max(x => x);
                maxNo++;
            }

            EvoucherTb evoucher = new EvoucherTb
            {
                VoucherNo = "EV-" + maxNo.ToString().PadLeft(4, '0'),
                Title = _request.Title,
                Description = _request.Description,
                ExpiryDate = _request.ExpiryDate,
                BuyType = _request.BuyType,
                VoucherAmount = _request.VoucherAmount,
                PaymentMethod = _request.PaymentMethod,
                SellingPrice = _request.SellingPrice,
                SellingDiscount = _request.SellingDiscount ?? 0,
                Quantity = _request.Quantity,
                MaxLimit = _request.MaxLimit,
                GiftPerUserLimit = _request.GiftPerUserLimit,
                Status = _request.Status
            };

            _evoucherepo.Insert(evoucher);
            response.EVoucherNo = evoucher.VoucherNo;
            return response;
        }

        public SubmitEVoucherResponse UpdateEVoucher(SubmitEVoucherRequest _request)
        {
            SubmitEVoucherResponse response = new SubmitEVoucherResponse();
            var evoucherTb = _evoucherepo.Get.Where(e => e.VoucherNo == _request.VoucherNo).FirstOrDefault();
            if (evoucherTb == null)
            {
                response.StatusCode = 404;
                response.ErrorType = "Record-Not Found";
                response.ErrorMessage = "No Voucher Found.";
                return response;
            }

            evoucherTb.Title = _request.Title;
            evoucherTb.Description = _request.Description;
            evoucherTb.ExpiryDate = _request.ExpiryDate;
            evoucherTb.BuyType = _request.BuyType;
            evoucherTb.VoucherAmount = _request.VoucherAmount;
            evoucherTb.PaymentMethod = _request.PaymentMethod;
            evoucherTb.SellingPrice = _request.SellingPrice;
            evoucherTb.SellingDiscount = _request.SellingDiscount ?? 0;
            evoucherTb.Quantity = _request.Quantity;
            evoucherTb.MaxLimit = _request.MaxLimit;
            evoucherTb.GiftPerUserLimit = _request.GiftPerUserLimit;
            evoucherTb.Status = _request.Status;

            _evoucherepo.Update(evoucherTb);
            response.EVoucherNo = evoucherTb.VoucherNo;

            return response;
        }

        private string ValidateEVoucherPaymentMethod(SubmitEVoucherRequest _request)
        {
            string validationMsg = "";

            var isValidPaymentMethod = (from p in _paymentmethodrepo.Get
                                        where p.PaymentMethod == _request.PaymentMethod
                                        select true).FirstOrDefault();
            if (!isValidPaymentMethod)
            {
                validationMsg = "Invalid Payment Method.";
            }

            if (!ImageHelper.IsBase64(_request.Image))
            {
                validationMsg = $"{validationMsg} /r/n Invalid Image String.";
            }
            return validationMsg;
        }


        public async Task<PagedListModel<GetEVoucherListingResponse>> GetEvoucherList(GetEVoucherListingRequest _request)
        {
            var evoucherList = await _evoucherepo.Get.Where(e => e.Status == _request.Status || _request.Status != 0).ToListAsync();

            var finalResult = new PagedListModel<GetEVoucherListingResponse>();
            List<GetEVoucherListingResponse> postPageList = new List<GetEVoucherListingResponse>();

            if (evoucherList.Count() != 0)
            {
                var totalCount = evoucherList.Select(c => c.Id).Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / _request.PageSize);
                var results = evoucherList.Skip(_request.PageSize * (_request.PageNumber - 1))
                                     .Take(_request.PageSize);

                var data = (from res in results
                            select new GetEVoucherListingResponse
                            {
                                id = res.Id,
                                VoucherNo = res.VoucherNo,
                                Title = res.Title,
                                ExpiryDate = res.ExpiryDate,
                                VoucherAmount = res.VoucherAmount,
                                Quantity = res.Quantity,
                                SellingPrice = res.SellingPrice,
                                Status = res.Status
                            });

                #region paging
                PagedListModel<GetEVoucherListingResponse> model = new PagedListModel<GetEVoucherListingResponse>()
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    prevLink = "",
                    nextLink = "",
                    Results = data.ToList(),
                };
                #endregion
                finalResult = model;
            }
            else
            {
                finalResult.Results = null;
                finalResult.TotalPages = 0;
                finalResult.TotalCount = 0;
            }
            return finalResult;

        }

        public GetEVoucherDetailResponse GetEvoucherDetail(string VoucherNo)
        {
            GetEVoucherDetailResponse response = new GetEVoucherDetailResponse();
            var evd = _evoucherepo.Get.Where(e => e.VoucherNo == VoucherNo).FirstOrDefault();

            response.VoucherNo = evd.VoucherNo;
            response.Title = evd.Title;
            response.Description = evd.Description;
            response.ExpiryDate = evd.ExpiryDate;
            if(evd.ImagePath != null)
            {
                response.Image = Path.Combine(_configuration["BaseURL"], evd.ImagePath);
            }
            else
            {
                response.Image = null;
            }
            response.BuyType = evd.BuyType;
            response.VoucherAmount = evd.VoucherAmount;
            response.PaymentMethod = evd.PaymentMethod;
            response.SellingPrice = evd.SellingPrice;
            response.SellingDiscount = evd.SellingDiscount;
            response.Quantity = evd.Quantity;
            response.MaxLimit = evd.MaxLimit;
            response.GiftPerUserLimit = evd.GiftPerUserLimit;
            response.Status = evd.Status;
            return response;
        }

        public UpdateEVoucherStatusResponse UpdateEVoucherStatus(UpdateEVoucherStatusRequest _request)
        {
            UpdateEVoucherStatusResponse response = new UpdateEVoucherStatusResponse();
            var tblEvoucher = _evoucherepo.Get.Where(e => e.VoucherNo == _request.VoucherNo).FirstOrDefault();
            if (tblEvoucher == null)
            {
                response.StatusCode = 404;
                response.ErrorType = "Record-Not Found";
                response.ErrorMessage = "No Voucher Found.";
                return response;
            }
            tblEvoucher.Status = _request.Status;

            _evoucherepo.Update(tblEvoucher);

            response.Updated = true;
            response.VoucherNo = tblEvoucher.VoucherNo;

            return response;
        }

    }
}
