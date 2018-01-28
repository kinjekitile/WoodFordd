using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.Controllers
{
    
    public class UserController : Controller
    {
        private ICommandBus _commandBus;
        private IQueryProcessor _query;

        public UserController(ICommandBus commandBus, IQueryProcessor query)
        {
            _commandBus = commandBus;
            _query = query;
        }

        //public ActionResult Index(int? p)
        //{
        //    if (!p.HasValue) {
        //        p = 1;
        //    }

        //    UserGetAllQuery query = new UserGetAllQuery { Pagination = new ListPaginationModel { CurrentPage = p.Value, ItemsPerPage = 20 } };
        //    ListOf<UserModel> users = _query.Process(query);

        //    return View(users);

        //}
        public ActionResult Login(string returnUrl) {
            if (Request.IsAuthenticated) {
                if (Roles.IsUserInRole("Branch")) {
                    return RedirectToAction("Index", "Users");
                }
                if (Roles.IsUserInRole("SEO")) {
                    return RedirectToAction("Index", "Herospace");
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                UserAuthenticateCommand command = new UserAuthenticateCommand { Username = model.Email, Password = model.Password };
                _commandBus.Submit(command);
                if (command.Success)
                {
                    if (Roles.IsUserInRole("Branch")) {
                        return RedirectToAction("Index", "Users");
                    }

                    if (Roles.IsUserInRole("SEO")) {
                        return RedirectToAction("Index", "Herospace");
                    }

                    return RedirectToLocal(returnUrl);
                }
            }
            return View(model);
        }
        
        [HttpPost]
        public ActionResult LogOff()
        {
            UserLogOffCommand command = new UserLogOffCommand();
            _commandBus.Submit(command);
            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}