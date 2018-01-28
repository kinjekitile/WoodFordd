using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Commands.Users;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Public.ModelValidators;
using Woodford.UI.Web.Public.ViewModels;

namespace Woodford.UI.Web.Public.Controllers {
    public class QuoteController : Controller {
        private IQueryProcessor _query;
        private ICommandBus _commandBus;
        private ISettingService _settings;
        private string[] mobileDevices = new string[] { "iphone", "ppc", "windows ce", "blackberry", "opera mini", "mobile", "palm", "portable", "opera mobi" };

        public QuoteController(IQueryProcessor query, ICommandBus commandBus, ISettingService settings) {
            _query = query;
            _commandBus = commandBus;
            _settings = settings;
        }
        // GET: Quote
        public ActionResult Index(string refCode) {
            ViewBag.QuoteExpired = false;
            CheckoutConfirmViewModel viewModel = new CheckoutConfirmViewModel();
            viewModel.IsMobile = false;

            if (mobileDevices.Any(x => Request.UserAgent.ToLower().Contains(x))) {
                viewModel.IsMobile = true;
            }
            var currentUser = _query.Process(new UserGetCurrentQuery());
            if (currentUser != null) {
                if (currentUser.CorporateId.HasValue) {
                    viewModel.IsCorporate = true;
                }
            }

            var reservation = _query.Process(new ReservationsGetQuery { Filter = new ReservationFilterModel { QuoteReference = refCode } }).Items.SingleOrDefault();

            if (reservation.QuoteSentDate < DateTime.Today.AddDays(-7)) {
                ViewBag.QuoteExpired = true;
            }
            
            viewModel.Reservation = _query.Process(new ReservationGetByIdQuery { Id = reservation.Id });

            viewModel.Vehicle = _query.Process(new VehicleGetByIdQuery { Id = viewModel.Reservation.VehicleId });
            if (viewModel.Reservation.VehicleUpgradeId.HasValue) {
                VehicleUpgradeModel u = _query.Process(new VehicleUpgradeGetByIdQuery { Id = viewModel.Reservation.VehicleUpgradeId.Value });
                viewModel.UpgradeToVehicle = _query.Process(new VehicleGetByIdQuery { Id = u.ToVehicle.Id });
            }

//            WaiversGetQuery waiversQuery = new WaiversGetQuery { Filter = new WaiverFilterModel { VehicleGroupId = reservation.Vehicle.VehicleGroupId } };

            viewModel.RentalDepositAmount = _settings.GetValue<decimal>(Setting.Rental_Deposit_Amount);

            return View(viewModel);
        }


    }
}