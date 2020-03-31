using M3Service.Model;
using M3Service.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace M3Service
{
    public class M3Client
    {
        private ClientConfiguration m3RestConfiguration;

        public M3Client(string user, string password, string url)
        {
            m3RestConfiguration = new ClientConfiguration();
            m3RestConfiguration.ContentType = "application/json";
            m3RestConfiguration.Accept = "application/json";
            m3RestConfiguration.User = user;
            m3RestConfiguration.Password = password;
            m3RestConfiguration.ServiceUrl = url;
        }

        public M3Response GetDataAsync<T>(string program, string transaction, object queryParam,bool outputAllFields = false, int maxrecs = 0, bool metadata = false, bool excludempty = false)
        {
            M3Response responseDto = new M3Response();

            HttpClient request = RestClientFactory.CreateBasicAuthRestClient(m3RestConfiguration);
            string requestUri = $"{program}/{transaction};metadata={metadata};maxrecs={maxrecs};excludempty={excludempty}";
            if (!outputAllFields) {
                requestUri += $";returncols={RestClientUtil.GetOutputParameters(typeof(T))}";
            }
            requestUri += $" {RestClientUtil.GetInputParameters(queryParam)}";

            HttpResponseMessage response = request.GetAsync(requestUri).Result;

            if (response.IsSuccessStatusCode == false)
            {
                responseDto.Success = false;
                responseDto.Message = "M3 call failed.";
                return responseDto;
            }

            var content = response.Content.ReadAsStringAsync().Result;
            var m3Error = this.GetM3ErrorMessage(content);

            if (!String.IsNullOrEmpty(m3Error))
            {
                responseDto.Success = false;
                responseDto.Message = m3Error;
                return responseDto;
            }

            dynamic responseMessage = JObject.Parse(content);
            responseDto.Data = this.GetValueListFromM3ResultByMultipleSelectors<T>(responseMessage, RestClientUtil.GetProperties(typeof(T)), metadata);

            return responseDto;
        }

        public M3Response GetDataAsync(string program, string transaction, string queryParam,List<string> outputs,bool outputAllFields = false, int maxrecs = 0, bool metadata = false, bool excludempty = false)
        {
            M3Response responseDto = new M3Response();

            HttpClient request = RestClientFactory.CreateBasicAuthRestClient(m3RestConfiguration);
            string requestUri = $"{program}/{transaction};metadata={metadata};maxrecs={maxrecs};excludempty={excludempty}";

            if (!outputAllFields && outputs.Count > 0)
            {
                requestUri += $";returncols={string.Join(",", outputs)}";
            }
            requestUri += $" {RestClientUtil.GetInputParametersFromJson(queryParam)}";

            HttpResponseMessage response = request.GetAsync(requestUri).Result;

            if (response.IsSuccessStatusCode == false)
            {
                responseDto.Success = false;
                responseDto.Message = "M3 call failed.";
                return responseDto;
            }

            var content = response.Content.ReadAsStringAsync().Result;
            var m3Error = this.GetM3ErrorMessage(content);

            if (!String.IsNullOrEmpty(m3Error))
            {
                responseDto.Success = false;
                responseDto.Message = m3Error;
                return responseDto;
            }

            dynamic responseMessage = JObject.Parse(content);
            responseDto.Data = this.GetValueListFromM3ResultByMultipleSelectors(responseMessage, outputs, outputAllFields, metadata);

            return responseDto;
        }

        public string ResponseAsText(JObject jObject)
        {

            string result = "";

            if (jObject == null)
            {
                return result = "Response is null";
            }
            var objCount = jObject.Children().Count();
            int childCount = objCount;

            if (childCount == 4)
            {
                if (!Regex.IsMatch(jObject.Children().ToList()[3].ToString(), @"^\d+$"))
                {
                    return jObject.ToString();
                }
                else
                {
                    try
                    {
                        string jsonString = string.Empty;
                        for (int i = 0; i < objCount; i++)
                        {
                            jsonString = jsonString + jObject.Children().ToList()[i].ToString().Remove(0, 12);


                        }
                        result = Regex.Replace(jsonString, "[^a-zA-Z0-9_.]+", " ", RegexOptions.Compiled);
                    }
                    catch (NullReferenceException)
                    {
                        return null;
                    }
                    return result;
                }
            }
            else
            {
                result = jObject.ToString();
            }

            return result;
        }

        public List<T> GetValueListFromM3ResultByMultipleSelectors<T>(JObject jObject, IList<string> keys, bool metadata)
        {
            List<T> resultList = new List<T>();
            try
            {
                var index = metadata == true ? 3 : 2;
                string jsonString = jObject.Children().ToList()[index].ToString().Remove(0, 12);

                var list = JsonConvert.DeserializeObject<JsonParent[]>(jsonString);
                List<object> result = new List<object>();

                for (int i = 0; i < list.Length; i++)
                {
                    JsonParent jsonParent = list[i];

                    Dictionary<string, string> dictionary = new Dictionary<string, string>();

                    foreach (string key in keys)
                    {
                        JsonChild child = jsonParent.NameValue.Where(a => a.Name.Equals(key)).FirstOrDefault<JsonChild>();

                        if (child != null)
                        {
                            dictionary.Add(key, child.Value.TrimEnd());
                        }
                    }

                    var serialized = JsonConvert.SerializeObject(dictionary);
                    T data = JsonConvert.DeserializeObject<T>(serialized);

                    resultList.Add(data);

                    dictionary.Clear();
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return resultList;
        }

        public dynamic GetValueListFromM3ResultByMultipleSelectors(JObject jObject, List<string> keys, bool outputAll, bool metadata)
        {
            List<dynamic> resultList = new List<dynamic>();
            try
            {
                var index = metadata == true ? 3 : 2;
                string jsonString = jObject.Children().ToList()[index].ToString().Remove(0, 12);

                var list = JsonConvert.DeserializeObject<JsonParent[]>(jsonString);
                List<object> result = new List<object>();

                if (outputAll || keys.Count <= 0) {
                    keys = list.FirstOrDefault() != null ? list.FirstOrDefault().NameValue.Select(a => a.Name).ToList() : new List<string>();
                }

                for (int i = 0; i < list.Length; i++)
                {
                    JsonParent jsonParent = list[i];

                    Dictionary<string, string> dictionary = new Dictionary<string, string>();

                    foreach (string key in keys)
                    {
                        JsonChild child = jsonParent.NameValue.Where(a => a.Name.Equals(key)).FirstOrDefault<JsonChild>();

                        if (child != null)
                        {
                            dictionary.Add(key, child.Value.TrimEnd());
                        }
                    }

                    resultList.Add(dictionary);                  
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return resultList;
        }

        private string GetM3ErrorMessage(string text)
        {
            string message = null;
            try
            {
                if (text == null)
                {
                    message = "No response from M3";
                }
                else if (text.Contains("NOK"))
                {
                    message = Regex.Split(text, "NOK")[1];
                }
                else if (text.Contains("\"Message\"")) 
                {
                    message = Regex.Split(text, "\"Message\"")[1];
                }

                if (!String.IsNullOrEmpty(message))
                {
                    Regex reg = new Regex("[*'\",_&#^@:{}]");
                    message = reg.Replace(message, string.Empty);
                }

            }
            catch (Exception ex)
            {
                message = "Error while reading response from M3";
            }
            return message;
        }
    }

    public class JsonParent
    {
        public string RowIndex { get; set; }
        public JsonChild[] NameValue { get; set; }
    }

    /// <summary>
    /// </summary>
    public class JsonChild
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
