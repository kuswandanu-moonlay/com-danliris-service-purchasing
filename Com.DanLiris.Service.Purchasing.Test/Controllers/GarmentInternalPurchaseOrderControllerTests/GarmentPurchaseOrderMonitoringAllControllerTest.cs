using AutoMapper;
using Com.DanLiris.Service.Purchasing.Lib.Interfaces;
using Com.DanLiris.Service.Purchasing.Lib.Services;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.GarmentInternalPurchaseOrderViewModel;
using Com.DanLiris.Service.Purchasing.Test.Helpers;
using Com.DanLiris.Service.Purchasing.WebApi.Controllers.v1.GarmentInternalPurchaseOrderControllers;
using Com.Moonlay.NetCore.Lib.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Com.DanLiris.Service.Purchasing.Test.Controllers.GarmentInternalPurchaseOrderControllerTests
{
    public class GarmentPurchaseOrderMonitoringAllControllerTest
    {
        private GarmentPurchaseOrderMonitoringAllViewModel ViewModel
        {
            get
            {
                return new GarmentPurchaseOrderMonitoringAllViewModel
                {
                };
            }
        }

        private ServiceValidationExeption GetServiceValidationExeption()
        {
            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
            List<ValidationResult> validationResults = new List<ValidationResult>();
            System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(ViewModel, serviceProvider.Object, null);
            return new ServiceValidationExeption(validationContext, validationResults);
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

        private GarmentPurchaseOrderMonitoringAllController GetController(Mock<IGarmentPurchaseOrderMonitoringAllFacade> facadeM)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);

            var servicePMock = GetServiceProvider();

            var controller = new GarmentPurchaseOrderMonitoringAllController(servicePMock.Object, facadeM.Object)
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
        public void Should_Success_GetReportAll()
        {
            var mockFacade = new Mock<IGarmentPurchaseOrderMonitoringAllFacade>();
            mockFacade.Setup(x => x.GetReport(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Tuple.Create(new List<GarmentPurchaseOrderMonitoringAllViewModel>(), 1));

            GarmentPurchaseOrderMonitoringAllController controller = GetController(mockFacade);
            var response = controller.GetReportAll("", "", "", "", "", "", "", "", DateTime.Now, DateTime.Now, "", 1, 25);
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public void Should_Error_GetReportAll()
        {
            var mockFacade = new Mock<IGarmentPurchaseOrderMonitoringAllFacade>();
            mockFacade.Setup(x => x.GetReport(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Tuple.Create(new List<GarmentPurchaseOrderMonitoringAllViewModel>(), 1));

            GarmentPurchaseOrderMonitoringAllController controller = new GarmentPurchaseOrderMonitoringAllController(GetServiceProvider().Object, mockFacade.Object);
            var response = controller.GetReportAll("", "", "", "", "", "", "", "", DateTime.Now, DateTime.Now, "", 1, 25);
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void Should_Success_GetXlsAll()
        {
            var mockFacade = new Mock<IGarmentPurchaseOrderMonitoringAllFacade>();
            mockFacade.Setup(x => x.GenerateExcel(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new System.IO.MemoryStream());

            GarmentPurchaseOrderMonitoringAllController controller = GetController(mockFacade);
            var response = controller.GetXlsAll("", "", "", "", "", "", "", "", DateTime.Now, DateTime.Now, "");
            Assert.Equal(null, response.GetType().GetProperty("FileStream"));
        }

        [Fact]
        public void Should_Error_GetXlsAll()
        {
            var mockFacade = new Mock<IGarmentPurchaseOrderMonitoringAllFacade>();

            GarmentPurchaseOrderMonitoringAllController controller = new GarmentPurchaseOrderMonitoringAllController(GetServiceProvider().Object, mockFacade.Object);
            var response = controller.GetXlsAll("", "", "", "", "", "", "", "", DateTime.Now, DateTime.Now, "");
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }
    }
}
