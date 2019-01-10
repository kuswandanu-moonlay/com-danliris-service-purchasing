using AutoMapper;
using Com.DanLiris.Service.Purchasing.Lib;
using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentBeacukaiFacade;
using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentCorrectionNoteFacades;
using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentDeliveryOrderFacades;
using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentExternalPurchaseOrderFacades;
using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentInternalPurchaseOrderFacades;
using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentInternNoteFacades;
using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentInvoiceFacades;
using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentPurchaseRequestFacades;
using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentUnitReceiptNoteFacades;
using Com.DanLiris.Service.Purchasing.Lib.Interfaces;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentDeliveryOrderModel;
using Com.DanLiris.Service.Purchasing.Lib.Services;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.GarmentBeacukaiDataUtils;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.GarmentCorrectionNoteDataUtils;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.GarmentDeliveryOrderDataUtils;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.GarmentExternalPurchaseOrderDataUtils;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.GarmentInternalPurchaseOrderDataUtils;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.GarmentInternNoteDataUtils;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.GarmentInvoiceDataUtils;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.GarmentPurchaseRequestDataUtils;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.GarmentUnitReceiptNoteDataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.DanLiris.Service.Purchasing.Test.Facades.GarmentInternalPurchaseOrderTests
{
    public class GarmentPurchaseOrderMonitoringAllTest
    {
        private const string ENTITY = "GarmentPurchaseOrderMonitoringAll";

        private const string USERNAME = "Unit Test";
        private IServiceProvider ServiceProvider { get; set; }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }
        private IServiceProvider GetServiceProvider()
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            httpResponseMessage.Content = new StringContent("{\"apiVersion\":\"1.0\",\"statusCode\":200,\"message\":\"Ok\",\"data\":[{\"Id\":7,\"code\":\"USD\",\"rate\":13700.0,\"date\":\"2018/10/20\"}],\"info\":{\"count\":1,\"page\":1,\"size\":1,\"total\":2,\"order\":{\"date\":\"desc\"},\"select\":[\"Id\",\"code\",\"rate\",\"date\"]}}");

            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
                .Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(httpResponseMessage);

            var mapper = new Mock<IMapper>();

            var mockGarmentDeliveryOrderFacade = new Mock<IGarmentDeliveryOrderFacade>();
            mockGarmentDeliveryOrderFacade
                .Setup(x => x.ReadById(It.IsAny<int>()))
                .Returns(new GarmentDeliveryOrder());

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IdentityService)))
                .Returns(new IdentityService { Username = USERNAME });
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IMapper)))
                .Returns(mapper.Object);
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IGarmentDeliveryOrderFacade)))
                .Returns(mockGarmentDeliveryOrderFacade.Object);


            return serviceProviderMock.Object;
        }

        private PurchasingDbContext _dbContext(string testName)
        {
            DbContextOptionsBuilder<PurchasingDbContext> optionsBuilder = new DbContextOptionsBuilder<PurchasingDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            PurchasingDbContext dbContext = new PurchasingDbContext(optionsBuilder.Options);

            return dbContext;
        }

        private async Task<(string unit, string category, string epoNo, string roNo, string prNo, string doNo, string supplier, string staff)> dataUtil(string testName)
        {
            var garmentPurchaseRequestFacade = new GarmentPurchaseRequestFacade(_dbContext(testName));
            var garmentPurchaseRequestDataUtil = new GarmentPurchaseRequestDataUtil(garmentPurchaseRequestFacade);

            var garmentInternalPurchaseOrderFacade = new GarmentInternalPurchaseOrderFacade(_dbContext(testName));
            var garmentInternalPurchaseOrderDataUtil = new GarmentInternalPurchaseOrderDataUtil(garmentInternalPurchaseOrderFacade, garmentPurchaseRequestDataUtil);

            var garmentExternalPurchaseOrderFacade = new GarmentExternalPurchaseOrderFacade(ServiceProvider, _dbContext(testName));
            var garmentExternalPurchaseOrderDataUtil = new GarmentExternalPurchaseOrderDataUtil(garmentExternalPurchaseOrderFacade, garmentInternalPurchaseOrderDataUtil);

            var garmentDeliveryOrderFacade = new GarmentDeliveryOrderFacade(GetServiceProvider(), _dbContext(testName));
            var garmentDeliveryOrderDataUtil = new GarmentDeliveryOrderDataUtil(garmentDeliveryOrderFacade, garmentExternalPurchaseOrderDataUtil);

            var garmentCorrectionNotePriceFacade = new GarmentCorrectionNotePriceFacade(GetServiceProvider(), _dbContext(testName));
            var garmentCorrectionNotePriceDataUtil = new GarmentCorrectionNoteDataUtil(garmentCorrectionNotePriceFacade, garmentDeliveryOrderDataUtil);
            await garmentCorrectionNotePriceDataUtil.GetTestDataKoreksiHargaTotal();

            var garmentBeacukaiFacade = new GarmentBeacukaiFacade(_dbContext(testName), GetServiceProvider());
            var garmentBeacukaiDataUtil = new GarmentBeacukaiDataUtil(garmentDeliveryOrderDataUtil , garmentBeacukaiFacade);
            await garmentBeacukaiDataUtil.GetTestData(USERNAME);

            var garmentUnitReceiptNoteFacade = new GarmentUnitReceiptNoteFacade(GetServiceProvider(), _dbContext(testName));
            var garmentUnitReceiptNoteDataUtil = new GarmentUnitReceiptNoteDataUtil(garmentUnitReceiptNoteFacade, garmentDeliveryOrderDataUtil);
            await garmentUnitReceiptNoteDataUtil.GetTestData();

            var garmentInvoiceFacade = new GarmentInvoiceFacade(_dbContext(testName), ServiceProvider);
            var garmentInvoiceDetailDataUtil = new GarmentInvoiceDetailDataUtil();
            var garmentInvoiceItemDataUtil = new GarmentInvoiceItemDataUtil(garmentInvoiceDetailDataUtil);
            var garmentInvoieDataUtil = new GarmentInvoiceDataUtil(garmentInvoiceItemDataUtil, garmentInvoiceDetailDataUtil, garmentDeliveryOrderDataUtil, garmentInvoiceFacade);
            await garmentInvoieDataUtil.GetTestData(USERNAME);

            var facade = new GarmentInternNoteFacades(_dbContext(testName), GetServiceProvider());
            var garmentInternNoteDataUtil = new GarmentInternNoteDataUtil(garmentInvoieDataUtil, facade);
            await garmentInternNoteDataUtil.GetTestData();

            return ("", "", "", "", "", "", "", "");
        }

        [Fact]
        public async void Should_Success_GetReport()
        {
            GarmentPurchaseOrderMonitoringAllFacade facade = new GarmentPurchaseOrderMonitoringAllFacade(_dbContext(GetCurrentMethod()));

            var data = await dataUtil(GetCurrentMethod());

            var result = facade.GetReport(data.unit, data.category, data.epoNo, data.roNo, data.prNo, data.doNo, data.supplier, data.staff, DateTime.Now, DateTime.Now, "", 1, 25, "{}", 7);

            Assert.NotEqual(0, result.Item2);
        }

        [Fact]
        public void Should_Success_GetReport_NoResult()
        {
            GarmentPurchaseOrderMonitoringAllFacade facade = new GarmentPurchaseOrderMonitoringAllFacade(_dbContext(GetCurrentMethod()));

            var result = facade.GetReport("", "", "", "", "", "", "", "", DateTime.Now, DateTime.Now, "", 1, 25, "{}", 7);

            Assert.Equal(0, result.Item2);
        }

        [Fact]
        public async void Should_Success_GenerateExcel()
        {
            GarmentPurchaseOrderMonitoringAllFacade facade = new GarmentPurchaseOrderMonitoringAllFacade(_dbContext(GetCurrentMethod()));

            //await Task.Run(() => dataUtil(GetCurrentMethod()));

            var result = facade.GenerateExcel("", "", "", "", "", "", "", "", DateTime.Now, DateTime.Now, "", 7);

            Assert.IsType(typeof(System.IO.MemoryStream), result);
        }
    }
}
