using Com.DanLiris.Service.Purchasing.Lib;
using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentPurchaseRequestFacades;
using Com.DanLiris.Service.Purchasing.Lib.Interfaces;
using Com.DanLiris.Service.Purchasing.Lib.Models.CoreModels;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentPurchaseRequestModel;
using Com.DanLiris.Service.Purchasing.Lib.Models.LocalGarmentMerchandiserModels;
using Com.DanLiris.Service.Purchasing.Lib.Services;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.LocalGarmentMerchandiserDataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace Com.DanLiris.Service.Purchasing.Test.Facades.GarmentPurchaseRequestTests
{
    public class ETLTest
    {
        private const string ENTITY = "GarmentPurchaseRequestETL";

        private const string USERNAME = "Unit Test";

        private Mock<IServiceProvider> GetServiceProvider()
        {
            var purchaseRequestFacade = new Mock<IGarmentPurchaseRequestFacade>();
            purchaseRequestFacade
                .Setup(x => x.Create(It.IsAny<GarmentPurchaseRequest>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(1);
            purchaseRequestFacade
                .Setup(x => x.Update(It.IsAny<int>(), It.IsAny<GarmentPurchaseRequest>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(1);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IdentityService)))
                .Returns(new IdentityService { Username = "Username", TimezoneOffset = 7 });
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IGarmentPurchaseRequestFacade)))
                .Returns(purchaseRequestFacade.Object);

            return serviceProviderMock;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        private PurchasingDbContext purchasingDbContext(string testName)
        {
            DbContextOptionsBuilder<PurchasingDbContext> optionsBuilder = new DbContextOptionsBuilder<PurchasingDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            PurchasingDbContext dbContext = new PurchasingDbContext(optionsBuilder.Options);

            return dbContext;
        }

        private CoreDbContext coreDbContext(string testName)
        {
            DbContextOptionsBuilder<CoreDbContext> optionsBuilder = new DbContextOptionsBuilder<CoreDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            CoreDbContext dbContext = new CoreDbContext(optionsBuilder.Options);

            return dbContext;
        }

        private LocalGarmentMerchandiserDbContext merchandiserDbContext(string testName)
        {
            DbContextOptionsBuilder<LocalGarmentMerchandiserDbContext> optionsBuilder = new DbContextOptionsBuilder<LocalGarmentMerchandiserDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            LocalGarmentMerchandiserDbContext dbContext = new LocalGarmentMerchandiserDbContext(optionsBuilder.Options);

            return dbContext;
        }

        private LocalGarmentMerchandiserDataUtil dataUtil(string testName)
        {
            return new LocalGarmentMerchandiserDataUtil(merchandiserDbContext(testName), coreDbContext(testName));
        }

        [Fact]
        public async void Should_Error_Run_ETL()
        {
            GarmentPurchaseRequestETLFacade facade = new GarmentPurchaseRequestETLFacade(GetServiceProvider().Object, merchandiserDbContext(GetCurrentMethod()), coreDbContext(GetCurrentMethod()));

            Exception e = await Assert.ThrowsAsync<Exception>(async () => await facade.Run());
            Assert.NotNull(e.Message);
        }

        [Fact]
        public async void Should_Success_Run_ETL_POrder()
        {
            GarmentPurchaseRequestETLFacade facade = new GarmentPurchaseRequestETLFacade(GetServiceProvider().Object, merchandiserDbContext(GetCurrentMethod()), coreDbContext(GetCurrentMethod()));

            var data = dataUtil(GetCurrentMethod()).GetTestDataPorder();

            var result = await facade.Run("Budget & POrder");

            Assert.NotEqual(0, result);
        }

        [Fact]
        public async void Should_Success_Run_ETL_POrder1()
        {
            GarmentPurchaseRequestETLFacade facade = new GarmentPurchaseRequestETLFacade(GetServiceProvider().Object, merchandiserDbContext(GetCurrentMethod()), coreDbContext(GetCurrentMethod()));

            var data = dataUtil(GetCurrentMethod()).GetTestDataPorder1();

            var result = await facade.Run("Budget1 & POrder1");

            Assert.NotEqual(0, result);
        }
    }
}
