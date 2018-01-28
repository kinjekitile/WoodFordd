using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.Interfaces.Providers;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.Controllers {
    [Authorize(Roles = "Administrator")]
    public class DataImportController : Controller {
        public DataImportController() {

        }
        // GET: DataImport
        public ActionResult Index() {
            return View();
        }

        public ActionResult BookingHistory() {
            return View();
        }

        [HttpPost]
        public ActionResult BookingHistory(FileUploadViewModel viewModel) {

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult AdvanceUsers() {
            FileUploadViewModel viewModel = new FileUploadViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AdvanceUsers(FileUploadViewModel viewModel) {


          

            
            
            IDataImportService importer = MvcApplication.Container.GetInstance<IDataImportService>();

            //string filename = "advancegreen.xlsx";

            //importer.ImportAdvanceUsers(filename);

            string filenamesilver = "advancesilver.xlsx";

            importer.ImportAdvanceUsers(filenamesilver);

            //string filenamegold = "advancegold.xlsx";

            //importer.ImportAdvanceUsers(filenamegold);

            return View(viewModel);
        }

    }
    
}