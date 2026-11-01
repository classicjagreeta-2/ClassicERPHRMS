using HRMS.Models.Master;
using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HRMS.Controllers.Test
{
    public class DropdownTestController : Controller
    {
        // GET: DropdownTest
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetEmployee(string UNIT, string searchString = null, int? page = 1, int pageSize = 25)
        {
            var url = string.Format("/api/Employee/GetByPage?page={0}&pageSize={1}", page, pageSize);
            if (!string.IsNullOrEmpty(UNIT))
            {
                url = string.Format("/api/Employee/GetByPage?page={0}&pageSize={1}&unit={2}", page, pageSize, UNIT);
                if (!string.IsNullOrEmpty(searchString))
                {
                    url = string.Format("/api/Employee/GetByPage?search={0}&page={1}&pageSize={2}&unit={3}", searchString, page, pageSize, UNIT);
                }
                var response = await Services.GetAsync<PageModel<M_EMPMAS>>(url);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> GetUnitDropdown(string searchString = null, int? page = 1, int pageSize = 5)
        {
            var response = await Services.GetAsync<PageModel<M_UNIT>>(string.Format("/api/Unit/GetByPage?page={0}&pageSize={0}&unit={0}", page, pageSize, searchString, Helpers.UserInfo.getUserName));


            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetDropdownOptions(string searchString = null, int? page = 1, int pageSize = 25)
        {
            var response = await Services.GetAsync<PageModel<M_EMPMAS>>(string.Format("/api/Employee/GetByPage?page={0}&pageSize={1}&search={2}&userName={3}", page, pageSize, searchString, Helpers.UserInfo.getUserName));


            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}