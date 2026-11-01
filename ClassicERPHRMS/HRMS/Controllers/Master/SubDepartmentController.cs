using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using HRMS.Models.Master;
using System.Web.Security;

namespace HRMS.Controllers.Master
{
    public class SubDepartmentController : Controller
    {
        // GET: SubDepartment
        public async Task<ViewResult> Index(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            var responce = await Services.GetAsync<PageModel<M_SUB_DEPT>>(string.Format("/api/SubDept/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, searchString)); // Helpers.UserInfo.ModuleCode

            if (!string.IsNullOrEmpty(searchString))
            {
                //searchString = currentFilter;
                //responce = responce.Data.Where(k=>k.suB_DEPT_DESCN == searchString).ToList();
            }

            ViewBag.CurrentFilter = searchString;
            switch (sortOrder)
            {

            }
            ViewBag.PageSize = pageSize;
            int pageNumber = (page ?? 1);
            var usersAsIPagedList = new StaticPagedList<M_SUB_DEPT>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
            return View(usersAsIPagedList);
        }

        public async Task<ActionResult> AddEditModal(int Id = 0)
        {
            var responce = await Services.GetAsync<List<M_DEPARTMENT>>("/api/Department/GetAll"); // Helpers.UserInfo.ModuleCode
            List<SelectListItem> Lst = responce.Data.Select(x => new SelectListItem
            {
                Value = x.pK_DEPT_CODE,
                Text = x.pK_DEPT_CODE + "-" + x.description
            }).ToList();
            ViewBag.Dept = Lst;

            if (Id > 0)
            {
                var mst_dept = await Services.GetAsync<M_SUB_DEPT>("/api/SubDept/GetByID?id=" + Id);
                return PartialView("AddEditModal", mst_dept.Data);
            }
            else
            {
                return View("AddEditModal");
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(M_SUB_DEPT entity)
        {
            try
            {
               
                entity.useR_NAME_NEW = "";
                entity.useR_ENTDT = DateTime.Now;
                entity.useR_ENTDT_NEW = DateTime.Now;

                entity.dtag = "";
             
                entity.ptype = "S";
                M_SUB_DEPT_dto _Dto = new M_SUB_DEPT_dto();
                _Dto.m_subdept = entity;
                if (entity.id == 0)
                {
                    var responce2 = await Services.GetAsync<M_SUB_DEPT>("/api/SubDept/GetByCode?search=" + entity.pK_SUB_DEPT);
                    if (responce2.Data != null)
                    {
                        TempData["toastrError"] = responce2.Data.pK_SUB_DEPT + " Dublicate Data Found";
                        return RedirectToAction("AddEditModal");
                    }
                    else
                    {
                        var responce = await Services.PostAsync<List<M_SUB_DEPT_dto>>("/api/SubDept/SaveOrUpdateAll", _Dto);
                        
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
                    var responce = await Services.PostAsync<List<M_SUB_DEPT_dto>>("/api/SubDept/SaveOrUpdateAll", _Dto);
                   
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
            return RedirectToAction("Index");
        }
    }
}