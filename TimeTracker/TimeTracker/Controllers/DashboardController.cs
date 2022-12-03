using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http.Headers;
using System.Text;
using TimeTracker.Models.Setting;
using TimeTracker_Repository;

namespace TimeTracker.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ISettingRepo _settingRepo;

        public DashboardController(ISettingRepo settingRepo)
        {
            _settingRepo = settingRepo;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddResource(ResourceViewModel model)
        {
            await ListOfResources(model);
            return RedirectToAction("Index");
        }

        private async Task ListOfResources(ResourceViewModel model)
        {
            for (int i = 0; i < i + 1; i++)
            {
                string url = $"https://prod.hirect.ai/hirect/candidate-service/candidates/search?searchDirect=true&cityId={model.CityId}&qs={model.Designation}&pageNum={i}&pageSize=20";

                HttpClient _httpClient = new HttpClient();
                _httpClient.BaseAddress = new Uri(url);
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                _httpClient.DefaultRequestHeaders.Add("authority", "prod.hirect.ai");
                _httpClient.DefaultRequestHeaders.Add("accept", "application/json, text/plain, */*");
                _httpClient.DefaultRequestHeaders.Add("accept-language", "en-US,en;q=0.9");
                _httpClient.DefaultRequestHeaders.Add("origin", "https://recruiter.hirect.in");
                _httpClient.DefaultRequestHeaders.Add("referer", "https://recruiter.hirect.in/");
                _httpClient.DefaultRequestHeaders.Add("region", "in");
                _httpClient.DefaultRequestHeaders.Add("sec-ch-ua", "\"Google Chrome\";v=\"107\", \"Chromium\";v=\"107\", \"Not=A?Brand\";v=\"24\"");
                _httpClient.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
                _httpClient.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
                _httpClient.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
                _httpClient.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
                _httpClient.DefaultRequestHeaders.Add("sec-fetch-site", "cross-site");
                _httpClient.DefaultRequestHeaders.Add("x-appversion", "2.1.0");
                _httpClient.DefaultRequestHeaders.Add("x-brand", "Windows");
                _httpClient.DefaultRequestHeaders.Add("x-deviceid", "792de51e4d5be52a35f55f3570193fc3"); //Change
                _httpClient.DefaultRequestHeaders.Add("x-idtoken", $"{model.Token}");
                _httpClient.DefaultRequestHeaders.Add("x-model", "10");
                _httpClient.DefaultRequestHeaders.Add("x-os", "webapp");
                _httpClient.DefaultRequestHeaders.Add("x-region", "in");
                _httpClient.DefaultRequestHeaders.Add("x-role", "1");
                _httpClient.DefaultRequestHeaders.Add("x-timestamp", "1669877252654"); //Change
                _httpClient.DefaultRequestHeaders.Add("x-uid", "eb02f4980a8546a2ad91f23e5398");

                string[] education = model.education.Split(',');
                string[] experience = model.experience.Split(',');
                string[] salary = model.salary.Split(',');
                var content = JsonContent.Create(new
                {
                    education = education,
                    experience = experience,
                    salary = salary,
                    gender = 0,
                    minAge = 18,
                });

                var response = await _httpClient.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;

                    var deserializeObject = JsonConvert.DeserializeObject<dynamic>(result);
                    var list = deserializeObject["data"]["list"];

                    if (((Newtonsoft.Json.Linq.JArray)list).Count == 0)
                    { break; }

                    foreach (var item in list)
                    {
                        string id = item["id"];
                        string preferenceId = item["preferenceId"];

                        var resource = await _settingRepo.GetResourceById(id);
                        if (resource == null || resource.Id == 0)
                        {
                            await GetResourceDetails(id, preferenceId, model.Token);
                        }
                    }
                }
            }
        }

        private async Task GetResourceDetails(string id, string preferenceId, string token)
        {
            string url = $"https://prod.hirect.ai/hirect/candidate-service/recruiters/candidates/{id}/profile?preferenceId={preferenceId}";

            HttpClient _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(url);
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
            _httpClient.DefaultRequestHeaders.Add("x-deviceid", "792de51e4d5be52a35f55f3570193fc3"); //Change
            _httpClient.DefaultRequestHeaders.Add("x-idtoken", $"{token}");
            _httpClient.DefaultRequestHeaders.Add("x-model", "10");
            _httpClient.DefaultRequestHeaders.Add("x-os", "webapp");
            _httpClient.DefaultRequestHeaders.Add("x-region", "in");
            _httpClient.DefaultRequestHeaders.Add("x-role", "1");
            _httpClient.DefaultRequestHeaders.Add("x-timestamp", "1669877252654"); //Change
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
