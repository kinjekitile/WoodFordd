using System;
using Quartz;
using Woodford.Core.Interfaces;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.Interfaces.Facades;
using Woodford.Core.Interfaces.Providers;

namespace Woodford.UI.Web.Public.TaskScheduler.Jobs {
    [DisallowConcurrentExecution]
    public class SendPostReservationThankYouJob : IJob {
        private IUserService _userService;
        private IReservationService _reservationService;
        private INotify _notify;
        private IInvoiceService _invoiceService;
        private ISettingService _settings;
        private INotificationBuilder _notificationBuilder;
        private IReviewRepository _reviewRepo;
        private IExternalReviewService _reviewService;

        public SendPostReservationThankYouJob(IUserService userService, IReservationService reservationService, INotify notify, IInvoiceService invoiceService, ISettingService settings, INotificationBuilder notificationBuilder, IReviewRepository reviewRepo, IExternalReviewService reviewService) {
            _userService = userService;
            _reservationService = reservationService;
            _notify = notify;
            _invoiceService = invoiceService;
            _settings = settings;
            _notificationBuilder = notificationBuilder;
            _reviewRepo = reviewRepo;
            _reviewService = reviewService;
        }
        public void Execute(IJobExecutionContext context) {

            int daysAfterDropOff = _settings.GetValue<int>(Setting.ReservationThanksDaysAfterDropoff);

            var reservations = _reservationService.Get(new ReservationFilterModel { DateFilterType = ReservationDateFilterTypes.DropOffDate, DateSearchStart = DateTime.Today.AddDays(-1 * daysAfterDropOff).Date, DateSearchEnd = DateTime.Today.AddDays(-1 * daysAfterDropOff).Date.AddDays(1), ReservationState = ReservationState.Completed, ThankYouSent = false, IsCancelled = false }, null).Items;

            string siteDomain = _settings.GetValue<string>(Setting.Public_Website_Domain);

            foreach (var item in reservations) {

                ReservationInvoiceNotificationModel model = _notificationBuilder.BuildReservationThankYouModel(item.Id);

                //NO LONGER CHECK FOR CUSTOMER PREVIOUS REVIEWS.
                //Check if customer email has been used for a trust pilot review, if not then create review record
                //bool customerReviewed = _reviewRepo.CustomerHasReviewed(item.Email);

                //if (!customerReviewed) {
                    //Create new review record
                    ReviewModel review = new ReviewModel();
                    review.ReservationId = item.Id;
                    review.Email = item.Email;
                    review = _reviewRepo.Create(review);


                    ReviewLinkRequestModel reviewLinkRequest = new ReviewLinkRequestModel();
                    reviewLinkRequest.Email = item.Email;
                    reviewLinkRequest.ReservationId = item.Id;
                    reviewLinkRequest.Name = item.FirstName + " " + item.LastName;
                    //review thank you page, with reservation id
                    reviewLinkRequest.ReturnUrl = siteDomain + "reviews/trustpilot?reservationId=" + item.Id;


                    //Call trustpilot api to get review link, pass in redirecturl that has reservation id appended
                    var customReviewResponse = _reviewService.GetReviewLink(reviewLinkRequest);

                    model.ShowReviewSection = true;
                    model.ReviewUrl = customReviewResponse.ReviewUrl;

                //}

                
                _notify.SendNotifyReservationThanks(model, Setting.Public_Website_Domain);
                _reservationService.SetThankYouSent(item.Id, thankYouSent: true);
                
            }
        }
    }
}