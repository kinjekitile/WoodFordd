using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Admin.ModelValidators;

namespace Woodford.UI.Web.Admin.ViewModels {
    [Validator(typeof(FileUploadValidator))]
    public class FileUploadViewModel {

        public FileUploadModel File { get; set; }
        public HttpPostedFileBase Upload { get; set; }

    }
}
