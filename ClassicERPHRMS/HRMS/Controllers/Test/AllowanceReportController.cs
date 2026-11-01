using HRMS.Models.Master;
using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static HRMS.Models.Model_Common;
using CrystalDecisions.CrystalReports.Engine;
using Newtonsoft.Json;
using System.Data;
using System.IO;
//using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using ClosedXML.Excel;
using HRMS.Models.TRN;


namespace HRMS.Controllers.Test
{
    public class AllowanceReportController : Controller
    {
        // GET: AllowanceReport
        public async Task<ActionResult> AllowanceReport()
        {
            var Unit = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll");
            List<ModelMaster> Lst = Unit.Data.Select(x => new ModelMaster
            {
                Value = x.code,
                Text = x.code + "-" + x.name

            }).ToList();
            ViewBag.Unit = Lst;
            return View();
        }
        public async Task<ActionResult> Exportallocation(string Unit)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                var path = Path.Combine(Server.MapPath("~/CrystalReports"), "allowance master.rpt");
                rd.Load(path);
                var ailowcal = await Services.GetAsync<List<M_ALLOWANCE>>(string.Format("/api/Allowance/GetByUnit?unit={0}", Unit));
                var Allowcalcmode = await Services.GetAsync<List<M_AllOW_CALC_MODE>>("/api/AllowCalcMode/GetAll");
                var companyname = @Session["SelectedCompany"];
                foreach (var item in ailowcal.Data)
                {

                    item._companyname = companyname.ToString();

                    if (item.incL_ESI == true)
                    {
                        item.bool_INCL_ESI = "Yes";
                    }
                    else
                    {
                        item.bool_INCL_ESI = "No";
                    }

                    if (item.incL_GROSS == true)
                    {
                        item.bool_incL_GROSS = "Yes";
                    }
                    else
                    {
                        item.bool_incL_GROSS = "No";
                    }

                    if (item.incL_OT == true)
                    {
                        item.bool_INCL_OT = "Yes";
                    }
                    else
                    {
                        item.bool_INCL_OT = "No";
                    }

                    if (item.incL_PF == true)
                    {
                        item.bool_INCL_PF = "Yes";
                    }
                    else
                    {
                        item.bool_INCL_PF = "No";
                    }

                    if (item.incL_TAXABLE == true)
                    {
                        item.bool_INCL_TAXABLE = "Yes";
                    }
                    else
                    {
                        item.bool_INCL_TAXABLE = "No";
                    }

                    if (item.incL_PTAX == true)
                    {
                        item.bool_INCL_PTAX = "Yes";
                    }
                    else
                    {
                        item.bool_INCL_PTAX = "No";
                    }

                    if (item.includE_VOUCHER == true)
                    {
                        item.boolincludE_VOUCHER = "Yes";
                    }
                    else
                    {
                        item.boolincludE_VOUCHER = "No";
                    }


                    var allowcalmode = Allowcalcmode.Data.FirstOrDefault(u => u.pK_CALC_MODE == item.fK_CALC_MODE);
                    if (allowcalmode != null)
                    {
                        item.fK_CALC_MODE = allowcalmode.description;
                    }
                }


                var allowancelist = ailowcal.Data.ToList();

                //allowance allowance = new allowance();

                var dataTable2 = JsonConvert.SerializeObject(allowancelist);

                var dataTable = JsonConvert.DeserializeObject<DataTable>(dataTable2);
                //allowance dsCustomers = GetData(t_PAYIMAGE);
                rd.SetDataSource(dataTable);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "allowancemaster.pdf");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ActionResult> GetExcelList(string Unit)
        {
            var ailowcal = await Services.GetAsync<List<M_ALLOWANCE>>(string.Format("/api/Allowance/GetByUnit?unit={0}", Unit));
            var Allowcalcmode = await Services.GetAsync<List<M_AllOW_CALC_MODE>>("/api/AllowCalcMode/GetAll");
            // var companyname = @Session["SelectedCompany"];
            foreach (var item in ailowcal.Data)
            {

                //item._companyname = companyname.ToString();

                if (item.incL_ESI == true)
                {
                    item.bool_INCL_ESI = "Yes";
                }
                else
                {
                    item.bool_INCL_ESI = "No";
                }

                if (item.incL_GROSS == true)
                {
                    item.bool_incL_GROSS = "Yes";
                }
                else
                {
                    item.bool_incL_GROSS = "No";
                }

                if (item.incL_OT == true)
                {
                    item.bool_INCL_OT = "Yes";
                }
                else
                {
                    item.bool_INCL_OT = "No";
                }

                if (item.incL_PF == true)
                {
                    item.bool_INCL_PF = "Yes";
                }
                else
                {
                    item.bool_INCL_PF = "No";
                }

                if (item.incL_TAXABLE == true)
                {
                    item.bool_INCL_TAXABLE = "Yes";
                }
                else
                {
                    item.bool_INCL_TAXABLE = "No";
                }

                if (item.incL_PTAX == true)
                {
                    item.bool_INCL_PTAX = "Yes";
                }
                else
                {
                    item.bool_INCL_PTAX = "No";
                }

                if (item.includE_VOUCHER == true)
                {
                    item.boolincludE_VOUCHER = "Yes";
                }
                else
                {
                    item.boolincludE_VOUCHER = "No";
                }


                var allowcalmode = Allowcalcmode.Data.FirstOrDefault(u => u.pK_CALC_MODE == item.fK_CALC_MODE);
                if (allowcalmode != null)
                {
                    item.fK_CALC_MODE = allowcalmode.description;
                }
            }


            var data = ailowcal.Data.ToList();



            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("AllowanceReport");
                int r = 1;
                worksheet.Cell(r, 1).Value = (string)HttpContext.Session["SelectedCompany"];       
                worksheet.Range(r, 1, r, 5).Merge().Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                r++;
                var currentDate = DateTime.Now.ToString("dd-MMM-yyyy");
                worksheet.Cell(r, 1).Value = $"ALLOWANCE REPORT AS ON {currentDate}";
                worksheet.Range(r, 1, r, 5).Merge().Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                r = 4;
                int c = 1;
                worksheet.Cell(r, c).Value = "Sl No"; c++;
                worksheet.Cell(r, c).Value = "Allowance Code"; c++;
                worksheet.Cell(r, c).Value = "Allowance Description"; c++;
                worksheet.Cell(r, c).Value = "Short Description";     c++;
                worksheet.Cell(r, c).Value = "PF";                    c++;
                worksheet.Cell(r, c).Value = "ESI";                   c++;
                worksheet.Cell(r, c).Value = "TAXABLE";               c++;
                worksheet.Cell(r, c).Value = "PTAX";                  c++;
                worksheet.Cell(r, c).Value = "VOUCHER";               c++;
                worksheet.Cell(r, c).Value = "OT";                    c++;
                worksheet.Cell(r, c).Value ="GROSS";                  c++;
                worksheet.Cell(r, c).Value ="CALCULATION";            
                worksheet.Range(r, 1, r, c).Style.Font.SetBold();
                worksheet.Range(r, 1, r, c).Style.Fill.BackgroundColor = XLColor.DarkBlue;  // Dark blue background
                worksheet.Range(r, 1, r, c).Style.Font.FontColor = XLColor.White;
                r++;
                int slno = 1;
             
                foreach (var item in data)
                {
                    c = 1;
                    worksheet.Cell(r, c).Value = slno; c++;
                    worksheet.Cell(r, c).Value = item.pK_alL_CODE; c++;
                    worksheet.Cell(r, c).Value = item.description;c++;
                    worksheet.Cell(r, c).Value = item.shorT_NAME;c++;
                    worksheet.Cell(r, c).Value = item.incL_PF;c++;
                    worksheet.Cell(r, c).Value = item.incL_ESI;c++;
                    worksheet.Cell(r, c).Value = item.incL_TAXABLE;c++;
                    worksheet.Cell(r, c).Value = item.incL_PTAX;c++;
                    worksheet.Cell(r, c).Value = item.includE_VOUCHER; c++;
                    worksheet.Cell(r, c).Value = item.incL_OT; c++;
                    worksheet.Cell(r, c).Value = item.incL_GROSS; c++;
                    worksheet.Cell(r, c).Value = item.fK_CALC_MODE; c++;
                    

                   
                    r++;
                    slno++;
                }



            worksheet.Columns().AdjustToContents();


                using (var stream = new System.IO.MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AllowanceReport.xlsx");
                }
            }

        }
    }
}
