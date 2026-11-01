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
    public class M_GradeController : Controller
    {
        // GET: M_Grade

        public async Task<ActionResult> M_Grade(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            try
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var responce = await Services.GetAsync<PageModel<M_GRADE>>(string.Format("/api/Grade/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, searchString)); // Helpers.UserInfo.ModuleCode
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
                    var usersAsIPagedList = new StaticPagedList<M_GRADE>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
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
            M_GRADE model = new M_GRADE();
            if (Id > 0)
            {
                var response = await Services.GetAsync<M_GRADE>("/api/Grade/GetByID?id=" + Id);
                model = response.Data;
                return View("AddEditModal", model);
            }
            else
            {
                return View("AddEditModal", model);
            }
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(M_GRADE entity)
        {
            try
            {

                entity.user_name_new = "";

                entity.os_user = "";
                entity.user_entdt_new = DateTime.Now;
                if (entity.starT_BASIC == null  )
                {

                    entity.starT_BASIC = 0;
                }
                if(entity.enD_BASIC == null) { 

                    entity.enD_BASIC = 0;

                }
                if (entity.incR_RATE == null)
                {

                    entity.incR_RATE = 0;
                }
                if (entity.nO_OF_YEAR == null) { 
                    entity.nO_OF_YEAR = 0;

                }
                entity.dtag = "";

                entity.ptype = "S";
                M_GRADE_Dto _Dto = new M_GRADE_Dto();
                _Dto.m_grade = entity;
                if (entity.id == 0)
                {
                    var responce2 = await Services.GetAsync<M_GRADE>("/api/Grade/GetByCode?search=" + entity.pK_GRADE_CODE);
                    if (responce2.Data != null)
                    {
                        TempData["toastrError"] = responce2.Data.pK_GRADE_CODE + " Dublicate Data Found";
                        return RedirectToAction("AddEditModal");
                    }
                    else
                    {
                        

                            var responce = await Services.PostAsync<List<M_GRADE_Dto>>("/api/Grade/SaveOrUpdateAll", _Dto);
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

                    var responce = await Services.PostAsync<List<M_GRADE_Dto>>("/api/Grade/SaveOrUpdateAll", _Dto);
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
            return RedirectToAction("M_Grade");
        }
    }
}