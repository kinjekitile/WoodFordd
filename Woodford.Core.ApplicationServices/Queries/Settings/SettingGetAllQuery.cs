﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class SettingGetAllQuery : IQuery<List<SettingModel>> {

    }

    public class SettingGetAllQueryHandler : IQueryHandler<SettingGetAllQuery, List<SettingModel>> {
        private readonly ISettingService _settingService;
        public SettingGetAllQueryHandler(ISettingService settingService) {
            _settingService = settingService;
        }

        public List<SettingModel> Process(SettingGetAllQuery query) {
            return _settingService.GetAll();
        }
    }
}
