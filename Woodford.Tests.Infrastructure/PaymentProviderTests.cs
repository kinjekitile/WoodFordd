using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Facades;
using Woodford.Core.Interfaces.Providers;
using Woodford.UI.Web.Admin.Code.Helpers;


namespace Woodford.Tests.Infrastructure {
    [TestClass]
    public class PaymentProviderTests : TestBase {

        [TestMethod]
        public void TestRefund() {
            IPaymentProcessor _payment = _container.GetInstance<IPaymentProcessor>();

            //995FA6DF-0A5B-4CEA-9944-09574A9388AA
            RefundRequestModel request = new RefundRequestModel();
            request.TransactionId = "6CB16F95-FA1C-45EE-AD48-941357C82E30";
            request.ReservationId = 100;
            request.InvoiceId = 1;
            request.Amount = 2.00m;

            var respone = _payment.ProcessRefund(request);

            int x = 3;
            x++;


    //            [0]: "Result||1"
    //[1]: "TransactionIndex||a099a1da-bef7-401b-bc16-84d133c68a63"
    //[2]: "Warning||8002||Service.Validate||Amount was ignored||Transaction Amount was Ignored. Refer to Data Element 9"
    //[3]: "AcquirerDateTime||2016/11/18 09:47:15 AM"
    //[4]: "AuthorisationID||031397"
        }
    }
}
