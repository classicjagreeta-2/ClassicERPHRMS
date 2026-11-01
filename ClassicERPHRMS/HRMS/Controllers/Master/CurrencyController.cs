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
using PagedList;


namespace HRMS.Controllers.Master
{
    public class CurrencyController : Controller
    {
        // GET: Currency       
        public async Task<ActionResult> Currency(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var responce = await Services.GetAsync<PageModel<M_CURRENCY>>(string.Format("/api/Currency/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, searchString)); // Helpers.UserInfo.ModuleCode
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
            var usersAsIPagedList = new StaticPagedList<M_CURRENCY>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
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
            M_CURRENCY _CURRENCY = new M_CURRENCY();
            if (Id > 0)
            {
                var response = await Services.GetAsync<M_CURRENCY>("/api/Currency/GetByID?id=" + Id);
                _CURRENCY = response.Data;
                return View("AddEditModal", _CURRENCY);
            }
            else
            {
                return View("AddEditModal", _CURRENCY);
            }
        }



        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(M_CURRENCY entity)
        {
            try
            {
                
               
               
                entity.ptype = "S";
            
                entity.USER_ENTDT_NEW = DateTime.Now;
              
               
                entity.SUBUNIT = "";
                entity.SHT_DESCN = "";
                M_CURRENCY_Dto _Dto = new M_CURRENCY_Dto();
                _Dto.m_currency= entity;
                if (entity.ID == 0)
                {
                    var responce2 = await Services.GetAsync<List<M_CURRENCY>>("/api/Currency/GetAll");
                    var dupcalmode = responce2.Data.Where(x => x.PK_CURR_CODE == entity.PK_CURR_CODE).FirstOrDefault();

                    if (dupcalmode != null)
                    {
                        TempData["toastrError"] = dupcalmode.PK_CURR_CODE + " Dublicate Data Found";
                        return RedirectToAction("AddEditModal");
                    }
                    else
                    {
                        var responce = await Services.PostAsync<List<M_CURRENCY_Dto>>("/api/Currency/SaveOrUpdateAll", _Dto);
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
                    var responce = await Services.PostAsync<List<M_CURRENCY_Dto>>("/api/Currency/SaveOrUpdateAll", _Dto);
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
            return RedirectToAction("Currency");
        }
    }
}