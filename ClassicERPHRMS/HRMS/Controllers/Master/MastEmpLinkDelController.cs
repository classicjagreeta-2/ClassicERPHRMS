using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using HRMS.Models;
using HRMS.Models.Master;
using PagedList;
using static HRMS.Models.Model_Common;
using static HRMS_VIEWMODEL24.Payroll_Common;

//using System.Windows.Interop;

namespace HRMS.Controllers.Master
{
    public class MastEmpLinkDelController : Controller
    {
        public async Task<ActionResult> SelectEmployee()
        {
            var emp = await Services.GetAsync<PageModel<M_EMPMAS>>(string.Format("/api/Employee/GetByPage?page=1&pageSize=25"));
            List<ModelMaster> EmpList = emp.Data.value.results.Select(x => new ModelMaster
            {
                Value = x.pK_EMPCODE,
                Text = x.pK_EMPCODE + "-" + x.fname + " " + x?.mname + " " + x.lname
            }).ToList();
            ViewBag.Emp = EmpList;
            return View();
        }

        // GET: MastEmpLinkDel
        [HttpGet]
        public async Task<ActionResult> MastEmpLinkDel(string EmpId, string UnitCode, string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            ViewBag.FK_ID = EmpId;
            ViewBag.DisplayName = "";
            var empLink = (dynamic)null;
            if (EmpId != null)
            {
                empLink = await Services.GetAsync<PageModel<M_EMP_LINK_DET>>(string.Format("/api/EmployeeLinkDet/GetByPage?search={0}&page={1}&pageSize={2}", EmpId, page, pageSize));
                var result = await Services.GetAsync<M_EMPMAS>(string.Format("/api/Employee/GetByCode?EMPCODE={0}", EmpId));
                ViewBag.DisplayName = result.Data.pK_EMPCODE + "-" + result.Data.fname + " " + result.Data.mname + " " + result.Data.lname;
                TempData["DisplayName"] = ViewBag.DisplayName;

                ViewBag.PageSize = pageSize;
                int pageNumber = (page ?? 1);
                var usersAsIPagedList = new StaticPagedList<M_EMP_LINK_DET>(empLink.Data.value.results, pageNumber, pageSize, empLink.Data.value.rowCount);

                var designationcode = await Services.GetAsync<List<M_DESIGNATION>>("/api/Designation/GetAll");
                foreach (var item in usersAsIPagedList)
                {


                    var designationname = designationcode.Data.FirstOrDefault(u => u.pK_DESG_CODE == item.FK_DESG_CODE);
                    if (designationname != null)
                    {
                        item.FK_DESG_CODE = designationname.pK_DESG_CODE + " : " + designationname.desG_DESCN;
                    }
                }
                var department = await Services.GetAsync<List<M_DEPARTMENT>>("/api/Department/GetAll");
                foreach (var item in usersAsIPagedList)
                {


                    var departmentname = department.Data.FirstOrDefault(u => u.pK_DEPT_CODE == item.FK_DEPT_CODE);
                    if (departmentname != null)
                    {
                        item.FK_DEPT_CODE = departmentname.pK_DEPT_CODE + " : " + departmentname.description;
                    }
                }
                var subdept = await Services.GetAsync<List<M_SUB_DEPT>>("/api/SubDept/GetAll");
                foreach (var item in usersAsIPagedList)
                {


                    var subdeptname = subdept.Data.FirstOrDefault(u => u.pK_SUB_DEPT == item.FK_SUB_DEPT_CODE);
                    if (subdeptname != null)
                    {
                        item.FK_SUB_DEPT_CODE = subdeptname.pK_SUB_DEPT + " : " + subdeptname.suB_DEPT_DESCN;
                    }
                }
                var grade = await Services.GetAsync<List<M_GRADE>>("/api/Grade/GetAll");
                foreach (var item in usersAsIPagedList)
                {


                    var gradename = grade.Data.FirstOrDefault(u => u.pK_GRADE_CODE == item.FK_GRADE_CODE);
                    if (gradename != null)
                    {
                        item.FK_GRADE_CODE = gradename.pK_GRADE_CODE + " : " + gradename.description;
                    }
                }
                var unitData = await Services.GetAsync<List<M_UNIT>>("/api/unit/GetAll");
                foreach (var item in usersAsIPagedList)
                {


                    var unitName = unitData.Data.FirstOrDefault(u => u.code == item.UNIT);
                    if (unitName != null)
                    {
                        item.FK_UNIT_CODE = unitName.code + " : " + unitName.name;
                    }
                }
                var costcenterdata = await Services.GetAsync<List<M_COST_CENTRE>>("/api/CostCentre/GetAll");
                foreach (var item in usersAsIPagedList) 
                {


                    var costcentername = costcenterdata.Data.FirstOrDefault(u => u.pK_CODE == item.FK_COST_CODE);
                    if (costcentername != null)
                    {
                        item.FK_COST_CODE = costcentername.pK_CODE + " : " + costcentername.descn;
                    }
                }
                return View(usersAsIPagedList);
            }
            else
            {
                TempData["toastrError"] = "No Employee Id Found";
                return View();
            }
        }

        [HttpPost]
        public ActionResult GetMastEmpLinkDel(PageModel pageModel, string id)
        {
            pageModel.page = 1;
            var Lst = Services.GetAsync<List<M_EMP_LINK_DET>>("/api/EmployeeLinkDet/GetByPage?search=" + pageModel.search + "&page=" + pageModel.page + "&pageSize=" + pageModel.pageSize).Result;
            return Json(new
            {
                data = Lst.Data.OrderBy(c => c.EFF_DATE)
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<ActionResult> AddEditModal(string Id, string Unit, string EmpId)
        {
            //var emp = Services.GetAsync<List<M_EMPMAS>>("/api/Employee/GetAll").Result.Data;
            var dept = await Services.GetAsync<List<M_DEPARTMENT>>("/api/Department/GetAll");
            var unit = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll");
            var cost = await Services.GetAsync<List<M_COST_CENTRE>>("/api/CostCentre/GetAll");
            var cat = await Services.GetAsync<List<M_CATEGORY>>("/api/Category/GetAll");
            var empType = await Services.GetAsync<List<M_EMP_TYPE>>("/api/EmpType/GetAll");
            var grade = await Services.GetAsync<List<M_GRADE>>("/api/Grade/GetAll");
            var subDept = await Services.GetAsync<List<M_SUB_DEPT>>("/api/SubDept/GetAll");
            var desig = await Services.GetAsync<List<M_DESIGNATION>>("/api/Designation/GetAll");
            //model.ID = 1;
            //ViewBag.YesNo = Services.GetAsync<List<M_ENUM_PARA>>("api/PAY/PAY_Master/GetData_M_ENUM_PARA").Result.Data;
            var YesNoList = new List<ModelMaster>
            {
            new ModelMaster { Value = "true", Text = "Yes" },
            new ModelMaster { Value = "false", Text = "No" }
            };
            List<ModelMaster> YesNo = YesNoList.Select(x => new ModelMaster
            {
                Value = x.Value,
                Text =  x.Text
            }).ToList();
            ViewBag.YesNo = YesNo;
            //ViewBag.Emp = emp;
            ViewBag.EmpType = empType.Data;
            ViewBag.Dept = dept.Data;
            ViewBag.Grade = grade.Data;
            ViewBag.Cost = cost.Data;
            ViewBag.Unit = unit.Data;
            ViewBag.Cat = cat.Data;
            ViewBag.SubDept = subDept.Data;
            ViewBag.Desig = desig.Data;

            if (EmpId != null)
            {
                var empResult = await Services.GetAsync<M_EMPMAS>(string.Format("/api/Employee/GetByCode?EMPCODE={0}", EmpId));
                //ViewBag.DisplayName = new  FullName = empResult.Data.pK_EMPCODE + "-" + empResult.Data.fname + " " + empResult.Data.mname + " " + empResult.Data.lname;
                ViewBag.DisplayName = empResult.Data.pK_EMPCODE + "-" + empResult.Data.fname + " " + empResult.Data.mname + " " + empResult.Data.lname;
            }

            M_EMP_LINK_DET m_EMP_LINK_DET = new M_EMP_LINK_DET();
            
            if (Convert.ToInt32(Id) <= 0)
            {
                m_EMP_LINK_DET.FK_EMPCODE = EmpId;
                return PartialView("~/Views/MastEmpLinkDel/_Master_EmpLinkAddEdit.cshtml", m_EMP_LINK_DET);
            }
            var linkResult = await Services.GetAsync<M_EMP_LINK_DET>(string.Format("/api/EmployeeLinkDet/GetByID?id={0}", Id));
            m_EMP_LINK_DET = linkResult.Data;
            return PartialView("~/Views/MastEmpLinkDel/_Master_EmpLinkAddEdit.cshtml", m_EMP_LINK_DET);
        }

        [HttpPost]
        public async Task<ActionResult> StoreOrEdit(M_EMP_LINK_DET m_EMP_LINK_DET)
        {
            try
            {
                var response = (dynamic)null;
                DateTime dtime = DateTime.Now;
                if (m_EMP_LINK_DET.FK_DEPT_CODE == null)
                {
                    m_EMP_LINK_DET.FK_DEPT_CODE =  "N/A";
                }
                if (m_EMP_LINK_DET.FK_SUB_DEPT_CODE == null)
                {
                    m_EMP_LINK_DET.FK_SUB_DEPT_CODE =  "N/A";
                }
                if (m_EMP_LINK_DET.FK_COST_CODE == null)
                {
                    m_EMP_LINK_DET.FK_COST_CODE =  "N/A";
                }
                if (m_EMP_LINK_DET.FK_GRADE_CODE == null)
                {
                    m_EMP_LINK_DET.FK_GRADE_CODE ="N/A";
                }
                if(string.IsNullOrEmpty(m_EMP_LINK_DET.UNIT)|| !string.IsNullOrEmpty(m_EMP_LINK_DET.FK_UNIT_CODE))
                {
                    m_EMP_LINK_DET.UNIT = m_EMP_LINK_DET.FK_UNIT_CODE;
                }
               
                m_EMP_LINK_DET.FK_EMPCODE = m_EMP_LINK_DET.FK_EMPCODE ?? Utility.SessionHelper.GetUser().getUserName;
                m_EMP_LINK_DET.PTYPE = "S";

                M_EMP_LINK_DET_Dto _Dto = new M_EMP_LINK_DET_Dto();
                _Dto.m_emplinkdet = m_EMP_LINK_DET;
                response = await Services.PostAsync<List<M_EMP_LINK_DET_Dto>>("/api/EmployeeLinkDet/SaveOrUpdateAll", _Dto);

                if (response.IsSuccessStatusCode)
                {
                    ////var userEmpSchemaLink1 = await Services.GetAsync<USER_SCHEMA_LINK>(string.Format("/api/UserSchemaLink/GetByEmpUnit?EmpId={0}&Unit={1}", m_EMP_LINK_DET.FK_EMPCODE, m_EMP_LINK_DET.UNIT));
                    //var userEmpSchemaLinkUrl = string.Format("/api/UserSchemaLink/GetByEmpUnitModuleDb?EmpId={0}&Unit={1}&Module={2}&Db={3}", m_EMP_LINK_DET.FK_EMPCODE, m_EMP_LINK_DET.UNIT, Helpers.UserInfo.ModuleCode, Helpers.UserInfo.getDbName);
                    //var userEmpSchemaLink = await Services.GetAsync<USER_SCHEMA_LINK>(userEmpSchemaLinkUrl);
                    //if (userEmpSchemaLink.IsSuccessStatusCode)
                    //{
                    //    if (userEmpSchemaLink.Data == null)
                    //    {
                    //        USER_SCHEMA_LINK _SCHEMA_LINK = new USER_SCHEMA_LINK
                    //        {
                    //            LOCA = m_EMP_LINK_DET.LOCA,
                    //            UNIT = m_EMP_LINK_DET.UNIT,
                    //            USER_ORA = m_EMP_LINK_DET.FK_EMPCODE,
                    //            USR_ID = Helpers.UserInfo.getDbName,
                    //            MODULE_NAME = Helpers.UserInfo.ModuleCode,
                    //            ENTDT = DateTime.Now,
                    //            ENTDT_NEW = DateTime.Now,
                    //        };
                    //        USER_SCHEMA_LINK_Dto dto = new USER_SCHEMA_LINK_Dto();
                    //        dto.uSER_SCHEMA_LINK = _SCHEMA_LINK;

                    //        var userSchemaLink = await Services.PostAsync<List<USER_SCHEMA_LINK_Dto>>("/api/UserSchemaLink/SaveOrUpdateAll", dto);
                    //        if (!userSchemaLink.IsSuccessStatusCode)
                    //        {
                    //            TempData["toastrError"] = userSchemaLink.Message;
                    //        }
                    //    }
                    //}
                    TempData["toastrSuccess"] = "Data has been successfully submitted";
                    return RedirectToAction("MastEmpLinkDel", new { EmpId = m_EMP_LINK_DET.FK_EMPCODE, UnitCode = m_EMP_LINK_DET.UNIT });
                }
                else
                {
                    TempData["toastrError"] = "Data not saved " + response.Message;
                    return RedirectToAction("MastEmpLinkDel", new { EmpId = m_EMP_LINK_DET.FK_EMPCODE, UnitCode = m_EMP_LINK_DET.UNIT });
                }
            }
            catch (Exception ex)
            {
                TempData["toastrError"] = "Something went wrong "+ ex.Message;
                return RedirectToAction("MastEmpLinkDel", new { EmpId = m_EMP_LINK_DET.FK_EMPCODE, UnitCode = m_EMP_LINK_DET.UNIT });
            }
        }
        //public async Task<JsonResult> ReturnEmpListDDLData(string searchString = null, int? page = 1, int pageSize = 25, string unit = "")
        //{
        //    var empList = await Services.GetAsync<PageModel<M_EMPMAS>>(string.Format(
        //        "/api/Employee/GetByPage?page={0}&pageSize={1}&search={2}&unit={3}", page, pageSize, searchString, unit));
        //    return Json(empList, JsonRequestBehavior.AllowGet);
        //}
    }
}