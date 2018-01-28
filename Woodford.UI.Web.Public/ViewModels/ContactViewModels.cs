using FluentValidation.Attributes;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Public.ModelValidators;

namespace Woodford.UI.Web.Public.ViewModels {
    [Validator(typeof(ContactValidator))]
    public class ContactViewModel {
        public ContactUsNotificationModel ContactUs { get; set; }
    }

    [Validator(typeof(BranchContactValidator))]
    public class BranchContactViewModel {
        public ContactUsNotificationModel ContactUs { get; set; }
        public BranchModel Branch { get; set; }
    }


    [Validator(typeof(RequestCallbackValidator))]
    public class RequestCallbackViewModel {
        public RequestCallbackNotificationModel RequestCallback { get; set; }        
    }

    [Validator(typeof(BranchRequestCallbackValidator))]
    public class BranchRequestCallbackViewModel {
        public RequestCallbackNotificationModel RequestCallback { get; set; }
        public BranchModel Branch { get; set; }
    }
}

