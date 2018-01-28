using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
	public interface ISettingService {
		void Initialize();
		string GetValue(Setting setting);
		T GetValue<T>(Setting setting);
		void SetValue(Setting setting, string value);
		void SetValue<T>(Setting setting, T value);
        List<SettingModel> GetAll();
        SettingModel Get(Setting setting);
	}
}
