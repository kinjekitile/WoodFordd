using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Routing;
using Woodford.Core.Interfaces;

namespace Woodford.UI.Web.Public.Code.Filters {
    public class CheckQuoteExpiredFilter : System.Web.Mvc.ActionFilterAttribute {

        public override void OnActionExecuting(ActionExecutingContext filterContext) {

            var parameters = filterContext.ActionParameters;



            if (parameters["id"] != null) {
                int id = Convert.ToInt32(parameters["id"]);

                IReservationService _reservationService = MvcApplication.Container.GetInstance<IReservationService>();

                var reservation = _reservationService.GetById(id);

                if (reservation.DateCreated < DateTime.Today.AddDays(-7)) {
                    //Reservation is more than one week old - expired
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            { "controller", "Checkout" },
                            { "action", "QuoteExpired" }
                        });
                    }
            }


        }

    }
}