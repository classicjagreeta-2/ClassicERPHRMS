using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HRMS.Models;
using HRMS.Models.Master;

using Newtonsoft.Json;
using PagedList;

namespace HRMS.Controllers.Master
{
    public class M_EmpTypeController : Controller
    {
        // GET: M_EmpType
        public async Task<ActionResult> M_EmpType(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            try
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var responce = await Services.GetAsync<PageModel<M_EMP_TYPE>>(string.Format("/api/EmpType/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, searchString)); // Helpers.UserInfo.ModuleCode
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
                    var usersAsIPagedList = new StaticPagedList<M_EMP_TYPE>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
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
            M_EMP_TYPE model = new M_EMP_TYPE();
            if (Id > 0)
            {
                var response = await Services.GetAsync<M_EMP_TYPE>("/api/EmpType/GetByID?id=" + Id);
                model = response.Data;
                return View("AddEditModal", model);
            }
            else
            {
                return View("AddEditModal", model);
            }
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(M_EMP_TYPE entity)
        {
            try
            {

                entity.useR_NAME_NEW = "";
                entity.useR_ENTDT_NEW = DateTime.Now;
                entity.dtag = "";

                entity.ptype = "S";
                M_EMP_TYPE_Dto _Dto = new M_EMP_TYPE_Dto();
                _Dto.m_emptype = entity;
                if (entity.id == 0)
                {
                    var responce2 = await Services.GetAsync<List<M_EMP_TYPE>>("/api/EmpType/GetAll");
                    var dupcalmode = responce2.Data.Where(x => x.pK_CODE == entity.pK_CODE).FirstOrDefault();

                    if (dupcalmode != null)
                    {
                        TempData["toastrError"] = dupcalmode.pK_CODE + " Dublicate Data Found";
                        return RedirectToAction("AddEditModal");
                    }
                    else
                    {
                        var responce = await Services.PostAsync<List<M_EMP_TYPE_Dto>>("/api/EmpType/SaveOrUpdateAll", _Dto);
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
                    var responce = await Services.PostAsync<List<M_EMP_TYPE_Dto>>("/api/EmpType/SaveOrUpdateAll", _Dto);
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
            return RedirectToAction("M_EmpType");
        }
    }
}