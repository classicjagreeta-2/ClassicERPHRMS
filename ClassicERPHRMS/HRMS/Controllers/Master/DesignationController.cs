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
using HRMS_VIEWMODEL24;
using Newtonsoft.Json;
using PagedList;

namespace HRMS.Controllers.Master
{
    public class DesignationController : Controller
    {
        // GET: Designation
        public async Task<ActionResult> Designation(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        
        {
            try
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var responce = await Services.GetAsync<PageModel<M_DESIGNATION>>(string.Format("/api/Designation/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, searchString)); // Helpers.UserInfo.ModuleCode
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
                var usersAsIPagedList = new StaticPagedList<M_DESIGNATION>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
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
            M_DESIGNATION model = new M_DESIGNATION();
            if (Id > 0)
            {
                var response = await Services.GetAsync<M_DESIGNATION>("/api/Designation/GetByID?id=" + Id);
                model = response.Data;
                return View("AddEditModal", model);
            }
            else
            {
                return View("AddEditModal", model);
            }
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(M_DESIGNATION entity)
        {
            try
            {
               
                entity.useR_NAME_NEW = "";
                
             
                entity.useR_ENTDT_NEW = DateTime.Now;
                
                entity.dtag = "";
                
                
                M_DESIGNATION_Dto _Dto = new M_DESIGNATION_Dto();
                _Dto.m_designation = entity;
                if (entity.id == 0)
                {
                    var responce2 = await Services.GetAsync<M_DESIGNATION>("/api/Designation/GetByCode?search=" + entity.pK_DESG_CODE);
                    if (responce2.Data != null)
                    {
                        TempData["toastrError"] = responce2.Data.pK_DESG_CODE + " Dublicate Data Found";
                        return RedirectToAction("AddEditModal");
                    }
                    else
                    {
                        var responce = await Services.PostAsync<List<M_DESIGNATION_Dto>>("/api/Designation/SaveOrUpdateAll", _Dto);
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
                    var responce = await Services.PostAsync<List<M_DESIGNATION_Dto>>("/api/Designation/SaveOrUpdateAll", _Dto);
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
            return RedirectToAction("Designation");
        }
    }
}