using HRMS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using toastr.Net;
using toastr.Net.OptionEnums;
using static HRMS.Helpers;

namespace HRMS.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        public async Task<ActionResult> Index()
        {
            try
            {
                var response = await Services.GetAsync<JObject>(string.Format("/api/Menu/GetAll?moduleCode={0}", Utility.SessionHelper.GetUser().ModuleCode)); // Utility.SessionHelper.GetUser().ModuleCode
                if (response.IsSuccessStatusCode)
                {
                    //var expiresMin = Session["expiresMin"];
                    //var expiresTime = (TimeSpan)Session["expiresTime"];
                    var currentTime = DateTime.Now.TimeOfDay;
                    //var dd = (currentTime - expiresTime).Minutes;
                    Session["MenuList"] = response.Data["manuDetails"];
                    var User_AEDV = response.Data["usracS_AEDV"];
                    var AEDVJson = (JsonConvert.SerializeObject(User_AEDV));
                    var _AEDV = JsonConvert.DeserializeObject<List<AEDV>>(AEDVJson);
                    Session["User_AEDV"] = _AEDV;
                    return View();
                }
            }
            catch (TaskCanceledException)
            {
                //return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Auth");
        }

        public async Task<ActionResult> GetMenu()
        {
            var responce = await Services.GetAsync<List<MODULE>>(string.Format("/api/Module/GetAll", Utility.SessionHelper.GetUser().getUserName));//  Utility.SessionHelper.GetUser().getUserName
            if (responce.IsSuccessStatusCode)
                Session["MenuList"] = responce;
            return View();
        }
    }
}