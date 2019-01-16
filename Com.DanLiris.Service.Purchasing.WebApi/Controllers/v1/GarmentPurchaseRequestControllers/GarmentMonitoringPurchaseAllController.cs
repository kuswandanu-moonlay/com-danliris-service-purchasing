﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Com.DanLiris.Service.Purchasing.Lib.Interfaces;
using Com.DanLiris.Service.Purchasing.Lib.Services;
using Com.DanLiris.Service.Purchasing.WebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Com.DanLiris.Service.Purchasing.WebApi.Controllers.v1.GarmentPurchaseRequestControllers
{
	[Produces("application/json")]
	[ApiVersion("1.0")]
	[Route("v{version:apiVersion}/garment-purchase-orders/monitoring")]
	[Authorize]
	public class GarmentMonitoringPurchaseAllController : Controller
	{
		private string ApiVersion = "1.0.0";
		public readonly IServiceProvider serviceProvider;
		private readonly IMapper mapper;
		private readonly IGarmentPurchaseRequestFacade facade;
		private readonly IdentityService identityService;

		public GarmentMonitoringPurchaseAllController(IServiceProvider serviceProvider, IGarmentPurchaseRequestFacade facade)
		{
			this.serviceProvider = serviceProvider;
			this.facade = facade;
			this.identityService = (IdentityService)serviceProvider.GetService(typeof(IdentityService));
		}
		[HttpGet]
		public IActionResult GetReport(string epono, string unit, string category,string roNo, string article,string poSerialNumber,string username,string doNo,string ipoStatus,string supplier, string status, DateTime? dateFrom, DateTime? dateTo, int page, int size, string Order = "{}")
		{
			int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
			string accept = Request.Headers["Accept"];

			try
			{

				var data = facade.GetMonitoringPurchaseReport(epono, unit, category, roNo, article, poSerialNumber, username, doNo, ipoStatus, supplier, status, dateFrom, dateTo, page, size, Order, offset);

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
		public IActionResult GetXls(string epono, string unit, string category,string roNo, string article,string poSerialNumber,string username,string doNo,string ipoStatus,string supplier, string status, DateTime? dateFrom, DateTime? dateTo, int page, int size, string Order = "{}")
		{
			try
			{
				byte[] xlsInBytes;

				int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
				var xls = facade.GenerateExcelPurchase(epono, unit, category, roNo, article, poSerialNumber, username, doNo, ipoStatus, supplier, status, dateFrom, dateTo, page, size, Order, offset);

				string filename = "Laporan Pembelian Garment";
				if (dateFrom != null) filename += " " + ((DateTime)dateFrom).ToString("dd-MM-yyyy");
				if (dateTo != null) filename += "_" + ((DateTime)dateTo).ToString("dd-MM-yyyy");
				filename += ".xlsx";

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
	}
}