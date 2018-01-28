using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Repositories;
using Woodford.Core.Interfaces.Services;

namespace Woodford.Core.DomainServices {
    public class EmailSignatureService : IEmailSignatureService {
        private readonly IEmailSignatureRepository _repo;
        private readonly IFileUploadService _fileUploadService;
        private readonly ISettingService _settings;
        public EmailSignatureService(IEmailSignatureRepository repo, IFileUploadService fileUploadService, ISettingService settings) {
            _repo = repo;
            _fileUploadService = fileUploadService;
            _settings = settings;
        }

        public EmailSignatureModel Create(EmailSignatureModel model) {
            return _repo.Create(model);
        }

        public ListOf<EmailSignatureModel> Get(EmailSignatureFilterModel filter, ListPaginationModel pagination) {

            ListOf<EmailSignatureModel> res = new ListOf<EmailSignatureModel>();

            res.Pagination = pagination;
            res.Items = _repo.Get(filter, res.Pagination);
            if (pagination != null) {
                res.Pagination.TotalItems = _repo.GetCount(filter);
            }

            return res;
        }

        public EmailSignatureModel GetById(int id) {
            return _repo.GetById(id);
        }

        public byte[] GetEmailSignatureImageData(EmailSignatureModel signature, EmailSignatureCampaignModel campaign) {

            var backgroundFile = signature.ShowSidePanel ? GetFile(campaign.MainContentNarrowFileUploadId) : GetFile(campaign.MainContentFileUploadId);

            var footerFile = signature.ShowSidePanel ? GetFile(campaign.FooterContentNarrowFileUploadId) : GetFile(campaign.FooterContentFileUploadId);

            var sidePanel = GetFile(campaign.SidePanelContentFileUploadId);
            var underline = GetFile(campaign.UnderlineContentFileUploadId);


            Bitmap baseImage = new Bitmap(SignatureLayout.BaseImageWidth, SignatureLayout.BaseImageHeight, PixelFormat.Format32bppPArgb);
            //Bitmap baseImage = new Bitmap(SignatureLayout.BaseImageWidth, SignatureLayout.BaseImageHeight, PixelFormat.Format64bppPArgb);

            Graphics graphic = System.Drawing.Graphics.FromImage(baseImage);

            graphic.Clear(Color.Transparent);

            //Underline
            graphic.DrawImage(GetImageFromArray(underline.FileContents), SignatureLayout.UnderlineStartX, SignatureLayout.UnderlineStartY, SignatureLayout.UnderlineWidth, SignatureLayout.UnderlineHeight);

            if (signature.ShowSidePanel) {
                //Footer Narrow
                graphic.DrawImage(GetImageFromArray(footerFile.FileContents), SignatureLayout.FooterNarrowStartX, SignatureLayout.FooterNarrowStartY, SignatureLayout.FooterNarrowWidth, SignatureLayout.FooterNarrowHeight);

                //Background Narrow
                graphic.DrawImage(GetImageFromArray(backgroundFile.FileContents), SignatureLayout.MainNarrowStartX, SignatureLayout.MainNarrowStartY, SignatureLayout.MainNarrowWidth, SignatureLayout.MainNarrowHeight);


                //Side Panel
                graphic.DrawImage(GetImageFromArray(sidePanel.FileContents), SignatureLayout.SideStartX, SignatureLayout.SideStartY, SignatureLayout.SideWidth, SignatureLayout.SideHeight);

            }
            else {
                //Footer
                graphic.DrawImage(GetImageFromArray(footerFile.FileContents), SignatureLayout.FooterStartX, SignatureLayout.FooterStartY, SignatureLayout.FooterWidth, SignatureLayout.FooterHeight);

                //Background
                graphic.DrawImage(GetImageFromArray(backgroundFile.FileContents), SignatureLayout.MainStartX, SignatureLayout.MainStartY, SignatureLayout.MainWidth, SignatureLayout.MainHeight);

            }

            //Gray semi transparent square for signature details
            using (SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(128, 255, 255, 255))) {
                graphic.FillRectangle(semiTransBrush, 100, 100, 320, 200);
            }

            string assetPath = _settings.GetValue<string>(Setting.EmailSignatureAssetsLocalPath);

            string telIconPath = Path.Combine(assetPath, "phone_icon.png");

            Image imgTelIcon = Image.FromFile(telIconPath);
            Bitmap bmpTelIcon = new Bitmap(telIconPath);
            bmpTelIcon.MakeTransparent();
            graphic.DrawImage(bmpTelIcon, 115, 198);




            SolidBrush brushBlack = new SolidBrush(Color.Black);
            SolidBrush brushWhite = new SolidBrush(Color.White);
            string hexGreen = "#19963c";
            SolidBrush brushGreen = new SolidBrush(ColorTranslator.FromHtml(hexGreen));
            Font boldFont = new Font("Georgia", 13, FontStyle.Bold);


            //Signature Text
            int greetingLeft = 113;
            int greetingTop = 110;

            graphic.DrawString("Yours Sincerely", boldFont, brushBlack, new Point(greetingLeft, greetingTop));

            int nameLeft = 113;
            int nameTop = 135;

            graphic.DrawString(signature.Name, boldFont, brushBlack, new Point(nameLeft, nameTop));

            int roleTop = 160;
            int roleLeft = 113;

            graphic.DrawString(signature.Department, boldFont, brushGreen, new Point(roleLeft, roleTop));

            Font normalFont = new Font("Georgia", 13);

            int fixedTop = 195;
            int fixedLeft = 140;

            graphic.DrawString(signature.FixedContact, normalFont, brushBlack, new Point(fixedLeft, fixedTop));

            if (!string.IsNullOrEmpty(signature.CellContact)) {
                string cellIconPath = Path.Combine(assetPath, "cell_icon.png");

                Image imgCellIcon = Image.FromFile(cellIconPath);
                Bitmap bmpCellIcon = new Bitmap(cellIconPath);
                graphic.DrawImage(bmpCellIcon, 120, 228);

                string emailIconPath = Path.Combine(assetPath, "email_icon.png");

                Image imgEmailIcon = Image.FromFile(emailIconPath);

                graphic.DrawImage(imgEmailIcon, 115, 253);

                int cellTop = 225;
                int cellLeft = 140;

                graphic.DrawString(signature.CellContact, normalFont, brushBlack, new Point(cellLeft, cellTop));

                int emailTop = 250;
                int emailLeft = 140;

                graphic.DrawString(signature.Email, normalFont, brushBlack, new Point(emailLeft, emailTop));
            }
            else {
                string emailIconPath = Path.Combine(assetPath, "email_icon.png");

                Image imgEmailIcon = Image.FromFile(emailIconPath);

                graphic.DrawImage(imgEmailIcon, 115, 228);

                int emailTop = 225;
                int emailLeft = 140;

                graphic.DrawString(signature.Email, normalFont, brushBlack, new Point(emailLeft, emailTop));
            }



            //Senior Text
            string seniorTextLine1 = "Should you be unhappy with my";
            string seniorTextLine2 = "performance, please contact my";
            string seniorTextLine3 = "superior.";

            int seniorTextLeft = 1010;
            int seniorTextTop = 165;

            //Side panel text
            if (signature.ShowSidePanel) {

                //Senior Text
                graphic.DrawString(seniorTextLine1, normalFont, brushBlack, new Point(seniorTextLeft, seniorTextTop));
                graphic.DrawString(seniorTextLine2, normalFont, brushBlack, new Point(seniorTextLeft, seniorTextTop + 20));
                graphic.DrawString(seniorTextLine3, normalFont, brushBlack, new Point(seniorTextLeft, seniorTextTop + 40));

                if (signature.ShowDirectorDetails && !signature.ShowSeniorDetails) {
                    //Manager signature only has Director, must take the place of nornal manager details
                    //Senior Details
                    int managerDirectorLeft = 1010;
                    int managerDirectorTop = 235;

                    graphic.DrawString(signature.DirectorName, boldFont, brushBlack, new Point(managerDirectorLeft, managerDirectorTop));


                    int managerDirectorEmailTop = 255;
                    int managerDirectorEmailLeft = 1010;

                    graphic.DrawString(signature.DirectorEmail, normalFont, brushBlack, new Point(managerDirectorEmailLeft, managerDirectorEmailTop));


                }
                else {
                    if (signature.ShowSeniorDetails && signature.ShowDirectorDetails) {


                        //Normal employee with both Senior and Director
                        //Senior Details
                        int seniorLeft = 1010;
                        int seniorTop = 235;

                        graphic.DrawString(signature.SeniorName, boldFont, brushBlack, new Point(seniorLeft, seniorTop));

                        int seniorFixedTop = 255;
                        int seniorFixedLeft = 1010;

                        graphic.DrawString(signature.SeniorFixedContact, normalFont, brushBlack, new Point(seniorFixedLeft, seniorFixedTop));

                        int seniorCellTop = 275;
                        int seniorCellLeft = 1010;

                        graphic.DrawString(signature.SeniorCellContact, normalFont, brushBlack, new Point(seniorCellLeft, seniorCellTop));

                        int seniorEmailTop = 295;
                        int seniorEmailLeft = 1010;

                        graphic.DrawString(signature.SeniorEmail, normalFont, brushBlack, new Point(seniorEmailLeft, seniorEmailTop));

                        //Director Text
                        string directorTextLine1 = "Should you require to escalate even";
                        string directorTextLine2 = "further, please contact my Director.";

                        int directorTextLeft = 1010;
                        int directorTextTop = 330;

                        graphic.DrawString(directorTextLine1, normalFont, brushBlack, new Point(directorTextLeft, directorTextTop));
                        graphic.DrawString(directorTextLine2, normalFont, brushBlack, new Point(directorTextLeft, directorTextTop + 20));


                        //Director Details
                        int directorLeft = 1010;
                        int directorTop = 390;

                        graphic.DrawString(signature.DirectorName, boldFont, brushBlack, new Point(directorLeft, directorTop));

                        int directorEmailTop = 410;
                        int directorEmailLeft = 1010;

                        graphic.DrawString(signature.DirectorEmail, normalFont, brushBlack, new Point(directorEmailLeft, directorEmailTop));


                    }
                    else {
                        //Director employee - code should not be hit has director has no side panel
                    }
                }

            }

            //if (signature.Id == 8) {
            //    //Resize task on hold for now.
            //    //75% of original
            Bitmap resized = new Bitmap(baseImage, new Size(1031, 357));

            //    //80% 1100 x 400
            //Bitmap resized = new Bitmap(baseImage, new Size(1100, 400));

            //    //90% 1100 x 400
            //    //Bitmap resized = new Bitmap(baseImage, new Size(1238, 450));

            using (var ms = new MemoryStream()) {
                resized.Save(ms, ImageFormat.Png);
                //baseImage.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
            //}
            //else {
            //using (var ms = new MemoryStream()) {
            //    baseImage.Save(ms, ImageFormat.Png);
            //    //baseImage.Save(ms, ImageFormat.Jpeg);
            //    return ms.ToArray();
            //}
            //}




        }

        private static ImageCodecInfo getImageCodec(string extension) {
            extension = extension.ToUpperInvariant();
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs) {
                if (codec.FilenameExtension.Contains(extension)) {
                    return codec;
                }
            }
            return codecs[1];
        }

        private FileUploadModel GetFile(int id) {
            return _fileUploadService.GetById(id, null, null, null, includeFileContents: true);
        }

        private Image GetImageFromArray(byte[] bytes) {
            using (var ms = new MemoryStream(bytes)) {
                return Image.FromStream(ms);
            }
        }

        public EmailSignatureModel Update(EmailSignatureModel model) {
            return _repo.Update(model);
        }
    }
}
