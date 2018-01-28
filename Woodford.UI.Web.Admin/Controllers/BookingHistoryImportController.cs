using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.Code;
using Woodford.UI.Web.Admin.ModelValidators;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class BookingHistoryImportController : Controller
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public BookingHistoryImportController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }
        // GET: BookingHistoryImport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add() {
            return View(new FileUploadViewModel());
        }

        [HttpPost]
        public ActionResult Add(FileUploadViewModel model) {
            if (ModelState.IsValid) {
                model.File = new FileUploadModel();
                model.File.Title = model.Upload.FileName;
                FileUploadModel f = FileUploadModelFactory.CreateInstance(model);
       
                ImportBookingHistoryCommand command = new ImportBookingHistoryCommand { Filename = model.Upload.FileName, FileContents = f.FileContents };
                _commandBus.Submit(command);
                return RedirectToAction("Success");

            }
            return View(model);
        }

        public ActionResult Success() {
            return View();
        }
    }
}