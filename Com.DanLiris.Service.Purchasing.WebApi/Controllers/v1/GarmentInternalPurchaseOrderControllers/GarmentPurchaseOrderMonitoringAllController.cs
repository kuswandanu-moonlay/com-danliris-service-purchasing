using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.GarmentInternalPurchaseOrderViewModel;
using AutoMapper;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentInternalPurchaseOrderModel;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.IntegrationViewModel;
using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentInternalPurchaseOrderFacades;
using Com.DanLiris.Service.Purchasing.WebApi.Helpers;
using Com.DanLiris.Service.Purchasing.Lib.Services;
using Com.Moonlay.NetCore.Lib.Service;

namespace Com.DanLiris.Service.Purchasing.WebApi.Controllers.v1.GarmentInternalPurchaseOrderControllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/garment-purchase-orders/monitoring")]
    [Authorize]
    public class GarmentPurchaseOrderMonitoringAllController : Controller
    {
        private string ApiVersion = "1.0.0";
        private readonly IMapper _mapper;
        private readonly GarmentPurchaseOrderMonitoringAllFacade _facade;
        private readonly IdentityService identityService;
        public GarmentPurchaseOrderMonitoringAllController(IMapper mapper, GarmentPurchaseOrderMonitoringAllFacade facade, IdentityService identityService)
        {
            _mapper = mapper;
            _facade = facade;
            this.identityService = identityService;
        }

        [HttpGet]
        public IActionResult GetReportAll(string unit, string category, string epoNo, string roNo, string prNo, string doNo, string supplier, string staff, DateTime? dateFrom, DateTime? dateTo, string status, int page, int size, string Order = "{}")
        {
            int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
            string accept = Request.Headers["Accept"];
            try
            {
                var data = _facade.GetReport(unit, category, epoNo, roNo, prNo, doNo, supplier, staff, dateFrom, dateTo, status, page, size, Order, offset);

                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = data.Item1,
                    info = new { total = data.Item2 },
                    message = General.OK_MESSAGE,
                    statusCode = General.OK_STATUS_CODE
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("download")]
        public IActionResult GetXlsAll(string unit, string category, string epoNo, string roNo, string prNo, string doNo, string supplier, string staff, DateTime? dateFrom, DateTime? dateTo, string status)
        {

            try
            {
                byte[] xlsInBytes;
                int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                DateTime DateFrom = dateFrom == null ? new DateTime(1970, 1, 1) : Convert.ToDateTime(dateFrom);
                DateTime DateTo = dateTo == null ? DateTime.Now : Convert.ToDateTime(dateTo);

                var xls = _facade.GenerateExcel(unit, category, epoNo, roNo, prNo, doNo, supplier, staff, dateFrom, dateTo, status, offset);

                string filename = String.Format("Monitoring Garment Purchase Order All - {0}.xlsx", DateTime.UtcNow.ToString("ddMMyyyy"));

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;

            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        //[HttpGet("by-user")]
        //public IActionResult GetReportByUser(string unit, string category, string epoNo, string roNo, string prNo, string article, string doNo, string supplier, string staff, DateTime? dateFrom, DateTime? dateTo, string status, int page, int size, string Order = "{}")
        //{
        //    int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        //    string accept = Request.Headers["Accept"];
        //    identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;
        //    try
        //    {

        //        var data = _facade.GetReport(unit, category, epoNo, roNo, prNo, article, doNo, supplier, staff, dateFrom, dateTo, status, page, size, Order, offset, identityService.Username);

        //        return Ok(new
        //        {
        //            apiVersion = ApiVersion,
        //            data = data.Item1,
        //            info = new { total = data.Item2 },
        //            message = General.OK_MESSAGE,
        //            statusCode = General.OK_STATUS_CODE
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        Dictionary<string, object> Result =
        //            new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
        //            .Fail();
        //        return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
        //    }
        //}

        //[HttpGet("by-user/download")]
        //public IActionResult GetXlsByUser(string unit, string category, string epoNo, string roNo, string prNo, string article, string doNo, string supplier, string staff, DateTime? dateFrom, DateTime? dateTo, string status)
        //{

        //    try
        //    {
        //        byte[] xlsInBytes;
        //        int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        //        identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;

        //        DateTime DateFrom = dateFrom == null ? new DateTime(1970, 1, 1) : Convert.ToDateTime(dateFrom);
        //        DateTime DateTo = dateTo == null ? DateTime.Now : Convert.ToDateTime(dateTo);

        //        var xls = _facade.GenerateExcel(unit, category, epoNo, roNo, prNo, article, doNo, supplier, staff, dateFrom, dateTo, status, offset, identityService.Username);

        //        string filename = String.Format("Monitoring Garment Purchase Order All - {0}.xlsx", DateTime.UtcNow.ToString("ddMMyyyy"));

        //        xlsInBytes = xls.ToArray();
        //        var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        //        return file;

        //    }
        //    catch (Exception e)
        //    {
        //        Dictionary<string, object> Result =
        //            new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
        //            .Fail();
        //        return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
        //    }
        //}
    }
}
