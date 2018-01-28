using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.UI.Web.Public.ViewModels;

namespace Woodford.UI.Web.Public.ModelValidators {
    public static class ContactValidationRuleSets {
        public const string Default = "default";
    }
    public class ContactValidator : AbstractValidator<ContactViewModel> {
        public ContactValidator() {
            RuleSet(ContactValidationRuleSets.Default, () => {
                RuleFor(x => x.ContactUs.Email)
                    .NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Please enter a valid email address");
                RuleFor(x => x.ContactUs.Message)
                    .NotEmpty().WithMessage("Message is required");
                RuleFor(x => x.ContactUs.FullName)
                    .NotEmpty().WithMessage("Full Name is required");
                RuleFor(x => x.ContactUs.Country)
                    .NotEmpty().WithMessage("Country is required");

            });
        }
    }

    public static class BranchContactValidationRuleSets {
        public const string Default = "default";
    }
    public class BranchContactValidator : AbstractValidator<BranchContactViewModel> {
        public BranchContactValidator() {
            RuleSet(ContactValidationRuleSets.Default, () => {
                RuleFor(x => x.ContactUs.Email)
                    .NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Please enter a valid email address");
                RuleFor(x => x.ContactUs.Message)
                    .NotEmpty().WithMessage("Message is required");
                RuleFor(x => x.ContactUs.FullName)
                    .NotEmpty().WithMessage("First Name is required");                
            });
        }
    }

    public static class RequestCallbackValidationRuleSets {
        public const string Default = "default";
    }
    public class RequestCallbackValidator : AbstractValidator<RequestCallbackViewModel> {
        public RequestCallbackValidator() {
            RuleSet(ContactValidationRuleSets.Default, () => {
                RuleFor(x => x.RequestCallback.FullName)
                    .NotEmpty().WithMessage("Full Name is required");
                RuleFor(x => x.RequestCallback.PhoneNumber)
                    .NotEmpty().WithMessage("Phone number is required");                
            });
        }
    }

    public static class BranchRequestCallbackValidationRuleSets {
        public const string Default = "default";
    }
    public class BranchRequestCallbackValidator : AbstractValidator<BranchRequestCallbackViewModel> {
        public BranchRequestCallbackValidator() {
            RuleSet(ContactValidationRuleSets.Default, () => {
                RuleFor(x => x.RequestCallback.FullName)
                    .NotEmpty().WithMessage("Full Name is required");
                RuleFor(x => x.RequestCallback.PhoneNumber)
                    .NotEmpty().WithMessage("Phone number is required");
            });
        }
    }
}
