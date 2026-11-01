using HRMS.Controllers;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace HRMS.Models
{
    public static class Services
    {
        private static HttpClient _client { get; set; }
        private static string _userName = null;
        private static string _token = null;
        //private static string _loca = null;
        private static string _dbName = null;
        private static string _baseDomain = null;
        private static string _dbType = null;
        static Services()
        {
            _baseDomain = ConfigurationManager.AppSettings["SubDomain"];
            _dbType = ConfigurationManager.AppSettings["DatabaseType"];
            _client = new HttpClient();
            _client.BaseAddress = new Uri(ConfigurationManager.AppSettings["BaseApiUrl"]);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<ResponseApiModel<TData>> PostAsync<TData>(string url, object model, Type type = null)
        {
            ResponseApiModel<TData> tdata = new ResponseApiModel<TData>();
            JsonContent content = JsonContent.Create(model);
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("x-module", "Payroll");
            //getTokenAndDB();
            try
            {
                if (!string.IsNullOrEmpty(Utility.SessionHelper.GetUser().getToken))
                {
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Utility.SessionHelper.GetUser().getToken);
                }

                if (!string.IsNullOrEmpty(Utility.SessionHelper.GetUser().getDbName))
                {
                    _client.DefaultRequestHeaders.Add("x-database", Utility.SessionHelper.GetUser().getDbName);
                }
                if (!string.IsNullOrEmpty(_dbType))
                {
                    _client.DefaultRequestHeaders.Add("x-dbtype", _dbType);
                }
                if (!string.IsNullOrEmpty(Utility.SessionHelper.GetUser().Loca))
                {
                    _client.DefaultRequestHeaders.Add("x-loca", Utility.SessionHelper.GetUser().Loca);
                }               
                if (!String.IsNullOrEmpty(_baseDomain))
                {
                    url = _baseDomain + url;
                }
                var rest = await _client.PostAsync(url, content);
                tdata = await ReadAsString<TData>(rest);
                return tdata;
                //try
                //{
                //    tdata = await ReadAsStringAsync<TData>(rest);
                //}
                //catch (Exception ex)
                //{
                //    tdata.Message = tdata.Message;
                //    tdata.StatusCode = rest.StatusCode.ToString();
                //    throw;
                //}

            }
            catch (Exception ex)
            {
                if ((ex.InnerException) != null)
                    tdata.Message = ex.InnerException.Message;
                //throw;
            }

            return tdata;
        }

        public static async Task<ResponseApiModel<TData>> GetAsync<TData>(string url)
        {
            ResponseApiModel<TData> tdata = new ResponseApiModel<TData>();
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("x-module", "Payroll");
            getTokenAndDB();
            if (!string.IsNullOrEmpty(_token))
            {
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
            }

            if (!string.IsNullOrEmpty(_dbName))
            {
                _client.DefaultRequestHeaders.Add("x-database", _dbName);
            }
            if (!string.IsNullOrEmpty(_dbType))
            {
                _client.DefaultRequestHeaders.Add("x-dbtype", _dbType);
            }
            if (!string.IsNullOrEmpty(Utility.SessionHelper.GetUser().Loca))
            {
                _client.DefaultRequestHeaders.Add("x-loca", Utility.SessionHelper.GetUser().Loca);
            }            
            if (!String.IsNullOrEmpty(_baseDomain))
            {
                url = _baseDomain + url;
            }
            var rest = await _client.GetAsync(url);
            tdata = await ReadAsString<TData>(rest);
            return tdata;
        }

         private static async Task<ResponseApiModel<TData>> ReadAsString<TData>(HttpResponseMessage rest)
        {

            ResponseApiModel<TData> tdata = new ResponseApiModel<TData>();
            try
            {
                if (rest.IsSuccessStatusCode)
                {
                    string responseBody =await rest.Content.ReadAsStringAsync();
                    //logger.Warn("rest LogError: - " + responseBody + ". Data : " + rest);
                    tdata.Data = JsonConvert.DeserializeObject<TData>(responseBody);
                    tdata.StatusCode = rest.StatusCode.ToString();
                    tdata.IsSuccessStatusCode = rest.IsSuccessStatusCode;
                }
                else
                {
                    string responseBody =await rest.Content.ReadAsStringAsync();
                   
                    //var res1 = JsonConvert.DeserializeObject<UnauthorizedModel>(responseBody);
                    if (!string.IsNullOrEmpty(responseBody))
                    {
                        var match = System.Text.RegularExpressions.Regex.Match(responseBody.ToString(), @"ORA-\d+:.*");
                        tdata.Message = match.Success ? match.Value.Replace("(", "").Replace(")", "") : responseBody;
                    }
                    //var res = JsonConvert.DeserializeObject<dynamic>(responseBody);
                    //tdata.IsSuccessStatusCode = rest.IsSuccessStatusCode;
                    //var res = JsonConvert.DeserializeObject<dynamic>(responseBody);
                    tdata.IsSuccessStatusCode = rest.IsSuccessStatusCode;
                    
                        //tdata.Message = res.message;
                    tdata.StatusCode = rest.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {                
                throw;
            }

            return tdata;
        }
        public class UnauthorizedModel
        {
            public string message { get; set; }
        }
        private static void getTokenAndDB()
        {
            //_dbName = "PAY_JGHPL2025";
            //_token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjUiLCJuYmYiOjE3MzI1MzAwMDgsImV4cCI6MTczNjQxODAwOCwiaWF0IjoxNzMyNTMwMDA4fQ.0AxicIvG7OWZmR_yuk7FyRddibK6K3V-ErUZVIxbuhQ";
            string InDevelopment = ConfigurationManager.AppSettings["IsDevelopment"];
            if (InDevelopment != "true")
            {
                _dbName = Utility.SessionHelper.GetUser().getDbName;
                _token = Utility.SessionHelper.GetUser().getToken;
            }
        }

    }

    public class Errors
    {
        public List<string> error { get; set; }
    }

    public class Root
    {
        public string type { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public Errors errors { get; set; }
        public string traceId { get; set; }
    }
}