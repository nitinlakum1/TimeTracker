﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http.Headers;
using TimeTracker.Models;
using TimeTracker.Models.Resource;
using TimeTracker_Model;
using TimeTracker_Model.Resources;
using TimeTracker_Repository.ResourcesRepo;

namespace TimeTracker.Controllers
{
    [Authorize(Roles = "Admin,HR")]
    public class ResourceController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IResourceRepo _resourcesRepo;

        public ResourceController(IMapper mapper, IResourceRepo resourcesRepo)
        {
            _mapper = mapper;
            _resourcesRepo = resourcesRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddResource()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddResource(ResourceViewModel model)
        {
            await ListOfResources(model);
            return RedirectToAction("AddResource");
        }

        private async Task ListOfResources(ResourceViewModel model)
        {
            for (int i = 0; i < i + 1; i++)
            {
                string url = $"https://prod.hirect.ai/hirect/candidate-service/candidates/search?searchDirect=true&cityId={model.CityId}&qs={model.Designation}&pageNum={i}&pageSize=20";
                var client = new RestClient(url);
                client.Timeout = -1;
                client.FollowRedirects = false;
                var request = new RestRequest(Method.POST);
                request.AddHeader("authority", "prod.hirect.ai");
                request.AddHeader("accept", "application/json, text/plain, */*");
                request.AddHeader("accept-language", "en-US,en;q=0.9");
                request.AddHeader("content-type", "application/json;charset=UTF-8 application/json;charset=UTF-8");
                request.AddHeader("origin", "https://recruiter.hirect.in");
                request.AddHeader("referer", "https://recruiter.hirect.in/");
                request.AddHeader("region", "in");
                request.AddHeader("sec-ch-ua", "\"Google Chrome\";v=\"107\", \"Chromium\";v=\"107\", \"Not=A?Brand\";v=\"24\"");
                request.AddHeader("sec-ch-ua-mobile", "?0");
                request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("sec-fetch-site", "cross-site");
                client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36";
                request.AddHeader("x-appversion", "2.1.0");
                request.AddHeader("x-brand", "Windows");
                request.AddHeader("x-deviceid", "6ae23cc7-1fe0-4830-946a-41f90b0b87e4");
                request.AddHeader("x-idtoken", $"{model.Token}");
                request.AddHeader("x-model", "10");
                request.AddHeader("x-os", "webapp");
                request.AddHeader("x-region", "in");
                request.AddHeader("x-role", "1");
                request.AddHeader("x-timestamp", "1669877252654");
                request.AddHeader("x-uid", "eb02f4980a8546a2ad91f23e5398");

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

                var body = JsonConvert.SerializeObject(content.Value);
                request.AddParameter("application/json;charset=UTF-8 application/json;charset=UTF-8", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    var deserializeObject = JsonConvert.DeserializeObject<dynamic>(response.Content);
                    var list = deserializeObject["data"]["list"];

                    if (((Newtonsoft.Json.Linq.JArray)list).Count == 0)
                    { break; }

                    foreach (var item in list)
                    {
                        string id = item["id"];
                        string preferenceId = item["preferenceId"];

                        var resource = await _resourcesRepo.GetResourceById(id);
                        if (string.IsNullOrWhiteSpace(resource.id))
                        {
                            await GetResourceDetails(id, preferenceId, model.Token);
                        }
                    }
                }
            }
        }

        //private async Task ListOfResources(ResourceViewModel model)
        //{
        //    for (int i = 0; i < i + 1; i++)
        //    {
        //        //string url = $"https://prod.hirect.ai/hirect/candidate-service/candidates/search?searchDirect=true&cityId={model.CityId}&qs={model.Designation}&pageNum={i}&pageSize=20";
        //        string url = "https://prod.hirect.ai/hirect/candidate-service/candidates/search?searchDirect=true&cityId=356&qs=Flutter&pageNum=0&pageSize=20";

        //        HttpClient _httpClient = new HttpClient();
        //        _httpClient.BaseAddress = new Uri(url);
        //        _httpClient.DefaultRequestHeaders.Accept.Clear();
        //        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        _httpClient.DefaultRequestHeaders.Add("authority", "prod.hirect.ai");
        //        _httpClient.DefaultRequestHeaders.Add("accept", "application/json, text/plain, */*");
        //        _httpClient.DefaultRequestHeaders.Add("accept-language", "en-US,en;q=0.9");
        //        _httpClient.DefaultRequestHeaders.Add("origin", "https://recruiter.hirect.in");
        //        _httpClient.DefaultRequestHeaders.Add("referer", "https://recruiter.hirect.in/");
        //        _httpClient.DefaultRequestHeaders.Add("region", "in");
        //        _httpClient.DefaultRequestHeaders.Add("sec-ch-ua", "\"Google Chrome\";v=\"107\", \"Chromium\";v=\"107\", \"Not=A?Brand\";v=\"24\"");
        //        _httpClient.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
        //        _httpClient.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
        //        _httpClient.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
        //        _httpClient.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
        //        _httpClient.DefaultRequestHeaders.Add("sec-fetch-site", "cross-site");
        //        _httpClient.DefaultRequestHeaders.Add("x-appversion", "2.1.0");
        //        _httpClient.DefaultRequestHeaders.Add("x-brand", "Windows");
        //        _httpClient.DefaultRequestHeaders.Add("x-deviceid", "792de51e4d5be52a35f55f3570193fc3"); //Change
        //        _httpClient.DefaultRequestHeaders.Add("x-idtoken", $"{model.Token}");
        //        _httpClient.DefaultRequestHeaders.Add("x-model", "10");
        //        _httpClient.DefaultRequestHeaders.Add("x-os", "webapp");
        //        _httpClient.DefaultRequestHeaders.Add("x-region", "in");
        //        _httpClient.DefaultRequestHeaders.Add("x-role", "1");
        //        _httpClient.DefaultRequestHeaders.Add("x-timestamp", "1669877252654"); //Change
        //        _httpClient.DefaultRequestHeaders.Add("x-uid", "eb02f4980a8546a2ad91f23e5398");

        //        string[] education = model.education.Split(',');
        //        string[] experience = model.experience.Split(',');
        //        string[] salary = model.salary.Split(',');
        //        var content = JsonContent.Create(new
        //        {
        //            education = education,
        //            experience = experience,
        //            salary = salary,
        //            gender = 0,
        //            minAge = 18,
        //        });

        //        var response = await _httpClient.PostAsync("", content);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var result = response.Content.ReadAsStringAsync().Result;

        //            var deserializeObject = JsonConvert.DeserializeObject<dynamic>(result);
        //            var list = deserializeObject["data"]["list"];

        //            if (((Newtonsoft.Json.Linq.JArray)list).Count == 0)
        //            { break; }

        //            foreach (var item in list)
        //            {
        //                string id = item["id"];
        //                string preferenceId = item["preferenceId"];

        //                var resource = await _resourcesRepo.GetResourceById(id);
        //                if (string.IsNullOrWhiteSpace(resource.id))
        //                {
        //                    await GetResourceDetails(id, preferenceId, model.Token);
        //                }
        //            }
        //        }
        //    }
        //}

        //private async Task ListOfResources(ResourceViewModel model)
        //{
        //    var deserializeObject = JsonConvert.DeserializeObject<dynamic>(model.Details);
        //    var list = deserializeObject["data"]["list"];

        //    foreach (var item in list)
        //    {
        //        string id = item["id"];
        //        string preferenceId = item["preferenceId"];

        //        var resource = await _resourcesRepo.GetResourceById(id);
        //        if (string.IsNullOrWhiteSpace(resource.id))
        //        {
        //            await GetResourceDetails(id, preferenceId, model.Token);
        //        }
        //    }
        //}

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
            _httpClient.DefaultRequestHeaders.Add("x-deviceid", "6ae23cc7-1fe0-4830-946a-41f90b0b87e4");
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
                var deserializeResult = JsonConvert.DeserializeObject<dynamic>(result);
                var data = JsonConvert.SerializeObject(deserializeResult["data"]);
                var deserializeObject = JsonConvert.DeserializeObject<ResourceModel>(data);

                if (deserializeObject != null)
                {
                    deserializeObject.preferenceId = preferenceId;

                    deserializeObject.companyExperiences = JsonConvert.SerializeObject(deserializeObject.experiences);

                    deserializeObject.city = JsonConvert.SerializeObject(deserializeObject.preferences);

                    await _resourcesRepo.AddResources(deserializeObject);
                }
            }
        }

        public async Task<IActionResult> GetResourcesList(DatatableParamViewModel param, string filter)
        {
            try
            {
                var dtParam = _mapper.Map<ResourceFilterModel>(param);

                if (!string.IsNullOrWhiteSpace(filter) && filter != "{}")
                {
                    var filterData = JsonConvert.DeserializeObject<ResourceFilterModel>(filter);
                    dtParam.Experience = filterData?.Experience ?? 0;
                    dtParam.Designation = filterData?.Designation;
                    dtParam.City = filterData?.City;
                    dtParam.Status = filterData?.Status;
                }

                var (resourceList, totalRecord) = await _resourcesRepo.GetResourcesList(dtParam);
                var lst = _mapper.Map<List<ResourceListViewModel>>(resourceList);

                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = totalRecord,
                    iTotalDisplayRecords = totalRecord,
                    aaData = lst,
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFollowup(FollowupAddViewModel model)
        {
            bool isSuccess = false;
            string message = "";
            try
            {
                if (ModelState.IsValid)
                {
                    var AddFollowup = _mapper.Map<FollowupModel>(model);
                    isSuccess = await _resourcesRepo.AddFollowup(AddFollowup);
                    message = isSuccess ? AppMessages.SAVE_SUCCESS : AppMessages.SOMETHING_WRONG;

                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json(new { isSuccess, message });
        }

        public async Task<IActionResult> GetFollowupList(string id)
        {
            try
            {
                var followupList = new List<FollowupListViewModel>();
                if (!string.IsNullOrWhiteSpace(id))
                {
                    var result = await _resourcesRepo.GetFollowupList(id);
                    followupList = _mapper.Map<List<FollowupListViewModel>>(result);
                }
                return PartialView("_FollowupList", followupList);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}