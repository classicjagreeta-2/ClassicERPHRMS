using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HRMS.Models;
using HRMS.Models.Master;
using PagedList;
using static HRMS.Models.Model_Common;

namespace HRMS.Controllers.Master
{
    public class CountryController : Controller
    {

        // GET: Country
        public async Task<ViewResult> Index(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var responce1 = await Services.GetAsync<PageModel<M_COUNTRY>>(string.Format("/api/Country/GetByPage?search={0}&page={1}&pageSize={2}", searchString, page, pageSize));
            if (responce1.IsSuccessStatusCode)
            {
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
            var usersAsIPagedList = new StaticPagedList<M_COUNTRY>(responce1.Data.value.results, pageNumber, pageSize, responce1.Data.value.rowCount);
            return View(usersAsIPagedList);
            }
            else
            {
                TempData["toastrError"] = responce1.Message;
                return View();
            }
        }
      
        public async Task<ActionResult> AddEditModal(int Id = 0)
        {
            M_COUNTRY m_COUNTRY = new M_COUNTRY();
            if (Convert.ToInt32(Id) <= 0)
            {
                return View(m_COUNTRY);
            }
            var country = await Services.GetAsync<M_COUNTRY>("/api/Country/GetByID?id=" + Id);
            m_COUNTRY = country.Data;
            return View(m_COUNTRY);
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(M_COUNTRY entity)
        {
            try
            {
                
                entity.useR_ENTDT=DateTime.Now;
              
                entity.useR_NAME_NEW = "";
              
                entity.dtag = "";                
                
              
                entity.ptype = "S";
                M_COUNTRY_Dto _Dto = new M_COUNTRY_Dto();
                _Dto.m_country = entity;
                if (entity.id == 0)
                {
                    var responce2 = await Services.GetAsync<M_COUNTRY>("/api/Country/GetByCode?search=" + entity.pK_COUNTRY_CODE);
                    if (responce2.Data != null)
                    {
                        TempData["toastrError"] = responce2.Data.pK_COUNTRY_CODE + " Dublicate Data Found";
                        return RedirectToAction("AddEditModal");
                    }
                    else
                    {
                        var responce = await Services.PostAsync<List<M_COUNTRY_Dto>>("/api/Country/SaveOrUpdateAll", _Dto);
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
                    var responce = await Services.PostAsync<List<M_COUNTRY_Dto>>("/api/Country/SaveOrUpdateAll", _Dto);
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