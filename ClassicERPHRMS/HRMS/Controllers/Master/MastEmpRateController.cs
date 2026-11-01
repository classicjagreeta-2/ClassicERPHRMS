using System;
using System.Collections.Generic;
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
    public class MastEmpRateController : Controller
    {
        public async Task<ActionResult> SelectEmpRate()
        {


            return View();
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


        public async Task<JsonResult> GetUnitDropdown(string searchString = null, int? page = 1, int pageSize = 5)
        {
            var response = await Services.GetAsync<PageModel<M_UNIT>>(string.Format("/api/Unit/GetByPage?page={0}&pageSize={1}", page, pageSize, Utility.SessionHelper.GetUser().getUserName));


            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> MastEmpRate(string Employee, string Unit, string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            if (Employee != null)
            {
                try
                {
                    var EmpName = await Services.GetAsync<M_EMPMAS>(string.Format("/api/Employee/GetByCode?EMPCODE={0}", Employee));
                    if (EmpName != null)
                    {
                       ViewBag.DisplayName = EmpName.Data.pK_EMPCODE + " " + EmpName.Data.fname + " " + EmpName.Data.mname + " " + EmpName.Data.lname;
                        ViewBag.EmployeeCode = EmpName.Data.pK_EMPCODE;
                        ViewBag.id = EmpName.Data.id;
                        ViewBag.unit = Unit;

                    }
                    //ViewBag.DisplayName = Bh21responce2.Data.ToList();

                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                    ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                    var responce = await Services.GetAsync<PageModel<M_EMPRATE>>(string.Format("/api/EmployeeRate/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, Employee)); // Helpers.UserInfo.ModuleCode
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
                        responce.Data.value.results = responce.Data.value.results.Where(r => r.EDATE.HasValue)
                                 .OrderByDescending(r => r.EDATE).ToList(); 
                        var usersAsIPagedList = new StaticPagedList<M_EMPRATE>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
                       
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

            TempData["toastrError"] = "Employee Code Not Found";
            return View();
        }



       

   
      

        public async Task<ActionResult> AddEditModal(string EmpCode,string Unit,int Id = 0)
        {
            M_EMPRATE emprate = new M_EMPRATE();
            if (Id != 0)
            {
                var mst_dept = await Services.GetAsync<M_EMPRATE>("/api/EmployeeRate/GetByID?id=" + Id);

                emprate = mst_dept.Data;
                emprate.EDATE = null;
                emprate.UNIT = Unit;               
            }
            else
            {
                emprate= new M_EMPRATE
                {
                    BASIC = 0m,
                    DA_RS = 0m,
                    DA_PERCENT = 0m,
                    HRA_PERCENT = 0m,
                    HRA_RS = 0m,
                    VDA_PERCENT = 0m,
                    VDA_RS = 0m,
                    CONVEYANCE_PERCENT = 0m,
                    CONVEYANCE_RS = 0m,
                    MAXPTAX = 0m,
                    ESI_PERCENT = 0m,
                    ESI_RS = 0m,
                    ESI_LIMIT = 0m,
                    PF = 0m,
                    EPF = 0m,
                    FPF = 0m,
                    SA_FUND_PER = 0m,
                    SA_FUND_AMT = 0m,
                    UNIT = Unit,
                    FK_EMPCODE = EmpCode
                };               
            }
            return View("AddEditModal", emprate);          
        }

        [HttpPost]        
        public async Task<ActionResult> AddOrUpdate(M_EMPRATE entity )
        {
            try
            {
               
                entity.USER_NAME_NEW = "";
              
              
                entity.USER_ENTDT_NEW = DateTime.Now;
           
                entity.DTAG = "";
                entity.FK_CURR_CODE = "INR";
            

                entity.PTYPE = "S";
                m_emp_Dto _Dto = new m_emp_Dto();
                _Dto.m_emprate = entity;
                var responce = await Services.PostAsync<List<m_emp_Dto>>("/api/EmployeeRate/SaveOrUpdateAll", _Dto);
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
            return RedirectToAction("MastEmpRate" , new { Employee = entity.FK_EMPCODE, Unit  = entity .UNIT});
        }
    }
}