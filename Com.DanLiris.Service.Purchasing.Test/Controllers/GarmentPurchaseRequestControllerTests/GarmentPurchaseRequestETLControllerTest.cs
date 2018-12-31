using AutoMapper;
using Com.DanLiris.Service.Purchasing.Lib.Interfaces;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentInternalPurchaseOrderModel;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentPurchaseRequestModel;
using Com.DanLiris.Service.Purchasing.Lib.Services;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.GarmentInternalPurchaseOrderViewModel;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.GarmentPurchaseRequestViewModel;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.NewIntegrationViewModel;
using Com.DanLiris.Service.Purchasing.Test.Helpers;
using Com.DanLiris.Service.Purchasing.WebApi.Controllers.v1.GarmentPurchaseRequestControllers;
using Com.Moonlay.NetCore.Lib.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Com.DanLiris.Service.Purchasing.Test.Controllers.GarmentPurchaseRequestControllerTests
{
    public class GarmentPurchaseRequestETLControllerTest
    {
        private GarmentPurchaseRequestViewModel ViewModel
        {
            get
            {
                return new GarmentPurchaseRequestViewModel
                {
                    Buyer = new BuyerViewModel(),
                    Unit = new UnitViewModel()
                };
            }
        }

        private GarmentPurchaseRequest Model
        {
            get
            {
                return new GarmentPurchaseRequest { };
            }
        }

        private ServiceValidationExeption GetServiceValidationExeption()
        {
            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
            List<ValidationResult> validationResults = new List<ValidationResult>();
            System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(this.ViewModel, serviceProvider.Object, null);
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

        private GarmentPurchaseRequestETLController GetController(Mock<IGarmentPurchaseRequestFacade> purchaseRequestFacadeM, Mock<IGarmentPurchaseRequestETLFacade> purchaseRequestETLFacadeM, Mock<IValidateService> validateM, Mock<IMapper> mapper)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);

            var servicePMock = GetServiceProvider();
            if(validateM != null)
            {
                servicePMock
                    .Setup(x => x.GetService(typeof(IValidateService)))
                    .Returns(validateM.Object);
            }

            GarmentPurchaseRequestETLController controller = new GarmentPurchaseRequestETLController(servicePMock.Object, mapper.Object, purchaseRequestFacadeM.Object, purchaseRequestETLFacadeM.Object)
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
        public void Should_Success_Get_All_Data()
        {
            var mockPurchaseRequestFacade = new Mock<IGarmentPurchaseRequestFacade>();
            mockPurchaseRequestFacade.Setup(x => x.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), null, It.IsAny<string>()))
                .Returns(Tuple.Create(new List<GarmentPurchaseRequest>(), 0, new Dictionary<string, string>()));

            var mockPurchaseRequestETLFacade = new Mock<IGarmentPurchaseRequestETLFacade>();

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<List<GarmentPurchaseRequestViewModel>>(It.IsAny<List<GarmentPurchaseRequest>>()))
                .Returns(new List<GarmentPurchaseRequestViewModel> { ViewModel });

            GarmentPurchaseRequestETLController controller = GetController(mockPurchaseRequestFacade, mockPurchaseRequestETLFacade, null, mockMapper);
            var response = controller.Get();
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public void Should_Error_Get_All_Data()
        {
            var mockPurchaseRequestFacade = new Mock<IGarmentPurchaseRequestFacade>();

            mockPurchaseRequestFacade.Setup(x => x.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), null, It.IsAny<string>()))
                .Returns(Tuple.Create(new List<GarmentPurchaseRequest>(), 0, new Dictionary<string, string>()));

            var mockPurchaseRequestETLFacade = new Mock<IGarmentPurchaseRequestETLFacade>();

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<List<GarmentPurchaseRequestViewModel>>(It.IsAny<List<GarmentPurchaseRequest>>()))
                .Returns(new List<GarmentPurchaseRequestViewModel> { ViewModel });

            GarmentPurchaseRequestETLController controller = new GarmentPurchaseRequestETLController(GetServiceProvider().Object, mockMapper.Object, mockPurchaseRequestFacade.Object, mockPurchaseRequestETLFacade.Object);
            var response = controller.Get();
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void Should_Success_Get_Data_By_Id()
        {
            var mockPurchaseRequestFacade = new Mock<IGarmentPurchaseRequestFacade>();
            mockPurchaseRequestFacade.Setup(x => x.ReadById(It.IsAny<int>()))
                .Returns(new GarmentPurchaseRequest());

            var mockPurchaseRequestETLFacade = new Mock<IGarmentPurchaseRequestETLFacade>();


            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<GarmentPurchaseRequestViewModel>(It.IsAny<GarmentPurchaseRequest>()))
                .Returns(ViewModel);

            GarmentPurchaseRequestETLController controller = GetController(mockPurchaseRequestFacade, mockPurchaseRequestETLFacade, null, mockMapper);
            var response = controller.Get(It.IsAny<int>());
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public void Should_Error_Get_Data_By_Id()
        {
            var mockPurchaseRequestFacade = new Mock<IGarmentPurchaseRequestFacade>();
            mockPurchaseRequestFacade.Setup(x => x.ReadById(It.IsAny<int>()))
                .Returns(new GarmentPurchaseRequest());

            var mockPurchaseRequestETLFacade = new Mock<IGarmentPurchaseRequestETLFacade>();

            var mockMapper = new Mock<IMapper>();

            GarmentPurchaseRequestETLController controller = new GarmentPurchaseRequestETLController(GetServiceProvider().Object, mockMapper.Object, mockPurchaseRequestFacade.Object, mockPurchaseRequestETLFacade.Object);
            var response = controller.Get(It.IsAny<int>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void Should_Success_Post_Data()
        {
            var validateMock = new Mock<IValidateService>();
            validateMock.Setup(s => s.Validate(It.IsAny<GarmentPurchaseRequestControllerETLPostParameter>())).Verifiable();

            var mockMapper = new Mock<IMapper>();
            var mockPurchaseRequestFacade = new Mock<IGarmentPurchaseRequestFacade>();

            var mockPurchaseRequestETLFacade = new Mock<IGarmentPurchaseRequestETLFacade>();
            mockPurchaseRequestETLFacade
                .Setup(x => x.Run(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<BuyerViewModel>()))
                .ReturnsAsync(1);

            var controller = GetController(mockPurchaseRequestFacade, mockPurchaseRequestETLFacade, validateMock, mockMapper);

            var response = controller.Post(new GarmentPurchaseRequestControllerETLPostParameter()).Result;
            Assert.Equal((int)HttpStatusCode.Created, GetStatusCode(response));
        }

        [Fact]
        public void Should_Error_Post_Data()
        {
            var validateMock = new Mock<IValidateService>();
            validateMock.Setup(s => s.Validate(It.IsAny<GarmentPurchaseRequestControllerETLPostParameter>())).Verifiable();

            var mockMapper = new Mock<IMapper>();
            var mockPurchaseRequestFacade = new Mock<IGarmentPurchaseRequestFacade>();
            var mockPurchaseRequestETLFacade = new Mock<IGarmentPurchaseRequestETLFacade>();

            var controller = new GarmentPurchaseRequestETLController(GetServiceProvider().Object, mockMapper.Object, mockPurchaseRequestFacade.Object, mockPurchaseRequestETLFacade.Object);

            var response = controller.Post(new GarmentPurchaseRequestControllerETLPostParameter()).Result;
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }
 
        [Fact]
        public void Should_Validate_Post_Data()
        {
            var validateMock = new Mock<IValidateService>();
            validateMock.Setup(s => s.Validate(It.IsAny<GarmentPurchaseRequestControllerETLPostParameter>())).Throws(GetServiceValidationExeption());

            var mockMapper = new Mock<IMapper>();
            var mockPurchaseRequestFacade = new Mock<IGarmentPurchaseRequestFacade>();
            var mockPurchaseRequestETLFacade = new Mock<IGarmentPurchaseRequestETLFacade>();

            var controller = GetController(mockPurchaseRequestFacade, mockPurchaseRequestETLFacade, validateMock, mockMapper);

            var response = controller.Post(new GarmentPurchaseRequestControllerETLPostParameter()).Result;
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(response));
        }

        [Fact]
        public void Should_Success_Validate_Data()
        {
            GarmentPurchaseRequestControllerETLPostParameter postParameter = new GarmentPurchaseRequestControllerETLPostParameter();
            Assert.True(postParameter.Validate(null).Count() > 0);
        }
    }
}
