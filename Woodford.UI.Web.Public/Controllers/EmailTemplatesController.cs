using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Facades;
using Woodford.Core.Interfaces.Providers;
using Woodford.UI.Web.Public.ModelValidators;
using Woodford.UI.Web.Public.ViewModels;
namespace Woodford.UI.Web.Public.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class EmailTemplatesController : Controller
    {

        private IQueryProcessor _query;
        private ICommandBus _commandBus;

        public EmailTemplatesController(IQueryProcessor query, ICommandBus commandBus) {
            _query = query;
            _commandBus = commandBus;
        }
        
        public ActionResult ReservationReminder(int id)
        {
            INotify notify = MvcApplication.Container.GetInstance<INotify>();
            INotificationBuilder notificationBuilder = MvcApplication.Container.GetInstance<INotificationBuilder>();
            ReservationInvoiceNotificationModel emailModel = new ReservationInvoiceNotificationModel();
            emailModel = notificationBuilder.BuildReservationReminderModel(237);

            return View(emailModel);
        }

        public ActionResult ReservationQuote(int id) {
            INotify notify = MvcApplication.Container.GetInstance<INotify>();
            IReservationService reservationService = MvcApplication.Container.GetInstance<IReservationService>();
            ISettingService settings = MvcApplication.Container.GetInstance<ISettingService>();
            IInvoiceService invoiceService = MvcApplication.Container.GetInstance<IInvoiceService>();
            IUserService userService = MvcApplication.Container.GetInstance<IUserService>();
            IWeatherService weatherService = MvcApplication.Container.GetInstance<IWeatherService>();

            var reservation = reservationService.GetById(id);
            ReservationInvoiceNotificationModel emailModel = new ReservationInvoiceNotificationModel();
            emailModel.Reservation = reservation;
            emailModel.Invoice = invoiceService.GetByReservationId(reservation.Id);
     

            return View(emailModel);
        }

        public ActionResult ReservationInvoice(int id) {
            INotify notify = MvcApplication.Container.GetInstance<INotify>();
            INotificationBuilder notifyBuilder = MvcApplication.Container.GetInstance<INotificationBuilder>();
            var emailModel = notifyBuilder.BuildReservationInvoiceModel(id);


            return View(emailModel);
            
        }

        public ActionResult ReservationQuoteNew(int id) {
            INotify notify = MvcApplication.Container.GetInstance<INotify>();
            INotificationBuilder notifyBuilder = MvcApplication.Container.GetInstance<INotificationBuilder>();
            var emailModel = notifyBuilder.BuildReservationInvoiceModel(id);


            return View(emailModel);
        }

        public ActionResult AdvanceReport(int id, string email = null) {
            INotificationBuilder notifyBuilder = MvcApplication.Container.GetInstance<INotificationBuilder>();
            IUserService userService = MvcApplication.Container.GetInstance<IUserService>();
            INotify _notify = MvcApplication.Container.GetInstance<INotify>();
            var user = userService.GetById(id);
            var newsAndCampaigns = notifyBuilder.BuildNewsAndCampaignItems();

            AdvanceReportNotificationModel model = notifyBuilder.BuildLoyaltyReport(user, newsAndCampaigns);
            if (string.IsNullOrEmpty(email)) {

            } else {
                model.User.Email = email;
                _notify.SendNotifyLoyaltyReport(model, Setting.Public_Website_Domain);
            }
            
            

            return View(model);
        }

        
    }
}