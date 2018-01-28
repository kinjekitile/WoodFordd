using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Woodford.Core.DomainServices {
    public class VoucherService : IVoucherService {

        private IVoucherRepository _repo;

        public VoucherService(IVoucherRepository repo) {
            _repo = repo;
        }

        private void cleanVoucherInput(VoucherModel model) {            
            if (model.IsMultiUse) {
                model.RequireClientValidation = false;
                model.ClientName = "";
                model.ClientEmail = "";
            } else {                
                model.VoucherNumber = generateNewVoucherNumber();
                //if (model.RequireClientValidation == false) {
                //    model.ClientName = "";
                //    model.ClientEmail = "";
                //}
            }
            switch (model.VoucherRewardType) {
                case VoucherRewardType.DiscountPercentage:
                    model.VoucherReward = "";
                    model.VoucherDiscount = null;
                    break;
                case VoucherRewardType.DiscountValue:
                    model.VoucherReward = "";
                    model.VoucherDiscountPercentage = null;
                    break;
                case VoucherRewardType.TextReward:
                    model.VoucherDiscount = null;
                    model.VoucherDiscountPercentage = null;
                    break;
            }

        }

        private string generateNewVoucherNumber() {
            String s = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random r = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 10; i++) {
                int idx = r.Next(0, 35);
                sb.Append(s.Substring(idx, 1));
            }
            return sb.ToString();
        }

        public VoucherModel Create(VoucherModel model) {

            cleanVoucherInput(model);

            return _repo.Create(model);
        }

        public ListOf<VoucherModel> Get(VoucherFilterModel filter, ListPaginationModel pagination) {
            ListOf<VoucherModel> res = new ListOf<VoucherModel>();
            res.Pagination = pagination;
            res.Items = _repo.Get(filter, pagination);

            if (pagination != null) {
                res.Pagination.TotalItems = _repo.GetCount(filter);
            }

            return res;
        }

        public VoucherModel GetById(int id) {
            return _repo.GetById(id);
        }

        public VoucherModel GetByVoucherNumber(string voucherNumber) {
            return _repo.GetByVoucherNumber(voucherNumber);
        }

        public VoucherRedeemModel RedeemVoucher(string voucherNumber, string clientEmail, int reservationId) {

            VoucherRedeemModel res = new VoucherRedeemModel();
            res.Voucher = _repo.GetByVoucherNumber(voucherNumber);

            if (res.Voucher.DateExpiry > DateTime.Now) {
                if (res.Voucher.IsMultiUse) {
                    res.Response.Success = true;
                    res.Response.ErrorMessage = "";
                } else {
                    if (res.Voucher.ClientEmail == clientEmail) {
                        res.Response.Success = true;
                        res.Response.ErrorMessage = "";
                    } else {
                        res.Response.Success = false;
                        res.Response.ErrorMessage = "Client Email does not match";
                    }
                }
            } else {
                res.Response.Success = false;
                res.Response.ErrorMessage = "Voucher Expired";
            }

            if (res.Response.Success) {
                if (!res.Voucher.IsMultiUse) {
                    res.Voucher.ReservationId = reservationId;
                    res.Voucher.DateRedeemed = DateTime.Now;
                    res.Voucher = _repo.Update(res.Voucher);
                }
            }

            return res;

        }

        public VoucherModel Update(VoucherModel model) {
            return _repo.Update(model);
        }

        public byte[] GeneratorVoucherImage(VoucherModel v, string voucherTemplatePath) {
            Image img = Image.FromFile(voucherTemplatePath);

            //write voucher details
            Font fName = new Font("Georgia", 26);
            Font fVoucherReward = new Font("Georgia", 18);
            Font fVoucherNumber = new Font("Georgia", 14);
            Font fValid = new Font("Arial", 7);

            SolidBrush brushBlack = new SolidBrush(Color.Black);
            SolidBrush brushWhite = new SolidBrush(Color.White);
            string hexGreen = "#19963c";
            SolidBrush brushGreen = new SolidBrush(ColorTranslator.FromHtml(hexGreen));

            Bitmap newImage = new Bitmap(img, img.Width, img.Height);
            Graphics graphic = System.Drawing.Graphics.FromImage(newImage);

            DateTime dateIssued = v.DateIssued;
            DateTime validUntil = v.DateExpiry;

            if (!string.IsNullOrEmpty(v.ClientName)) {
                graphic.DrawString(v.ClientName, fName, brushWhite, new Point(20, 20));
            }
            graphic.DrawString(v.VoucherNumber, fVoucherNumber, brushBlack, new Point(425, 250));

            string rewardText = "";

            switch (v.VoucherRewardType) {
                case VoucherRewardType.DiscountPercentage:

                    rewardText = decimal.Round(v.VoucherDiscountPercentage.Value, 0).ToString() + "% off your next booking";
                    break;
                case VoucherRewardType.DiscountValue:
                    rewardText = string.Format("{0:c}", v.VoucherDiscount) + " off your next booking";
                    break;
                case VoucherRewardType.TextReward:
                    rewardText = v.VoucherReward;
                    break;
            }
            graphic.DrawString(rewardText, fVoucherReward, brushGreen, new Point(250, 90));

            graphic.DrawString("Issued on: " + dateIssued.ToString("dd/MM/yyyy"), fValid, brushBlack, new Point(20, 245));
            graphic.DrawString("Valid until: " + validUntil.ToString("dd/MM/yyyy"), fValid, brushBlack, new Point(20, 255));

            MemoryStream ms = new MemoryStream();
            using (EncoderParameters encoderParameters = new EncoderParameters(1)) {
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Convert.ToInt64(95));
                newImage.Save(ms, getEncoderInfo("image/jpeg"), encoderParameters);
            }
            graphic.Dispose();
            newImage.Dispose();
            img.Dispose();

            return ms.ToArray();
        }

        //Returns the image codec with the given mime type
        private ImageCodecInfo getEncoderInfo(string mimeType) {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            for (int i = 0; i < codecs.Length; i++) {
                if (codecs[i].MimeType == mimeType) {
                    return codecs[i];
                }
            }

            return null;
        }
    }
}
