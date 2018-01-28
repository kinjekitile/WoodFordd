using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.ApplicationServices.Utilities;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Facades;
using Woodford.Core.Interfaces.Providers;

namespace Woodford.Core.ApplicationServices {
    public class NotifcationBuilder : INotificationBuilder {

        private readonly ISettingService _settings;
        private readonly IInvoiceService _invoiceService;
        private readonly IPaymentTransactionService _paymentTransactionService;
        private readonly IReservationService _reservationService;
        private readonly IUserService _userService;
        private readonly ILoyaltyService _loyaltyService;
        private readonly IVehicleService _vehicleService;
        private readonly IBookingHistoryService _bookingHistoryService;
        private readonly IVehicleUpgradeService _upgradeService;
        private readonly IBranchService _branchService;
        private readonly IWeatherService _weatherService;
        private readonly ICampaignService _campaignService;
        private readonly INewsService _newsService;

        private readonly INewsCategoryService _newsCatService;

        public NotifcationBuilder(ISettingService settings, IInvoiceService invoiceService, IPaymentTransactionService paymentTransactionService, IReservationService reservationService, IUserService userService, ILoyaltyService loyaltyService, IVehicleService vehicleService, IBookingHistoryService bookingHistoryService, IVehicleUpgradeService upgradeService, IBranchService branchService, IWeatherService weatherService, ICampaignService campaignService, INewsService newsService, INewsCategoryService newsCatService) {

            _settings = settings;
            _invoiceService = invoiceService;
            _paymentTransactionService = paymentTransactionService;
            _reservationService = reservationService;

            _userService = userService;
            _loyaltyService = loyaltyService;
            _vehicleService = vehicleService;
            _bookingHistoryService = bookingHistoryService;
            _upgradeService = upgradeService;
            _branchService = branchService;
            _weatherService = weatherService;
            _campaignService = campaignService;
            _newsService = newsService;
            _newsCatService = newsCatService;
        }

        public ReservationInvoiceNotificationModel BuildReservationInvoiceModel(int reservationId) {
            ReservationInvoiceNotificationModel model = new ReservationInvoiceNotificationModel();

            model.SiteDomain = _settings.GetValue<string>(Setting.Public_Website_Domain);

            var reservation = _reservationService.GetById(reservationId);
            reservation.Vehicle = _vehicleService.GetById(reservation.VehicleId, includePageContent: false);
            if (reservation.VehicleUpgradeId.HasValue) {
                var upgrade = _upgradeService.GetById(reservation.VehicleUpgradeId.Value);
                reservation.VehicleUpgrade = _vehicleService.GetById(upgrade.ToVehicleId, includePageContent: false);
            }
            var invoice = _invoiceService.GetByReservationId(reservation.Id);

            model.PickupBranch = _branchService.GetById(reservation.PickupBranchId);

            model.Invoice = invoice;
            model.Reservation = reservation;

            UserModel user = null;
            if (reservation.UserId.HasValue) {
                user = _userService.GetById(reservation.UserId.Value);
                model.User = user;
                model.Reservation.User = user;
                model.UserLoyaltyTier = user.LoyaltyTier.GetEnumDescription();
            }



            return model;
        }

        public LoyaltyPointsEarnedNotificationModel BuildLoyaltyPointsEarnedModel(BookingHistoryModel booking) {

            var user = _userService.GetById(booking.UserId.Value);
            LoyaltyPointsEarnedNotificationModel emailModel = new LoyaltyPointsEarnedNotificationModel();

            //emailModel.SiteDomain = _settings.GetValue<string>(Setting.Public_Website_Domain);

            emailModel.User = user;
            emailModel.PickupDate = booking.PickupDate;
            emailModel.DropoffDate = booking.DropOffDate;
            emailModel.PointsEarned = booking.LoyaltyPointsEarned.Value;
            emailModel.MobileNumber = user.MobileNumber;

            var reservations = _reservationService.Get(new ReservationFilterModel { UserId = user.Id, IsCompletedInvoice = true }, null).Items;



            var allUserBookings = _bookingHistoryService.Get(new BookingHistoryFilterModel { UserId = user.Id }, null).Items;
            allUserBookings = allUserBookings.Where(x => x.PickupDate >= user.LoyaltySignUpDate).ToList();
            decimal pointsSpent = 0m;
            decimal pointsEarned = 0m;

            if (reservations.Sum(x => x.LoyaltyPointsSpent).HasValue) {
                pointsSpent = reservations.Sum(x => x.LoyaltyPointsSpent).Value;
            }

            if (allUserBookings.Sum(x => x.LoyaltyPointsEarned).HasValue) {
                pointsEarned = allUserBookings.Sum(x => x.LoyaltyPointsEarned).Value;
            }

            decimal pointsRemainging = pointsEarned - pointsSpent;

            emailModel.PointsRemaining = pointsRemainging;
            emailModel.LoyaltyTier = user.LoyaltyTier.GetEnumDescription();

            return emailModel;
        }

        public ReservationInvoiceNotificationModel BuildReservationThankYouModel(int reservationId) {
            ReservationInvoiceNotificationModel model = new ReservationInvoiceNotificationModel();

            model.SiteDomain = _settings.GetValue<string>(Setting.Public_Website_Domain);

            var item = _reservationService.GetById(reservationId);
            model.Reservation = item;
            model.Invoice = _invoiceService.GetByReservationId(item.Id);
            if (item.UserId.HasValue) {
                model.User = _userService.GetById(item.UserId.Value);
            }


            return model;
        }

        public ReservationInvoiceNotificationModel BuildReservationReminderModel(int reservationId) {
            var reservation = _reservationService.GetById(reservationId);

            ReservationWeatherNotificationModel weather = _weatherService.GetWeatherForPickupLocation(reservation.PickupBranchId);
            weather.Items = weather.Items.Where(x => x.ReportDate >= reservation.PickupDate.Date).ToList();

            ReservationInvoiceNotificationModel model = new ReservationInvoiceNotificationModel();

            model.SiteDomain = _settings.GetValue<string>(Setting.Public_Website_Domain);

            if (reservation.UserId.HasValue) {
                var user = _userService.GetById(reservation.UserId.Value);
                model.UserLoyaltyTier = user.LoyaltyTier.GetEnumDescription();
                model.User = user;
            }

            model.PickupBranch = _branchService.GetById(reservation.PickupBranchId);

            model.Reservation = reservation;
            model.Invoice = _invoiceService.GetByReservationId(reservation.Id);
            model.Reservation.Vehicle = _vehicleService.GetById(model.Reservation.VehicleId, includePageContent: false);
            if (model.Reservation.VehicleUpgradeId.HasValue) {
                var upgrade = _upgradeService.GetById(model.Reservation.VehicleUpgradeId.Value);
                model.Reservation.VehicleUpgrade = _vehicleService.GetById(upgrade.ToVehicleId, includePageContent: false);
            }
            model.Weather = weather;

            return model;
        }

        public LoyaltyPointsSpentNotificationModel BuildLoyaltyPointsSpentModel(int reservationId) {

            var reservation = _reservationService.GetById(reservationId);

            if (!reservation.UserId.HasValue) {
                return null;
            }

            var user = _userService.GetById(reservation.UserId.Value);

            LoyaltyPointsSpentNotificationModel emailModel = new LoyaltyPointsSpentNotificationModel();

            //emailModel.SiteDomain = _settings.GetValue<string>(Setting.Public_Website_Domain);

            emailModel.LoyaltyTier = user.LoyaltyTier.GetEnumDescription();
            emailModel.User = user;
            emailModel.PickupDate = reservation.PickupDate;
            emailModel.DropoffDate = reservation.DropOffDate;
            emailModel.PointsSpent = reservation.LoyaltyPointsSpent.Value;
            emailModel.MobileNumber = reservation.MobileNumber;

            var reservations = _reservationService.Get(new ReservationFilterModel { UserId = user.Id, IsCompletedInvoice = true }, null).Items;

            var allUserBookings = _bookingHistoryService.Get(new BookingHistoryFilterModel { UserId = user.Id }, null).Items;
            allUserBookings = allUserBookings.Where(x => x.PickupDate >= user.LoyaltySignUpDate).ToList();
            decimal pointsSpent = 0m;
            decimal pointsEarned = 0m;

            if (reservations.Sum(x => x.LoyaltyPointsSpent).HasValue) {
                pointsSpent = reservations.Sum(x => x.LoyaltyPointsSpent).Value;
            }

            if (allUserBookings.Sum(x => x.LoyaltyPointsEarned).HasValue) {
                pointsEarned = allUserBookings.Sum(x => x.LoyaltyPointsEarned).Value;
            }

            decimal pointsRemainging = pointsEarned - pointsSpent;

            emailModel.PointsRemaining = pointsRemainging;

            return emailModel;
        }

        public List<AdvanceReportNewsAndCampaignItem> BuildNewsAndCampaignItems() {

            List<AdvanceReportNewsAndCampaignItem> items = new List<AdvanceReportNewsAndCampaignItem>();

            var cats = _newsCatService.Get(new NewsCategoryFilterModel { IsArchived = false }, null).Items;

            //Get latest campaign, and latest two news items
            var latestCampaigns = _campaignService.Get(new CampaignFilterModel { IsArchived = false }, null).Items;

            latestCampaigns = latestCampaigns.Where(x => x.StartDate <= DateTime.Today && x.EndDate >= DateTime.Today).ToList();
            CampaignModel latestCampaign = null; // = new CampaignModel();
            if (latestCampaigns.Count() > 0) {
                latestCampaign = latestCampaigns.OrderByDescending(x => x.Id).FirstOrDefault();
            }

            int newsCount = 2;
            if (latestCampaign == null) {
                newsCount = 3;
            }
            else {
                AdvanceReportNewsAndCampaignItem campaignItem = new AdvanceReportNewsAndCampaignItem();
                campaignItem.Title = latestCampaign.Title;
                campaignItem.UrlPart = "campaigns/" + latestCampaign.PageUrl;
                campaignItem.ImageUrlPart = "uploads/index/" + latestCampaign.FileUploadId.Value;
                campaignItem.UseWidth = true;
                items.Add(campaignItem);
            }

            var news = _newsService.Get(new NewsFilterModel { IsArchived = false }, null).Items;
            news = news.Where(x => x.FileUploadId.HasValue).Take(newsCount).ToList();

            foreach (var item in news) {
                AdvanceReportNewsAndCampaignItem newsItem = new AdvanceReportNewsAndCampaignItem();
                newsItem.Title = item.Headline;
                newsItem.UrlPart = "news/article/" + item.PageUrl;
                newsItem.ImageUrlPart = "uploads/index/" + item.FileUploadId.Value;
                items.Add(newsItem);
            }

            return items;
        }

        public AdvanceReportNotificationModel BuildLoyaltyReport(UserModel user, List<AdvanceReportNewsAndCampaignItem> newsAndCampaigns) {
            AdvanceReportNotificationModel model = new AdvanceReportNotificationModel();

            model.User = user;
            model.SiteDomain = _settings.GetValue<string>(Setting.Public_Website_Domain);
            model.SiteDomain = "https://www.woodford.co.za/";


            var loyaltyTier = (LoyaltyTierLevel)user.LoyaltyTierId;

            switch (loyaltyTier) {
                case LoyaltyTierLevel.Green:
                    var greenTier = _loyaltyService.GetByLevel(LoyaltyTierLevel.Green);
                    model.BookingsRequiredForCurrentLoyaltyTier = greenTier.BookingThresholdPerPeriod;
                    model.BookingsRequiredForNextLoyaltyTier = _loyaltyService.GetByLevel(LoyaltyTierLevel.Silver).BookingThresholdPerPeriod;
                    model.LoyaltyTierPointsPercentage = greenTier.PointsEarnedPerRandSpent;
                    break;
                case LoyaltyTierLevel.Silver:
                    var silverTier = _loyaltyService.GetByLevel(LoyaltyTierLevel.Silver);
                    model.BookingsRequiredForCurrentLoyaltyTier = silverTier.BookingThresholdPerPeriod;
                    model.BookingsRequiredForNextLoyaltyTier = _loyaltyService.GetByLevel(LoyaltyTierLevel.Gold).BookingThresholdPerPeriod;
                    model.LoyaltyTierPointsPercentage = silverTier.PointsEarnedPerRandSpent;
                    break;

                case LoyaltyTierLevel.Gold:
                    var goldTier = _loyaltyService.GetByLevel(LoyaltyTierLevel.Gold);
                    model.BookingsRequiredForCurrentLoyaltyTier = goldTier.BookingThresholdPerPeriod;
                    model.BookingsRequiredForNextLoyaltyTier = _loyaltyService.GetByLevel(LoyaltyTierLevel.Gold).BookingThresholdPerPeriod;
                    model.LoyaltyTierPointsPercentage = goldTier.PointsEarnedPerRandSpent;
                    model.IsFinalTier = true;
                    break;
            }





            var bookings = _bookingHistoryService.Get(new BookingHistoryFilterModel { UserId = user.Id }, null).Items;
            bookings = bookings.Where(x => x.PickupDate >= user.LoyaltySignUpDate).ToList();
            var bookingsPerPeriod = bookings.Where(x => x.PickupDate >= user.LoyaltyPeriodStart.Value && x.PickupDate <= user.LoyaltyPeriodEnd).ToList();




            LoyaltyOverviewModel overview = new LoyaltyOverviewModel();
            overview.HistoryItemsForPeriod = bookingsPerPeriod;


            overview.BookingPerLoyaltyPeriod = bookingsPerPeriod.Count();
            model.LoyaltyBookingsPerPeriod = bookingsPerPeriod.Count();

            decimal? pointsEarnedPerPeriod = bookingsPerPeriod.Sum(x => x.LoyaltyPointsEarned);
            if (pointsEarnedPerPeriod.HasValue) {
                overview.PointsEarnedPerPeriod = pointsEarnedPerPeriod.Value;
            }
            decimal? totalPointsEarned = user.LoyaltyPointsEarned;
            if (totalPointsEarned.HasValue) {
                overview.TotalPointsEarned = totalPointsEarned.Value;
            }

            decimal? totalPointsSpent = user.LoyaltyPointsSpent;
            if (totalPointsSpent.HasValue) {
                overview.TotalPointsSpent = totalPointsSpent.Value;
            }

            model.NewsAndCampaigns.AddRange(newsAndCampaigns);

            model.Overview = overview;


            return model;
        }
    }
}
