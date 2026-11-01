
using HRMS.Models;
using HRMS.Models.Master;
using HRMS.Models.TRN;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static HRMS.Models.Model_Common;
using static HRMS.Models.TRN.T_Leave_App;
//using static HRMS_VIEWMODEL24.Payroll_Common;

namespace HRMS.Controllers.Transaction
{
    public class LeaveApplyController : Controller
    {
        #region Leave apply using Calender Form
        // GET: LeaveApply
        //[HttpGet]
        //public async Task<ActionResult> EmployeeLeaveCalender(string Id = "COSAM024")
        //{

        //    //List<T_LEAVE_APPLICATION> t_LEAVE_APPLICATIONs = new List<T_LEAVE_APPLICATION>();
        //    //var t_Leave = await Services.GetAsync<List<T_LEAVE_APPLICATION>>(string.Format("/api/LeaveApplication/GetByEmpCode?search={0}", Id));
        //    //t_LEAVE_APPLICATIONs = t_Leave.Data ?? "0";
        //    List<T_LEAVE_APPLICATION> t_LEAVE_APPLICATIONs = new List<T_LEAVE_APPLICATION>();
        //    var t_Leave = await Services.GetAsync<List<T_LEAVE_APPLICATION>>(string.Format("/api/LeaveApplication/GetByEmpCode?search={0}", Id));

        //    t_LEAVE_APPLICATIONs = t_Leave.Data ?? new List<T_LEAVE_APPLICATION>(); // Use an empty list if Data is null

        //    try
        //    {
        //        DateTime nullDate = DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //        ViewBag.NullDate = nullDate;

        //        var EmpList = await Services.GetAsync<M_EMPMAS>(string.Format("/api/Employee/GetByID?id={0}", Id));
        //        TempData["EmpList"] = EmpList;
        //        ViewBag.EmpId = EmpList.Data.pK_EMPCODE;
        //        var reason = await Services.GetAsync<List<M_REASON_OF_LEAVING>>(string.Format("/api/ReasonOfLeaving/GetByType?search={0}", "L"));
        //        TempData["Reason"] = reason;
        //        var attend = await Services.GetAsync<List<M_ATTEND_DAYS_TYPE>>("/api/AttendDaysType/GetAll");
        //        TempData["Leave"] = attend;


        //        t_LEAVE_APPLICATIONs.ForEach(x =>
        //        {
        //            var emp = EmpList.Data;
        //            if (emp != null)
        //            {
        //                x.EmpName = $"{emp.pK_EMPCODE}-{emp.fname} {emp?.mname} {emp.lname}";
        //            }
        //            var rea = reason.Data.FirstOrDefault(s => s.pK_CODE == x.FK_ATTEND_CODE);
        //            if (rea != null)
        //            {
        //                x.Reason = $"{rea.description}";
        //            }
        //            var att = attend.Data.FirstOrDefault(s => s.pK_ATTEND_CODE == x.FK_ATTEND_CODE);
        //            if (att != null)
        //            {
        //                x.Attend = $"{att.description}";
        //            }
        //        });

        //        List<T_Leave_App> entity = t_LEAVE_APPLICATIONs.Select(l => new T_Leave_App { FK_EMPCODE = l.FK_EMPCODE, APPLY_DATE = l.APPLY_DATE, REMARKS = l.REMARKS, FROM_DATE = l.FROM_DATE, TO_DATE = l.TO_DATE, FK_REASON_CODE = l.FK_REASON_CODE, FK_ATTEND_CODE = l.FK_ATTEND_CODE, NO_OF_DAYS = l.NO_OF_DAYS, APPROVED = l.APPROVED, EmpName = l.EmpName, Reason = l.Reason, Attend = l.Attend }).Distinct().ToList();
        //        var data = CalenderDataList(entity);

        //        ViewBag.calender = data; ///string.Join(Environment.NewLine, data);
        //        if (TempData["StartDate"] != null)
        //        {
        //            DateTime startDate = (DateTime)TempData["StartDate"];
        //            ViewBag.ApproveMonth = startDate.ToString("yyyy-MM-dd");
        //        }
        //        else
        //        {
        //            var dt = DateTime.Now.ToString("yyyy-MM-dd");
        //            ViewBag.ApproveMonth = dt;
        //        }
        //        TempData.Remove("StartDate");
        //        return View(t_LEAVE_APPLICATIONs);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["toastrError"] = "Employee Id Not found";
        //        return View(t_LEAVE_APPLICATIONs);
        //    }
        //}
        //public List<object> CalenderDataList(List<T_Leave_App> entity)
        //{
        //    try
        //    {
        //        List<object> obj = new List<object>();
        //        foreach (var item in entity)
        //        {
        //            var color = "";
        //            switch (item.APPROVED)
        //            {
        //                case "P":
        //                    color = "#ffcc00";
        //                    break;
        //                case "A":
        //                    color = "#009933";
        //                    break;
        //                case "R":
        //                    color = "#cc0000";
        //                    break;
        //                case "C":
        //                    color = "#80ced6";
        //                    break;
        //                default:
        //                    break;
        //            }
        //            obj.Add(new { title = item.EmpName, start = item.FROM_DATE.AddSeconds(1).ToString("yyyy-MM-ddTHH:mm:ss"), end = item.TO_DATE.AddSeconds(1).ToString("yyyy-MM-ddTHH:mm:ss"), details = $"{item.Attend}:{item.Reason}-{item.REMARKS}", color = color, });

        //        }
        //        return obj;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //}
        //[HttpGet]
        //public ActionResult T_Lev_App(string Id)
        //{
        //    if (TempData["LeaveList"] != null && TempData["toastrSuccess"] == null)
        //    {
        //        var LeaveList = (TempData["LeaveList"]) as List<T_LEAVE_APPLICATION>;
        //        ViewBag.LeaveList = LeaveList;
        //        TempData.Keep("LeaveList");
        //    }

        //    var EmpList = (TempData["EmpList"]) as M_EMPMAS;
        //    TempData.Keep("EmpList");
        //    M_EMPMAS m_EMPMAS = EmpList;

        //    ViewBag.Name = m_EMPMAS?.pK_EMPCODE + " - " + m_EMPMAS?.title + " " + " " + m_EMPMAS?.fname + " " + " " + m_EMPMAS?.lname + " " + " ";


        //    var reason = (TempData["Reason"]) as List<M_REASON_OF_LEAVING>;
        //    TempData.Keep("Reason");

        //    var attend = (TempData["Leave"]) as List<M_ATTEND_DAYS_TYPE>;
        //    TempData.Keep("Leave");

        //    var gender = m_EMPMAS.sex;

        //    if (gender == "M")
        //    {
        //        attend = attend
        //       .Where(x => !x.pK_ATTEND_CODE.Contains("P")
        //        && !x.pK_ATTEND_CODE.Contains("PH")
        //        && !x.pK_ATTEND_CODE.Contains("W")
        //        && !x.pK_ATTEND_CODE.Contains("HF")
        //        && !x.pK_ATTEND_CODE.Contains("M")
        //        && !x.pK_ATTEND_CODE.Contains("A")).ToList();
        //    }
        //    else
        //    {
        //        attend = attend
        //        .Where(x => !x.pK_ATTEND_CODE.Contains("P")
        //         && !x.pK_ATTEND_CODE.Contains("PH")
        //         && !x.pK_ATTEND_CODE.Contains("W")
        //         && !x.pK_ATTEND_CODE.Contains("HF")
        //         && !x.pK_ATTEND_CODE.Contains("A")).ToList();
        //    }
        //    //model.ID = 9;
        //    //ViewBag.Session = serviceObj.SubmitPost<List<M_ENUM_PARA>>("api/PAY/PAY_Master/GetData_M_ENUM_PARA", model).Data;

        //    List<ModelMaster> Lstt = reason.Select(x => new ModelMaster
        //    {
        //        Value = x.pK_CODE,
        //        Text = x.pK_CODE + "-" + x.description

        //    }).ToList();
        //    ViewBag.Reason = Lstt;
        //    List<ModelMaster> attendLstt = attend.Select(x => new ModelMaster
        //    {
        //        Value = x.pK_ATTEND_CODE,
        //        Text = x.pK_ATTEND_CODE + "-" + x.description
        //    }).ToList();
        //    ViewBag.Attend = attendLstt;
        //    T_LEAVE_APPLICATION t_LEAVE_APPLICATION = new T_LEAVE_APPLICATION();
        //    t_LEAVE_APPLICATION.FK_EMPCODE = m_EMPMAS.pK_EMPCODE;
        //    if (TempData["t_LEAVE_APPLICATION"] != null)
        //    {
        //        t_LEAVE_APPLICATION = TempData["t_LEAVE_APPLICATION"] as T_LEAVE_APPLICATION;
        //    }
        //    DateTime nullDate = DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //    ViewBag.NullDate = nullDate;

        //    return PartialView("~/Views/T_Lev_App/_addEditCalender.cshtml", t_LEAVE_APPLICATION);
        //}
        //public JsonResult AppliedLeaveWithNoOfDays(string FK_ATTEND_CODE, string FK_REASON_CODE, string REMARKS, string FROM_DATE, string TO_DATE)
        //{
        //    var FrmDate = DateTime.ParseExact(FROM_DATE, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        //    var ToDate = DateTime.ParseExact(TO_DATE, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        //    try
        //    {
        //        var empId = "COSAM024";
        //        T_LEAVE_APPLICATION t_LEAVE_APPLICATION = new T_LEAVE_APPLICATION();

        //        //List<T_LEAVE_APPLICATION> t_LEAVE_APPLICATIONs = new List<T_LEAVE_APPLICATION>();
        //        TempData["t_LEAVE_APPLICATION"] = t_LEAVE_APPLICATION;
        //        var response = (dynamic)null;
        //        string msg = null;

        //        //HttpCookie Rt = Request.Cookies["Model_Session_HP"];
        //        //var LoginDetails = Utility.GetLoggedInUserDetails();
        //        //Model_SessionOtherProject jsonDataR = Newtonsoft.Json.JsonConvert.DeserializeObject<Model_SessionOtherProject>(Rt.Value);
        //        #region Leave Balance

        //        //SearchModelExtra model = new SearchModelExtra();
        //        //model.IsForDll = true;

        //        string yearNow = DateTime.Now.ToString("yyyy");

        //        //var t_DAILY_ATTEND = serviceObj.SubmitPost<List<T_DAILY_ATTEND>>("api/PAY/PAY_Master/GetData_T_DAILY_ATTEND", model).Data
        //        var t_DAILY_ATTEND = Services.GetAsync<List<T_DAILY_ATTEND>>(string.Format("/api/DailyAttend/GetByYear"));
        //        //.Where(x => x.FK_MONTHCODE.ToString().StartsWith(yearNow)).ToList();

        //        var TotalLeaveApplied = t_DAILY_ATTEND.Result.Data
        //             .Where(a => a.FK_ATTEND_CODE == FK_ATTEND_CODE)
        //             .Sum(a => a.LEAVE);

        //        if (new string[2] { "S", "L" }.Contains(FK_ATTEND_CODE))

        //        {
        //            //Payroll_T_LEAVE_OP_DECLARE payroll_T_LEAVE_OP_DECLARE = new Payroll_T_LEAVE_OP_DECLARE();
        //            ////payroll_T_LEAVE_OP_DECLARE.EmpCode = EmpCode;
        //            //payroll_T_LEAVE_OP_DECLARE.ID = 1;

        //            var TotalLeaveTaken = Services.GetAsync<List<T_LEAVE_OP_DECLARE>>(string.Format("/api/LeaveDeclaration/GetByLeaveType?search={0}", FK_ATTEND_CODE)).Result.Data
        //                //.Where(d => d.FK_ATTEND_CODE == FK_ATTEND_CODE).ToList()
        //                .Select(d => new { subtraction = d.ALLOTED - d.OPENING }).FirstOrDefault();

        //            var totalLeaveCal = 0m;
        //            if (TotalLeaveTaken != null)
        //            {
        //                totalLeaveCal = TotalLeaveTaken.subtraction;
        //            }

        //            if (TotalLeaveApplied <= totalLeaveCal)
        //            {
        //                ViewBag.LeaveRemaining = totalLeaveCal - TotalLeaveApplied;
        //            }
        //            else
        //            {
        //                ViewBag.LeaveRemaining = 0;
        //            }
        //        }
        //        #endregion

        //        List<T_LEAVE_APPLICATION> t_LEAVEs = new List<T_LEAVE_APPLICATION>();
        //        {
        //            decimal daysDifference = Math.Abs((ToDate.Day - FrmDate.Day));
        //            for (DateTime dt = FrmDate; dt <= ToDate; dt = dt.AddDays(1))
        //            {
        //                T_LEAVE_APPLICATION t = new T_LEAVE_APPLICATION();
        //                t.USER_NAME = "";
        //                t.LOCA = "";
        //                t.USER_ENTDT = Convert.ToDateTime(DateTime.Now);
        //                t.OS_USER = "";
        //                t.TERMINAL_ID = "";
        //                t.UNIT = "";

        //                t.FROM_DATE = FrmDate;
        //                t.TO_DATE = ToDate;
        //                t.FK_REASON_CODE = FK_REASON_CODE;
        //                t.FK_ATTEND_CODE = FK_ATTEND_CODE;
        //                t.FK_EMPCODE = empId;
        //                t.LEAVE_DATE = dt;
        //                t.SESSION = 1m;
        //                t.NO_OF_DAYS = daysDifference + 1;
        //                t.REMARKS = REMARKS;
        //                t.APPLY_DATE = DateTime.Now;
        //                t.APPROVED = "P";
        //                t_LEAVEs.Add(t);
        //            }
        //        }
        //        T_LEAVE_APPLICATION_Dto payroll_T_LeaveApplication = new T_LEAVE_APPLICATION_Dto();
        //        payroll_T_LeaveApplication.t_leaveapplication = t_LEAVEs;

        //        response = Services.PostAsync<List<T_LEAVE_APPLICATION>>("/api/LeaveApplication/SaveOrUpdateAll", payroll_T_LeaveApplication);

        //        var t_LEAVE_APPLICATIONs = Services.GetAsync<List<T_LEAVE_APPLICATION>>(string.Format("/api/LeaveApplication/GetByEmpCode?search={0}", empId));


        //        //var EmpList = serviceObj.SubmitPost<List<M_EMPMAS>>("api/PAY/PAY_Master/GetEMPMASData", model).Data.ToList();
        //        var EmpList = Services.GetAsync<List<M_EMPMAS>>(string.Format("/api/Employee/GetByID?id={0}", empId));
        //        //.Where(x => x.PK_EMPCODE == jsonDataR.UserId).SingleOrDefault();

        //        //var reason = serviceObj.SubmitPost<List<M_REASON_OF_LEAVING>>("api/PAY/PAY_Master/GetData_M_REASON_OF_LEAVING", model).Data
        //        //   .Where(x => x.TYPE == "L");
        //        var reason = Services.GetAsync<List<M_REASON_OF_LEAVING>>(string.Format("/api/ReasonOfLeaving/GetByType?search={0}", "L"));
        //        var attend = Services.GetAsync<List<M_ATTEND_DAYS_TYPE>>("/api/AttendDaysType/GetAll");

        //        foreach (var le in t_LEAVE_APPLICATIONs.Result.Data)
        //        {
        //            var emp = EmpList.Result.Data.FirstOrDefault(q => q.pK_EMPCODE == le.FK_EMPCODE);
        //            if (emp != null) { le.EmpName = $"{emp.pK_EMPCODE}-{emp.fname} {emp.mname} {emp.lname}"; }


        //            var rea = reason.Result.Data.FirstOrDefault(s => s.pK_CODE == le.FK_REASON_CODE);
        //            if (rea != null)
        //            {
        //                le.Reason = $"{rea.description}";
        //            }

        //            var att = attend.Result.Data.FirstOrDefault(s => s.pK_ATTEND_CODE == le.FK_ATTEND_CODE);
        //            if (att != null)
        //            {
        //                le.Attend = $"{att.description}";
        //            }
        //        }
        //        List<T_Leave_App> entity = t_LEAVE_APPLICATIONs.Result.Data.Select(l => new T_Leave_App { FK_EMPCODE = l.FK_EMPCODE, APPLY_DATE = l.APPLY_DATE, REMARKS = l.REMARKS, FROM_DATE = l.FROM_DATE, TO_DATE = l.TO_DATE, FK_REASON_CODE = l.FK_REASON_CODE, FK_ATTEND_CODE = l.FK_ATTEND_CODE, NO_OF_DAYS = l.NO_OF_DAYS, APPROVED = l.APPROVED, EmpName = l.EmpName, Reason = l.Reason, Attend = l.Attend }).Distinct().ToList();
        //        var data = CalenderDataList(entity);
        //        msg = "Saved Succesfully";


        //        if (response.StatusCode == 1)
        //        {
        //            TempData["toastrSuccess"] = "Leave has been successfully Applied.";
        //            return Json(new { msg = TempData["toastrSuccess"], date = FrmDate.ToString("yyyy-MM-dd"), data = data, success = true }, JsonRequestBehavior.AllowGet);

        //        }
        //        else
        //        {

        //            TempData["toastrError"] = "Something Went Wrong.";
        //            return Json(new { msg = TempData["toastrError"], date = FrmDate.ToString("yyyy-MM-dd"), success = false }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["toastrError"] = ex.Message;
        //        return Json(new { msg = TempData["toastrError"], date = FrmDate.ToString("yyyy-MM-dd"), success = false }, JsonRequestBehavior.AllowGet);
        //    }

        //    //return RedirectToAction("EmployeeLeaveCalender", new { Id = jsonDataR.UserId });
        //}
        #endregion

        #region leave apply using Normal Form
        [HttpGet]
        public async Task<ActionResult> ApplyLeaveModule(string Id)
        {
            var EmpId = Utility.SessionHelper.GetUser().getUserName;
            
            List <T_LEAVE_APPLICATION> t_LEAVE_APPLICATIONs = new List<T_LEAVE_APPLICATION>();
            var t_Leave = await Services.GetAsync<List<T_LEAVE_APPLICATION>>(string.Format("/api/LeaveApplication/GetByEmpCode?search={0}", EmpId));
            t_LEAVE_APPLICATIONs = t_Leave.Data ?? new List<T_LEAVE_APPLICATION>(); // Use an empty list if Data is null
            var Year = DateTime.Now.Year;
            
            
            try
            {
                DateTime nullDate = DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                ViewBag.NullDate = nullDate;
                var reason = await Services.GetAsync<List<M_REASON_OF_LEAVING>>(string.Format("/api/ReasonOfLeaving/GetByType?search={0}", "L"));
                TempData["Reason"] = reason;

                var attend = await Services.GetAsync<List<M_ATTEND_DAYS_TYPE>>("/api/AttendDaysType/GetByLeave");
                TempData["Leave"] = attend;

                List<ModelMaster> Lstt = reason.Data.Select(x => new ModelMaster
                {
                    Value = x.pK_CODE,
                    Text = x.pK_CODE + "-" + x.description

                }).ToList();
                ViewBag.Reason = Lstt;
                List<ModelMaster> attendLstt = attend.Data.Select(x => new ModelMaster
                {
                    Value = x.pK_ATTEND_CODE,
                    Text = x.pK_ATTEND_CODE + "-" + x.description
                }).ToList();
                ViewBag.Attend = attendLstt;
                var EmpList = await Services.GetAsync<M_EMPMAS>(string.Format("/api/Employee/GetByID?id={0}", EmpId));
                if(EmpList.Data == null)
                {
                    TempData["toastrError"] = "Employee Id Not found";
                    ViewBag.LeaveData = null;
                    return View(t_LEAVE_APPLICATIONs);
                }   
                TempData["EmpList"] = EmpList;
                ViewBag.EmpId = EmpList.Data != null ? EmpList.Data.pK_EMPCODE : null;

                var leaveBalance = await Services.GetAsync<List<T_MONTHLY_ATTEND_DAYS_DET>>(string.Format("/api/MonthlyAttendDaysDetails/GetLeaveBalance?unit={0}&year={1}&empCode={2}", EmpList.Data.unit, Year, EmpId));
                if (leaveBalance.Data.Any())
                {
                    ViewBag.LeaveData = leaveBalance.Data;
                }
                else
                {
                    //var generateMonthlyAttfrmDailyAtt = await Services.GetAsync<T_DAILY_ATTEND>("/api/Attend/GetCalculationByCode?code=10001&Monthcode=202408&location=AGRI&paytype=P&Ptype=S", EmpId,);
                }

                
                var gender = EmpList.Data.sex;

                if (gender == "M")
                {
                    attend.Data = attend.Data
                   .Where(x => !x.pK_ATTEND_CODE.Contains("P")
                    && !x.pK_ATTEND_CODE.Contains("PH")
                    && !x.pK_ATTEND_CODE.Contains("W")
                    && !x.pK_ATTEND_CODE.Contains("HF")
                    && !x.pK_ATTEND_CODE.Contains("M")
                    && !x.pK_ATTEND_CODE.Contains("A")).ToList();
                }
                else
                {
                    attend.Data = attend.Data
                    .Where(x => !x.pK_ATTEND_CODE.Contains("P")
                     && !x.pK_ATTEND_CODE.Contains("PH")
                     && !x.pK_ATTEND_CODE.Contains("W")
                     && !x.pK_ATTEND_CODE.Contains("HF")
                     && !x.pK_ATTEND_CODE.Contains("A")).ToList();
                }

                t_LEAVE_APPLICATIONs.ForEach(x =>
                {
                    var emp = EmpList.Data;
                    if (emp != null)
                    {
                        x.EmpName = $"{emp.pK_EMPCODE}-{emp.fname} {emp?.mname} {emp.lname}";
                    }
                    var rea = reason.Data.FirstOrDefault(s => s.pK_CODE == x.FK_REASON_CODE);
                    if (rea != null)
                    {
                        x.Reason = $"{rea.description}";
                    }
                    var att = attend.Data.FirstOrDefault(s => s.pK_ATTEND_CODE == x.FK_ATTEND_CODE);
                    if (att != null)
                    {
                        x.Attend = $"{att.description}";
                    }
                });
                return View(t_LEAVE_APPLICATIONs);
            }
            catch (Exception ex)
            {
                TempData["toastrError"] = "Employee Id Not found";
                return View(t_LEAVE_APPLICATIONs);
            }
        }
        [HttpPost]
        public async Task<ActionResult> ApplyLeaveWithNoOfDays(string FK_ATTEND_CODE, string FK_REASON_CODE, string REMARKS, string FROM_DATE, string TO_DATE)
        {
            var FrmDate = DateTime.ParseExact(FROM_DATE, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            var ToDate = DateTime.ParseExact(TO_DATE, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            var empId = Utility.SessionHelper.GetUser().getUserName;
            try
            {
               
                T_LEAVE_APPLICATION t_LEAVE_APPLICATION = new T_LEAVE_APPLICATION();
                TempData["t_LEAVE_APPLICATION"] = t_LEAVE_APPLICATION;
                var response = new ResponseApiModel<List<T_LEAVE_APPLICATION_Dto>>();

                var daysDifference = 0m;
                var EmpList = await Services.GetAsync<M_EMPMAS>(string.Format("/api/Employee/GetByID?id={0}", empId));
                var HolidayList = await Services.GetAsync<List<M_HOLIDAY>>(string.Format("/api/Holiday/GetByUnit?unit={0}", EmpList.Data.unit));

                var holidayDates = new HashSet<DateTime>(
                                                        HolidayList.Data
                                                        .Select(h => h.hoL_DATE) // Assuming hoL_DATE is DateTime?
                                                        .Where(date => date.HasValue) // Filter out null values
                                                        .Select(date => date.Value) // Get the actual DateTime values
);
                for (DateTime dt = FrmDate; dt <= ToDate; dt = dt.AddDays(1))
                {
                    if (dt.DayOfWeek != DayOfWeek.Sunday && dt.DayOfWeek != DayOfWeek.Saturday && !holidayDates.Contains(dt.Date))
                    {
                        daysDifference++;
                    }
                }

                T_LEAVE_APPLICATION t = new T_LEAVE_APPLICATION();
                t.USER_NAME = "";
                t.LOCA = "";
                t.USER_ENTDT = Convert.ToDateTime(DateTime.Now);
                t.OS_USER = "";
                t.TERMINAL_ID = "";
                t.UNIT = EmpList.Data.unit;

                t.FROM_DATE = FrmDate;
                t.TO_DATE = ToDate;
                t.FK_REASON_CODE = FK_REASON_CODE;
                t.FK_ATTEND_CODE = FK_ATTEND_CODE;
                t.FK_EMPCODE = empId;
                t.LEAVE_DATE = FrmDate; // saving single data for multiple days leave
                t.SESSION = 1m;
                t.NO_OF_DAYS = daysDifference == 0 ? 1 : daysDifference;
                t.REMARKS = REMARKS;
                t.APPLY_DATE = DateTime.Now;
                t.APPROVED = "P";
                //t_LEAVEs.Add(t);
                //    }
                //}
                T_LEAVE_APPLICATION_Dto payroll_T_LeaveApplication = new T_LEAVE_APPLICATION_Dto();
                payroll_T_LeaveApplication.t_leaveapplication = t;
                response = await Services.PostAsync<List<T_LEAVE_APPLICATION_Dto>>("/api/LeaveApplication/SaveOrUpdateAll", payroll_T_LeaveApplication);

                if (response.IsSuccessStatusCode)
                {
                    TempData["toastrSuccess"] = "Leave has been successfully Applied.";
                    return RedirectToAction("ApplyLeaveModule", new { Id = empId });
                }
                else
                {
                    TempData["toastrError"] = "Something Went Wrong."+response.Message;
                    return RedirectToAction("ApplyLeaveModule", new { Id = empId });
                }
            }
            catch (Exception ex)
            {
                TempData["toastrError"] ="Error : "+ ex.Message;
                return RedirectToAction("ApplyLeaveModule", new { Id = empId });
            }
        }
        #endregion
    }
}