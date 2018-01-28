using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Commands.Vouchers;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.DomainServices;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.Code;
using Woodford.UI.Web.Admin.ModelValidators;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class VouchersController : Controller
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;
        private readonly ISettingService _settings;
        private readonly IVoucherService _voucherService;

        public VouchersController(ICommandBus commandBus, IQueryProcessor query, ISettingService settings, IVoucherService voucherService) {
            _commandBus = commandBus;
            _query = query;
            _settings = settings;
            _voucherService = voucherService;
        }
        
        public ActionResult Index(int p = 1)
        {
            var viewModel = new VoucherFilterAndResultsViewModel();
            viewModel.Vouchers = getVouchers(new VoucherFilterModel {  }, p);
            viewModel.IsFiltered = true;
            return View(viewModel);
        }
        
        private ListOf<VoucherModel> getVouchers(VoucherFilterModel filter, int p) {
            VouchersGetQuery query = new VouchersGetQuery { Filter = filter, Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 } };
            return _query.Process(query);
        }

        private VoucherModel getVoucher(int id) {
            VoucherGetByIdQuery query = new VoucherGetByIdQuery { Id = id };
            return _query.Process(query);
        }

        public ActionResult Filter(VoucherFilterAndResultsViewModel model, int? p) {
            p = (p ?? 1);
            if (ModelState.IsValid) {
                model.Vouchers = getVouchers(new VoucherFilterModel { IsExpired = model.IsExpired, IsMultiUse = model.IsMultiUse, IsRedeemed = model.IsRedeemed }, p.Value);
                model.IsFiltered = true;
            }
            return View("Index", model);
        }

        public ActionResult Add() {
            return View();
        }

        [HttpPost]
        public ActionResult Add([CustomizeValidator(RuleSet = VoucherValidationRuleSets.Default)] VoucherViewModel model) {
            if (ModelState.IsValid) {

               
                //cleanVoucherInput(model);
                model.Voucher.DateExpiry = DateTime.Now.AddMonths(6);
                model.Voucher.DateIssued = DateTime.Now;
                VoucherAddCommand command = new VoucherAddCommand { Model = model.Voucher };
                _commandBus.Submit(command);
               

                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Details(int id) {
            var voucher = _query.Process(new VoucherGetByIdQuery { Id = id });
            return View(voucher);
        }

        [HttpPost]
        public FileResult DownloadVoucher(int id) {
            VoucherModel v = getVoucher(id);           
            byte[] voucherContents = _voucherService.GeneratorVoucherImage(v, Server.MapPath("~/Content/images/voucher-template.jpg"));
            var cd = new System.Net.Mime.ContentDisposition {
                FileName = "voucher-" + v.VoucherNumber + ".jpg",
                Inline = false
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(voucherContents, MimeTypes.Get("jpg"));            
        }

        [HttpGet]
        public ActionResult EmailVoucher(int id) {
            VoucherModel v = getVoucher(id);

            VoucherViewModel viewModel = new VoucherViewModel();
            viewModel.Voucher = v;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EmailVoucher(VoucherViewModel viewModel) {
            VoucherModel v = getVoucher(viewModel.Voucher.Id);


            VoucherEmailToUserCommand sendVoucher = new VoucherEmailToUserCommand();
            sendVoucher.VoucherId = v.Id;
            sendVoucher.Email = viewModel.Voucher.ClientEmail;
            sendVoucher.CustomerName = viewModel.Voucher.ClientName;

            _commandBus.Submit(sendVoucher);

            if (sendVoucher.OUTRedeemed) {
                return RedirectToAction("VoucherRedeemed");
            }

            return RedirectToAction("VoucherSent");
        }

        public ActionResult VoucherSent() {
            return View();
        }

        public ActionResult VoucherRedeemed() {
            return View();
        }


    }
}