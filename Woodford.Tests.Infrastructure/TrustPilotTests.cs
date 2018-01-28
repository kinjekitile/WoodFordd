using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces.Providers;

namespace Woodford.Tests.Infrastructure {

    [TestClass]
   public class TrustPilotTests : TestBase {


        [TestMethod]
        public void GetAuthToken() {

            IExternalReviewService _reviewService = _container.GetInstance<IExternalReviewService>();

            string authToken = _reviewService.GetAuthToken();
        }

        [TestMethod]
        public void TestReviewLinkRequest() {
            IExternalReviewService _reviewService = _container.GetInstance<IExternalReviewService>();
            ReviewLinkRequestModel request = new ReviewLinkRequestModel();
            request.Email = "oshepherd+tp1@gmail.com";
            request.Name = "Oli Shep TP1";
            request.ReservationId = 100;
            request.ReturnUrl = "https://woodford.co.za/abddef";


            var response = _reviewService.GetReviewLink(request);
        }

    }
}
