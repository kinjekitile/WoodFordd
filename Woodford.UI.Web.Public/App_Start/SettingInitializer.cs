//[assembly: WebActivator.PostApplicationStartMethod(typeof(OnionArchitecture.UI.Web.Admin.App_Start.SettingInitializer), "Initialize")]

namespace Woodford.UI.Web.Public.App_Start {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using Woodford.Core.Interfaces;

	public static class SettingInitializer {
		public static void Initialize() {
			ISettingService _service = MvcApplication.Container.GetInstance<ISettingService>();
			_service.Initialize();
		}
	}
}