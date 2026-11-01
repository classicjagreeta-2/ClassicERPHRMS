using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
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
    public class M_MonthController : Controller
    {
        // GET: M_Month
        public async Task<ActionResult> M_Month(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            try
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var responce = await Services.GetAsync<PageModel<M_MONTH>>(string.Format("/api/Month/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, searchString)); // Helpers.UserInfo.ModuleCode
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
                    var usersAsIPagedList = new StaticPagedList<M_MONTH>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
                    //var unitData = await Services.GetAsync<List<M_UNIT>>("/api/unit/GetAll");
                    //foreach (var item in usersAsIPagedList)
                    //{
                    //    var unitName = unitData.Data.FirstOrDefault(u=>u.code==item.unit);
                    //    if (unitName != null)
                    //    {
                    //        item.unit = unitName.code + " : " + unitName.name;
                    //    }
                    //}
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
            try
            {
                int year = DateTime.Now.Year;
                List<M_MONTH> monthList = GetMonthList();
                ViewBag.GetMonthList = monthList;
                List<ModelMaster> day = monthList.Select(x => new ModelMaster
                {
                    Value = x.pK_MONTHCODE,
                    Text = x.pK_MONTHCODE + "-" + x.MonthName,

                }).ToList();
                ViewBag.MonthList = day;
                var unit = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll");
                List<ModelMaster> Lst = unit.Data.Select(x => new ModelMaster
                {
                    Value = x.code,
                    Text = x.code + "-" + x.name

                }).ToList();
                ViewBag.Unit = Lst;
                var ofday = await Services.GetAsync<List<M_WEEKLY_OFF>>("/api/WeeklyOff/GetAll");


                ViewBag.OFD = ofday.Data.Select(x => x.ofF_DAY1).FirstOrDefault();
                ViewBag.OFD2 = ofday.Data.Select(x => x.ofF_DAY2).FirstOrDefault();
                ViewBag.Year = year;

                M_MONTH model = new M_MONTH();
                if (Id > 0)
                {
                    var response = await Services.GetAsync<M_MONTH>("/api/Month/GetByID?id=" + Id);
                    model = response.Data;
                    return View("AddEditModal", model);
                }
                else
                {
                    return View("AddEditModal", model);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(M_MONTH entity)
        {
            try
            {
                entity.useR_NAME_NEW = "";

                entity.useR_ENTDT_NEW = DateTime.Now;

                entity.dtag = "";
                if (entity.paiD_HOLIDAY == null)
                {
                    entity.paiD_HOLIDAY = 0m;
                }
                if (entity.weeK_OFF == null)
                {
                    entity.weeK_OFF = 0m;
                }
                entity.locK_CALCULATE_PAYROLL = "";
                entity.ptype = "S";
                M_montyh_dto _Dto = new M_montyh_dto();
                _Dto.m_month = entity;

                if (entity.id == 0)
                {
                    var responce2 = await Services.GetAsync<M_MONTH>("/api/Month/GetByUnitMonthCode?monthCode=" + entity.pK_MONTHCODE+ "&UNIT="+ entity.unit);
                    if (responce2.Data != null)
                    {
                        TempData["toastrError"] = responce2.Data.pK_MONTHCODE + " Dublicate Data Found";
                        return RedirectToAction("AddEditModal");
                    }
                    else
                    {
                        var responce = await Services.PostAsync<List<M_montyh_dto>>("/api/Month/SaveOrUpdateAll", _Dto);
                        if (responce.IsSuccessStatusCode == true)
                        {
                            TempData["toastrSuccess"] = "Data has been successfully submitted";
                        }
                        else
                        {
                            TempData["toastrError"] = responce.Message;
                        }
                    }
                }
                else
                {
                    var responce = await Services.PostAsync<List<M_montyh_dto>>("/api/Month/SaveOrUpdateAll", _Dto);

                    if (responce.IsSuccessStatusCode == true)
                    {
                        TempData["toastrSuccess"] = "Data has been successfully submitted";
                    }
                    else
                    {
                        TempData["toastrError"] = responce.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["toastrError"] = ex.Message;
            }
            return RedirectToAction("M_Month");
        }

        static List<M_MONTH> GetMonthList()
        {
            int year = DateTime.Now.Year;
            List<M_MONTH> monthList = new List<M_MONTH>();

            for (int month = 1; month <= 12; month++)
            {
                M_MONTH date = new M_MONTH();
                date.pK_MONTHCODE = GetMonthName(year, month);
                date.nuM_DAYS = DateTime.DaysInMonth(year, month);
                date.froM_PERIOD = new DateTime(year, month, 1);
                date.tO_PERIOD = new DateTime(year, month, date.nuM_DAYS);
                date.MonthName = new DateTime(year, month, 1).ToString("MMMM");
                monthList.Add(date);
            }
            return monthList;
        }
        static string GetMonthName(int year, int month)
        {
            // Returns the name of the month based on the month number
            return new DateTime(year, month, 1).ToString("yyyyMM");
        }
    }
}