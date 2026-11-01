using HRMS.Models;
using HRMS.Models.Master;
using HRMS.Models.TRN;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace HRMS.Controllers.Transaction
{
    public class DaysWiseAttendenceController : Controller
    {
        // GET: DaysWiseAttendence
        public async Task<ActionResult>Index()
        {
            //var ailowcal2 = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll");

            //List<SelectListItem> all2 = ailowcal2.Data.Select(x => new SelectListItem
            //{
            //    Value = x.code,
            //    Text = x.code + "-" + x.name

            //}).ToList();
            //ViewBag.AllowCal2 = all2;

            var ailowcal3 = await Services.GetAsync<List<M_SHIFT>>("/api/Shift/GetAll");

            List<SelectListItem> all3 = ailowcal3.Data.Select(x => new SelectListItem
            {
                Value = x.pK_SHIFT_CODE,
                Text = x.pK_SHIFT_CODE + "-" + x.description

            }).ToList();
            ViewBag.AllowCal3 = all3;

            var ailowcal4 = await Services.GetAsync<List<M_DEPARTMENT>>("/api/Department/GetAll");

            List<SelectListItem> all4 = ailowcal4.Data.Select(x => new SelectListItem
            {
                Value = x.pK_DEPT_CODE,
                Text = x.pK_DEPT_CODE + "-" + x.description

            }).ToList();
            ViewBag.AllowCal4 = all4;

            return View();
        }

        //public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        //{
        //    ViewBag.CurrentSort = sortOrder;
        //    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
        //    var responce1 = await Services.GetAsync<PageModel<T_DAILY_ATTEND>>(string.Format("/api/DailyAttend/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, searchString)); // Helpers.UserInfo.ModuleCode

        //    if (!string.IsNullOrEmpty(searchString))
        //    {

        //    }
        //    ViewBag.CurrentFilter = searchString;
        //    switch (sortOrder)
        //    {

        //    }
        //    ViewBag.PageSize = pageSize;
        //    int pageNumber = (page ?? 1);
        //    var usersAsIPagedList = new StaticPagedList<T_DAILY_ATTEND>(responce1.Data.value.results, pageNumber, pageSize, responce1.Data.value.rowCount);
           

        //    var ailowcal2 = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll");

        //    List<SelectListItem> all2 = ailowcal2.Data.Select(x => new SelectListItem
        //    {
        //        Value = x.code,
        //        Text = x.name

        //    }).ToList();
        //    ViewBag.AllowCal2 = all2;

        //    return View(usersAsIPagedList);
        //}
    }
}