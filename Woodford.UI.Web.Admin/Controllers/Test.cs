using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Facades;
using Woodford.Core.Interfaces.Providers;

namespace Woodford.UI.Web.Admin.Controllers {
    [Authorize(Roles = "Administrator")]
    public class TestController : Controller {
        private readonly ISettingService _settings;
        private readonly INotificationBuilder _notifyBuilder;
        private readonly INotify _notify;
        private readonly IReservationService _reservationService;

        private readonly IReviewRepository _reviewRepo;
        private readonly IExternalReviewService _externalReviewService;


        public TestController(ISettingService settings, INotificationBuilder notifyBuilder, INotify notify, IReservationService reservationService, IReviewRepository reviewRepo, IExternalReviewService externalReviewService) {
            _settings = settings;
            _notifyBuilder = notifyBuilder;
            _notify = notify;
            _reservationService = reservationService;
            _reviewRepo = reviewRepo;
            _externalReviewService = externalReviewService;
        }

        public ActionResult SendReviewInvites() {
            ReservationFilterModel filter = new ReservationFilterModel();
            filter.DateFilterType = ReservationDateFilterTypes.BookingDate;
            filter.DateSearchStart = new DateTime(2016, 07, 01);
            filter.DateSearchEnd = filter.DateSearchStart.Value.AddMonths(3);
            filter.IsCompletedInvoice = true;
            filter.IsCancelled = false;
            filter.IsQuote = false;


            string siteDomain = _settings.GetValue<string>(Setting.Public_Website_Domain);

            var items = _reservationService.Get(filter, null);

            foreach (var item in items.Items.OrderByDescending(x => x.Id)) {
                //Check that customer has not already been sent an invite, use email address to determine
                bool customerHasReviewed = _reviewRepo.CustomerHasReviewed(item.Email);

                if (!customerHasReviewed) {
                    //We can invite the customer

                    ReservationInvoiceNotificationModel model = _notifyBuilder.BuildReservationThankYouModel(item.Id);

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
                    //Set in getreviewlink method
                    //reviewLinkRequest.ReturnUrl = siteDomain + "reviews/trustpilot?reservationId=" + item.Id;


                    //Call trustpilot api to get review link, pass in redirecturl that has reservation id appended
                    var customReviewResponse = _externalReviewService.GetReviewLink(reviewLinkRequest);

                    model.ShowReviewSection = true;
                    model.ReviewUrl = customReviewResponse.ReviewUrl;

                    //}


                    _notify.SendNotifyReservationThanks(model, Setting.Public_Website_Domain);

                }
            }

            return View();
        }

    }



    public class Foo {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DateBooked { get; set; }
    }
}