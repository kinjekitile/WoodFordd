using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Providers;

namespace Woodford.Infrastructure.MailChimpBulkMailing
{
    public class MailChimpBulkMailingService : IBulkMailingService
    {
        private readonly ISettingService _settings;
        public MailChimpBulkMailingService(ISettingService settings) {
            _settings = settings;
        }

        public void Signup(string email) {
                        
            throw new NotImplementedException();
        }
    }
}
