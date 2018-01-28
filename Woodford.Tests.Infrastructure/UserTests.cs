using Microsoft.VisualStudio.TestTools.UnitTesting;
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

namespace Woodford.Tests.Infrastructure {
    [TestClass]
    public class UserTests : TestBase {
        [TestMethod]
        public void AddRole() {
            
            IUserService userService = _container.GetInstance<IUserService>();

            int userId = 15; //oshepherd
            int roleId = 5; //SEO
            string roleName = "SEO";

            List<UserRoles> roles = new List<UserRoles>();

            //userService.AddUserToRole(userId, roleId, roleName);
            //_command.Submit(new UserAssignRolesCommand {User = new UserModel { Email = "oshepherd@gmail.com" }, Roles = new List<UserRoles>().Add(UserRoles.SEO))


        }
    }
}
