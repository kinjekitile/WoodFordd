using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.Interfaces.Providers;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Configuration;

namespace Woodford.Infrastructure.Reviews {
    public class ExternalReviewService : IExternalReviewService {
        private readonly ISettingService _settings;

        public ExternalReviewService(ISettingService settings) {
            _settings = settings;
        }

        public ReviewLinkResponseModel GetReviewLink(ReviewLinkRequestModel request) {

            string method = "/invitation-links";
            string baseUrl = _settings.GetValue<string>(Setting.ReviewServiceAPIEndpoint);
            string businessUnitId = _settings.GetValue<string>(Setting.ReviewServiceBusinessUnitId);
            string siteDomain = _settings.GetValue<string>(Setting.Public_Website_Domain);

            string authToken = GetAuthToken();
            using (HttpClient client = new HttpClient()) {
                ReviewLinkRequest linkRequest = new ReviewLinkRequest();
                linkRequest.email = request.Email;
                linkRequest.name = request.Name;
                linkRequest.referenceId = request.ReservationId.ToString();
                linkRequest.locale = "en-ZA";
                linkRequest.redirectUri = siteDomain + "reviews/trustpilot?reservationId=" + request.ReservationId.ToString();


                string jsonRequest = JsonConvert.SerializeObject(linkRequest);

                var stringContent = new StringContent(jsonRequest,Encoding.UTF8 ,"application/json");
                
                string methodUrl = baseUrl + businessUnitId + method;

                client.BaseAddress = new Uri(methodUrl);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("token", authToken);

                HttpResponseMessage httpResponseMessage = client.PostAsync(methodUrl, stringContent).Result;

                var requestsLinkResponseString = httpResponseMessage.Content.ReadAsStringAsync().Result;

                var requestsLinkResponseObject = JsonConvert.DeserializeObject<RootObjectLinkResponse>(requestsLinkResponseString);


                ReviewLinkResponseModel response = new ReviewLinkResponseModel();
                response.ReviewUrl = requestsLinkResponseObject.url;
                response.Id = requestsLinkResponseObject.id;

                return response;

            }


            throw new NotImplementedException();
        }

        public string GetAuthToken() {
            string apiKey = ConfigurationManager.AppSettings["TrustPilotAPIKey"];
            string apiSecret = ConfigurationManager.AppSettings["TrustPilotAPISecret"];


            string trustPilotAccessTokenUrl = _settings.GetValue<string>(Setting.ReviewServiceAPIAuthUrl);
            string username = ConfigurationManager.AppSettings["TrustPilotUsername"];
            string password = ConfigurationManager.AppSettings["TrustPilotPassword"];


            using (HttpClient httpClient = new HttpClient()) {


                httpClient.BaseAddress = new Uri(trustPilotAccessTokenUrl);

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                var authString = apiKey + ":" + apiSecret;

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Base64Encode(authString));

                var stringPayload = "grant_type=password&username=" + username + "&password=" + password;

                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/x-www-form-urlencoded");

                HttpResponseMessage httpResponseMessage = httpClient.PostAsync(trustPilotAccessTokenUrl, httpContent).Result;

                var accessTokenResponseString = httpResponseMessage.Content.ReadAsStringAsync().Result;

                var accessTokenResponseObject = JsonConvert.DeserializeObject<RootObjectAuthResponse>(accessTokenResponseString);

                return accessTokenResponseObject.access_token;
            }


            throw new NotImplementedException();

        }

        private string Base64Encode(string plainText) {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

    }

    public class ReviewLinkRequest {
        public string referenceId { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string locale { get; set; }
        public string tags { get; set; }
        public string redirectUri { get; set; }
    }

    public class RootObjectAuthResponse {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string expires_in { get; set; }
    }

    public class RootObjectLinkResponse {
        public string id { get; set; }
        public string url { get; set; }
    }
}
