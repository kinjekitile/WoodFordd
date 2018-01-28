
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.DomainServices;
using Woodford.Core.Interfaces;

namespace Woodford.UI.Web.Admin.Controllers {
    public class EmailSignatureController : Controller {

        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public EmailSignatureController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        // GET: EmailSignature
        public ActionResult Preview() {

            //string path = Server.MapPath("~/Content/images/email_signature/");

            //bool isJunior = true;

            //if (!isJunior) {
            //    path = Path.Combine(path, "background.jpg");
            //} else {
            //    path = Path.Combine(path, "background_junior.jpg");
            //}

            //EmailSignatureModel model = new EmailSignatureModel();
            //model.Name = "Oliver Shepherd";
            //model.Email = "reservations@woodford.co.za";
            //model.Title = "Operations";
            //model.CellContact = "+27 (0)83 123 4567";
            //model.FixedContact = "+27 (0)31 123 4567";

            //model.SeniorName = "Zufer Khan";

            //model.DirectorName = "Essa Suleman";
            //model.DirectorEmail = "essa@woodford.co.za";

            //byte[] sigData = GenerateEmailSignatureImage(model, path);


            //var cd = new System.Net.Mime.ContentDisposition {
            //    FileName = "signature_" + model.Name + ".jpg",
            //    Inline = false
            //};
            //Response.AppendHeader("Content-Disposition", cd.ToString());
            //return File(sigData, MimeTypes.Get("jpg"));

            return View();
        }


        public ActionResult Index(int p = 1) {

            EmailSignatureFilterModel filter = new EmailSignatureFilterModel();
            ListPaginationModel pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };

            ListOf<EmailSignatureModel> model = _query.Process(new EmailSignatureGetQuery { Filter = filter, Pagination = pagination });


            return View(model);
        }


        [HttpGet]
        public ActionResult Add() {
            EmailSignatureModel model = new EmailSignatureModel();
    
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(EmailSignatureModel model) {
         


            if (ModelState.IsValid) {
                _commandBus.Submit(new EmailSignatureAddCommand { Model = model });
                return RedirectToAction("Index");
            }

            return View(model);

            //string path = Server.MapPath("~/Content/images/email_signature/");

            //bool isJunior = true;

            //if (!isJunior) {
            //    path = Path.Combine(path, "background.jpg");
            //}
            //else {
            //    path = Path.Combine(path, "background_junior.jpg");
            //}

            //byte[] sigData = GenerateEmailSignatureImage(model, path);


            //var cd = new System.Net.Mime.ContentDisposition {
            //    FileName = "signature_" + model.Name + ".jpg",
            //    Inline = false
            //};
            //Response.AppendHeader("Content-Disposition", cd.ToString());
            //return File(sigData, MimeTypes.Get("jpg"));
        }


        [HttpGet]
        public ActionResult Edit(int id) {
            var model = _query.Process(new EmailSignatureGetByIdQuery { Id = id });
            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(EmailSignatureModel model) {


            if (ModelState.IsValid) {
                _commandBus.Submit(new EmailSignatureEditCommand { Model = model });
                return RedirectToAction("Index");
            }

            return View(model);
        }


        [HttpPost]
        public ActionResult SendSignature(int id) {

            EmailSignatureSendToUserCommand sendSignature = new EmailSignatureSendToUserCommand();
            sendSignature.SignatureId = id;

            _commandBus.Submit(sendSignature);

            EmailSignatureModel model = _query.Process(new EmailSignatureGetByIdQuery { Id = id });
            return View(model);
        }

        public byte[] GenerateEmailSignatureImage(EmailSignatureModel employeeDetails, string signatureTemplatePath) {
            Image img = Image.FromFile(signatureTemplatePath);

            //write voucher details

         
         
         

            SolidBrush brushBlack = new SolidBrush(Color.Black);
            SolidBrush brushWhite = new SolidBrush(Color.White);
            string hexGreen = "#19963c";
            SolidBrush brushGreen = new SolidBrush(ColorTranslator.FromHtml(hexGreen));

            Bitmap newImage = new Bitmap(img, img.Width, img.Height);
            Graphics graphic = System.Drawing.Graphics.FromImage(newImage);

            Font boldFont = new Font("Georgia", 13, FontStyle.Bold);


            int greetingLeft = 113;
            int greetingTop = 145;

            graphic.DrawString("Yours Sincerely", boldFont, brushBlack, new Point(greetingLeft, greetingTop));


            int nameLeft = 113;
            int nameTop = 165;

            graphic.DrawString(employeeDetails.Name, boldFont, brushBlack, new Point(nameLeft, nameTop));


            int roleTop = 185;
            int roleLeft = 113;

            graphic.DrawString(employeeDetails.Department, boldFont, brushGreen, new Point(roleLeft, roleTop));

            Font normalFont = new Font("Georgia", 13);

            int fixedTop = 225;
            int fixedLeft = 125;

            graphic.DrawString(employeeDetails.FixedContact, normalFont, brushBlack, new Point(fixedLeft, fixedTop));


            int cellTop = 255;
            int cellLeft = 125;

            graphic.DrawString(employeeDetails.CellContact, normalFont, brushBlack, new Point(cellLeft, cellTop));

            int emailTop = 285;
            int emailLeft = 125;

            graphic.DrawString(employeeDetails.Email, normalFont, brushBlack, new Point(emailLeft, emailTop));


            //Senior Details

            int seniorLeft = 1015;
            int seniorTop = 245;

            graphic.DrawString(employeeDetails.SeniorName, boldFont, brushBlack, new Point(seniorLeft, seniorTop));


            int seniorFixedTop = 265;
            int seniorFixedLeft = 1015;

            graphic.DrawString(employeeDetails.FixedContact, normalFont, brushBlack, new Point(seniorFixedLeft, seniorFixedTop));


            int seniorCellTop = 285;
            int seniorCellLeft = 1015;

            graphic.DrawString(employeeDetails.CellContact, normalFont, brushBlack, new Point(seniorCellLeft, seniorCellTop));

            int seniorEmailTop = 305;
            int seniorEmailLeft = 1015;

            graphic.DrawString(employeeDetails.Email, normalFont, brushBlack, new Point(seniorEmailLeft, seniorEmailTop));






            //Director Details

            int directorLeft = 1015;
            int directorTop = 400;

            graphic.DrawString(employeeDetails.DirectorName, boldFont, brushBlack, new Point(directorLeft, directorTop));




            int directorEmailTop = 420;
            int directorEmailLeft = 1015;

            graphic.DrawString(employeeDetails.DirectorEmail, normalFont, brushBlack, new Point(directorEmailLeft, directorEmailTop));


            MemoryStream ms = new MemoryStream();
            using (EncoderParameters encoderParameters = new EncoderParameters(1)) {
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Convert.ToInt64(95));
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                newImage.Save(ms, codecs.ToList().Single(x => x.MimeType == "image/jpeg"), encoderParameters);
            }
            graphic.Dispose();
            newImage.Dispose();
            img.Dispose();

            return ms.ToArray();
        }
    }
}



//int greetingLeft = 113;
//int greetingTop = 140;

//graphic.DrawString("Yours Sincerely", boldFont, brushBlack, new Point(greetingLeft, greetingTop));


//            int nameLeft = 113;
//int nameTop = 160;

//graphic.DrawString(employeeDetails.Name, boldFont, brushBlack, new Point(nameLeft, nameTop));


//            int roleTop = 180;
//int roleLeft = 113;

//graphic.DrawString(employeeDetails.Title, boldFont, brushGreen, new Point(roleLeft, roleTop));

//            Font normalFont = new Font("Georgia", 13);

//int fixedTop = 220;
//int fixedLeft = 125;

//graphic.DrawString(employeeDetails.FixedContact, normalFont, brushBlack, new Point(fixedLeft, fixedTop));


//            int cellTop = 250;
//int cellLeft = 125;

//graphic.DrawString(employeeDetails.CellContact, normalFont, brushBlack, new Point(cellLeft, cellTop));

//            int emailTop = 280;
//int emailLeft = 125;

//graphic.DrawString(employeeDetails.Email, normalFont, brushBlack, new Point(emailLeft, emailTop));