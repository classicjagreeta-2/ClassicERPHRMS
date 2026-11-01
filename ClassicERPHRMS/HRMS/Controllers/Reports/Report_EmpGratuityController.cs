using HRMS.Models.Master;
using HRMS.Models.TRN;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static HRMS.Models.Model_Common;

namespace HRMS.Controllers.Reports
{
    public class Report_EmpGratuityController : Controller
    {
        // GET: Report_EmpGratuity       
        public ActionResult Index()
        {
            try
            {
                //Model_SessionOtherProject jsonDataR = getCookies.GetStoredCookies();
                //var FinancialYear = jsonDataR.FinancialYearName;

                //ViewBag.LOCA = jsonDataR.Loca;

                //string startDate = FinancialYear.Split('-')[0].Trim();
                //DateTime date = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);

                //SearchModelExtra model = new SearchModelExtra();
                //model.IsForDll = true;
                //ViewBag.Unit = servicesObj.SubmitPost<List<M_UNIT>>("api/PAY/PAY_Master/GetData_M_UNIT", model).Data
                //    .Where(x => x.LOCA == jsonDataR.Loca);

                //ViewBag.Dept = servicesObj.SubmitPost<List<M_DEPARTMENT>>("api/PAY/PAY_Master/GetData_M_DEPARTMENT", model).Data;

                //// Convert DateTime object to desired format "yyyyMM"
                //string monthPrev = date.ToString("yyyyMM");
                //string monthNow = DateTime.Now.ToString("yyyyMM");

                //int prevMonthCode = int.Parse(monthPrev);
                //int currentMonthCode = int.Parse(monthNow);

                //// Fetch and filter GratuityMonth data
                //var GratuityMonth = servicesObj.SubmitPost<List<T_GRATUITY>>("api/PAY/PAY_Master/GetData_T_GRATUITY", model).Data;

                //var filteredGratuityMonth = GratuityMonth
                //    .Where(a => int.Parse(a.FK_MONTHCODE) >= int.Parse(monthPrev) && int.Parse(a.FK_MONTHCODE) <= int.Parse(monthNow))
                //    .GroupBy(a => a.FK_MONTHCODE)
                //    .Select(g => g.First()) // or `g.FirstOrDefault()` if you want to allow for empty groups
                //    .OrderByDescending(a => a.FK_MONTHCODE)
                //    .ToList();


                //// Fetch EmpList data
                //var EmpList = servicesObj.SubmitPost<List<M_EMPMAS>>("api/PAY/PAY_Master/GetEMPMASData", model).Data;

                //// Filter EmpList based on filtered GratuityMonth
                //var empCodesInGratuity = new HashSet<string>(filteredGratuityMonth.Select(g => g.FK_EMPCODE)).ToList(); // Adjust FK_EMPCODE to match the actual field

                //var EmployeeList = EmpList.Where(x => empCodesInGratuity.Contains(x.PK_EMPCODE)).ToList();
                //List<ModelMaster> ELst = EmployeeList.Select(x => new ModelMaster
                //{
                //    Value = x.PK_EMPCODE,
                //    Text = x.PK_EMPCODE + "-" + x.PK_EMPCODE + "-" + x.FNAME + " " + x.LNAME

                //}).ToList();
                //ViewBag.EmpList = ELst;
                //// Assign filtered GratuityMonth to ViewBag
                //List<ModelMaster> GratuityMonthList = filteredGratuityMonth.Select(x => new ModelMaster
                //{
                //    Value = x.FK_MONTHCODE,
                //    Text = DateTime.ParseExact(x.FK_MONTHCODE, "yyyyMM", CultureInfo.InvariantCulture).ToString("MMM") + " " + DateTime.ParseExact(x.FK_MONTHCODE, "yyyyMM", CultureInfo.InvariantCulture).ToString("yyyy")

                //}).ToList();

                //ViewBag.GratuityMonth = GratuityMonthList;
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"{ex.Message}";
                return View();
            }
        }

        public ActionResult GetGratuityReport(string MonthCode, string EmpCode, T_GRATUITY t_GRATUITY)
        {
            //ModelFromFilter modelFromFilter = new ModelFromFilter();
            //modelFromFilter.ReportCaseCode = "17";
            //modelFromFilter.MonthCode = MonthCode;
            //modelFromFilter.EmpCode = EmpCode ?? "";

            //if (MonthCode == null)
            //{
            //    TempData["ErrorMessage"] = "Select Month  Code !";
            //    return RedirectToAction("Index");
            //}
            //REPORT_COMMONController rEPORTController = new REPORT_COMMONController();
            //Model_ReportFilter_ForCheck model_ReportFilter_ForCheck = new Model_ReportFilter_ForCheck();
            //byte[] buffer = rEPORTController.GetReportData(modelFromFilter, model_ReportFilter_ForCheck);
            //if (buffer == null)
            //{
            //    TempData["ErrorMessage"] = "Data not found";
            //    return RedirectToAction("Index");
            //}
            //else
            //{
            //    Pdf_DownLoadWithByte(buffer);
            //}
            return RedirectToAction("Index");
        }
        public void Pdf_DownLoadWithByte(byte[] buffer)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            //merge the all reports & show the reports            
            Response.BinaryWrite(buffer);
            Response.End();
        }

        public JsonResult GetEmployeesByMonth(string monthCode)
        {
           
            //// Fetch data based on the selected month
            //var employees = servicesObj.SubmitPost<List<M_EMPMAS>>("api/PAY/PAY_Master/GetEMPMASData", model).Data;

            //var gratuityData = servicesObj.SubmitPost<List<T_GRATUITY>>("api/PAY/PAY_Master/GetData_T_GRATUITY", model).Data;
            //var filteredGratuityData = gratuityData
            //    .Where(a => a.FK_MONTHCODE == monthCode)
            //    .Select(g => g.FK_EMPCODE)
            //    .Distinct()
            //    .ToList();

            //var filteredEmployees = employees
            //    .Where(e => filteredGratuityData.Contains(e.PK_EMPCODE))
            //    .ToList();

            //return Json(filteredEmployees, JsonRequestBehavior.AllowGet);
            return Json( JsonRequestBehavior.AllowGet);
        }
    }
}
