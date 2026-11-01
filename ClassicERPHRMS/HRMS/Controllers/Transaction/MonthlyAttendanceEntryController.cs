using HRMS.Models.Master;
using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Drawing.Printing;
using System.Web.UI;
using PagedList;
using static HRMS.Models.Model_Common;
using System.Web.UI.WebControls;
using HRMS.Models.TRN;
using System.Security.Policy;

namespace HRMS.Controllers.Transaction
{
    public class MonthlyAttendanceEntryController : Controller
    {
        // GET: MonthlyAttendanceEntry
        public async Task<ActionResult> MonthlyAttendanceEntry(string UNIT, string MONTHCODE, string EMPCODE, string searchString = null, int? page = 1, int pageSize = 25)
        {
            List<T_MONTHLY_ATTEND_DAYS_DET> t_MONTHLY_ATTEND_DAYS_DETs = new List<T_MONTHLY_ATTEND_DAYS_DET>();
            MONTHLY_ATTENDANCE_ENTRY shift_det = new MONTHLY_ATTENDANCE_ENTRY();
            shift_det.ATTEND_DAYS_DET = new List<ATTEND_DAYS_DET>();
            shift_det.SHIFT_DET = new List<SHIFT_DET>();
            shift_det.UNIT = UNIT;
            shift_det.MONTHCODE = MONTHCODE;
            shift_det.EMPCODE = EMPCODE;

            var unit = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll");
            List<SelectListItem> all2 = unit.Data.Select(x => new SelectListItem
            {
                Value = x.code,
                Text = x.code + "-" + x.name

            }).ToList();
            ViewBag.AllowCal2 = all2;

            if (!string.IsNullOrEmpty(UNIT))
            {
                if (!string.IsNullOrEmpty(UNIT))
                {
                    var monthResult = await Services.GetAsync<List<M_MONTH>>("/api/Month/GetAll");
                    List<SelectListItem> monthList = monthResult.Data.Where(m => m.unit == UNIT).Select(x => new SelectListItem
                    {
                        Value = x.pK_MONTHCODE,
                        Text = x.pK_MONTHCODE

                    }).ToList();
                    ViewBag.AllowCal4 = monthList;
                    if (!string.IsNullOrEmpty(MONTHCODE))
                    {
                        var selectedmonth = monthResult.Data.Where(l => l.pK_MONTHCODE == MONTHCODE).FirstOrDefault();
                        ViewBag.fromdate = selectedmonth.froM_PERIOD.ToString("yyyy-MM-dd");
                        ViewBag.todate = selectedmonth.tO_PERIOD.ToString("yyyy-MM-dd");
                    }
                    if (!string.IsNullOrEmpty(EMPCODE))
                    {
                        var url = string.Format("/api/Employee/GetByCode?EMPCODE={0}", EMPCODE);
                        var response = await Services.GetAsync<M_EMPMAS>(url);
                        ViewBag.emplist = new List<SelectListItem>
                                            {
                                                new SelectListItem {
                                                    Text = response.Data.pK_EMPCODE + "-" + response.Data.fname,
                                                                Value = response.Data.pK_EMPCODE,
                                                                Selected = true
                                                }
                                            };

                    }

                }
            }

            if (!string.IsNullOrEmpty(UNIT) && !string.IsNullOrEmpty(MONTHCODE) && !string.IsNullOrEmpty(EMPCODE))
            {
                var sift = await Services.GetAsync<List<SHIFT_DET>>(string.Format("/api/MonthlyAttendShiftDetails/GetShiftDetails?unit={0}&month={1}&empCode={2}", UNIT, MONTHCODE, EMPCODE));
                ViewBag.RCP = sift.Data.ToList();
                shift_det.SHIFT_DET = sift.Data.ToList();

                var days = await Services.GetAsync<List<ATTEND_DAYS_DET>>(string.Format("/api/MonthlyAttendDaysDetails/GetDaysDetails?unit={0}&month={1}&empCode={2}", UNIT, MONTHCODE, EMPCODE));
                ViewBag.RCP = days.Data.ToList();
                shift_det.ATTEND_DAYS_DET = days.Data.ToList();
            }
            return View(shift_det);
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


        [HttpPost]
        public async Task<ActionResult> InsertOrUpdate(MONTHLY_ATTENDANCE_ENTRY entity)
        {
            try
            {
                var shiftList = entity.SHIFT_DET.Select(k => new T_MONTHLY_ATTEND_SHIFT_DET
                {
                    MINS = k.MINS,
                    UNIT = entity.UNIT,
                    FK_SHIFT_CODE = k.PK_SHIFT_CODE,
                    FK_MONTHCODE = entity.MONTHCODE,
                    DAYS = k.DAYS,
                    HOURS = k.HOURS,
                    FK_EMPCODE = entity.EMPCODE,
                    ID = k.ID,
                });
                var daysList = entity.ATTEND_DAYS_DET.Select(k => new T_MONTHLY_ATTEND_DAYS_DET
                {
                    ID = k.ID,
                    MINS = k.MINS,
                    UNIT = entity.UNIT,
                    FK_ATTEND_CODE = k.PK_ATTEND_CODE,
                    FK_MONTHCODE = entity.MONTHCODE,
                    DAYS = k.DAYS,
                    HOURS = k.HOURS,
                    FK_EMPCODE = entity.EMPCODE,
                });

                var t_MONTHLY_ATTEND_SHIFT_DET = new List<T_MONTHLY_ATTEND_SHIFT_DET>();
                t_MONTHLY_ATTEND_SHIFT_DET.AddRange(shiftList);
                var t_MONTHLY_ATTEND_DAYS_DET = new List<T_MONTHLY_ATTEND_DAYS_DET>();
                t_MONTHLY_ATTEND_DAYS_DET.AddRange(daysList);

         

                var responce1 = await Services.PostAsync<List<T_MONTHLY_ATTEND_SHIFT_DET>>("/api/MonthlyAttendShiftDetails/SaveOrUpdateShiftDetails", t_MONTHLY_ATTEND_SHIFT_DET);
                var responce2 = await Services.PostAsync<List<T_MONTHLY_ATTEND_DAYS_DET>>("/api/MonthlyAttendDaysDetails/SaveOrUpdateDaysDetails", t_MONTHLY_ATTEND_DAYS_DET);
                if (responce1.IsSuccessStatusCode == true && responce2.IsSuccessStatusCode == true)
                {
                    TempData["toastrSuccess"] = "Data has been successfully submitted";
                }
                else
                {
                    TempData["toastrError"] = responce1.Message +" - " + responce2;
                    return View("MonthlyAttendanceEntry", new { entity.UNIT, entity.MONTHCODE, entity.EMPCODE });
                }
            }
            catch (Exception ex)
            {
                TempData["toastrError"] = ex.Message;
            }
            return RedirectToAction("MonthlyAttendanceEntry");
        }

        public async Task<ActionResult> monthlyshiftsaveupdate(FormCollection bb, List<T_MONTHLY_ATTEND_DAYS_DET> model)
        {
            try
            {
                var responce = await Services.PostAsync<List<T_MONTHLY_ATTEND_DAYS_DET>>("/api/Country/SaveOrUpdateAll", model);
                if (Response.StatusCode == 200)
                {
                    TempData["SuccessMessage"] = "Data has been successfully submitted";
                }
                else
                {
                    TempData["ErrorMessage"] = "Something Went Wrong";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("MonthlyAttendanceEntry");
        }
    }
}