﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http.Headers;
using System.Text;
using TimeTracker_Repository;

namespace TimeTracker.Controllers
{
    //[Authorize]
    public class DashboardController : Controller
    {
        private readonly ISettingRepo _settingRepo;

        public DashboardController(ISettingRepo settingRepo)
        {
            _settingRepo = settingRepo;
        }
        public async Task<IActionResult> Index()
        {
            await ListOfResources();
            return View();
        }

        private async Task ListOfResources()
        {
            HttpClient _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://prod.hirect.ai/hirect/candidate-service/candidates/search?searchDirect=true&cityId=356&qs=Flutter&pageNum=0&pageSize=20");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _httpClient.DefaultRequestHeaders.Add("authority", "prod.hirect.ai");
            _httpClient.DefaultRequestHeaders.Add("accept", "application/json, text/plain, */*");
            _httpClient.DefaultRequestHeaders.Add("accept-language", "en-US,en;q=0.9");
            _httpClient.DefaultRequestHeaders.Add("origin", "https://recruiter.hirect.in");
            _httpClient.DefaultRequestHeaders.Add("referer", "https://recruiter.hirect.in/");
            _httpClient.DefaultRequestHeaders.Add("region", "in");
            _httpClient.DefaultRequestHeaders.Add("sec-ch-ua", "\"Microsoft Edge\";v=\"107\", \"Chromium\";v=\"107\", \"Not=A?Brand\";v=\"24\"");
            _httpClient.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            _httpClient.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            _httpClient.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
            _httpClient.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
            _httpClient.DefaultRequestHeaders.Add("sec-fetch-site", "cross-site");
            _httpClient.DefaultRequestHeaders.Add("x-appversion", "2.1.0");
            _httpClient.DefaultRequestHeaders.Add("x-brand", "Windows");
            _httpClient.DefaultRequestHeaders.Add("x-deviceid", "985300e24227239fe9f5b338b930473d");
            _httpClient.DefaultRequestHeaders.Add("x-idtoken", "eyJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoiZWIwMmY0OTgwYTg1NDZhMmFkOTFmMjNlNTM5OCIsImNyZWF0ZVRpbWUiOjE2Njk2MTE1NDQ3MTQsImlhdCI6MTY2OTYxMTU0NCwiaXNzIjoiaGlyZWN0X2FwcF9pc3N1ZXIifQ.TyHQntYrIIx7bsnxl42Lcgp4fSnsYrd5CoENBr0hHls");
            _httpClient.DefaultRequestHeaders.Add("x-model", "10");
            _httpClient.DefaultRequestHeaders.Add("x-os", "webapp");
            _httpClient.DefaultRequestHeaders.Add("x-region", "in");
            _httpClient.DefaultRequestHeaders.Add("x-role", "1");
            _httpClient.DefaultRequestHeaders.Add("x-timestamp", "1669797482071");
            _httpClient.DefaultRequestHeaders.Add("x-uid", "eb02f4980a8546a2ad91f23e5398");

            var content = JsonContent.Create(new
            {
                education = new string[] { "1" },
                experience = new string[] { "4" },
                salary = new string[] { "1" },
                gender = 0,
                minAge = 22,
            });

            var response = await _httpClient.PostAsync("", content);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;

                var deserializeObject = JsonConvert.DeserializeObject<dynamic>(result);
                var list = deserializeObject["data"]["list"];

                foreach (var item in list)
                {
                    string id = item["id"];
                    string preferenceId = item["preferenceId"];

                    var resource = await _settingRepo.GetResourceById(id);
                    if (resource == null || resource.Id == 0)
                    {
                        await GetResourceDetails(id, preferenceId);
                    }
                }
            }
        }

        private async Task GetResourceDetails(string id, string preferenceId)
        {
            HttpClient _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri($"https://prod.hirect.ai/hirect/candidate-service/recruiters/candidates/{id}/profile?preferenceId={preferenceId}");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _httpClient.DefaultRequestHeaders.Add("authority", "prod.hirect.ai");
            _httpClient.DefaultRequestHeaders.Add("authority", "prod.hirect.ai");
            _httpClient.DefaultRequestHeaders.Add("accept", "application/json, text/plain, */*");
            _httpClient.DefaultRequestHeaders.Add("accept-language", "en-US,en;q=0.9");
            _httpClient.DefaultRequestHeaders.Add("origin", "https://recruiter.hirect.in");
            _httpClient.DefaultRequestHeaders.Add("referer", "https://recruiter.hirect.in/");
            _httpClient.DefaultRequestHeaders.Add("region", "in");
            _httpClient.DefaultRequestHeaders.Add("sec-ch-ua", "\"Microsoft Edge\";v=\"107\", \"Chromium\";v=\"107\", \"Not=A?Brand\";v=\"24\"");
            _httpClient.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            _httpClient.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            _httpClient.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
            _httpClient.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
            _httpClient.DefaultRequestHeaders.Add("sec-fetch-site", "cross-site");
            _httpClient.DefaultRequestHeaders.Add("x-appversion", "2.1.0");
            _httpClient.DefaultRequestHeaders.Add("x-brand", "Windows");
            _httpClient.DefaultRequestHeaders.Add("x-deviceid", "985300e24227239fe9f5b338b930473d");
            _httpClient.DefaultRequestHeaders.Add("x-idtoken", "eyJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoiZWIwMmY0OTgwYTg1NDZhMmFkOTFmMjNlNTM5OCIsImNyZWF0ZVRpbWUiOjE2Njk2MTE1NDQ3MTQsImlhdCI6MTY2OTYxMTU0NCwiaXNzIjoiaGlyZWN0X2FwcF9pc3N1ZXIifQ.TyHQntYrIIx7bsnxl42Lcgp4fSnsYrd5CoENBr0hHls");
            _httpClient.DefaultRequestHeaders.Add("x-model", "10");
            _httpClient.DefaultRequestHeaders.Add("x-os", "webapp");
            _httpClient.DefaultRequestHeaders.Add("x-region", "in");
            _httpClient.DefaultRequestHeaders.Add("x-role", "1");
            _httpClient.DefaultRequestHeaders.Add("x-timestamp", "1669797197438");
            _httpClient.DefaultRequestHeaders.Add("x-uid", "eb02f4980a8546a2ad91f23e5398");

            var response = await _httpClient.GetAsync("");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var deserializeObject = JsonConvert.DeserializeObject<dynamic>(result);
                var data = JsonConvert.SerializeObject(deserializeObject["data"]);
                await _settingRepo.AddResources(id, preferenceId, data);
            }
        }
    }
}
