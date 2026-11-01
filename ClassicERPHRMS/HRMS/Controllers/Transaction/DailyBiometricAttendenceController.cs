using HRMS.Models.Master;
using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HRMS.Controllers.Transaction
{
    public class DailyBiometricAttendenceController : Controller
    {
        // GET: DailyBiometricAttendence
        public async Task<ActionResult> Index()
        {
            //var ailowcal2 = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll");

            //List<SelectListItem> all2 = ailowcal2.Data.Select(x => new SelectListItem
            //{
            //    Value = x.code,
            //    Text = x.code + "-" + x.name

            //}).ToList();
            //ViewBag.AllowCal2 = all2;
            return View();
        }
    }
}