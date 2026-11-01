using ClosedXML.Excel;
using ClosedXML.Graphics;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using HRMS.CrystalReports;
using HRMS.Models;
using HRMS.Models.Master;
using HRMS.Models.TRN;
using Newtonsoft.Json;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using static HRMS.Models.Model_Common;

namespace HRMS.Controllers.Report
{
    public class ReportController : Controller
    {
        // GET: Report
        public async Task<ActionResult> Index(string unit)
        {
            var unitddl = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll");
            //var unit = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll");
            List<ModelMaster> Lst = unitddl.Data.Select(x => new ModelMaster
            {
                Value = x.code,
                Text = x.code + "-" + x.name

            }).ToList();
            ViewBag.Unit = Lst;
            if (unit != null)
            {
                var result = await Services.GetAsync<List<M_MONTH>>(string.Format("/api/Month/GetByUnit?unit={0}", unit));
                ViewBag.Month = result.Data;
            }
            return View();
        }
        public async Task<ActionResult> Payslip(string MonthCode, string UNIT, string EmpCode)
        {
            var result = await Services.GetAsync<List<T_PAYIMAGE>>(string.Format("/api/Payimage/GetPaySlipAll?Monthcode={0}&unit={1}&paytype=P&Ptype=S", MonthCode, UNIT));

            if (result.IsSuccessStatusCode)
            {
                try
                {
                    ReportDocument rd = new ReportDocument();
                    //ReportClass rd = new ReportClass();
                    var path = Path.Combine(Server.MapPath("~/CrystalReports"), "TD_RptPaySlip.rpt");
                    rd.Load(path);
                    //rd.SetDataSource(payslipList);
                    rd.SetDataSource(result.Data);
                    //rd.DataDefinition.FormulaFields["Header1"].Text = "COMPANY NAME";
                    //rd.DataDefinition.FormulaFields["CompAddress"].Text = "COMPANY NAME ADDRESS";
                    //rd.DataDefinition.FormulaFields["CompTelFax"].Text = "Tel    34235245      :  Fax   1414141      :  Email Id : test@mail.com ";
                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    //return File(stream, "application/pdf", "CustomerList.pdf");
                    Response.AppendHeader("Content-Disposition", "inline; filename=PaySlip_" + MonthCode + ".pdf");
                    return new FileStreamResult(stream, "application/pdf");
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
            {
                Response.ClearContent();
            }
            return View();
        }

        public async Task<ActionResult> ExportCustomers()
        {

            List<object> payslipList = new List<object>();
            var t_PAYIMAGE = new
            {
                LOCA = "AGR",
                UNIT = "AGRI",
                FK_EMPCODE = "10001",
                TITLE = "MR",
                FNAME = "NAJMUDDIN",
                MNAME = "YUSUFI",
                LNAME = "BHAVNAGARWALA",
                SEX = "M",
                BANK_NAME = "EMIRATES NBD PJSC",
                AC_NO = "",
                IBAN_NO = "",
                DATEOFBIRTH = "05 - 11 - 1987 00:00",
                DATEOFJOINING = "01 - 10 - 2015 00:00",
                DEPARTMENT = "NONE",
                CURRENCY = "AED",
                SUBUNIT = "Cent",
                SHT_DESCN = "AED",
                INTERNATIONAL_FMT = "",
                DESG_DESCN = "EXECUTIVE",
                MONTH = "August",
                YR = 2024,
                BASIC_PAY = 22500,
                ACT_BASIC_PAY = 22500,
                DA_PAY = 162500,
                ACT_DA_PAY = 162500,
                OTHER_DED_AMT = 0,
                TOTAL_ALLOW_AMT = 0,
                EARNED_WAGES = 185000,
                NET_PAY = 185000,
                NET_SAL_PAID = 185000,
                ABSENT = 0,
                DAYS_PRESENT = 22,
                PAID_HOLIDAY = 0,
                TOT_PAYABLE_DAYS = 0,
                PAID_LEAVE = 0,
                WEEK_LEAVE = 9,
                SICK_LEAVE = 0,
                MATERNITY_LEAVE = 0,
                BALANCE_LEAVE = 0,
                TOTAL_DED_AMT = 0,
                MONTHDAYS = 31,
                logo_fld = @"C:\inetpub\wwwroot\Agrifields\AgriFieldsLogo\LOGO.PNG",
                DESC_1 = "Staff Loan",
                AMT_1 = 0,
                DESC_2 = "Staff Advance",
                AMT_2 = 0,
                DESC_3 = "Other Deduction",
                AMT_3 = 0,
                BALANCE_S = 0,
                BALANCE_M = 0,
                BALANCE_L = 24,
            };
            payslipList.Add(t_PAYIMAGE);

            try
            {
                ReportDocument rd = new ReportDocument();
                var path = Path.Combine(Server.MapPath("~/CrystalReports"), "TD_RptPaySlip.rpt");
                rd.Load(path);
                rd.SetDataSource(payslipList);

                rd.DataDefinition.FormulaFields["Header1"].Text = "COMPANY NAME";
                rd.DataDefinition.FormulaFields["Header2"].Text = "COMPANY NAME";
                rd.DataDefinition.FormulaFields["CompAddress"].Text = "COMPANY NAME ADDRESS";
                rd.DataDefinition.FormulaFields["CompTelFax"].Text = "'Tel    34235245      :  Fax   1414141      :  'Email Id : test@mail.com ";

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "CustomerList.pdf");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [ActionName("BankWagesPayment")]
        public async Task<ActionResult> BankWagesPayment()
        {
            var EmpTypeList = await Services.GetAsync<List<M_EMP_TYPE>>("/api/EmpType/GetAll");
            List<ModelMaster> EmpTypeLst = EmpTypeList.Data.Select(x => new ModelMaster
            {
                Value = x.pK_CODE,
                Text = x.description
            }).ToList();
            ViewBag.EmpType = EmpTypeLst;
            var DepartmentList = await Services.GetAsync<List<M_DEPARTMENT>>("/api/Department/GetAll");
            List<ModelMaster> DepartmentListLst = DepartmentList.Data.Select(x => new ModelMaster
            {
                Value = x.pK_DEPT_CODE,
                Text = x.description
            }).ToList();
            ViewBag.Department = DepartmentListLst;
            var CostCentreList = await Services.GetAsync<List<M_COST_CENTRE>>("/api/CostCentre/GetAll");
            List<ModelMaster> CostCentreListLst = CostCentreList.Data.Select(x => new ModelMaster
            {
                Value = x.pK_CODE,
                Text = x.descn
            }).ToList();
            ViewBag.CostCentre = CostCentreListLst;

            return View();
        }

        [ActionName("BankWagesPayment")]
        [HttpPost]
        public async Task<ActionResult> BankWagesPayment(string dailyMonth, string dailyUnit, string dataImport)
        //GetPayRegister(string Monthcode, string unit, string Empcode = "", string CostCentre = "", string Department = "", string EmpType = "", string paytype = "P", string Ptype = "S")
        {
            #region Graphics Report
            if (dataImport == "Graphics")
            {
                var result = await Services.GetAsync<List<T_PAYREGISTER>>(string.Format("/api/Payimage/GetPayRegister?Monthcode={0}&unit={1}&paytype=P&Ptype=S", dailyMonth, dailyUnit));

                if (result.IsSuccessStatusCode)
                {
                    try
                    {
                        ReportDocument rd = new ReportDocument();
                        //ReportClass rd = new ReportClass();
                        var path = Path.Combine(Server.MapPath("~/CrystalReports"), "BankStatement.rpt");
                        rd.Load(path);
                        rd.SetDataSource(result.Data);
                        rd.DataDefinition.FormulaFields["Header"].Text = "'" + (string)HttpContext.Session["SelectedCompany"] + "'";
                        rd.DataDefinition.FormulaFields["Header1"].Text = "'" + (string)HttpContext.Session["SelectedLocation"] + "'";
                       // rd.DataDefinition.FormulaFields["Header2"].Text = "'BANK STATEMENT FOR THE MONTH OF " + result.Data.FirstOrDefault().FROM_PERIOD.ToString("MMMM") + " " + result.Data.FirstOrDefault().FROM_PERIOD.Year.ToString() + "'";
                        Response.Buffer = false;
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                        stream.Seek(0, SeekOrigin.Begin);
                        //return File(stream, "application/pdf", "CustomerList.pdf");
                        Response.AppendHeader("Content-Disposition", "inline; filename=BankStatement_" + dailyMonth + ".pdf");
                        return new FileStreamResult(stream, "application/pdf");
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                else
                {
                    Response.ClearContent();
                }
            }
            #endregion 

            #region Excel Report
            if (dataImport == "Excel")
            {
                //var exceldata=GetBankWagesPayment_Excel(dailyMonth, dailyUnit);
                var result = await Services.GetAsync<List<T_PAYREGISTER>>(string.Format("/api/Payimage/GetPayRegister?Monthcode={0}&unit={1}&paytype=P&Ptype=S", dailyMonth, dailyUnit));

                var rowcount = result.Data.ToList().Count;
                if (rowcount == 0)
                {
                    return Json(new { error = "No data available" }, JsonRequestBehavior.AllowGet);
                }

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("PaymenytRegister");

                    int r = 1;
                    worksheet.Cell(r, 1).Value = (string)HttpContext.Session["SelectedCompany"];
                    worksheet.Range(r, 1, r, 4).Merge().Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    r++;
                    var currentDate = DateTime.Now.ToString("dd-MMM-yyyy");
                    //worksheet.Cell(r, 1).Value = $"EMPLOYEE SALARY PAYMENT FOR THE MONTH OF " + dailyMonth;
                    worksheet.Cell(r, 1).Value = $"EMPLOYEE SALARY PAYMENT FOR THE MONTH OF {result.Data.FirstOrDefault()?.FROM_PERIOD?.ToString("dd-MMM-yyyy") ?? "N/A"} TO {result.Data.FirstOrDefault()?.TO_PERIOD?.ToString("dd-MMM-yyyy") ?? "N/A"}"; ;
                    worksheet.Range(r, 1, r, 4).Merge().Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    r = 4;
                    int c = 1;
                    worksheet.Cell(r, c).Value = "Sl No"; c++;
                    worksheet.Cell(r, c).Value = "Code"; c++;
                    worksheet.Cell(r, c).Value = "Name"; c++;
                    worksheet.Cell(r, c).Value = "Bank Name"; c++;
                    worksheet.Cell(r, c).Value = "A/C No."; c++;
                    worksheet.Cell(r, c).Value = "Amount";
                    worksheet.Range(r, 1, r, c).Style.Font.SetBold();
                    worksheet.Range(r, 1, r, c).Style.Fill.BackgroundColor.Color.ToArgb();

                    r++;
                    int slno = 1;
                    decimal tot = 0;
                    foreach (var item in result.Data.ToList())
                    {
                        c = 1;
                        worksheet.Cell(r, c).Value = slno; c++;
                        worksheet.Cell(r, c).Value = item.FK_EMPCODE; c++;
                        worksheet.Cell(r, c).Value = item.FNAME + ' ' + item.MNAME + ' ' + item.LNAME; c++;
                        worksheet.Cell(r, c).Value = item.BANK_NAME; c++;
                        worksheet.Cell(r, c).Value = item.AC_NO; c++;
                        worksheet.Cell(r, c).Value = item.NET_PAY;

                        tot = tot + item.NET_PAY;
                        slno++;
                        r++;
                    }
                    c = 1;
                    c++;
                    worksheet.Cell(r, c).Value = "Total"; c++; c++; c++; c++;
                    worksheet.Cell(r, c).Value = tot;
                    worksheet.Range(r, 1, r, c).Style.Font.SetBold();

                    worksheet.Columns().AdjustToContents();

                    using (var stream = new System.IO.MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BankPaymentRegister.xlsx");
                    }
                }
            }
            #endregion
            return View();
        }



        [ActionName("LeaveRegister")]
        public async Task<ActionResult> LeaveRegister()
        {
            return View();
        }

        [ActionName("LeaveRegister")]
        [HttpPost]
        public async Task<ActionResult> LeaveRegister(string dailyMonth, string dailyUnit, string dataImport)
        {
            #region Graphics Report
            if (dataImport == "Graphics")
            {
                var result = await Services.GetAsync<List<T_LEAVEREGISTER>>(string.Format("/api/LeaveReport/GetLeaveRegister?location={1}&Monthcode={0}&LeaveType=LD&paytype=P&Ptype=S", dailyMonth, dailyUnit));
                if (result.IsSuccessStatusCode)
                {
                    try
                    {
                        ReportDocument rd = new ReportDocument();
                        //ReportClass rd = new ReportClass();
                        var path = Path.Combine(Server.MapPath("~/CrystalReports"), "LeaveRegister2.rpt");
                        rd.Load(path);
                        rd.SetDataSource(result.Data);
                        rd.DataDefinition.FormulaFields["Header"].Text = "'" + (string)HttpContext.Session["SelectedCompany"] + "'";
                        rd.DataDefinition.FormulaFields["Header1"].Text = "'" + (string)HttpContext.Session["SelectedLocation"] + "'";
                        rd.DataDefinition.FormulaFields["Header2"].Text = "'LEAVE REGISTER FOR THE MONTH " + result.Data.FirstOrDefault().FROM_PERIOD.ToString("MMMM") + " " + result.Data.FirstOrDefault().FROM_PERIOD.Year.ToString() + "'";
                        rd.DataDefinition.FormulaFields["Header3"].Text = "'" + result.Data.FirstOrDefault().LEAVE_DESC + "'";

                        Response.Buffer = false;
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                        stream.Seek(0, SeekOrigin.Begin);
                        //return File(stream, "application/pdf", "CustomerList.pdf");
                        Response.AppendHeader("Content-Disposition", "inline; filename=LeaveRegister_" + dailyMonth + ".pdf");
                        return new FileStreamResult(stream, "application/pdf");
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                else
                {
                    Response.ClearContent();
                }
            }
            #endregion
            #region Excel Report
            if (dataImport == "Excel")
            {
                var result = await Services.GetAsync<List<T_LEAVEREGISTER>>(string.Format("/api/LeaveReport/GetLeaveRegister?location={1}&Monthcode={0}&LeaveType=LD&paytype=P&Ptype=S", dailyMonth, dailyUnit));

                var rowcount = result.Data.ToList().Count;
                if (rowcount == 0)
                {
                    return Json(new { error = "No data available" }, JsonRequestBehavior.AllowGet);
                }

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("LeaveRegister");

                    int r = 1;
                    worksheet.Cell(r, 1).Value = (string)HttpContext.Session["SelectedCompany"];        // "JAY SHREE TEA & INDUSTRIES LTD. HEAD_OFFICE";
                    worksheet.Range(r, 1, r, 4).Merge().Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    r++;
                    var currentDate = DateTime.Now.ToString("dd-MMM-yyyy");
                    worksheet.Cell(r, 1).Value = $"LEAVE REGISTER FOR THE MONTH OF " + result.Data.FirstOrDefault().FROM_PERIOD.ToString("MMMM") + " " + result.Data.FirstOrDefault().FROM_PERIOD.Year.ToString();
                    worksheet.Range(r, 1, r, 4).Merge().Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    r = 4;
                    int c = 1;
                    worksheet.Cell(r, c).Value = "Sl No"; c++;
                    worksheet.Cell(r, c).Value = "Code"; c++;
                    worksheet.Cell(r, c).Value = "Name"; c++;
                    worksheet.Cell(r, c).Value = "Opening"; c++;
                    worksheet.Cell(r, c).Value = "Alloted"; c++;
                    worksheet.Cell(r, c).Value = "Taken"; c++;
                    worksheet.Cell(r, c).Value = "Closing";
                    worksheet.Range(r, 1, r, c).Style.Font.SetBold();
                    worksheet.Range(r, 1, r, c).Style.Fill.BackgroundColor.Color.ToArgb();

                    r++;
                    int slno = 1;
                    decimal tot_OP = 0; decimal tot_ALOT = 0; decimal tot_TAKEN = 0; decimal tot_CLBAL = 0;
                    foreach (var item in result.Data.ToList())
                    {
                        c = 1;
                        worksheet.Cell(r, c).Value = slno; c++;
                        worksheet.Cell(r, c).Value = item.FK_EMPCODE; c++;
                        worksheet.Cell(r, c).Value = item.FNAME + ' ' + item.MNAME + ' ' + item.LNAME; c++;
                        worksheet.Cell(r, c).Value = item.OPENING; c++;
                        worksheet.Cell(r, c).Value = item.ALLOTED; c++;
                        worksheet.Cell(r, c).Value = item.DAYS; c++;
                        worksheet.Cell(r, c).Value = item.OPENING + item.ALLOTED - item.DAYS; c++;
                        tot_OP = tot_OP + item.OPENING;
                        tot_ALOT = tot_ALOT + item.ALLOTED;
                        tot_TAKEN = tot_TAKEN + item.DAYS;
                        tot_CLBAL = tot_CLBAL + (item.OPENING + item.ALLOTED - item.DAYS);
                        slno++;
                        r++;
                    }
                    c = 1;
                    c++;
                    worksheet.Cell(r, c).Value = "Total"; c++; c++;
                    worksheet.Cell(r, c).Value = tot_OP; c++;
                    worksheet.Cell(r, c).Value = tot_ALOT; c++;
                    worksheet.Cell(r, c).Value = tot_TAKEN; c++;
                    worksheet.Cell(r, c).Value = tot_CLBAL;
                    worksheet.Range(r, 1, r, c).Style.Font.SetBold();

                    worksheet.Columns().AdjustToContents();

                    using (var stream = new System.IO.MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LeaveRegister.xlsx");
                    }
                }
            }
            #endregion
            return View();
        }

        public async Task<ActionResult>MonthlyChecklistReport()
        {
            var CostCentreList = await Services.GetAsync<List<M_COST_CENTRE>>("/api/CostCentre/GetAll");
            List<ModelMaster> CostCentreListLst = CostCentreList.Data.Select(x => new ModelMaster
            {
                Value = x.pK_CODE,
                Text = x.descn
            }).ToList();
            ViewBag.CostCentre = CostCentreListLst;

            var EmpTypeList = await Services.GetAsync<List<M_EMP_TYPE>>("/api/EmpType/GetAll");
            List<ModelMaster> EmpTypeLst = EmpTypeList.Data.Select(x => new ModelMaster
            {
                Value = x.pK_CODE,
                Text = x.description
            }).ToList();
            ViewBag.EmpType = EmpTypeLst;

            var monthcode = await Services.GetAsync<List<M_MONTH>>("/api/Month/GetAll");
            List<ModelMaster> month = monthcode.Data.Select(x => new ModelMaster
            {
                Value = x.pK_MONTHCODE,
                Text = x.MonthName
            }).ToList();
            ViewBag.Month = month;

            return View();
        }


        [ActionName("MonthlyChecklistReport")]
        [HttpPost]
        public async Task<ActionResult> MonthlyChecklistReport(string Month, string unit, string employee, string dataradio)
        {
            if (dataradio == "monthly")
            {
                var result = await Services.GetAsync<List<T_PAYREGISTER>>(string.Format("/api/Payimage/GetPayRegister?Monthcode={0}&unit={1}&Empcode={2}&paytype=P&Ptype=S", Month, unit, employee));

                var rowcount = result.Data.ToList().Count;
                if (rowcount == 0)
                {
                    return Json(new { error = "No data available" }, JsonRequestBehavior.AllowGet);
                }

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Monthly_Pay_File");

                    int r = 1;
                    worksheet.Cell(r, 1).Value = (string)HttpContext.Session["SelectedCompany"];        
                    worksheet.Range(r, 1, r, 5).Merge().Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    r++;
                    var currentDate = DateTime.Now.ToString("dd-MMM-yyyy");
                    worksheet.Cell(r, 1).Value = $"Monthly Pay file Report from{result.Data.FirstOrDefault()?.FROM_PERIOD?.ToString("dd-MMM-yyyy") ?? "N/A"} TO {result.Data.FirstOrDefault()?.TO_PERIOD?.ToString("dd-MMM-yyyy") ?? "N/A"}";
                    worksheet.Range(r, 1, r, 5).Merge().Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    r = 4;
                    int c = 1;
                    worksheet.Cell(r, c).Value = "Sl No"; c++;
                    worksheet.Cell(r, c).Value = "Unit"; c++;
                    worksheet.Cell(r, c).Value = "Unit Description"; c++;
                    worksheet.Cell(r, c).Value = "Employee ID"; c++;
                    worksheet.Cell(r, c).Value = "Employee Name"; c++;
                    worksheet.Cell(r, c).Value = "Designation"; c++;
                    worksheet.Cell(r, c).Value = "Department"; c++;
                    worksheet.Cell(r, c).Value = "Cost Centre"; c++;
                    worksheet.Cell(r, c).Value = "UAN NO."; c++;
                    worksheet.Cell(r, c).Value = "ESI NO"; c++;
                    worksheet.Cell(r, c).Value = "Bank Name"; c++;
                    worksheet.Cell(r, c).Value = "BANK A/C NO"; c++;
                    worksheet.Cell(r, c).Value = "IFSC CODE"; c++;
                    worksheet.Cell(r, c).Value = "Date of Birth"; c++;
                    worksheet.Cell(r, c).Value = "Date of Joining"; c++;
                    worksheet.Cell(r, c).Value = "Pay Type"; c++;
                    worksheet.Cell(r, c).Value = "Present Days"; c++;
                    worksheet.Cell(r, c).Value = "Week Off"; c++;
                    worksheet.Cell(r, c).Value = "Paid Holiday"; c++;
                    worksheet.Cell(r, c).Value = "Total Days"; c++;
                    worksheet.Cell(r, c).Value = "Arrear Days"; c++;
                    worksheet.Cell(r, c).Value = "Arrear Month"; c++;
                    worksheet.Cell(r, c).Value = "Leave Encash Days"; c++;
                    worksheet.Cell(r, c).Value = "Leave without Pay"; c++;
                    worksheet.Cell(r, c).Value = "Leave with Pay"; c++;
                    worksheet.Cell(r, c).Value = "Extra days"; c++;
                    worksheet.Cell(r, c).Value = "Late Days"; c++;
                    worksheet.Cell(r, c).Value = "Absent /Unpaid"; c++;
                    worksheet.Cell(r, c).Value = "Basic Pay"; c++;
                    worksheet.Cell(r, c).Value = "HRA"; c++;
                    worksheet.Cell(r, c).Value = "CA"; c++;
                    worksheet.Cell(r, c).Value = "Total"; c++;
                    worksheet.Cell(r, c).Value = "Arrear Pay"; c++;
                    worksheet.Cell(r, c).Value = "Other Allowance"; c++;
                    worksheet.Cell(r, c).Value = "Incentive"; c++;
                    worksheet.Cell(r, c).Value = "Total"; c++;
                    worksheet.Cell(r, c).Value = "Providend Fund"; c++;
                    worksheet.Cell(r, c).Value = "E.S.I"; c++;
                    worksheet.Cell(r, c).Value = "Professional Tax"; c++;
                    worksheet.Cell(r, c).Value = "Income Tax"; c++;
                    worksheet.Cell(r, c).Value = "Other Deduction"; c++;
                    worksheet.Cell(r, c).Value = "Total Deduction"; c++;
                    worksheet.Cell(r, c).Value = "Net Pay";
                    worksheet.Range(r, 1, r, c).Style.Font.SetBold();
                    worksheet.Range(r, 1, r, c).Style.Fill.BackgroundColor = XLColor.DarkBlue;  // Dark blue background
                    worksheet.Range(r, 1, r, c).Style.Font.FontColor = XLColor.White;
                    r++;
                    int slno = 1;
                    foreach (var item in result.Data.ToList())
                    {
                        c = 1;
                        worksheet.Cell(r, c).Value = slno; c++;
                        worksheet.Cell(r, c).Value = item.UNIT; c++;
                        worksheet.Cell(r, c).Value = item.DESCRIPTION; c++;
                        worksheet.Cell(r, c).Value = item.FK_EMPCODE; c++;
                        worksheet.Cell(r, c).Value = item.FNAME + ' ' + item.MNAME + ' ' + item.LNAME; c++;
                        worksheet.Cell(r, c).Value = item.DESG_DESCN; c++;
                        worksheet.Cell(r, c).Value = item.DESCRIPTION; c++;
                        worksheet.Cell(r, c).Value = item.DESCN; c++;
                        worksheet.Cell(r, c).Value = item.UAN; c++;
                        worksheet.Cell(r, c).Value = item.ESINO; c++;
                        worksheet.Cell(r, c).Value = item.BANK_NAME; c++;
                        worksheet.Cell(r, c).Value = item.AC_NO; c++;
                        worksheet.Cell(r, c).Value = item.IFSC_CODE; c++;
                        worksheet.Cell(r, c).Value = item.DATEOFBIRTH;
                        worksheet.Cell(r, c).Style.NumberFormat.Format = "dd/MM/yyyy"; c++;
                        worksheet.Cell(r, c).Value = item.DATEOFJOINING;
                        worksheet.Cell(r, c).Style.NumberFormat.Format = "dd/MM/yyyy"; c++;
                        worksheet.Cell(r, c).Value = item.PAY_TYPE; c++;
                        worksheet.Cell(r, c).Value = item.DAYS_PRESENT; c++;
                        worksheet.Cell(r, c).Value = item.WEEK_LEAVE; c++;
                        worksheet.Cell(r, c).Value = item.PAID_HOLIDAY; c++;
                        worksheet.Cell(r, c).Value = item.TotalDays; c++;
                        worksheet.Cell(r, c).Value = item.ARREAR_DAY; c++;
                        worksheet.Cell(r, c).Value = item.ARREAR_MONTH; c++;
                        worksheet.Cell(r, c).Value = item.LeaveEncashDays; c++;
                        worksheet.Cell(r, c).Value = item.LeavewithoutPay; c++;
                        worksheet.Cell(r, c).Value = item.LeavewithPay; c++;
                        worksheet.Cell(r, c).Value = item.Extradays; c++;
                        worksheet.Cell(r, c).Value = item.LateDays; c++;
                        worksheet.Cell(r, c).Value = item.ABSENT; c++;
                        worksheet.Cell(r, c).Value = item.BASIC_PAY; c++;
                        worksheet.Cell(r, c).Value = item.HRA_PAY; c++;
                        worksheet.Cell(r, c).Value = item.CA; c++;
                        worksheet.Cell(r, c).Value = item.BasicpayHRACATotal; c++;
                        worksheet.Cell(r, c).Value = item.ArrearPay; c++;
                        worksheet.Cell(r, c).Value = item.OtherAllowance; c++;
                        worksheet.Cell(r, c).Value = item.Incentive; c++;
                        worksheet.Cell(r, c).Value = item.TOTAL_ALLOW_AMT; c++;
                        worksheet.Cell(r, c).Value = item.PF; c++;
                        worksheet.Cell(r, c).Value = item.ESI; c++;
                        worksheet.Cell(r, c).Value = item.PTAX_AMT; c++;
                        worksheet.Cell(r, c).Value = item.INCOMETAX_AMT; c++;
                        worksheet.Cell(r, c).Value = item.OTHER_DED_AMT; c++;
                        worksheet.Cell(r, c).Value = item.TOTAL_DED_AMT; c++;
                        worksheet.Cell(r, c).Value = item.NET_PAY; c++;
                        slno++;
                        r++;
                    }


                    worksheet.Columns().AdjustToContents();

                    using (var stream = new System.IO.MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MonthlyChecklist.xlsx");
                    }
                }
            }
            else if (dataradio == "PFAttendance")
            {
                var result = await Services.GetAsync<List<T_PAYREGISTER>>(string.Format("/api/Payimage/GetAttendanceFile?Monthcode={0}&unit={1}&Empcode={2}&paytype=P&Ptype=S", Month, unit, employee));
                var rowcount = result.Data.ToList().Count;
                if (rowcount == 0)
                {
                    return Json(new { error = "No data available" }, JsonRequestBehavior.AllowGet);
                }

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Attendance_Report");

                    int r = 1;
                    worksheet.Cell(r, 1).Value = (string)HttpContext.Session["SelectedCompany"];        // "JAY SHREE TEA & INDUSTRIES LTD. HEAD_OFFICE";
                    worksheet.Range(r, 1, r, 5).Merge().Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    r++;
                    var currentDate = DateTime.Now.ToString("dd-MMM-yyyy");
                    worksheet.Cell(r, 1).Value = $"Attendance File Report from {result.Data.FirstOrDefault()?.FROM_PERIOD?.ToString("dd-MMM-yyyy") ?? "N/A"} TO {result.Data.FirstOrDefault()?.TO_PERIOD?.ToString("dd-MMM-yyyy") ?? "N/A"}";
                    worksheet.Range(r, 1, r, 5).Merge().Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    r = 4;
                    int c = 1;
                    worksheet.Cell(r, c).Value = "slno"; c++;
                    worksheet.Cell(r, c).Value = "Unit"; c++;
                    worksheet.Cell(r, c).Value = "Unit Description"; c++;
                    worksheet.Cell(r, c).Value = "Employee ID"; c++;
                    worksheet.Cell(r, c).Value = "Employee Name"; c++;
                    worksheet.Cell(r, c).Value = "Designation"; c++;
                    worksheet.Cell(r, c).Value = "Department"; c++;
                    worksheet.Cell(r, c).Value = "Cost Centre"; c++;
                    worksheet.Cell(r, c).Value = "UAN NO."; c++;
                    worksheet.Cell(r, c).Value = "ESI NO"; c++;
                    worksheet.Cell(r, c).Value = "Bank Name"; c++;
                    worksheet.Cell(r, c).Value = "BANK A/C NO"; c++;
                    worksheet.Cell(r, c).Value = "IFSC CODE"; c++;
                    worksheet.Cell(r, c).Value = "Date of Birth"; c++;
                    worksheet.Cell(r, c).Value = "Date of Joining"; c++;
                    worksheet.Cell(r, c).Value = "Pay Type"; c++;
                    worksheet.Cell(r, c).Value = "Present Days"; c++;
                    worksheet.Cell(r, c).Value = "Week Off"; c++;
                    worksheet.Cell(r, c).Value = "Paid Holiday"; c++;
                    worksheet.Cell(r, c).Value = "Total Days"; c++;
                    worksheet.Cell(r, c).Value = "Arrear Days"; c++;
                    worksheet.Cell(r, c).Value = "Arrear Month"; c++;
                    worksheet.Cell(r, c).Value = "Leave Encash Days"; c++;
                    worksheet.Cell(r, c).Value = "Leave without Pay"; c++;
                    worksheet.Cell(r, c).Value = "Leave with Pay"; c++;
                    worksheet.Cell(r, c).Value = "Extra days"; c++;
                    worksheet.Cell(r, c).Value = "Late Days"; c++;
                    worksheet.Cell(r, c).Value = "Absent /Unpaid"; c++;
                    worksheet.Range(r, 1, r, c).Style.Font.SetBold();
                    worksheet.Range(r, 1, r, c).Style.Fill.BackgroundColor = XLColor.DarkBlue;
                    worksheet.Range(r, 1, r, c).Style.Font.FontColor = XLColor.White;
                    r++;
                    int slno = 1;
                    foreach (var item in result.Data.ToList())
                    {
                        c = 1;
                        worksheet.Cell(r, c).Value = slno; c++;
                        worksheet.Cell(r, c).Value = item.UNIT; c++;
                        worksheet.Cell(r, c).Value = item.DESCRIPTION; c++;
                        worksheet.Cell(r, c).Value = item.FK_EMPCODE; c++;
                        worksheet.Cell(r, c).Value = item.FNAME + ' ' + item.MNAME + ' ' + item.LNAME; c++;
                        worksheet.Cell(r, c).Value = item.DESG_DESCN; c++;
                        worksheet.Cell(r, c).Value = item.DESCRIPTION; c++;
                        worksheet.Cell(r, c).Value = item.DESCN; c++;
                        worksheet.Cell(r, c).Value = item.UAN; c++;
                        worksheet.Cell(r, c).Value = item.ESINO; c++;
                        worksheet.Cell(r, c).Value = item.BANK_NAME; c++;
                        worksheet.Cell(r, c).Value = item.AC_NO; c++;
                        worksheet.Cell(r, c).Value = item.IFSC_CODE; c++;
                        worksheet.Cell(r, c).Value = item.DATEOFBIRTH;
                        worksheet.Cell(r, c).Style.NumberFormat.Format = "dd/MM/yyyy"; c++;
                        worksheet.Cell(r, c).Value = item.DATEOFJOINING;
                        worksheet.Cell(r, c).Style.NumberFormat.Format = "dd/MM/yyyy"; c++;
                        worksheet.Cell(r, c).Value = item.PAY_TYPE; c++;
                        worksheet.Cell(r, c).Value = item.DAYS_PRESENT; c++;
                        worksheet.Cell(r, c).Value = item.WEEK_LEAVE; c++;
                        worksheet.Cell(r, c).Value = item.PAID_HOLIDAY; c++;
                        worksheet.Cell(r, c).Value = item.TotalDays; c++;
                        worksheet.Cell(r, c).Value = item.ARREAR_DAY; c++;
                        worksheet.Cell(r, c).Value = item.ARREAR_MONTH; c++;
                        worksheet.Cell(r, c).Value = item.LeaveEncashDays; c++;
                        worksheet.Cell(r, c).Value = item.LeavewithoutPay; c++;
                        worksheet.Cell(r, c).Value = item.LeavewithPay; c++;
                        worksheet.Cell(r, c).Value = item.Extradays; c++;
                        worksheet.Cell(r, c).Value = item.LateDays; c++;
                        worksheet.Cell(r, c).Value = item.ABSENT; c++;
                        slno++;
                        r++;
                    }


                    worksheet.Columns().AdjustToContents();

                    using (var stream = new System.IO.MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Attendance_Report.xlsx");
                    }
                }
            }
            return View();
        }

        public async Task<JsonResult> MonthlyChecklistReportAsync(string unit, string costcenter, string empType, string employee, string month, string reportType)
        {
            var data = await Services.GetAsync<List<T_PAYREGISTER>>(string.Format("/api/Payimage/GetPayRegister?Monthcode={0}&unit={1}&Empcode={2}&paytype=P&Ptype=S", month, unit, employee));

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> AttendanceReportAsync(string unit, string costcenter, string empType, string employee, string month, string reportType)
        {
            var data = await Services.GetAsync<List<T_PAYREGISTER>>(string.Format("/api/Payimage/GetPayRegister?Monthcode={0}&unit={1}&Empcode={2}&paytype=P&Ptype=S", month, unit, employee));
            return Json(data, JsonRequestBehavior.AllowGet);
        }



        public async Task<ActionResult> EmployeeLinkDetails()
        {

            var DepartmentList = await Services.GetAsync<List<M_DEPARTMENT>>("/api/Department/GetAll");
            List<ModelMaster> DepartmentListLst = DepartmentList.Data.Select(x => new ModelMaster
            {
                Value = x.pK_DEPT_CODE,
                Text = x.description
            }).ToList();
            ViewBag.Department = DepartmentListLst;

            var CostCentreList = await Services.GetAsync<List<M_COST_CENTRE>>("/api/CostCentre/GetAll");
            List<ModelMaster> CostCentreListLst = CostCentreList.Data.Select(x => new ModelMaster
            {
                Value = x.pK_CODE,
                Text = x.descn
            }).ToList();
            ViewBag.CostCentre = CostCentreListLst;

            var EmpTypeList = await Services.GetAsync<List<M_EMP_TYPE>>("/api/EmpType/GetAll");
            List<ModelMaster> EmpTypeLst = EmpTypeList.Data.Select(x => new ModelMaster
            {
                Value = x.pK_CODE,
                Text = x.description
            }).ToList();
            ViewBag.EmpType = EmpTypeLst;

            return View();
        }
        //[ActionName("EmployeeLinkDetails")]
        //[HttpPost]
        //public async Task<ActionResult> EmployeeLinkDetails(string Month, string unit, string employee, string dataradio)
        //{
        //    if (dataradio == "monthly")
        //    {
        //        var result = await Services.GetAsync<List<T_PAYREGISTER>>(string.Format("/api/Payimage/GetPayRegister?Monthcode={0}&unit={1}&Empcode={2}&paytype=P&Ptype=S", Month, unit, employee));

        //        var rowcount = result.Data.ToList().Count;
        //        if (rowcount == 0)
        //        {
        //            return Json(new { error = "No data available" }, JsonRequestBehavior.AllowGet);
        //        }

        //        using (var workbook = new XLWorkbook())
        //        {
        //            var worksheet = workbook.Worksheets.Add("Monthly_Pay_File");

        //            int r = 1;
        //            worksheet.Cell(r, 1).Value = (string)HttpContext.Session["SelectedCompany"];        // "JAY SHREE TEA & INDUSTRIES LTD. HEAD_OFFICE";
        //            worksheet.Range(r, 1, r, 5).Merge().Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //            r++;
        //            var currentDate = DateTime.Now.ToString("dd-MMM-yyyy");
        //            worksheet.Cell(r, 1).Value = $"Monthly Pay file Report from {result.Data.FirstOrDefault().FROM_PERIOD.ToString("dd-MMM-yyyy")} TO {result.Data.FirstOrDefault().TO_PERIOD.ToString("dd-MMM-yyyy")}";
        //            worksheet.Range(r, 1, r, 5).Merge().Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //            r = 4;
        //            int c = 1;
        //            worksheet.Cell(r, c).Value = "Sl No"; c++;
        //            worksheet.Cell(r, c).Value = "Unit"; c++;
        //            worksheet.Cell(r, c).Value = "Unit Description"; c++;
        //            worksheet.Cell(r, c).Value = "Employee ID"; c++;
        //            worksheet.Cell(r, c).Value = "Employee Name"; c++;
        //            worksheet.Cell(r, c).Value = "Designation"; c++;
        //            worksheet.Cell(r, c).Value = "Department"; c++;
        //            worksheet.Cell(r, c).Value = "Cost Centre"; c++;
        //            worksheet.Cell(r, c).Value = "UAN NO."; c++;
        //            worksheet.Cell(r, c).Value = "ESI NO"; c++;
        //            worksheet.Cell(r, c).Value = "Bank Name"; c++;
        //            worksheet.Cell(r, c).Value = "BANK A/C NO"; c++;
        //            worksheet.Cell(r, c).Value = "IFSC CODE"; c++;
        //            worksheet.Cell(r, c).Value = "Date of Birth"; c++;
        //            worksheet.Cell(r, c).Value = "Date of Joining"; c++;
        //            worksheet.Cell(r, c).Value = "Pay Type"; c++;
        //            worksheet.Cell(r, c).Value = "Present Days"; c++;
        //            worksheet.Cell(r, c).Value = "Week Off"; c++;
        //            worksheet.Cell(r, c).Value = "Paid Holiday"; c++;
        //            worksheet.Cell(r, c).Value = "Total Days"; c++;
        //            worksheet.Cell(r, c).Value = "Arrear Days"; c++;
        //            worksheet.Cell(r, c).Value = "Arrear Month"; c++;
        //            worksheet.Cell(r, c).Value = "Leave Encash Days"; c++;
        //            worksheet.Cell(r, c).Value = "Leave without Pay"; c++;
        //            worksheet.Cell(r, c).Value = "Leave with Pay"; c++;
        //            worksheet.Cell(r, c).Value = "Extra days"; c++;
        //            worksheet.Cell(r, c).Value = "Late Days"; c++;
        //            worksheet.Cell(r, c).Value = "Absent /Unpaid"; c++;
        //            worksheet.Cell(r, c).Value = "Basic Pay"; c++;
        //            worksheet.Cell(r, c).Value = "HRA"; c++;
        //            worksheet.Cell(r, c).Value = "CA"; c++;
        //            worksheet.Cell(r, c).Value = "Total"; c++;
        //            worksheet.Cell(r, c).Value = "Arrear Pay"; c++;
        //            worksheet.Cell(r, c).Value = "Other Allowance"; c++;
        //            worksheet.Cell(r, c).Value = "Incentive"; c++;
        //            worksheet.Cell(r, c).Value = "Total"; c++;
        //            worksheet.Cell(r, c).Value = "Providend Fund"; c++;
        //            worksheet.Cell(r, c).Value = "E.S.I"; c++;
        //            worksheet.Cell(r, c).Value = "Professional Tax"; c++;
        //            worksheet.Cell(r, c).Value = "Income Tax"; c++;
        //            worksheet.Cell(r, c).Value = "Other Deduction"; c++;
        //            worksheet.Cell(r, c).Value = "Total Deduction"; c++;
        //            worksheet.Cell(r, c).Value = "Net Pay";
        //            worksheet.Range(r, 1, r, c).Style.Font.SetBold();
        //            worksheet.Range(r, 1, r, c).Style.Fill.BackgroundColor = XLColor.DarkBlue;  // Dark blue background
        //            worksheet.Range(r, 1, r, c).Style.Font.FontColor = XLColor.White;
        //            r++;
        //            int slno = 1;
        //            foreach (var item in result.Data.ToList())
        //            {
        //                c = 1;
        //                worksheet.Cell(r, c).Value = slno; c++;
        //                worksheet.Cell(r, c).Value = item.UNIT; c++;
        //                worksheet.Cell(r, c).Value = item.DESCRIPTION; c++;
        //                worksheet.Cell(r, c).Value = item.FK_EMPCODE; c++;
        //                worksheet.Cell(r, c).Value = item.FNAME + ' ' + item.MNAME + ' ' + item.LNAME; c++;
        //                worksheet.Cell(r, c).Value = item.DESG_DESCN; c++;
        //                worksheet.Cell(r, c).Value = item.DESCRIPTION; c++;
        //                worksheet.Cell(r, c).Value = item.DESCN; c++;
        //                worksheet.Cell(r, c).Value = item.UAN; c++;
        //                worksheet.Cell(r, c).Value = item.ESINO; c++;
        //                worksheet.Cell(r, c).Value = item.BANK_NAME; c++;
        //                worksheet.Cell(r, c).Value = item.AC_NO; c++;
        //                worksheet.Cell(r, c).Value = item.IFSC_CODE; c++;
        //                worksheet.Cell(r, c).Value = item.DATEOFBIRTH;
        //                worksheet.Cell(r, c).Style.NumberFormat.Format = "dd/MM/yyyy"; c++;
        //                worksheet.Cell(r, c).Value = item.DATEOFJOINING;
        //                worksheet.Cell(r, c).Style.NumberFormat.Format = "dd/MM/yyyy"; c++;
        //                worksheet.Cell(r, c).Value = item.PAY_TYPE; c++;
        //                worksheet.Cell(r, c).Value = item.DAYS_PRESENT; c++;
        //                worksheet.Cell(r, c).Value = item.WEEK_LEAVE; c++;
        //                worksheet.Cell(r, c).Value = item.PAID_HOLIDAY; c++;
        //                worksheet.Cell(r, c).Value = item.TotalDays; c++;
        //                worksheet.Cell(r, c).Value = item.ARREAR_DAY; c++;
        //                worksheet.Cell(r, c).Value = item.ARREAR_MONTH; c++;
        //                worksheet.Cell(r, c).Value = item.LeaveEncashDays; c++;
        //                worksheet.Cell(r, c).Value = item.LeavewithoutPay; c++;
        //                worksheet.Cell(r, c).Value = item.LeavewithPay; c++;
        //                worksheet.Cell(r, c).Value = item.Extradays; c++;
        //                worksheet.Cell(r, c).Value = item.LateDays; c++;
        //                worksheet.Cell(r, c).Value = item.ABSENT; c++;
        //                worksheet.Cell(r, c).Value = item.BASIC_PAY; c++;
        //                worksheet.Cell(r, c).Value = item.HRA_PAY; c++;
        //                worksheet.Cell(r, c).Value = item.CA; c++;
        //                worksheet.Cell(r, c).Value = item.BasicpayHRACATotal; c++;
        //                worksheet.Cell(r, c).Value = item.ArrearPay; c++;
        //                worksheet.Cell(r, c).Value = item.OtherAllowance; c++;
        //                worksheet.Cell(r, c).Value = item.Incentive; c++;
        //                worksheet.Cell(r, c).Value = item.TOTAL_ALLOW_AMT; c++;
        //                worksheet.Cell(r, c).Value = item.PF; c++;
        //                worksheet.Cell(r, c).Value = item.ESI; c++;
        //                worksheet.Cell(r, c).Value = item.PTAX_AMT; c++;
        //                worksheet.Cell(r, c).Value = item.INCOMETAX_AMT; c++;
        //                worksheet.Cell(r, c).Value = item.OTHER_DED_AMT; c++;
        //                worksheet.Cell(r, c).Value = item.TOTAL_DED_AMT; c++;
        //                worksheet.Cell(r, c).Value = item.NET_PAY; c++;
        //                slno++;
        //                r++;
        //            }


        //            worksheet.Columns().AdjustToContents();

        //            using (var stream = new System.IO.MemoryStream())
        //            {
        //                workbook.SaveAs(stream);
        //                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MonthlyChecklist.xlsx");
        //            }
        //        }
        //    }
            
        //    return View();
        //}


    }
}




