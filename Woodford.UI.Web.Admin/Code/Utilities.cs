using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Woodford.Core.Interfaces;

namespace Woodford.UI.Web.Admin.Code {
    public static class Utilities {

        public static string GeneratePassword(int length) {

            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--) {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }


        public static string GetAuditEntityNameForModel(string objType) {
            IAuditService _auditService = MvcApplication.Container.GetInstance<IAuditService>();



            string result = _auditService.GetAuditEntityTypeKeyForModel(objType);

            return result;
        }

        public static string GenerateSlug(this string phrase, int maxLength = 50) {
            string str = phrase.ToLower();
            // invalid chars, make into spaces
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces/hyphens into one space       
            str = Regex.Replace(str, @"[\s-]+", " ").Trim();
            // cut and trim it
            str = str.Substring(0, str.Length <= maxLength ? str.Length : maxLength).Trim();
            // hyphens
            str = Regex.Replace(str, @"\s", "-");

            return str;
        }

        public static byte[] ConvertPostedFileContentsToByteArray(HttpPostedFileBase uploadedFile) {
            if (uploadedFile == null) {
                return null;
            }
            else
                using (MemoryStream ms = new MemoryStream()) {
                    uploadedFile.InputStream.CopyTo(ms);
                    return ms.ToArray();
                }
            //MemoryStream target = new MemoryStream();
            //uploadedFile.InputStream.CopyTo(target);
            //byte[] fileContent = target.ToArray();
            //return fileContent;
        }
    }

}

