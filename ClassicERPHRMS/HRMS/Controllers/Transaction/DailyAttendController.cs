
//using CrystalDecisions.ReportAppServer.CommonControls;
//using CrystalDecisions.Shared;
using HRMS.Models;
using HRMS.Models.Master;
using HRMS.Models.TRN;
using Org.BouncyCastle.Ocsp;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using static HRMS.Models.Model_Common;
using static HRMS_VIEWMODEL24.Payroll_Common;
using static iTextSharp.text.pdf.AcroFields;

namespace HRMS.Controllers.Transaction
{
    public class DailyAttendController : Controller
    {
        // GET: DailyAttend       
        public async Task<ActionResult> SelectMonth(string mcode)
        {
            try
            {
                string yearNow = DateTime.Now.ToString("yyyy");
                var response = await Services.GetAsync<List<M_MONTH>>("/api/Month/GetMonthByCurrentYear");
                List<SelectListItem> monthList = response.Data.Select(x => new SelectListItem
                {
                    Text = DateTime.ParseExact(x.pK_MONTHCODE, "yyyyMM", CultureInfo.InvariantCulture).ToString("MMM") + " " + DateTime.ParseExact(x.pK_MONTHCODE, "yyyyMM", CultureInfo.InvariantCulture).ToString("yyyy"),
                    Value = x.pK_MONTHCODE.ToString()
                }).ToList();
                ViewBag.monthList = monthList;
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ActionResult> DailyAttend(string sortOrder, string currentFilter, string Unit, string month, string year,
                                                                string MONTHCODE, int? page = 1, int pageSize = 25)
        {
            try
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var url = string.Format("/api/DailyAttend/GetByMonthCode?page={0}&pageSize={1}&monthcode={2}", page, pageSize, MONTHCODE);
                if (!string.IsNullOrEmpty(Unit))
                {
                    url = string.Format("/api/DailyAttend/GetByMonthCode?page={0}&pageSize={1}&monthcode={2}&search={3}", page, pageSize, MONTHCODE, Unit);
                }
                var response = await Services.GetAsync<PageModel<T_DAILY_ATTEND>>(url); // Helpers.UserInfo.ModuleCode
                if (response.IsSuccessStatusCode)
                {
                    //if (!string.IsNullOrEmpty(searchString))
                    //{

                    //}
                    //ViewBag.CurrentFilter = searchString;
                    //switch (sortOrder)
                    //{

                    //}
                    ViewBag.PageSize = pageSize;
                    int pageNumber = (page ?? 1);
                    var usersAsIPagedList = new StaticPagedList<T_DAILY_ATTEND>(response.Data.value.results, pageNumber, pageSize, response.Data.value.rowCount);
                    foreach (var item in usersAsIPagedList)
                    {
                        item._empCode = item.FK_EMPCODE.Substring(0, item.FK_EMPCODE.IndexOf(':'));
                        item._name = item.FK_EMPCODE.Substring(@item.FK_EMPCODE.IndexOf(':') + 2);
                    }

                    //ViewBag.Month = string.IsNullOrEmpty(month) ? DateTime.Now.Month.ToString("0#") : month;
                    //ViewBag.Year = string.IsNullOrEmpty(year) ? DateTime.Now.Year.ToString("0#") : year;

                    var unitddl = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll");
                    List<ModelMaster> Lst = unitddl.Data.Select(x => new ModelMaster
                    {
                        Value = x.code,
                        Text = x.code + "-" + x.name
                    }).ToList();
                    ViewBag.Unit = Lst;

                    //var monthddl = await Services.GetAsync<List<M_MONTH>>("/api/Month/GetAll");
                    //List<ModelMaster> Ls2t = monthddl.Data.Select(x => new ModelMaster
                    //{
                    //    Value = x.pK_MONTHCODE,
                    //    Text = x.pK_MONTHCODE + "-" + x.MonthName,
                    //    year = x.pK_MONTHCODE.Substring(0, 4),
                    //    month = x.pK_MONTHCODE.Substring(4, 2),

                    //}).ToList();
                    //ViewBag.year = Ls2t;
                    //ViewBag.Month = Ls2t;
                    return View(usersAsIPagedList);
                }
                else
                {
                    TempData["toastrError"] = response.Message;
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<JsonResult> GetMonthCodeDropDown(string UNIT, string searchString = null, int? page = 1, int pageSize = 25)
        {
            var url = (dynamic)null;            
            if (!string.IsNullOrEmpty(UNIT))
            {
                //UNIT = "";
                url = string.Format("/api/Month/GetByPage?Unit={0}&page={1}&pageSize={2}", UNIT, page, pageSize);
                if (!string.IsNullOrEmpty(searchString))
                {
                    url = string.Format("/api/Month/GetByPage?Unit={0}&search={1}&page={2}&pageSize={3}", UNIT, searchString, page, pageSize);
                }
                var response = await Services.GetAsync<PageModel<M_MONTH>>(url);
                foreach (var item in response.Data.value.results)
                {
                    item.MonthName = DateTime.ParseExact(item.pK_MONTHCODE, "yyyyMM", CultureInfo.InvariantCulture).ToString("MMM") + " " + DateTime.ParseExact(item.pK_MONTHCODE, "yyyyMM", CultureInfo.InvariantCulture).ToString("yyyy");
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> GetUnitDropdown(string searchString = null, int? page = 1, int pageSize = 5)
        {
            var response = await Services.GetAsync<PageModel<M_UNIT>>(string.Format("/api/Unit/GetByPage?page={0}&pageSize={1}", page, pageSize, Utility.SessionHelper.GetUser().getUserName));
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> AddEmployeeEntry(string EmpId, string mcode, string Id, string monthcode)
        {
            var empcode = EmpId;
            var today = DateTime.Now;

            return View();
        }
        // Add Employee Daily Attendance
        public async Task<ActionResult> AddEditEmployeeDailyAttendance(string EmpId, string mcode, string monthcode, string Unit = "", string paytype = "P", string ptype = "S")
        {
            var response = (dynamic)null;
            var response2 = (dynamic)null;
            if (EmpId == "Select")
            {
                EmpId = "";
            }
            if (EmpId.Any())
            {
                response = await Services.GetAsync<List<T_DAILY_ATTEND>>(string.Format("/api/DailyAttend/DailyManualAttendanceGenerator?Unit={0}&Monthcode={1}&Empcode={2}&ptype={3}", Unit, mcode, EmpId, ptype));
                //response2 = await Services.GetAsync<List<T_DAILY_ATTEND>>(string.Format("/api/Attend/GetCalculationByCode?code={0}&Monthcode={1}&location={2}&paytype={3}&Ptype={4}", EmpId, mcode, Unit, "P", ptype));
            }
            else
            {
                response = await Services.GetAsync<List<T_DAILY_ATTEND>>(string.Format("/api/DailyAttend/DailyManualAttendanceGenerator?Unit={0}&Monthcode={1}&ptype={2}", Unit, mcode, ptype));
                //response2 = await Services.GetAsync<List<T_DAILY_ATTEND>>(string.Format("/api/Attend/GetCalculationByCode?Monthcode={1}&location={2}&paytype={3}&Ptype={4}", mcode, Unit, "P", ptype));
            }
            // Helpers.UserInfo.ModuleCode
            List<T_DAILY_ATTEND> entity = response.Data;

            T_DAILY_ATTEND_Dto _Dto = new T_DAILY_ATTEND_Dto();
            _Dto.t_dailyattend = entity;
            var result = await Services.PostAsync<List<T_DAILY_ATTEND_Dto>>("/api/DailyAttend/SaveOrUpdateAll", _Dto);
            if (result.Data.Count > 0)
            {
                if (EmpId.Any())
                {
                    //response = await Services.GetAsync<List<T_DAILY_ATTEND>>(string.Format("/api/DailyAttend/DailyManualAttendanceGenerator?Unit={0}&Monthcode={1}&Empcode={2}&ptype={3}", Unit, mcode, EmpId, ptype));
                    response2 = await Services.GetAsync<T_MONTHLY_ATTEND>(string.Format("/api/Attend/GetCalculationByCode?code={0}&Monthcode={1}&location={2}&paytype={3}&Ptype={4}", EmpId, mcode, Unit, "P", ptype));
                }
                else
                {
                    //response = await Services.GetAsync<List<T_DAILY_ATTEND>>(string.Format("/api/DailyAttend/DailyManualAttendanceGenerator?Unit={0}&Monthcode={1}&ptype={3}", Unit, mcode, ptype));
                    response2 = await Services.GetAsync<T_MONTHLY_ATTEND>(string.Format("/api/Attend/GetCalculationByAll?Monthcode={0}&location={1}&paytype={2}&Ptype={3}", mcode, Unit, "P", ptype));
                }
                if (result.IsSuccessStatusCode == true)
                {
                    TempData["toastrSuccess"] = "Data has been successfully submitted";
                }
                else
                {
                    TempData["toastrError"] = result.Message;
                }
            }
            else
            {
                TempData["toastrError"] = "Already Attendance found,No Attendance was Generated";
            }
            //var responce = await Services.PostAsync<M_WEEKLY_OFF>("/api/WeeklyOff/SaveOrUpdateAll", entity);

            return RedirectToAction("DailyAttend");
        }

        public async Task<ActionResult> EmployeesList(string Unit, string EmpId, string mcode, string Month, string Year, string Id, int? page = 1, int pageSize = 31)
        {

            if (string.IsNullOrEmpty(mcode))
            {
                mcode = string.Format("{0}{1}", Year, Month);
            }
            if (Month == null && Year == null)
            {
                //DateTime.ParseExact(item.pK_MONTHCODE, "yyyyMM", CultureInfo.InvariantCulture).ToString("MMM")
                Month = DateTime.ParseExact(mcode, "yyyyMM", CultureInfo.InvariantCulture).ToString("MM");
                Year = DateTime.ParseExact(mcode, "yyyyMM", CultureInfo.InvariantCulture).ToString("yyyy");

            }
            ViewBag.MonthCodeId = mcode;
            ViewBag.MonthDigit = Month;
            ViewBag.YearDigit = Year;
            ViewBag.EmpId = EmpId;
            var response = await Services.GetAsync<PageModel<T_DAILY_ATTEND>>(string.Format("/api/DailyAttend/GetByMonthCode?page={0}&pageSize={1}&monthcode={2}&EmpId={3}&search={4}", page, pageSize, mcode, EmpId, Unit));
            int pageNumber = (page ?? 1);
            var responseData = new StaticPagedList<T_DAILY_ATTEND>(response.Data.value.results, pageNumber, pageSize, response.Data.value.rowCount)
                                        .OrderBy(b => b.EDATE).ToList();

            var AttendType = await Services.GetAsync<List<M_ATTEND_DAYS_TYPE>>("/api/AttendDaysType/GetAll");
            var EmpDetails = await Services.GetAsync<M_EMPMAS>(string.Format("/api/Employee/GetByID?id={0}", EmpId));
            var gender = EmpDetails.Data.sex.ToString();
            //var unitName = EmpDetails.Data.unit.ToString();
            if (gender == "M")
            {
                //ViewBag.AttendType = AttendType.Data.Where(x => x.pK_ATTEND_CODE != "M" && x.unit == unitName);
                ViewBag.AttendType = AttendType.Data.Where(x => x.pK_ATTEND_CODE != "M");
            }
            else
            {
                //ViewBag.AttendType = AttendType.Data.Where(x=> x.unit == unitName);
                ViewBag.AttendType = AttendType.Data;
            }
            List<T_DAILY_ATTEND> t_DAILY_ATTEND = new List<T_DAILY_ATTEND>();
            ViewBag.DisplayName = EmpDetails.Data.pK_EMPCODE + "-" + EmpDetails.Data.fname + "-" + EmpDetails.Data.mname + "-" + EmpDetails.Data.lname;
            try
            {
                string yearNow = DateTime.Now.ToString("yyyy");
                var responseMonth = await Services.GetAsync<List<M_MONTH>>("/api/Month/GetMonthByCurrentYear");
                List<SelectListItem> monthList = responseMonth.Data.Select(x => new SelectListItem
                {
                    Text = DateTime.ParseExact(x.pK_MONTHCODE, "yyyyMM", CultureInfo.InvariantCulture).ToString("MMM") + " " + DateTime.ParseExact(x.pK_MONTHCODE, "yyyyMM", CultureInfo.InvariantCulture).ToString("yyyy"),
                    Value = x.pK_MONTHCODE.ToString()
                }).ToList();
                ViewBag.monthList = monthList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(responseData);
        }
        [HttpPost]
        public async Task<ActionResult> BulkStoreOrEditEmployeesList(List<T_DAILY_ATTEND> t_DAILY_ATTEND)
        {
            var response = (dynamic)null;
            //var JstReturn = (dynamic)null;
            var Parameter = t_DAILY_ATTEND.Select(x => new { x.FK_EMPCODE, x.FK_MONTHCODE,x.UNIT }).FirstOrDefault();
            DateTime dtime = DateTime.Now;
            try
            {
                List<T_DAILY_ATTEND> t_DAILY_ATTEND_List = new List<T_DAILY_ATTEND>();

                foreach (var item in t_DAILY_ATTEND)
                {
                    //var value = item.FK_ATTEND_CODE;
                    //T_DAILY_ATTEND t_DAILY_ATTEND1 = servicesObj.SubmitGet<T_DAILY_ATTEND>("api/PAY/PAY_Master/Edit_T_DAILY_ATTEND?id=" + item.ID.ToString());
                    T_DAILY_ATTEND t_DAILY_ATTEND1 = new T_DAILY_ATTEND();
                    t_DAILY_ATTEND1.UNIT = item.UNIT;
                    t_DAILY_ATTEND1.ID = item.ID;
                    t_DAILY_ATTEND1.EDATE = item.EDATE;
                    t_DAILY_ATTEND1.FK_SHIFT_CODE = item.FK_SHIFT_CODE;
                    t_DAILY_ATTEND1.FK_EMPCODE = item.FK_EMPCODE.Substring(0, item.FK_EMPCODE.IndexOf(':')).Trim();
                    t_DAILY_ATTEND1.IN_TIME = item.IN_TIME;
                    t_DAILY_ATTEND1.OUT_TIME = item.OUT_TIME;
                    t_DAILY_ATTEND1.TOT_HOURS_WORKED = item.TOT_HOURS_WORKED;
                    t_DAILY_ATTEND1.REMARKS = item.REMARKS != null ? item.REMARKS : "";
                    t_DAILY_ATTEND1.PTYPE = item.PTYPE;

                    t_DAILY_ATTEND1.DTAG = "d";
                    t_DAILY_ATTEND1.USER_NAME_NEW = "UN";
                    t_DAILY_ATTEND1.USER_NAME = "U";
                    //t_DAILY_ATTEND1.LOCA = "kol";
                    t_DAILY_ATTEND1.USER_ENTDT = Convert.ToDateTime(dtime);
                    t_DAILY_ATTEND1.USER_ENTDT_NEW = Convert.ToDateTime(dtime);
                    t_DAILY_ATTEND1.OS_USER = "O";
                    t_DAILY_ATTEND1.TERMINAL_ID = "T";
                    //t_DAILY_ATTEND1.UNIT = "unit";
                    t_DAILY_ATTEND1.FK_MONTHCODE = item.FK_MONTHCODE;
                    t_DAILY_ATTEND1.HOLIDAY = item.HOLIDAY;

                    t_DAILY_ATTEND1.ATTEND = 0;
                    t_DAILY_ATTEND1.LEAVE = 0;
                    t_DAILY_ATTEND1.WEEKLY_OFF = 0;
                    t_DAILY_ATTEND1.ABSENT = 0;
                    t_DAILY_ATTEND1.LATE_TIME = 0;

                    t_DAILY_ATTEND1.FK_ATTEND_CODE = item.FK_ATTEND_CODE;
                    if (item.FK_ATTEND_CODE == "P")
                    {
                        t_DAILY_ATTEND1.ATTEND = 1;
                        t_DAILY_ATTEND1.LATE_TIME = Convert.ToDecimal(item.LATE_TIME);
                    }
                    if (item.FK_ATTEND_CODE == "HF")
                    {
                        t_DAILY_ATTEND1.ATTEND = (Decimal)0.50;
                        t_DAILY_ATTEND1.LATE_TIME = Convert.ToDecimal(item.LATE_TIME);
                    }
                    if (item.FK_ATTEND_CODE == "S" || item.FK_ATTEND_CODE == "M" || item.FK_ATTEND_CODE == "PH" || item.FK_ATTEND_CODE == "L")
                    {
                        t_DAILY_ATTEND1.LEAVE = 1;
                        t_DAILY_ATTEND1.TOT_HOURS_WORKED = 0;
                    }
                    if (item.FK_ATTEND_CODE == "W")
                    {
                        t_DAILY_ATTEND1.WEEKLY_OFF = 1;
                        t_DAILY_ATTEND1.TOT_HOURS_WORKED = 0;
                    }
                    if (item.FK_ATTEND_CODE == "A")
                    {
                        t_DAILY_ATTEND1.ABSENT = 1;
                        t_DAILY_ATTEND1.TOT_HOURS_WORKED = 0;
                    }
                    t_DAILY_ATTEND_List.Add(t_DAILY_ATTEND1);
                    // Process the key-value data here
                }


                var result3 = t_DAILY_ATTEND_List.Select(d => new
                {
                    ID = d.ID,
                    FK_ATTEND_CODE = d.FK_ATTEND_CODE,
                    EDATE = d.EDATE,
                    new_status =

                        (((DateTime.Parse(d.EDATE.ToString()).DayOfWeek == DayOfWeek.Saturday
                        && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -1, d.FK_EMPCODE) == "L"
                        && (UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "L"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "S"
                               || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "A"))

                        || (DateTime.Parse(d.EDATE.ToString()).DayOfWeek == DayOfWeek.Sunday
                        && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -2, d.FK_EMPCODE) == "L"
                        && (UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "L"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "S"
                               || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "A")) ? "L" :

                        (((DateTime.Parse(d.EDATE.ToString()).DayOfWeek == DayOfWeek.Saturday
                        && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -1, d.FK_EMPCODE) == "S"
                        && (UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "L"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "S"
                               || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "A"))

                        || (DateTime.Parse(d.EDATE.ToString()).DayOfWeek == DayOfWeek.Sunday
                        && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -2, d.FK_EMPCODE) == "S"
                        && (UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "L"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "S"
                               || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "A")) ? "S" :

                        (((DateTime.Parse(d.EDATE.ToString()).DayOfWeek == DayOfWeek.Saturday
                        && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -1, d.FK_EMPCODE) == "A"
                        && (UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "L"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "S"
                               || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "A"))

                        || (DateTime.Parse(d.EDATE.ToString()).DayOfWeek == DayOfWeek.Sunday
                        && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -2, d.FK_EMPCODE) == "A"
                        && (UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "L"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "S"
                               || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "A")) ? "A" :

                        (((DateTime.Parse(d.EDATE.ToString()).DayOfWeek == DayOfWeek.Friday
                        && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 0, d.FK_EMPCODE) == "PH" && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -1, d.FK_EMPCODE) == "A"
                        && (UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 3, d.FK_EMPCODE) == "L"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 3, d.FK_EMPCODE) == "S"
                               || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 3, d.FK_EMPCODE) == "A"))

                        || (DateTime.Parse(d.EDATE.ToString()).DayOfWeek == DayOfWeek.Monday
                        && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 0, d.FK_EMPCODE) == "PH" && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -3, d.FK_EMPCODE) == "A"
                        && (UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "L"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "S"
                               || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "A")) ? "A" :

                        (((DateTime.Parse(d.EDATE.ToString()).DayOfWeek == DayOfWeek.Friday
                        && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 0, d.FK_EMPCODE) == "PH" && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -1, d.FK_EMPCODE) == "S"
                        && (UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 3, d.FK_EMPCODE) == "L"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 3, d.FK_EMPCODE) == "S"
                               || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 3, d.FK_EMPCODE) == "S"))

                        || (DateTime.Parse(d.EDATE.ToString()).DayOfWeek == DayOfWeek.Monday
                        && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 0, d.FK_EMPCODE) == "PH" && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -3, d.FK_EMPCODE) == "S"
                        && (UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "L"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "S"
                               || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "A")) ? "S" :

                        (((DateTime.Parse(d.EDATE.ToString()).DayOfWeek == DayOfWeek.Friday
                        && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 0, d.FK_EMPCODE) == "PH" && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -1, d.FK_EMPCODE) == "M"
                        && (UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 3, d.FK_EMPCODE) == "L"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 3, d.FK_EMPCODE) == "S"
                               || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 3, d.FK_EMPCODE) == "M"))

                        || (DateTime.Parse(d.EDATE.ToString()).DayOfWeek == DayOfWeek.Monday
                        && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 0, d.FK_EMPCODE) == "PH" && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -3, d.FK_EMPCODE) == "M"
                        && (UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "L"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "S"
                               || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "A")) ? "M" :

                        (((DateTime.Parse(d.EDATE.ToString()).DayOfWeek == DayOfWeek.Saturday
                        && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -1, d.FK_EMPCODE) == "M"
                        && (UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "L"
                         || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "M"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "S"
                               || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "M"))

                        || (DateTime.Parse(d.EDATE.ToString()).DayOfWeek == DayOfWeek.Sunday
                        && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -2, d.FK_EMPCODE) == "M"
                        && (UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "L"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "S"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "M"
                               || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "A")) ? "M" :

                        (((DateTime.Parse(d.EDATE.ToString()).DayOfWeek == DayOfWeek.Saturday
                        && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -1, d.FK_EMPCODE) == "PH" && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -2, d.FK_EMPCODE) == "M"
                        && (UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "L"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "S"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "M"
                               || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "M"))

                        || (DateTime.Parse(d.EDATE.ToString()).DayOfWeek == DayOfWeek.Sunday
                        && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -2, d.FK_EMPCODE) == "PH" && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -3, d.FK_EMPCODE) == "M"
                        && (UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "L"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "S"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "M"
                               || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "A")) ? "M" :

                               (((DateTime.Parse(d.EDATE.ToString()).DayOfWeek == DayOfWeek.Saturday
                        && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -1, d.FK_EMPCODE) == "PH" && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -2, d.FK_EMPCODE) == "S"
                        && (UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "L"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "S"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "M"
                               || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "S"))

                        || (DateTime.Parse(d.EDATE.ToString()).DayOfWeek == DayOfWeek.Sunday
                        && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -2, d.FK_EMPCODE) == "PH" && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -3, d.FK_EMPCODE) == "S"
                        && (UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "L"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "S"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "M"
                               || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "A")) ? "S" :

                               (((DateTime.Parse(d.EDATE.ToString()).DayOfWeek == DayOfWeek.Saturday
                        && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -1, d.FK_EMPCODE) == "PH" && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -2, d.FK_EMPCODE) == "L"
                        && (UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "L"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "S"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "M"
                               || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "M"))

                        || (DateTime.Parse(d.EDATE.ToString()).DayOfWeek == DayOfWeek.Sunday
                        && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -2, d.FK_EMPCODE) == "PH" && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -3, d.FK_EMPCODE) == "L"
                        && (UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "L"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "S"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "M"
                               || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "A")) ? "M" :

                               (((DateTime.Parse(d.EDATE.ToString()).DayOfWeek == DayOfWeek.Saturday
                        && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -1, d.FK_EMPCODE) == "PH" && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -2, d.FK_EMPCODE) == "A"
                        && (UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "L"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "S"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "M"
                               || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 2, d.FK_EMPCODE) == "A"))

                        || (DateTime.Parse(d.EDATE.ToString()).DayOfWeek == DayOfWeek.Sunday
                        && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -2, d.FK_EMPCODE) == "PH" && UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, -3, d.FK_EMPCODE) == "A"
                        && (UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "L"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "S"
                           || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "M"
                               || UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, 1, d.FK_EMPCODE) == "A")) ? "A" : null))))))))))))))))))))))
                }).ToList();
                List<T_DAILY_ATTEND> t_DAILY_ATTENDs = new List<T_DAILY_ATTEND>();

                //var Parameter = t_DAILY_ATTEND_List.Select(x => new { x.FK_EMPCODE, x.FK_MONTHCODE }).FirstOrDefault();

                foreach (var item in result3)
                {
                    var employee = t_DAILY_ATTEND_List.FirstOrDefault(e => e.ID == item.ID);

                    if (employee != null)
                    {
                        if (item.new_status != null)
                        {
                            employee.ATTEND = 0;
                            employee.LEAVE = 0;
                            employee.WEEKLY_OFF = 0;
                            employee.ABSENT = 0;
                            employee.LATE_TIME = 0;

                            employee.FK_ATTEND_CODE = item.new_status;
                            if (employee.FK_ATTEND_CODE == "P")
                            {
                                employee.ATTEND = 1;
                                employee.LATE_TIME = Convert.ToDecimal(employee.LATE_TIME);
                            }
                            if (employee.FK_ATTEND_CODE == "HF")
                            {
                                employee.ATTEND = (Decimal)0.50;
                                employee.LATE_TIME = Convert.ToDecimal(employee.LATE_TIME);
                            }
                            if (employee.FK_ATTEND_CODE == "S" || item.FK_ATTEND_CODE == "M" || item.FK_ATTEND_CODE == "PH" || item.FK_ATTEND_CODE == "L")
                            {
                                employee.LEAVE = 1;
                            }
                            if (employee.FK_ATTEND_CODE == "W")
                            {
                                employee.WEEKLY_OFF = 1;
                            }
                            if (employee.FK_ATTEND_CODE == "A")
                            {
                                employee.ABSENT = 1;
                            }
                            t_DAILY_ATTENDs.Add(employee);
                        }
                        else
                        {
                            t_DAILY_ATTENDs.Add(employee);
                        }
                    }
                }
                T_DAILY_ATTEND_Dto _Dto = new T_DAILY_ATTEND_Dto();
                _Dto.t_dailyattend = t_DAILY_ATTENDs;

                var responce = await Services.PostAsync<List<T_DAILY_ATTEND_Dto>>("/api/DailyAttend/BulkUpdateAll", _Dto);
                var response2 = (dynamic)null;

                if (responce.IsSuccessStatusCode == true)
                {
                    response2 = await Services.GetAsync<T_MONTHLY_ATTEND>(string.Format("/api/Attend/GetCalculationByCode?code={0}&Monthcode={1}&location={2}&paytype={3}&Ptype={4}", Parameter.FK_EMPCODE, Parameter.FK_MONTHCODE, Parameter.UNIT, "P", "S"));
                
                    TempData["toastrSuccess"] = "Data has been successfully submitted";
                    return RedirectToAction("EmployeesList", new { mcode = Parameter.FK_MONTHCODE, EmpId = Parameter.FK_EMPCODE.Substring(0, Parameter.FK_EMPCODE.IndexOf(':')).Trim() });
                }
                else
                {
                    TempData["toastrError"] = responce.Message;

                    //return RedirectToAction("EmployeesList");
                    return RedirectToAction("EmployeesList", new { mcode = Parameter.FK_MONTHCODE, EmpId = Parameter.FK_EMPCODE.Substring(0, Parameter.FK_EMPCODE.IndexOf(':')).Trim() });
                }
            }
            catch (Exception ex)
            {
                TempData["toastrError"] = ex.Message;

                //return RedirectToAction("EmployeesList");
                return RedirectToAction("EmployeesList", new { mcode = Parameter.FK_MONTHCODE, Id = Parameter.FK_EMPCODE.Substring(0, Parameter.FK_EMPCODE.IndexOf(':')).Trim() });

            }
        }

        public string UDF(List<T_DAILY_ATTEND> t_DAILY_ATTEND_List, int AUTOID, DateTime? EDATE, int number, string EmpId)
        {
            var FK_ATTEND_CODE = t_DAILY_ATTEND_List
                .Where(a => a.EDATE == EDATE?.Date.AddDays(number))
                .Select(a => a.FK_ATTEND_CODE)
                .FirstOrDefault();
            return FK_ATTEND_CODE;
        }
        [HttpPost]
        public async Task<ActionResult> BulkStoreOrEditEmployeesList1(List<T_DAILY_ATTEND> t_DAILY_ATTEND)
        {
            var response = (dynamic)null;
            //var JstReturn = (dynamic)null;
            var Parameter = t_DAILY_ATTEND.Select(x => new { x.FK_EMPCODE, x.FK_MONTHCODE }).FirstOrDefault();
            DateTime dtime = DateTime.Now;
            try
            {
                List<T_DAILY_ATTEND> t_DAILY_ATTEND_List = t_DAILY_ATTEND.Select(item => new T_DAILY_ATTEND
                {
                    UNIT = item.UNIT,
                    ID = item.ID,
                    EDATE = item.EDATE,
                    FK_SHIFT_CODE = item.FK_SHIFT_CODE,
                    FK_EMPCODE = item.FK_EMPCODE.Substring(0, item.FK_EMPCODE.IndexOf(':')).Trim(),
                    IN_TIME = item.IN_TIME,
                    OUT_TIME = item.OUT_TIME,
                    TOT_HOURS_WORKED = item.TOT_HOURS_WORKED,
                    REMARKS = item.REMARKS ?? "",
                    PTYPE = item.PTYPE,
                    DTAG = "d",
                    USER_ENTDT = dtime,
                    USER_ENTDT_NEW = dtime,
                    OS_USER = "O",
                    TERMINAL_ID = "T",
                    FK_MONTHCODE = item.FK_MONTHCODE,
                    HOLIDAY = item.HOLIDAY,
                    ATTEND = 0,
                    LEAVE = 0,
                    WEEKLY_OFF = 0,
                    ABSENT = 0,
                    LATE_TIME = 0,
                    FK_ATTEND_CODE = item.FK_ATTEND_CODE
                }).ToList();

                foreach (var item in t_DAILY_ATTEND_List)
                {
                    SetAttendanceStatus(item);
                }
                var result3 = t_DAILY_ATTEND_List.Select(d => new
                {
                    ID = d.ID,
                    FK_ATTEND_CODE = d.FK_ATTEND_CODE,
                    EDATE = d.EDATE,
                    new_status = GetAttendanceStatus(d, t_DAILY_ATTEND_List)
                }).ToList();
                List<T_DAILY_ATTEND> updatedList = UpdateAttendanceList(result3, t_DAILY_ATTEND_List);

                T_DAILY_ATTEND_Dto _Dto = new T_DAILY_ATTEND_Dto();
                _Dto.t_dailyattend = updatedList;

                var responce = await Services.PostAsync<List<T_DAILY_ATTEND_Dto>>("/api/DailyAttend/BulkUpdateAll", _Dto);

                if (responce.IsSuccessStatusCode == true)
                {
                    TempData["toastrSuccess"] = "Data has been successfully submitted";
                    return RedirectToAction("EmployeesList", new { mcode = Parameter.FK_MONTHCODE, EmpId = Parameter.FK_EMPCODE.Substring(0, Parameter.FK_EMPCODE.IndexOf(':')).Trim() });
                }
                else
                {
                    TempData["toastrError"] = responce.Message;
                    return RedirectToAction("EmployeesList", new { mcode = Parameter.FK_MONTHCODE, EmpId = Parameter.FK_EMPCODE.Substring(0, Parameter.FK_EMPCODE.IndexOf(':')).Trim() });
                }
            }
            catch (Exception ex)
            {
                TempData["toastrError"] = ex.Message;
                return RedirectToAction("EmployeesList", new { mcode = Parameter.FK_MONTHCODE, Id = Parameter.FK_EMPCODE.Substring(0, Parameter.FK_EMPCODE.IndexOf(':')).Trim() });
            }
        }
        private void SetAttendanceStatus(T_DAILY_ATTEND item)
        {
            if (item.FK_ATTEND_CODE == "P")
            {
                item.ATTEND = 1;
                item.LATE_TIME = Convert.ToDecimal(item.LATE_TIME);
            }
            else if (item.FK_ATTEND_CODE == "HF")
            {
                item.ATTEND = 0.50m;
                item.LATE_TIME = Convert.ToDecimal(item.LATE_TIME);
            }
            else if (new[] { "S", "M", "PH", "L" }.Contains(item.FK_ATTEND_CODE))
            {
                item.LEAVE = 1;
            }
            else if (item.FK_ATTEND_CODE == "W")
            {
                item.WEEKLY_OFF = 1;
            }
            else if (item.FK_ATTEND_CODE == "A")
            {
                item.ABSENT = 1;
            }
        }
        private List<T_DAILY_ATTEND> UpdateAttendanceList(IEnumerable<dynamic> result3, List<T_DAILY_ATTEND> t_DAILY_ATTEND_List)
        {
            List<T_DAILY_ATTEND> updatedList = new List<T_DAILY_ATTEND>();

            foreach (var item in result3)
            {
                var employee = t_DAILY_ATTEND_List.FirstOrDefault(e => e.ID == item.ID);
                if (employee != null && item.new_status != null)
                {
                    employee.ATTEND = 0;
                    employee.LEAVE = 0;
                    employee.WEEKLY_OFF = 0;
                    employee.ABSENT = 0;
                    employee.LATE_TIME = 0;
                    employee.FK_ATTEND_CODE = item.new_status;
                    SetAttendanceStatus(employee);
                    updatedList.Add(employee);
                }
                else
                {
                    updatedList.Add(employee);
                }
            }

            return updatedList;
        }
        private string GetAttendanceStatus(T_DAILY_ATTEND d, List<T_DAILY_ATTEND> t_DAILY_ATTEND_List)
        {
            DateTime date = DateTime.Parse(d.EDATE.ToString());
            DayOfWeek day = date.DayOfWeek;

            // Helper function to simplify comparisons
            bool Matches(string dayStatus, int offset, params string[] statuses)
            {
                string status = UDF(t_DAILY_ATTEND_List, d.ID, d.EDATE, offset, d.FK_EMPCODE);
                return statuses.Contains(status);
            }

            // Check conditions based on the day of the week
            if (day == DayOfWeek.Saturday)
            {
                if (Matches("L", -1) && Matches("L", 2, "L", "S", "A")) return "L";
                if (Matches("S", -1) && Matches("S", 2, "L", "S", "A")) return "S";
                if (Matches("A", -1) && Matches("A", 2, "L", "S", "A")) return "A";
            }
            else if (day == DayOfWeek.Sunday)
            {
                if (Matches("L", -2) && Matches("L", 1, "L", "S", "A")) return "L";
                if (Matches("S", -2) && Matches("S", 1, "L", "S", "A")) return "S";
                if (Matches("A", -2) && Matches("A", 1, "L", "S", "A")) return "A";
            }
            else if (day == DayOfWeek.Friday || day == DayOfWeek.Monday)
            {
                int offset = (day == DayOfWeek.Friday) ? -1 : -3;
                int forwardOffset = (day == DayOfWeek.Friday) ? 3 : 1;

                if (Matches("PH", 0) && Matches("A", offset) && Matches("A", forwardOffset, "L", "S", "A")) return "A";
                if (Matches("PH", 0) && Matches("S", offset) && Matches("S", forwardOffset, "L", "S", "S")) return "S";
                if (Matches("PH", 0) && Matches("M", offset) && Matches("M", forwardOffset, "L", "S", "M")) return "M";
            }

            return null; // Return null if no conditions are met
        }


        public string UDF1(List<T_DAILY_ATTEND> t_DAILY_ATTEND_List, int AUTOID, DateTime? EDATE, int number, string EmpId)
        {
            if (t_DAILY_ATTEND_List == null || !EDATE.HasValue || string.IsNullOrEmpty(EmpId))
            {
                return null; // Return null if any required parameter is invalid
            }

            // Find the attendance code based on the provided criteria
            var FK_ATTEND_CODE = t_DAILY_ATTEND_List
                .Where(a => a.EDATE?.Date == EDATE.Value.Date.AddDays(number) && a.FK_EMPCODE == EmpId)
                .Select(a => a.FK_ATTEND_CODE)
                .FirstOrDefault();

            return FK_ATTEND_CODE;
        }
        //Calender

        public async Task<ActionResult> EmployeesCalenderList(string EmpId, string mcode, string Id, int? page = 1, int pageSize = 31)
        {
            //PayModel_Daily_Attend payModel_Daily_Attend = new PayModel_Daily_Attend();
            //ApiResponse<PayMaster_ModelView> payMaster_ModelView = new ApiResponse<PayMaster_ModelView>();
            //ModuleDailyAttendance moduleDailyAttendance = new ModuleDailyAttendance();

            //SpDefinedParamters spDefinedParamters = new SpDefinedParamters();
            //Model_MasterDynamicSp model_MasterDynamicSp = new Model_MasterDynamicSp();

            //var LoginDetails = Utility.GetLoggedInUserDetails();
            //var UserName = LoginDetails.UserId;

            //SearchModelExtra model1 = new SearchModelExtra();
            //model1.IsForDll = true;
            ViewBag.Month = await Services.GetAsync<M_MONTH>(string.Format("/api/Month/GetByMonthCode?search={0}", mcode));

            ViewBag.MonthCodeId = mcode;
            DateTime MonthDate = DateTime.ParseExact(mcode + "01", "yyyyMMdd", CultureInfo.InvariantCulture);
            ViewBag.MonthDate = MonthDate;

            var response = await Services.GetAsync<PageModel<T_DAILY_ATTEND>>(string.Format("/api/DailyAttend/GetByMonthCode?page={0}&pageSize={1}&monthcode={2}&EmpId={3}", page, pageSize, mcode, EmpId));

            int pageNumber = (page ?? 1);
            var responseData = new StaticPagedList<T_DAILY_ATTEND>(response.Data.value.results, pageNumber, pageSize, response.Data.value.rowCount)
                                        .OrderBy(b => b.EDATE).ToList();


            ViewBag.MonthCodeId = mcode;


            //var empId = responseData.Select(b => b.FK_EMPCODE).FirstOrDefault();
            var AttendType = await Services.GetAsync<List<M_ATTEND_DAYS_TYPE>>("/api/AttendDaysType/GetAll");
            var EmpDetails = await Services.GetAsync<M_EMPMAS>(string.Format("/api/Employee/GetByID?id={0}", EmpId));
            var gender = EmpDetails.Data.sex.ToString();
            if (gender == "M")
            {
                ViewBag.AttendType = AttendType.Data.Where(x => x.pK_ATTEND_CODE != "M");
            }
            else
            {
                ViewBag.AttendType = AttendType;
            }
            //var TotalLeaveApplied = payModel_Daily_Attend.payMaster_ModelView
            // .Where(a => a.t_DAILY_ATTEND.FK_EMPCODE == Id && a.t_DAILY_ATTEND.FK_ATTEND_CODE == "C")
            // .Sum(a => a.t_DAILY_ATTEND.LEAVE);

            //return View("~/Views/Attendance/EmployeesList.cshtml", payModel_Daily_Attend);
            List<T_DAILY_ATTEND> t_DAILY_ATTEND = new List<T_DAILY_ATTEND>();

            //ViewBag.DisplayName = "";
            ViewBag.DisplayName = EmpDetails.Data.pK_EMPCODE + "-" + EmpDetails.Data.fname + "-" + EmpDetails.Data.mname + "-" + EmpDetails.Data.lname;


            //.Where(x => x.FK_MONTHCODE == mcode && x.FK_EMPCODE == Id).ToList();

            responseData.ForEach(x =>
            {
                //var emp = EmpList;
                //if (emp != null)
                //{
                //    x.EmpName = $"{emp.PK_EMPCODE}-{emp.FNAME} {emp?.MNAME} {emp.LNAME}";
                //}
                //var rea = reason.FirstOrDefault(s => s.PK_CODE == x.FK_REASON_CODE);
                //if (rea != null)
                //{
                //    x.Reason = $"{rea.DESCRIPTION}";
                //}

                var att = AttendType.Data.FirstOrDefault(s => s.pK_ATTEND_CODE == x.FK_ATTEND_CODE);
                if (att != null)
                {
                    x.Attends = $"{att.description}";
                }
            });

            var data = CalenderDataList(responseData);
            ViewBag.calender = data;

            return View(responseData);
        }

        public List<object> CalenderDataList(List<T_DAILY_ATTEND> t_DAILY_ATTEND)
        {
            List<object> obj = new List<object>();
            foreach (var item in t_DAILY_ATTEND)
            {
                var color = "";
                var className = "";
                switch (item.FK_ATTEND_CODE)
                {
                    case "P":
                        color = "#4dff4d";
                        className = "present-day";
                        break;
                    case "S":
                        color = "#ff4d4d";
                        className = "offday";
                        break;
                    case "M":
                        color = "#ff4da6";
                        className = "maternity-leave";
                        break;
                    case "PH":
                        color = "#ffd633";
                        className = "festival-holiday";
                        break;
                    case "L":
                        color = "#4d79ff";
                        className = "casual-leave";
                        break;
                    case "W":
                        color = "#9933ff";
                        className = "weekly-off";
                        break;
                    case "A":
                        color = "#8585ad";
                        className = "absent";
                        break;
                    default:
                        break;
                }

                obj.Add(new
                {
                    title = $"{(item.IN_TIME ?? "NA")} - {(item.OUT_TIME ?? "NA")}   ({item.FK_ATTEND_CODE})",
                    start = item.EDATE?.AddSeconds(1).ToString("yyyy-MM-ddTHH:mm:ss"),
                    end = item.EDATE?.AddSeconds(1).ToString("yyyy-MM-ddTHH:mm:ss"),
                    details = $"{item.Attends}",
                    color = color,
                    className = className
                });

            }
            return obj;
        }
        //endCalender
    }
}