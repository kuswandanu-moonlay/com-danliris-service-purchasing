using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentStockOpnameFacades;
using Com.DanLiris.Service.Purchasing.Lib.Helpers.ReadResponse;
using Com.DanLiris.Service.Purchasing.Lib.Services;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.NewIntegrationViewModel;
using Com.DanLiris.Service.Purchasing.WebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Com.DanLiris.Service.Purchasing.WebApi.Controllers.v1
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/stock-opnames")]
    [Authorize]
    public class StockOpnameController : Controller
    {
        private readonly IGarmentStockOpnameFacade _facade;
        private readonly IdentityService _identityService;
        private const string ApiVersion = "1.0";

        public StockOpnameController(IGarmentStockOpnameFacade service, IServiceProvider serviceProvider)
        {
            _facade = service;
            _identityService = serviceProvider.GetService<IdentityService>();
        }

        private void VerifyUser()
        {
            _identityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            _identityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            _identityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        class StockOpname
        {
            public DateTimeOffset Date { get; set; }
            public UnitViewModel Unit { get; set; }
            public StorageViewModel Storage { get; set; }
            public ICollection<StockOpnameItem> Items { get; set; }
        }

        class StockOpnameItem
        {
            public long DOItemId { get; set; }
            public string PONo { get; set; }
            public string RONo { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string DesignColor { get; set; }
            public decimal BeforeQuantity { get; set; }
            public decimal Quantity { get; set; }
        }

        [HttpGet]
        public IActionResult Get(int page = 1, int size = 25, string order = "{}", string keyword = null, string filter = "{}", string select = null, string search = "[]")
        {
            try
            {
                var readResponse = new ReadResponse<StockOpname>(new List<StockOpname>
                {
                    new StockOpname
                    {
                        Date = DateTimeOffset.Now,
                        Unit = new UnitViewModel
                        {
                            Code = "UC"
                        },
                        Storage = new StorageViewModel
                        {
                            Code = "SS"
                        }
                    }
                }, 1, new Dictionary<string, string>());

                var info = new Dictionary<string, object>
                    {
                        { "count", readResponse.Data.Count },
                        { "total", readResponse.TotalData },
                        { "order", readResponse.Order },
                        { "page", page },
                        { "size", size }
                    };

                Dictionary<string, object> Result =new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(readResponse.Data, info);
                return Ok(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result = new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("download")]
        public IActionResult DownloadFile(DateTimeOffset? date, string unit, string storage, string storageName)
        {
            try
            {
                if (date == null || unit == null || storage == null)
                {
                    throw new Exception("Semua filter harus diisi");
                }

                var stream = _facade.Download(date.Value, unit, storage, storageName);
                stream.Position = 0;
                FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/excel");
                fileStreamResult.FileDownloadName = "Output.xlsx";
                return fileStreamResult;

                //var stream = (MemoryStream)_facade.Download(date.Value, unit, storage, storageName);
                //byte[] xlsInBytes = stream.ToArray();
                //var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Output.xlsx");
                //return file;
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result = new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail(e.Message);
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpPost("upload")]
        public IActionResult UploadFile()
        {
            if (Request.Form.Files.Count > 0)
            {
                var UploadedFile = Request.Form.Files[0];
                var hasHeader = false;

                ExcelPackage excelPackage = new ExcelPackage();
                excelPackage.Load(UploadedFile.OpenReadStream());

                var ws = excelPackage.Workbook.Worksheets[0];

                var storage = ((string)ws.Cells["B3"].Value).Split("-");

                var data = new StockOpname
                {
                    Date = DateTimeOffset.Parse((string)ws.Cells["B1"].Value),
                    Unit = new UnitViewModel
                    {
                        Code = (string)ws.Cells["B2"].Value
                    },
                    Storage = new StorageViewModel
                    {
                        Code = storage[0].Trim(),
                        Name = storage[1].Trim()
                    },
                    Items = new List<StockOpnameItem>()
                };

                for (int row = 6; row <= ws.Dimension.End.Row; row++)
                {
                    if (!string.IsNullOrWhiteSpace(ws.Cells[row, 1].Text))
                    {
                        StockOpnameItem item = new StockOpnameItem
                        {
                            DOItemId = (int)(double)ws.Cells[row, 1].Value,
                            BeforeQuantity = (decimal)(double)ws.Cells[row, 7].Value,
                            Quantity = (decimal)(double)ws.Cells[row, 8].Value
                        };

                        if (item.BeforeQuantity != item.Quantity)
                        {
                            data.Items.Add(item);
                        }
                    }
                }

                Dictionary<string, object> Result = new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(new { data, ws.Dimension.End.Column, ws.Dimension.End.Row });
                return Ok(Result);


                //Get all details as DataTable -because Datatable make life easy :)
                DataTable excelasTable = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    //Get colummn details
                    if (!string.IsNullOrEmpty(firstRowCell.Text))
                    {
                        string firstColumn = string.Format("Column {0}", firstRowCell.Start.Column);
                        excelasTable.Columns.Add(hasHeader ? firstRowCell.Text : firstColumn);
                    }
                }
                var startRow = hasHeader ? 2 : 1;
                //Get row details
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, excelasTable.Columns.Count];
                    DataRow row = excelasTable.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                //Get everything as generics and let end user decides on casting to required type
                //var generatedType = JsonConvert.SerializeObject(excelasTable);

                return Ok(excelasTable);
            }

            return Ok();
        }
    }
}
