using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woodford.Core.Interfaces.Providers;
using Woodford.Core.Interfaces;

namespace Woodford.Tests.Infrastructure {
    [TestClass]
    public class DataImportTests : TestBase {

        [TestMethod]
        public void ImportUsers() {

            
            IDataImportService importer = _container.GetInstance<IDataImportService>();

            var result = importer.ImportAdvanceUsers("advancegreen.xlsx");
        }


        [TestMethod]
        public void TestImport() {

           

            IDataImportService importer = _container.GetInstance<IDataImportService>();

            var result = importer.ImportExternalBookings("test3.xlsx");

        }
    }
}
