using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woodford.Core.Interfaces.Providers;
using Woodford.Core.Interfaces;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Tests.Infrastructure {
    [TestClass]
    public class AuditTests : TestBase {
        [TestMethod]
        public void TestGetTypeMapp() {



            IAuditRepository importer = _container.GetInstance<IAuditRepository>();

            var result = importer.GetAuditEntityTypeKeyForModel(typeof(HerospaceItemModel).ToString());

            int x = 0;
            x++;


        }
    }
}
