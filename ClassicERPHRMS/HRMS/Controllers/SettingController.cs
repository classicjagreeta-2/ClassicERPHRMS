using HRMS.Models.Master;
using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static HRMS.Models.Model_Common;
using System.Collections;

namespace HRMS.Controllers
{
    public class SettingController : Controller
    {
        // GET: Setting
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetEmployee(string UNIT, string searchString = null, int? page = 1, int pageSize = 25)
        {
            if (!string.IsNullOrEmpty(UNIT))
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

                    var addSelectAll = new M_EMPMAS()
                    {
                        pK_EMPCODE = "Select",
                        fname = "All",
                    };
                    if (page == 1)
                    {
                        response.Data.value.results.Insert(0, addSelectAll);
                    }
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelState.AddModelError("Unit", "Please select a UNIT");
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

    }

}