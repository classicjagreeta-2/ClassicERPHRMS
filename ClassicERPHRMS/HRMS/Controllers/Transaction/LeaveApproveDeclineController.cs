using HRMS.Models;
using HRMS.Models.Master;
using HRMS.Models.TRN;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static HRMS.Models.TRN.T_Leave_App;

namespace HRMS.Controllers.Transaction
{
    public class LeaveApproveDeclineController : Controller
    {
        #region Calender Leave Approval
        // GET: LeaveApproveDecline
        [HttpGet]
        public async Task<ActionResult> T_Lev_Calender(string Id = "COSAM024")
        {

            List<T_LEAVE_APPLICATION> t_LEAVE_APPLICATIONs = new List<T_LEAVE_APPLICATION>();

            DateTime nullDate = DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            ViewBag.NullDate = nullDate;
            var t_LEAVE_List = await Services.GetAsync<List<T_LEAVE_APPLICATION>>("/api/LeaveApplication/GetByPendingLeave");

            t_LEAVE_APPLICATIONs = t_LEAVE_List.Data;

            var EmpList = await Services.GetAsync<M_EMPMAS>(string.Format("/api/Employee/GetByID?id={0}", Id));
            TempData["EmpList"] = EmpList;
            var reason = await Services.GetAsync<List<M_REASON_OF_LEAVING>>(string.Format("/api/ReasonOfLeaving/GetByType?search={0}", "L"));
            TempData["Reason"] = reason;
            var attend = await Services.GetAsync<List<M_ATTEND_DAYS_TYPE>>("/api/AttendDaysType/GetAll");
            TempData["Leave"] = attend;

            t_LEAVE_APPLICATIONs.ForEach(x =>
            {
                var emp = EmpList.Data;
                if (emp != null)
                {
                    x.EmpName = $"{emp.pK_EMPCODE}-{emp.fname} {emp?.mname} {emp.lname}";
                }
                var rea = reason.Data.FirstOrDefault(s => s.pK_CODE == x.FK_ATTEND_CODE);
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

            List<T_Leave_App> entity = t_LEAVE_APPLICATIONs.Select(l => new T_Leave_App { FK_EMPCODE = l.FK_EMPCODE, APPLY_DATE = l.APPLY_DATE, REMARKS = l.REMARKS, FROM_DATE = l.FROM_DATE, TO_DATE = l.TO_DATE, FK_REASON_CODE = l.FK_REASON_CODE, FK_ATTEND_CODE = l.FK_ATTEND_CODE, NO_OF_DAYS = l.NO_OF_DAYS, APPROVED = l.APPROVED, EmpName = l.EmpName, Reason = l.Reason, Attend = l.Attend }).Distinct().ToList();
            var data = CalenderDataList(entity);

            ViewBag.calender = data; ///string.Join(Environment.NewLine, data);
            if (TempData["StartDate"] != null)
            {
                DateTime startDate = (DateTime)TempData["StartDate"];
                ViewBag.ApproveMonth = startDate.ToString("yyyy-MM-dd");
            }
            else
            {
                var dt = DateTime.Now.ToString("yyyy-MM-dd");
                ViewBag.ApproveMonth = dt;
            }
            TempData.Remove("StartDate");
            return View(t_LEAVE_APPLICATIONs);
        }
        public List<object> CalenderDataList(List<T_Leave_App> entity)
        {
            try
            {
                List<object> obj = new List<object>();
                foreach (var item in entity)
                {
                    var color = "";
                    switch (item.APPROVED)
                    {
                        case "P":
                            color = "#ffcc00";
                            break;
                        case "A":
                            color = "#009933";
                            break;
                        case "R":
                            color = "#cc0000";
                            break;
                        case "C":
                            color = "#80ced6";
                            break;
                        default:
                            break;
                    }
                    obj.Add(new { title = item.EmpName, start = item.FROM_DATE.AddSeconds(1).ToString("yyyy-MM-ddTHH:mm:ss"), end = item.TO_DATE.AddSeconds(1).ToString("yyyy-MM-ddTHH:mm:ss"), details = $"{item.Attend}:{item.Reason}-{item.REMARKS}", color = color, });
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<ActionResult> LeaveApproval(string Name, string startDate, string endDate, string Status, int? page = 1, int pageSize = 500)
        {
            //var EmpId = Helpers.UserInfo.getUserId;
            var EmpName = Utility.SessionHelper.GetUser().getUserName;
            try
            {
                var EMPCODE = Name.Substring(0, Name.IndexOf(':'));
                var response = (dynamic)null;
                var leaveApplication = await Services.GetAsync<T_LEAVE_APPLICATION>(string.Format("/api/LeaveApplication/GetFilterLeaveData?empCode={0}&fdate={1}&tdate={2}&approved={3}", EMPCODE, startDate, endDate, "P"));
                if (leaveApplication != null)
                {
                    //foreach (var item in leaveApplication)
                    //{
                    leaveApplication.Data.APPROVED = Status;
                    leaveApplication.Data.USER_NAME_NEW = EmpName;
                    //}
                }
                T_LEAVE_APPLICATION_Dto payroll_T_LeaveApplication = new T_LEAVE_APPLICATION_Dto();
                payroll_T_LeaveApplication.t_leaveapplication = leaveApplication.Data;



                response = await Services.PostAsync<List<T_LEAVE_APPLICATION>>("/api/LeaveApplication/SaveOrUpdateAll", payroll_T_LeaveApplication);
                if (response.IsSuccessStatusCode)
                {
                    if (Status == "A")
                    {
                        TempData["toastrSuccess"] = "Leave Approval has been APPROVED.";
                    }
                    else if (Status == "R")
                    {
                        TempData["toastrSuccess"] = "Leave Approval has been REJECTED.";
                    }
                    else if (Status == "C")
                    {
                        TempData["toastrSuccess"] = "Applied Leave has been CANCELLED.";
                    }
                }
                else
                {
                    TempData["toastrError"] = "Sorry Something Went Wrong.";
                }
            }
            catch (Exception ex)
            {
                TempData["toastrError"] = "Undefined Data...!";
            }
            return RedirectToAction("LeaveApproval");
        }
        #endregion
        public async Task<ActionResult>LeaveApproval(string UNIT, string Employee, string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {


            try
            {
                var response = (dynamic)null;
                if (UNIT != null & Employee != null)
                {

                    if (!string.IsNullOrEmpty(Employee))
                    {
                        response = await Services.GetAsync<PageModel<T_LEAVE_APPLICATION>>(string.Format("/api/LeaveApplication/GetByPendingLeave?page={0}&pageSize={1}&search={2}&Unit={3}", page, pageSize, Employee, UNIT));
                    }
                }
                else
                {
                    response = await Services.GetAsync<PageModel<T_LEAVE_APPLICATION>>(string.Format("/api/LeaveApplication/GetByPendingLeave?page={0}&pageSize={1}", page, pageSize));
                }
                if (response.IsSuccessStatusCode)
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
                        var usersAsIPagedList = new StaticPagedList<T_LEAVE_APPLICATION>(response.Data.value.results, pageNumber, pageSize, response.Data.value.rowCount);
                        foreach (var item in usersAsIPagedList)
                        {
                            if (item.FK_EMPCODE != null)
                            {
                                item.EmpName = item.FK_EMPCODE.Substring(@item.FK_EMPCODE.IndexOf(':') + 2);
                                //item.FK_EMPCODE = item.FK_EMPCODE.Substring(0, item.FK_EMPCODE.IndexOf(':')).Trim();
                            }

                        }
                        return View(usersAsIPagedList);
                    }

                    else
                    {
                        //TempData["toastrError"] = "Employee Code Not Found"; 
                        return View();

                    }
                

            }
            catch (Exception ex)
            {
                throw;
            }

            
        }



        //public async Task<JsonResult> UnitWiseEmployee(string UNIT, string searchString = null, int? page = 1, int pageSize = 25)
        //{
        //    if (!string.IsNullOrEmpty(UNIT))
        //    {

        //        //List<string> list = JsonConvert.DeserializeObject<List<string>>(UNIT);

        //        ////var emp = await Services.GetAsync<PageModel<M_EMPMAS>>(string.Format("/api/Employee/GetByPage?page=1&pageSize=25"));
        //        //List<string> namesList = list.ToList();
        //        //foreach (string name in namesList) { 
        //        //}


        //        var emp = await Services.GetAsync<List<M_EMPMAS>>(string.Format("/api/Employee/GetByPage?unit=1001&page=1&pageSize=25", UNIT, page, pageSize));
        //        List<ModelMaster> EmpList = emp.Data.Select(x => new ModelMaster
        //        {
        //            Value = x.pK_EMPCODE,
        //            Text = x.pK_EMPCODE + "-" + x.fname + " " + x?.mname + " " + x.lname
        //        }).ToList();

        //        ViewBag.Text = EmpList;
        //        ViewBag.Value = EmpList;
        //    }
        //    return Json("", JsonRequestBehavior.AllowGet);
        //}


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
            var response = await Services.GetAsync<PageModel<M_UNIT>>(string.Format("/api/Unit/GetByPage?page={0}&pageSize={1}", page, pageSize, Utility.SessionHelper.GetUser().getUserName));


            return Json(response, JsonRequestBehavior.AllowGet);
        }



    }
}