using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using HRMS.Models;
using HRMS.Models.Master;
using Newtonsoft.Json;
using PagedList;

namespace HRMS.Controllers.Master
{
    public class CategoryController : Controller
    {

        // GET: Category
        public async Task<ActionResult> Category(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var responce = await Services.GetAsync<PageModel<M_CATEGORY>>(string.Format("/api/Category/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, searchString)); // Helpers.UserInfo.ModuleCode
            if (responce.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(searchString))
            {

            }
            var asas  = Request.Headers["User-Agent"];
            ViewBag.CurrentFilter = searchString;
            switch (sortOrder)
            {

            }
            ViewBag.PageSize = pageSize;
            int pageNumber = (page ?? 1);
            var usersAsIPagedList = new StaticPagedList<M_CATEGORY>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
            return View(usersAsIPagedList);
            }
            else
            {
                TempData["toastrError"] = responce.Message;
                return View();
            }
        }



        public async Task<ActionResult> AddEditModal(int Id = 0)
        {
            M_CATEGORY _CATEGORY = new M_CATEGORY();
            if (Id > 0)
            {
                var mst_dept = await Services.GetAsync<M_CATEGORY>("/api/Category/GetByID?id=" + Id);
                _CATEGORY = mst_dept.Data;
                return View("AddEditModal", _CATEGORY);
            }
            else
            {
                return View("AddEditModal", _CATEGORY);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(M_CATEGORY entity)
        {
            try
            {
              
               
                entity.user_entdt_new = DateTime.Now;
               

                entity.dtag = "";
                entity.user_name_new = "";
                entity.PTYPE = "S";
                M_CATEGORY_Dto _Dto = new M_CATEGORY_Dto();
                _Dto.m_category = entity;
                if (entity.id == 0)
                {
                    var responce2 = await Services.GetAsync<M_CATEGORY>("/api/Category/GetByCode?search=" + entity.pK_CATE_CODE);
                    if (responce2.Data != null)
                    {
                        TempData["toastrError"] = responce2.Data.pK_CATE_CODE + " Dublicate Data Found";
                        return RedirectToAction("AddEditModal");
                    }
                    else
                    {
                        var responce = await Services.PostAsync<List<M_CATEGORY_Dto>>("/api/Category/SaveOrUpdateAll", _Dto);
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
                    var responce = await Services.PostAsync<List<M_CATEGORY_Dto>>("/api/Category/SaveOrUpdateAll", _Dto);
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
            return RedirectToAction("Category");
        }
    }
}