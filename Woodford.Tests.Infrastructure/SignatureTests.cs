﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Facades;
using Woodford.Core.Interfaces.Providers;
using Woodford.UI.Web.Admin.Code.Helpers;

namespace Woodford.Tests.Infrastructure {
    [TestClass]
    public class SignatureTests : TestBase {

        [TestMethod]
        public void SendSignatureZip() {

            EmailSignatureSendToUserCommand sendSignature = new EmailSignatureSendToUserCommand();
            sendSignature.SignatureId = 38;

            _command.Submit(sendSignature);


        }
    }
}


//EmailSignatureSendToUserCommand