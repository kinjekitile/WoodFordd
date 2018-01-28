using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Routing;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.UI.Web.Public.Code.Filters {
    public class CheckLeadTimeFilter : System.Web.Mvc.ActionFilterAttribute {

        public override void OnActionExecuting(ActionExecutingContext filterContext) {

            ISettingService _settings = MvcApplication.Container.GetInstance<ISettingService>();
            IBranchRepository _branchRepo = MvcApplication.Container.GetInstance<IBranchRepository>();

            int leadTime = _settings.GetValue<int>(Setting.Booking_Lead_Time_Hours);

            var parameters = filterContext.ActionParameters;

            //Check if user booking from search results
            if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Search") {
                if (parameters["pickupDate"] != null) {

                    //Book from search results
                    DateTime pickupDate = Convert.ToDateTime(parameters["pickupDate"]);
                    int pickupTime = Convert.ToInt32(parameters["pickupTime"]);
                    string pickupTimeFUll = Convert.ToString(parameters["pickupTimeFull"]);
                    int pickupLocationId = Convert.ToInt32(parameters["pickupBranchId"]);

                    bool isAM = pickupTimeFUll.Contains("AM");

                    int hours = Convert.ToInt32(pickupTimeFUll.Split(new string[] { ":" }, StringSplitOptions.None)[0]);

                    if (!isAM) {
                        hours += 12;
                    }
                    pickupDate = pickupDate.AddHours(hours);

                    var b = _branchRepo.GetById(pickupLocationId);

                    if (DateTime.Now.Hour > 17 || DateTime.Now.Hour < 8) {
                        if (b.BookingLeadTimeNight.HasValue) {
                            leadTime = b.BookingLeadTimeNight.Value;
                        }
                    }
                    else {
                        if (b.BookingLeadTimeDay.HasValue) {
                            leadTime = b.BookingLeadTimeDay.Value;
                        }
                    }


                    if (pickupDate < DateTime.Now.AddHours(leadTime)) {
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                            { "controller", "Search" },
                            { "action", "SearchExpired" }
                            });

                    }
                }
            }

            if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Checkout") {
                if (parameters["id"] != null) {
                    int id = Convert.ToInt32(parameters["id"]);

                    IReservationService _reservationService = MvcApplication.Container.GetInstance<IReservationService>();



                    var reservation = _reservationService.GetById(id);

                    var b = _branchRepo.GetById(reservation.PickupBranchId);

                    if (DateTime.Now.Hour > 17 || DateTime.Now.Hour < 8) {
                        if (b.BookingLeadTimeNight.HasValue) {
                            leadTime = b.BookingLeadTimeNight.Value;
                        }
                    } else {
                        if (b.BookingLeadTimeDay.HasValue) {
                            leadTime = b.BookingLeadTimeDay.Value;
                        }
                    }

                    if (reservation.PickupDate < DateTime.Now.AddHours(leadTime)) {
                        //Reservation is more than one week old - expired
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary
                            {
                            { "controller", "Checkout" },
                            { "action", "QuoteExpiredNoLeadTime" }
                            });
                    }
                }
            }



        }

    }
}