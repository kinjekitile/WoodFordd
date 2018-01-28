using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class SettingsRepository : RepositoryBase, ISettingsRepository {

        public SettingsRepository(IDbConnectionConfig connection) : base(connection) { }

        public SettingModel Get(Setting setting) {
            SettingModel model = GetAll().SingleOrDefault(x => x.Type == setting);
            if (model == null) {
                AppSetting a = new AppSetting();
                a.Title = setting.ToString();
                a.SettingValue = GetSettingDefaultValue(setting);
                _db.AppSettings.Add(a);
                _db.SaveChanges();
                model = GetAll().SingleOrDefault(x => x.Type == setting);
            }
            return model;
        }

        public void Set(SettingModel model) {
            var currentModel = Get(model.Type);
            string settingName = model.Type.ToString();
            AppSetting a = _db.AppSettings.SingleOrDefault(x => x.Title == settingName);
            a.SettingValue = model.Value;
            _db.SaveChanges();
        }

        private string GetSettingDefaultValue(Setting setting) {
            string defaultValue = "";
            foreach (FieldInfo fieldInfo in typeof(Setting).GetFields()) {
                if (fieldInfo.Name == setting.ToString()) {
                    if (Attribute.IsDefined(fieldInfo, typeof(SiteSettingDefaultValue))) {
                        SiteSettingDefaultValue defaultValueAttribute = (SiteSettingDefaultValue)Attribute.GetCustomAttribute(fieldInfo, typeof(SiteSettingDefaultValue));
                        defaultValue = defaultValueAttribute.DefaultValue;
                        break;
                    }
                }
            }
            return defaultValue;
        }

        public List<SettingModel> GetAll() {

            //addMissingSettings();
            return getAllSettings();
        }

        private List<SettingModel> getAllSettings() {
            return _db.AppSettings
                .ToList()
                .Select(x => new SettingModel {
                    Type = (Setting)Enum.Parse(typeof(Setting), x.Title),
                    Value = x.SettingValue
                }).ToList();
        }

        //private void addMissingSettings() {
        //    List<SettingModel> currentSettings = getAllSettings();

        //    foreach (FieldInfo fieldInfo in typeof(Setting).GetFields()) {
        //        SettingModel s = currentSettings.Where(x => x.Type.ToString() == fieldInfo.Name).SingleOrDefault();
        //        if (!fieldInfo.IsSpecialName) {
        //            if (s == null) {
        //                string defaultValue = "";
        //                if (Attribute.IsDefined(fieldInfo, typeof(SiteSettingDefaultValue))) {
        //                    SiteSettingDefaultValue defaultValueAttribute = (SiteSettingDefaultValue)Attribute.GetCustomAttribute(fieldInfo, typeof(SiteSettingDefaultValue));
        //                    defaultValue = defaultValueAttribute.DefaultValue;
        //                }
        //                AppSetting a = new AppSetting();
        //                a.Title = fieldInfo.Name;
        //                a.SettingValue = defaultValue;
        //                _db.AppSettings.Add(a);
        //            }
        //        }
        //    }
        //    _db.SaveChanges();
        //}
    }
}
