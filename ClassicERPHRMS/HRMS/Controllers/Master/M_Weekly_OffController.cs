using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using HRMS.Models;
using HRMS.Models.Master;
using PagedList;
using static HRMS.Models.Model_Common;

namespace HRMS.Controllers.Master
{
    public class M_Weekly_OffController : Controller
    {
        // GET: M_Weekly_Off

        public async Task<ActionResult> M_Weekly_Off(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {

            try
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var responce = await Services.GetAsync<PageModel<M_WEEKLY_OFF>>(string.Format("/api/WeeklyOff/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, searchString)); // Helpers.UserInfo.ModuleCode
                if (responce.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(searchString))
                    {

                    }
                    ViewBag.CurrentFilter = searchString;
                    switch (sortOrder)
                    {

                    }
                    ViewBag.PageSize = pageSize;
                    int pageNumber = (page ?? 1);
                    var usersAsIPagedList = new StaticPagedList<M_WEEKLY_OFF>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
                    var unitData = await Services.GetAsync<List<M_UNIT>>("/api/unit/GetAll");
                    foreach (var item in usersAsIPagedList)
                    {


                        var unitName = unitData.Data.FirstOrDefault(u => u.code == item.unit);
                        if (unitName != null)
                        {
                            item.unit = unitName.code + " : " + unitName.name;
                        }
                    }
                    var attendence = await Services.GetAsync<List<M_ATTEND_DAYS_TYPE>>("/api/AttendDaysType/GetAll");
                    foreach (var item in usersAsIPagedList)
                    {


                        var attendencename = attendence.Data.FirstOrDefault(u => u.pK_ATTEND_CODE == item.fK_ATTEND_CODE);
                        if (attendencename != null)
                        {
                            item.fK_ATTEND_CODE = attendencename.pK_ATTEND_CODE + " : " + attendencename.description;
                        }
                    }
                    return View(usersAsIPagedList);
                }
                else
                {
                    TempData["toastrError"] = responce.Message;
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<ActionResult> AddEditModal(int Id = 0)
        {
            //ViewBag.DropdownListDay_Mode = Services.GetAsync<List<M_ENUM_PARA>>("api/PAY/PAY_Master/GetData_M_ENUM_PARA").Result.Data;
            M_WEEKLY_OFF model = new M_WEEKLY_OFF();
         
            var OffDayList = new List<ModelMaster>
            {
            new ModelMaster { Value = "0", Text = "Sunday" },
            new ModelMaster { Value = "1", Text = "Monday" },
            new ModelMaster { Value = "2", Text = "Tuesday" },
            new ModelMaster { Value = "3", Text = "Wednesday" },
            new ModelMaster { Value = "4", Text = "Thursday" },
            new ModelMaster { Value = "5", Text = "Friday" },
            new ModelMaster { Value = "6", Text = "Saturday" },

            };
            List<ModelMaster> day = OffDayList.Select(x => new ModelMaster
            {
                Value = x.Value,
                Text = x.Value + "-" + x.Text

            }).ToList();
            ViewBag.DayList = day;

            var unit = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll");
            List<ModelMaster> uu = unit.Data.Select(x => new ModelMaster
            {
                Value = x.code,
                Text = x.code + "-" + x.name

            }).ToList();
            ViewBag.Unit = uu;

            var attend = await Services.GetAsync<List<M_ATTEND_DAYS_TYPE>>("/api/AttendDaysType/GetAll");
            attend.Data.Where(x => x.weeklY_OFF == true).OrderBy(x => x.pK_ATTEND_CODE).ToList();
            List<ModelMaster> Lst = attend.Data.Select(x => new ModelMaster
            {
                Value = x.pK_ATTEND_CODE,
                Text = x.pK_ATTEND_CODE + "-" + x.description

            }).ToList();
            ViewBag.Attend = Lst;
            if (Id > 0)
            {
                var response = await Services.GetAsync<M_WEEKLY_OFF>("/api/WeeklyOff/GetByID?id=" + Id);
                model = response.Data;
                return View("AddEditModal", model);
            }
            else
            {
                return View("AddEditModal", model);
            }
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(M_WEEKLY_OFF entity)
        {
            try
            {
                entity.useR_NAME_NEW = "";
                entity.useR_ENTDT_NEW = DateTime.Now;
                entity.dtag = "";
                entity.fK_EMPCODE = "";
                entity.fK_CAT_CODE = "";
                entity.fK_EMP_TYPE = "";
                entity.fK_DEPT_CODE = "";
                if (entity.ofF_DAY2 == null)
                {
                    entity.ofF_DAY2 = "";
                }
                

                entity.ptype = "S";
                M_WEEKLY_OFF_Dto _Dto = new M_WEEKLY_OFF_Dto();
                _Dto.m_weeklyoff = entity;

                var responce = await Services.PostAsync<List<M_WEEKLY_OFF_Dto>>("/api/WeeklyOff/SaveOrUpdateAll", _Dto);
                //var responce = await Services.PostAsync<M_WEEKLY_OFF>("/api/WeeklyOff/SaveOrUpdateAll", entity);
                if (responce.IsSuccessStatusCode == true)
                {
                    TempData["toastrSuccess"] = "Data has been successfully submitted";
                }
                else
                {
                    TempData["toastrError"] = responce.Message;
                }
            }
            catch (Exception ex)
            {
                TempData["toastrError"] = ex.Message;
            }
            return RedirectToAction("M_Weekly_Off");
        }
    }
}