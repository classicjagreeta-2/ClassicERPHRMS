
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HRMS.Models;
using HRMS.Models.Master;
using HRMS_VIEWMODEL24;

using Newtonsoft.Json;
using PagedList;
using static HRMS.Models.Model_Common;

namespace HRMS.Controllers.Master
{
    public class M_DepartmentController : Controller
    {
        // GET: M_Department
        public async Task<ActionResult> M_Department(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            try
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var responce = await Services.GetAsync<PageModel<M_DEPARTMENT>>(string.Format("/api/Department/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, searchString)); // Helpers.UserInfo.ModuleCode
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
                var usersAsIPagedList = new StaticPagedList<M_DEPARTMENT>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
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
   
        public async Task<ActionResult> AddEditModal(int Id = 0)
        {
            M_DEPARTMENT model = new M_DEPARTMENT();
            if (Id > 0)
            {
                var response = await Services.GetAsync<M_DEPARTMENT>("/api/Department/GetByID?id=" + Id);
                model = response.Data;
                return View(model);
            }
            else
            {
                return View(model);
            }
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(M_DEPARTMENT entity)
        {
            try
            {
                entity.useR_NAME_NEW = "";
                
               
                entity.useR_ENTDT_NEW = DateTime.Now;
           
         
                entity.dtag = "";                
             
                entity.ptype = "S";
                entity.reF_CODE = entity?.reF_CODE ?? "";
                entity.cosT_CENTRE = entity?.cosT_CENTRE ?? "";
                M_DEPARTMENT_Dto _Dto = new M_DEPARTMENT_Dto();
                _Dto.m_department= entity;
                if (entity.id == 0)
                {
                    var responce2 = await Services.GetAsync<M_DEPARTMENT>("/api/Department/GetByCode?search=" + entity.pK_DEPT_CODE);
                    if (responce2.Data != null)
                    {
                        TempData["toastrError"] = responce2.Data.pK_DEPT_CODE + " Dublicate Data Found";
                        return RedirectToAction("AddEditModal");
                    }
                    else
                    {
                        var responce = await Services.PostAsync<List<M_DEPARTMENT_Dto>>("/api/Department/SaveOrUpdateAll", _Dto);
                        if (responce.IsSuccessStatusCode == true)
                        {
                            TempData["toastrSuccess"] = "Data has been successfully submitted";
                        }
                        else
                        {
                            TempData["toastrError"] = responce.Message;
                        }
                    }
                }
                else
                {
                    var responce = await Services.PostAsync<List<M_DEPARTMENT_Dto>>("/api/Department/SaveOrUpdateAll", _Dto);
                    if (responce.IsSuccessStatusCode == true)
                    {
                        TempData["toastrSuccess"] = "Data has been successfully submitted";
                    }
                    else
                    {
                        TempData["toastrError"] = responce.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["toastrError"] = ex.Message;
            }
            return RedirectToAction("M_Department");
        }
    }
}