using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.UI.Web.Admin.Code {
    public static class VoucherImageGenerator {

        public static byte[] GeneratorVoucherImage(VoucherModel v, string voucherTemplatePath) {
            Image img = Image.FromFile(voucherTemplatePath);

            //write voucher details
            Font fName = new Font("Georgia", 16);
            Font fVoucherNumber = new Font("Georgia", 14);
            Font fValid = new Font("Arial", 7);

            SolidBrush brushBlack = new SolidBrush(Color.Black);            
            Bitmap newImage = new Bitmap(img, img.Width, img.Height);
            Graphics graphic = System.Drawing.Graphics.FromImage(newImage);

            DateTime dateIssued = v.DateIssued;
            DateTime validUntil = v.DateExpiry;

            if (!string.IsNullOrEmpty(v.ClientName)) { 
                graphic.DrawString(v.ClientName, fName, brushBlack, new Point(200, 114));
            }
            graphic.DrawString(v.VoucherNumber, fVoucherNumber, brushBlack, new Point(275, 175));
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
        private static ImageCodecInfo getEncoderInfo(string mimeType) {

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
