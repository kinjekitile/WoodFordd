using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Resources
{
    public class FilePath : IFilePath {
        public string GetEmailResourcesPath() {
            return ConfigurationSettings.AppSettings["EmailResourcePath"].ToString();
        }
    }
}
