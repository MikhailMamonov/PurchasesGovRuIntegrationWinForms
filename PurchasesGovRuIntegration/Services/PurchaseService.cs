using PurchasesGovRuIntegration.Helpers;
using PurchasesGovRuIntegration.Services.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;

namespace PurchasesGovRuIntegration.Services
{
    internal class PurchaseService : IPurchaseService
    {
        
        public PurchaseService()
        {
        }

        public async Task<Dictionary<string,Dictionary<string,string>>> Find(string regNumber)
        {
            HttpResponseMessage response;

            using (HttpClient _httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders.Add("User-Agent","Postman");
                _httpClient.BaseAddress = new Uri("https://zakupki.gov.ru");
                if (regNumber.Length < 19)
                {
                    response = await _httpClient.GetAsync($"/api/mobile/proxy/917/223/purchase/public/purchase/info/common-info.html?regNumber={regNumber}");

                    if (response.IsSuccessStatusCode == false)
                        throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");
                    var dataAsString = await response.Content.ReadAsStringAsync();

                    var forecastNode = JObject.Parse(dataAsString);
                    
                    var noticeId = (int)forecastNode["data"]["noticeInfo"]["id"];

                    response = await _httpClient.GetAsync($"/epz/order/notice/notice223/common-info.html?noticeInfoId={noticeId}");
                    return await response.ReadContent223FZAsync();
                }
                else
                {
                    response = await _httpClient.GetAsync($"/epz/order/notice/ea20/view/common-info.html?regNumber={regNumber}");

                    return await response.ReadContent44FZAsync();
                }
            }
        }
    }
}
