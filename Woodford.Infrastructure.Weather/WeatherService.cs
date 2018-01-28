using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Providers;

namespace Woodford.Infrastructure.Weather {
    public class WeatherService : IWeatherService {
        private IBranchService _branchService;
        private ISettingService _settings;
        //        {"_id":1007311,"name":"Durban","country":"ZA","coord":{"lon":31.0292,"lat":-29.857901}}

        //{"_id":3369157,"name":"Cape Town","country":"ZA","coord":{"lon":18.42322,"lat":-33.925838}}

        //{"_id":993800,"name":"Johannesburg","country":"ZA","coord":{"lon":28.043631,"lat":-26.202271}}

        //{"_id":953975,"name":"Port Elizabeth Airport","country":"ZA","coord":{"lon":25.611361,"lat":-33.984039}}


        public WeatherService(IBranchService branchService, ISettingService settings) {
            _branchService = branchService;
            _settings = settings;
        }

        public ReservationWeatherNotificationModel GetWeatherForPickupLocation(int pickupLocationId) {
            
            var branch = _branchService.GetById(pickupLocationId);

            if (!string.IsNullOrEmpty(branch.WeatherApiId)) {
                using (WebClient wc = new WebClient()) {
                    string url = "http://api.openweathermap.org/data/2.5/forecast?id={1}&mode=xml&appid={0}";
                    string api = _settings.GetValue<string>(Setting.WeatherApiAccountId);
                    string cityId = branch.WeatherApiId;
                    url = string.Format(url, api, cityId);
                    var response = wc.DownloadString(url);
                    XDocument doc = XDocument.Parse(response);
                    var items = doc.Element("weatherdata").Element("forecast").Elements("time").ToList();

                    List<WeatherItem> weatherPredictions = new List<WeatherItem>();

                    foreach (var item in items) {

                        DateTime recordDateFrom = Convert.ToDateTime(item.Attribute("from").Value);
                        DateTime recordDateTo = Convert.ToDateTime(item.Attribute("to").Value);
                        decimal max = 0m;
                        decimal min = 0m;

                        try {
                            max = Convert.ToDecimal(item.Element("temperature").Attribute("max").Value.ToString().Replace(".", ","));
                            min = Convert.ToDecimal(item.Element("temperature").Attribute("min").Value.ToString().Replace(".", ","));
                        } catch (Exception) {
                            max = Convert.ToDecimal(item.Element("temperature").Attribute("max").Value.ToString());
                            min = Convert.ToDecimal(item.Element("temperature").Attribute("min").Value.ToString());
                        }




                        weatherPredictions.Add(new WeatherItem { RecordDateFrom = recordDateFrom, RecordDateTo = recordDateTo, RecordTempMax = max, RecordTempMin = min });

                    }

                    List<WeatherDateGroup> weatherDates = new List<WeatherDateGroup>();

                    var groups = weatherPredictions.GroupBy(x => x.RecordDateFrom.Date, x => x, (date, values) => new WeatherDateGroup { GroupDate = date, Items = values.ToList() });

                    ReservationWeatherNotificationModel model = new ReservationWeatherNotificationModel();
                    model.Items = groups.Select(x => new WeatherReportDay { ReportDate = x.GroupDate, ReportTempMax = x.MaxTemp, ReportTempMin = x.MinTemp }).ToList();

                    return model;
                }
            }
            else {
                return null;
            }
            
        }
    }

    public class WeatherDateGroup {
        public DateTime GroupDate { get; set; }
        public WeatherDateGroup() {
            Items = new List<WeatherItem>();
        }
        public List<WeatherItem> Items { get; set; }

        public decimal MinTemp {
            get {
                return Items.OrderBy(x => x.RecordTempMin).First().RecordTempMin;
            }
        }

        public decimal MaxTemp {
            get {
                return Items.OrderBy(x => x.RecordTempMax).Last().RecordTempMax;
            }
        }

    }

    public class WeatherItem {

        public DateTime RecordDateFrom { get; set; }
        public DateTime RecordDateTo { get; set; }
        public decimal RecordTempMax { get; set; }
        public decimal RecordTempMin { get; set; }

    }
}
