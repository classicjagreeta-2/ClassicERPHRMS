//using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HRMS.Controllers.Transaction
{
    public class ImportAttendanceController : Controller
    {
        // GET: ImportAttendance
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> ImportExcel()
        {
            return View();
        }
        //public async Task<ActionResult> AttendanceExcel(HttpPostedFileBase file)
        //{
        //    string path = Server.MapPath("~/Excel_Import/" + file.FileName);
        //    file.SaveAs(path);


        //    dynamic worksheet = "Classic_ERP_Employee_Master_Upload_Template";
        //    //string path = "C:/Users/User/Documents/Classic_ERP_Attendance_Upload_Template.xlsx";
        //    //string path = "C:/Users/User/Documents/Classic_ERP_Attendance_Upload_Template.xlsx";
        //    DataTable dt = new DataTable();
        //    //Open the Excel file using ClosedXML.
        //    using (XLWorkbook workBook = new XLWorkbook(path))
        //    {
        //        //Read the first Sheet from Excel file.
        //        IXLWorksheet workSheet = workBook.Worksheet(worksheet);

        //        //Create a new DataTable.

        //        //Loop through the Worksheet rows.
        //        bool firstRow = true;
        //        foreach (IXLRow row in workSheet.Rows())
        //        {
        //            //Use the first row to add columns to DataTable.
        //            if (firstRow)
        //            {
        //                foreach (IXLCell cell in row.Cells())
        //                {
        //                    if (!string.IsNullOrEmpty(cell.Value.ToString()))
        //                    {
        //                        dt.Columns.Add(cell.Value.ToString());
        //                    }
        //                    else
        //                    {
        //                        break;
        //                    }
        //                }
        //                firstRow = false;
        //            }
        //            else
        //            {
        //                int i = 0;
        //                DataRow toInsert = dt.NewRow();
        //                foreach (IXLCell cell in row.Cells(1, dt.Columns.Count))
        //                {
        //                    try
        //                    {
        //                        toInsert[i] = cell.Value.ToString();
        //                    }
        //                    catch (Exception ex)
        //                    {

        //                    }
        //                    i++;
        //                }
        //                dt.Rows.Add(toInsert);
        //            }
        //        }
        //    }
        //    return View();
        //}
    }
}