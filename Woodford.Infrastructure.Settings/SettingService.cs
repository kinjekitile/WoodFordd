using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Settings {
	public class SettingService : ISettingService {

		private ISettingsRepository _repo;
		public SettingService(ISettingsRepository repo) {
			_repo = repo;
		}

		public void Initialize() {
			foreach (Setting s in Enum.GetValues(typeof(Setting))) {
				_repo.Get(s);
			}
		}

		public T GetValue<T>(Setting setting) {
			string value = GetValue(setting);
			return (T)Convert.ChangeType(value, typeof(T));
		}

		public string GetValue(Setting setting) {
            if (setting == Setting.Payment_Gateway_Password) {
                return ConfigurationManager.AppSettings["Payment_Gateway_Password"];
            }
			return _repo.Get(setting).Value;
		}

		public void SetValue(Setting setting, string value) {
			_repo.Set(new SettingModel { Type = setting, Value = value });
		}

		public void SetValue<T>(Setting setting, T value) {
			_repo.Set(new SettingModel { Type = setting, Value = value.ToString() });
		}

		//public void Dispose() {
		//	_repo.Dispose();
		//}

        public List<SettingModel> GetAll() {
            return _repo.GetAll();
        }

        public SettingModel Get(Setting setting) {
            return _repo.Get(setting);
        }
    }
}
