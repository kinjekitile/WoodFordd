using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.ApplicationServices.Queries.Reviews;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.UI.Web.Public.Controllers
{
    public class ReviewsController : Controller
    {

        private ICommandBus _commandBus;
        private IQueryProcessor _query;

        public ReviewsController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        // GET: Reviews
        public ActionResult TrustPilot(int reservationId)
        {



            //Check if code exists
            //If exists, check that it has not been used
            //If not used, then create new voucher
            //Display voucher on page
            //Send voucher via email to user

            var reservation = _query.Process(new ReservationGetByIdQuery { Id = reservationId });

            var review = _query.Process(new ReviewGetByReservationIdQuery { ReservationId = reservation.Id });

            if (review.VoucherSent) {
                return RedirectToAction("VoucherAlreadyClaimed");
            }

            VoucherModel voucher = new VoucherModel();
            voucher.IsMultiUse = false;
            voucher.ClientEmail = reservation.Email;
            voucher.ClientName = reservation.FirstName + " " + reservation.LastName;
            voucher.VoucherRewardType = Core.DomainModel.Enums.VoucherRewardType.DiscountPercentage;
            voucher.VoucherDiscountPercentage = 10m;
            voucher.DateIssued = DateTime.Now;
            voucher.DateExpiry = voucher.DateIssued.AddMonths(6);

            VoucherAddCommand addVoucher = new VoucherAddCommand();
            addVoucher.Model = voucher;
            _commandBus.Submit(addVoucher);

            review.VoucherId = addVoucher.Model.Id;
            review.VoucherSent = true;
            review.VoucherSentDate = DateTime.Now;

            ReviewEditCommand editReview = new ReviewEditCommand();
            editReview.Review = review;

            _commandBus.Submit(editReview);



            return View(addVoucher.Model);
        }
    }
}