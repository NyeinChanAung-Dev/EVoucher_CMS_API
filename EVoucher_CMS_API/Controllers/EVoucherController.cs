using EVoucher_CMS_API.Helper;
using EVoucher_CMS_API.Manager;
using EVoucher_CMS_API.Models.ViewModel.RequestModels;
using EVoucher_CMS_API.Models.ViewModel.ResponseModels;
using Microsoft.AspNetCore.Authorization;
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
    public class EVoucherController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private EVoucherManager _evouchernmgr;

        public EVoucherController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _evouchernmgr = new EVoucherManager(_configuration);
        }

        [HttpPost]
        [Authorize()]
        [Route("api/evoucher/upsertevoucher")]
        public IActionResult UpsertEvoucher(SubmitEVoucherRequest _request)
        {
            try
            {
                var response = _evouchernmgr.UpsertEvoucher(_request);
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
                return StatusCode(500, new Error("internal-error", e.Message));
            }
        }

        [HttpGet]
        [Authorize()]
        [Route("api/evoucher/getevoucherlist")]
        public async Task<IActionResult> GetEvoucherList(short Status, int PageNumber = 1, int PageSize = 10)
        {
            GetEVoucherListingRequest _request = new GetEVoucherListingRequest()
            {
                Status = Status,
                PageNumber = PageNumber,
                PageSize = PageSize
            };
            try
            {
                PagedListModel<GetEVoucherListingResponse> result = await _evouchernmgr.GetEvoucherList(_request);

                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new Error("Not-Found", "No Record Available"));
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new Error("internal-error", e.Message));
            }
        }

        [HttpGet]
        [Authorize()]
        [Route("api/evoucher/getevoucherdetail")]
        public IActionResult GetEvoucherDetail(string VoucherNo)
        {
            try
            {
                var response = _evouchernmgr.GetEvoucherDetail(VoucherNo);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return NotFound(new Error("RecordNotFound", "Record Not Found"));
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new Error("internal-error", e.Message));
            }
        }


        [HttpPost]
        [Authorize()]
        [Route("api/evoucher/updateevoucherstatus")]
        public IActionResult UpdateEVoucherStatus(UpdateEVoucherStatusRequest _request)
        {
            try
            {
                var response = _evouchernmgr.UpdateEVoucherStatus(_request);
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
                return StatusCode(500, new Error("internal-error", e.Message));
            }
        }


    }
}
