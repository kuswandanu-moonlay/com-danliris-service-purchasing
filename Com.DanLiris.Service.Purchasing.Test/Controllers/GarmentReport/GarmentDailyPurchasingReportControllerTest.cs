﻿using AutoMapper;
using Com.DanLiris.Service.Purchasing.Lib.Interfaces;
using Com.DanLiris.Service.Purchasing.Lib.Services;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.GarmentDailyPurchasingReportViewModel;
using Com.DanLiris.Service.Purchasing.Test.Helpers;
using Com.DanLiris.Service.Purchasing.WebApi.Controllers.v1.GarmentReports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Com.DanLiris.Service.Purchasing.Test.Controllers.GarmentReports
{
    public class DailyGarmentPurchaseReportControllerTest
    {
        private GarmentDailyPurchasingReportViewModel ViewModel
        {
            get
            {
                return new GarmentDailyPurchasingReportViewModel
                {
                    SupplierName = "",
                    UnitName = "",
                    BillNo = "",
                    PaymentBill = "",
                    DONo = "",
                    InternNo = "",
                    ProductName = "",
                    CodeRequirement = "",
                    UOMUnit = "",
                    Quantity = 0,
                    Amount = 0,
                    Amount1 = 0,
                    Amount2 = 0,
                    Amount3 = 0,
                };
            }
        }
        private Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(IdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test" });

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new HttpClientTestService());

            return serviceProvider;
        }

        private GarmentDailyPurchasingReportController GetController(Mock<IGarmentDailyPurchasingReportFacade> facadeM)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);

            var servicePMock = GetServiceProvider();

            var controller = new GarmentDailyPurchasingReportController(facadeM.Object, servicePMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = user.Object
                    }
                }
            };
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer unittesttoken";
            controller.ControllerContext.HttpContext.Request.Path = new PathString("/v1/unit-test");
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "7";

            return controller;
        }
        protected int GetStatusCode(IActionResult response)
        {
            return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
        }


        [Fact]
        public void Should_Success_Get_Report()
        {
            var mockFacade = new Mock<IGarmentDailyPurchasingReportFacade>();
            mockFacade.Setup(x => x.GetGDailyPurchasingReport(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(Tuple.Create(new List<GarmentDailyPurchasingReportViewModel> { ViewModel }, 25));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<List<GarmentDailyPurchasingReportViewModel>>(It.IsAny<List<GarmentDailyPurchasingReportViewModel>>()))
                .Returns(new List<GarmentDailyPurchasingReportViewModel> { ViewModel });

            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentDailyPurchasingReportController controller = new GarmentDailyPurchasingReportController(mockFacade.Object, GetServiceProvider().Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user.Object
                }
            };

            var response = controller.GetReport(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>());
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));

        }

        [Fact]
        public void Should_Success_Get_Xls()
        {
            var mockFacade = new Mock<IGarmentDailyPurchasingReportFacade>();
            mockFacade.Setup(x => x.GenerateExcelGDailyPurchasingReport(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(new MemoryStream());
        
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<List<GarmentDailyPurchasingReportViewModel>>(It.IsAny<List<GarmentDailyPurchasingReportViewModel>>()))
                .Returns(new List<GarmentDailyPurchasingReportViewModel> { ViewModel });

            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentDailyPurchasingReportController controller = new GarmentDailyPurchasingReportController(mockFacade.Object, GetServiceProvider().Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user.Object
                }
            };

            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "0";
            var response = controller.GetXls(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>());
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", response.GetType().GetProperty("ContentType").GetValue(response, null));
           }

        [Fact]
        public void Should_Error_Get_Report_Data()
        {
            var mockFacade = new Mock<IGarmentDailyPurchasingReportFacade>();
            mockFacade.Setup(x => x.GetGDailyPurchasingReport(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(Tuple.Create(new List<GarmentDailyPurchasingReportViewModel> { ViewModel }, 25));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<List<GarmentDailyPurchasingReportViewModel>>(It.IsAny<List<GarmentDailyPurchasingReportViewModel>>()))
                .Returns(new List<GarmentDailyPurchasingReportViewModel> { ViewModel });

            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentDailyPurchasingReportController controller = new GarmentDailyPurchasingReportController(mockFacade.Object, GetServiceProvider().Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user.Object
                }
            };
            //controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "0";
            var response = controller.GetReport(null, It.IsAny<bool>(), null, null, null);
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void Should_Error_Get_Report_Xls_Data()
        {
            var mockFacade = new Mock<IGarmentDailyPurchasingReportFacade>();
            mockFacade.Setup(x => x.GetGDailyPurchasingReport(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(Tuple.Create(new List<GarmentDailyPurchasingReportViewModel> { ViewModel }, 25));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<List<GarmentDailyPurchasingReportViewModel>>(It.IsAny<List<GarmentDailyPurchasingReportViewModel>>()))
                .Returns(new List<GarmentDailyPurchasingReportViewModel> { ViewModel });

            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentDailyPurchasingReportController controller = new GarmentDailyPurchasingReportController(mockFacade.Object, GetServiceProvider().Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user.Object
                }
            };
            //controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "0";
            var response = controller.GetXls(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }
    }
}
