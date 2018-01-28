﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.ApplicationServices;
using Woodford.Core.ApplicationServices.CommandDecorators;
using Woodford.Core.DomainServices;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Facades;
using Woodford.Core.Interfaces.Providers;
using Woodford.Core.Interfaces.Repositories;
using Woodford.Core.Interfaces.Services;
using Woodford.Infrastructure.Authenticate;
using Woodford.Infrastructure.Compression;
using Woodford.Infrastructure.Data;
using Woodford.Infrastructure.DataImporter;
using Woodford.Infrastructure.ExternalBookingSystem;
using Woodford.Infrastructure.ImageMananger;
using Woodford.Infrastructure.MailChimpBulkMailing;
using Woodford.Infrastructure.MyGatePaymentProcessor;
using Woodford.Infrastructure.Notifications;
using Woodford.Infrastructure.Resources;
using Woodford.Infrastructure.Reviews;
using Woodford.Infrastructure.Settings;
using Woodford.Infrastructure.Weather;

namespace Woodford.Tests.Infrastructure {
    public class TestBase {

        public static Container _container { get; set; }
        public ICommandBus _command;
        public IQueryProcessor _query;
        public Woodford2015Entities db;
        private bool _createNewDb = false;
        public TestBase(bool createNewDb = false) {
            _createNewDb = createNewDb;

            var container = new Container();

            InitializeContainer(container);


            container.Verify();



            TestBase._container = container;


        }

     

        [TestInitializeAttribute]
        public void Initialize() {


            IDbConnectionConfig connection = _container.GetInstance<IDbConnectionConfig>();

            
            _command = _container.GetInstance<ICommandBus>();
            _query = _container.GetInstance<IQueryProcessor>();

            db = new Woodford2015Entities(connection);



          


        }

        //[TestCleanupAttribute]
        //public void Cleanup() {
        //    IDbConnection _connection = _container.GetInstance<IDbConnection>();

        //    DbEntitiesData db = new DbEntitiesData(_connection.GetConnectionString());

        //    if (db.DatabaseExists() && _createNewDb) {
        //        SqlConnection.ClearAllPools();
        //        db.DeleteDatabase();
        //    }
        //}

        private static void InitializeContainer(SimpleInjector.Container container) {


            // For instance:

            container.Register<IAuthenticate, AuthenticationService>(Lifestyle.Transient);
            container.Register<IDbConnectionConfig, DbConnectionConfig>(Lifestyle.Transient);

            container.Register(typeof(ICommandHandler<>), AppDomain.CurrentDomain.GetAssemblies());
            container.Register(typeof(IQueryHandler<,>), AppDomain.CurrentDomain.GetAssemblies());

            container.Register<ICommandBus, DefaultCommandBus>();
            container.Register<IQueryProcessor, DefaultQueryProcessor>();

            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(TransactionCommandHandlerDecorator<>));

            container.Register<IUserService, UserService>(Lifestyle.Transient);
            container.Register<IUserRepository, UserRepository>(Lifestyle.Transient);
            container.Register<ISettingService, SettingService>(Lifestyle.Transient);
            container.Register<ISettingsRepository, SettingsRepository>(Lifestyle.Transient);
            container.Register<IBranchRepository, BranchRepository>(Lifestyle.Transient);
            container.Register<IBranchService, BranchService>(Lifestyle.Transient);

            container.Register<INotificationQueueService, EmailQueueService>(Lifestyle.Transient);
            container.Register<IMessageQueueRepository, MessageQueueRepository>(Lifestyle.Transient);

            container.Register<IFilePath, FilePath>(Lifestyle.Transient);

            container.Register<INotify, EmailNotificationService>(Lifestyle.Transient);

            container.Register<IFileUploadService, FileUploadService>(Lifestyle.Transient);
            container.Register<IFileUploadRepository, FileUploadRepository>(Lifestyle.Transient);
            container.Register<IImageSizer, ImageResizer>(Lifestyle.Transient);

            container.Register<IDynamicPageService, DynamicPageService>(Lifestyle.Transient);
            container.Register<IDynamicPageRepository, DynamicPageRespository>(Lifestyle.Transient);

            container.Register<IVehicleGroupService, VehicleGroupService>(Lifestyle.Transient);
            container.Register<IVehicleGroupRepository, VehicleGroupRepository>(Lifestyle.Transient);

            container.Register<IVehicleService, VehicleService>(Lifestyle.Transient);
            container.Register<IVehicleRepository, VehicleRepository>(Lifestyle.Transient);

            container.Register<IPageContentService, PageContentService>(Lifestyle.Transient);
            container.Register<IPageContentRepository, PageContentRepository>(Lifestyle.Transient);

            container.Register<ICampaignService, CampaignService>(Lifestyle.Transient);
            container.Register<ICampaignRepository, CampaignRepository>(Lifestyle.Transient);

            container.Register<IRateRuleService, RateRuleService>(Lifestyle.Transient);
            container.Register<IRateRuleRepository, RateRuleRepository>(Lifestyle.Transient);

            container.Register<IRateCodeService, RateCodeService>(Lifestyle.Transient);
            container.Register<IRateCodeRepository, RateCodeRepository>(Lifestyle.Transient);

            container.Register<IRateService, RateService>(Lifestyle.Transient);
            container.Register<IRateRepository, RateRepository>(Lifestyle.Transient);

            container.Register<IHerospaceService, HerospaceService>(Lifestyle.Transient);
            container.Register<IHerospaceRepository, HerospaceRepository>(Lifestyle.Transient);

            container.Register<IRateAdjustmentService, RateAdjustmentService>(Lifestyle.Transient);
            container.Register<IRateAdjustmentRepository, RateAdjustmentRepository>(Lifestyle.Transient);

            container.Register<IBranchVehicleService, BranchVehicleService>(Lifestyle.Transient);
            container.Register<IBranchVehicleRepository, BranchVehicleRepository>(Lifestyle.Transient);

            container.Register<IBranchVehicleExclusionService, BranchVehicleExclusionService>(Lifestyle.Transient);
            container.Register<IBranchVehicleExclusionRepository, BranchVehicleExclusionRepository>(Lifestyle.Transient);

            container.Register<IVoucherService, VoucherService>(Lifestyle.Transient);
            container.Register<IVoucherRepository, VoucherRepository>(Lifestyle.Transient);

            container.Register<ILoyaltyService, LoyaltyService>(Lifestyle.Transient);
            container.Register<ILoyaltyRepository, LoyaltyRepository>(Lifestyle.Transient);

            container.Register<IVehicleUpgradeService, VehicleUpgradeService>(Lifestyle.Transient);
            container.Register<IVehicleUpgradeRepository, VehicleUpgradeRepository>(Lifestyle.Transient);

            container.Register<ICountdownSpecialService, CountdownSpecialService>(Lifestyle.Transient);
            container.Register<ICountdownSpecialRepository, CountdownSpecialRepository>(Lifestyle.Transient);

            container.Register<ISearchService, SearchService>(Lifestyle.Transient);

            container.Register<ICorporateService, CorporateService>(Lifestyle.Transient);
            container.Register<ICorporateRepository, CorporateRepository>(Lifestyle.Transient);

            container.Register<IVehicleExtrasService, VehicleExtrasService>(Lifestyle.Transient);
            container.Register<IVehicleExtrasRepository, VehicleExtrasRepository>(Lifestyle.Transient);

            container.Register<IInterBranchDropOffFeeService, InterBranchDropOffFeeService>(Lifestyle.Transient);
            container.Register<IInterBranchDropOffFeeRepository, InterBranchDropOffFeeRepository>(Lifestyle.Transient);

            container.Register<IReservationBuilder, ReservationBuilder>(Lifestyle.Transient);
            container.Register<IReservationService, ReservationService>(Lifestyle.Transient);
            container.Register<IReservationRepository, ReservationRepository>(Lifestyle.Transient);

            container.Register<IInvoiceService, InvoiceService>(Lifestyle.Transient);
            container.Register<IInvoiceRepository, InvoiceRepository>(Lifestyle.Transient);

            container.Register<IPaymentTransactionService, PaymentTransactionService>(Lifestyle.Transient);
            container.Register<IPaymentTransactionRepository, MyGatePaymentTransactionRepository>(Lifestyle.Transient);

            container.Register<IWaiverService, WaiverService>(Lifestyle.Transient);
            container.Register<IWaiverRepository, WaiverRepository>(Lifestyle.Transient);

            container.Register<INewsService, NewsService>(Lifestyle.Transient);
            container.Register<INewsRepository, NewsRepository>(Lifestyle.Transient);

            container.Register<INewsCategoryService, NewsCategoryService>(Lifestyle.Transient);
            container.Register<INewsCategoryRepository, NewsCategoryRepository>(Lifestyle.Transient);

            container.Register<IBookingHistoryService, BookingHistoryService>(Lifestyle.Transient);
            container.Register<IBookingHistoryRepository, BookingHistoryRepository>(Lifestyle.Transient);

            container.Register<IUrlRedirectService, UrlRedirectService>(Lifestyle.Transient);
            container.Register<IUrlRedirectRepository, UrlRedirectRepository>(Lifestyle.Transient);

            container.Register<IBranchSurchargeService, BranchSurchargeService>(Lifestyle.Transient);
            container.Register<IBranchSurchageRepository, BranchSurchargeRepository>(Lifestyle.Transient);

            container.Register<IBranchRateCodeExclusionService, BranchRateCodeExclusionService>(Lifestyle.Transient);
            container.Register<IBranchRateCodeExclusionRepository, BranchRateCodeExclusionRepository>(Lifestyle.Transient);

            container.Register<IRolesRepository, RolesRepository>(Lifestyle.Transient);
            container.Register<IRolesService, RolesService>(Lifestyle.Transient);

            container.Register<IReportRepository, ReportRepository>(Lifestyle.Transient);
            container.Register<IReportService, ReportService>(Lifestyle.Transient);


            container.Register<IEmailSignatureRepository, EmailSignatureRepository>(Lifestyle.Transient);
            container.Register<IEmailSignatureService, EmailSignatureService>(Lifestyle.Transient);

            container.Register<IEmailSignatureCampaignRepository, EmailSignatureCampaignRepository>(Lifestyle.Transient);
            container.Register<IEmailSignatureCampaignService, EmailSignatureCampaignService>(Lifestyle.Transient);

            container.Register<IAuditService, AuditService>(Lifestyle.Transient);
            container.Register<IAuditRepository, AuditRepository>(Lifestyle.Transient);

            container.Register<IPaymentProcessor, MyGatePaymentProcessor>(Lifestyle.Transient);

            container.Register<IDataImportService, RezCentralDataImporter>(Lifestyle.Transient);

            container.Register<IWeatherService, WeatherService>(Lifestyle.Transient);

            container.Register<IBulkMailingService, MailChimpBulkMailingService>(Lifestyle.Transient);

            container.Register<INotificationBuilder, NotifcationBuilder>(Lifestyle.Transient);
            //INotificationBuilder
            container.Register<IClaimBookingRepository, ClaimBookingRepository>(Lifestyle.Transient);
            container.Register<IExternalSystemService, TSDExternalBookingSystem>(Lifestyle.Transient);

            container.Register<IReviewRepository, ReviewRepository>(Lifestyle.Transient);
            container.Register<IReviewService, ReviewService>(Lifestyle.Transient);

            container.Register<IExternalReviewService, ExternalReviewService>(Lifestyle.Transient);

            container.Register<ICompressService, CompressionService>(Lifestyle.Transient);

            container.Register<IVehicleManufacturerRepository, VehicleManufacturerRepository>(Lifestyle.Transient);

        }


    }

    public class DefaultCommandBus : ICommandBus {

        public void Submit<TCommand>(TCommand command) where TCommand : ICommand {
            var handler = TestBase._container.GetInstance<ICommandHandler<TCommand>>();
            if (!((handler != null) && handler is ICommandHandler<TCommand>)) {
                throw new Exception("Command Handler not found");
            }
            handler.Handle(command);
        }
    }

    public class DefaultQueryProcessor : IQueryProcessor {
        public TResult Process<TResult>(IQuery<TResult> query) {

            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

            dynamic handler = TestBase._container.GetInstance(handlerType);

            return handler.Process((dynamic)query);
        }
    }


}
