using HRMS.Models;
using HRMS.Models.Master;
using HRMS.Models.TRN;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using static HRMS.Models.Model_Common;
using static iTextSharp.text.pdf.events.IndexEvents;

namespace HRMS.Controllers.Master
{
    public class Attend_Days_TypeController : Controller
    {
        // GET: Attend_Days_Type

        public async Task<ActionResult> Attend_Days_Type(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            try
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var responce = await Services.GetAsync<PageModel<M_ATTEND_DAYS_TYPE>>(string.Format("/api/AttendDaysType/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, searchString)); // Helpers.UserInfo.ModuleCode
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
                    var usersAsIPagedList = new StaticPagedList<M_ATTEND_DAYS_TYPE>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
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
            M_ATTEND_DAYS_TYPE model = new M_ATTEND_DAYS_TYPE();
            if (Id > 0)
            {
                var response = await Services.GetAsync<M_ATTEND_DAYS_TYPE>("/api/AttendDaysType/GetByID?id=" + Id);
                model = response.Data;
                if (model.withouT_PAY == true)
                {
                    model.attendencetype = "WithoutPay";
                }
                if (model.extrA_DAYS == true)
                {
                    model.attendencetype = "Extra Days";
                }
                if (model.leave == true)
                {
                    model.attendencetype = "Leave";
                }
                if (model.overtime == true)
                {
                    model.attendencetype = "Overtime";
                }
                if (model.weeklY_OFF == true)
                {
                    model.attendencetype = "Weekly Off";
                }
                if (model.holiday == true)
                {
                    model.attendencetype = "Holiday";
                }
                return View("AddEditModal", model);
            }
            else
            {
                return View("AddEditModal", model);
            }
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(M_ATTEND_DAYS_TYPE entity)
        {                      
            try
            {
                if (entity.attendencetype == "WithoutPay")
                {
                    entity.withouT_PAY = true;
                }
                else if (entity.attendencetype == "Extra Days")
                { 
                    entity.extrA_DAYS = true;
                }
                else if (entity.attendencetype == "Leave")
                {
                    entity.leave = true;
                }
                else if (entity.attendencetype == "Overtime")
                {
                    entity.overtime = true;
                }
                else if (entity.attendencetype == "Weekly Off")
                {
                    entity.weeklY_OFF = true;
                }
                else if (entity.attendencetype == "Holiday")
                {
                    entity.holiday = true;
                }




                entity.useR_ENTDT_NEW = DateTime.Now;
                entity.dtag = "";
                entity.oT_TYPE = "";

               
                entity.defaulT_CODE = "0";
                entity.ptype = " ";
                if (entity.pK_ATTEND_CODE == "P")
                {
                    var a = "1";
                    entity.defaulT_CODE = a;
                }
                entity.useR_NAME_NEW = "";
                M_ATTEND_DAYS_TYPE_Dto _Dto = new M_ATTEND_DAYS_TYPE_Dto();
                _Dto.m_attenddaystype = entity;
                if (entity.id == 0)
                {
                    var responce2 = await Services.GetAsync<List<M_ATTEND_DAYS_TYPE>>("/api/AttendDaysType/GetAll");
                    var dupcalmode = responce2.Data.Where(x => x.pK_ATTEND_CODE == entity.pK_ATTEND_CODE).FirstOrDefault();

                    if (dupcalmode != null)
                    {
                        TempData["toastrError"] = dupcalmode.pK_ATTEND_CODE + " Dublicate Data Found";
                        return RedirectToAction("AddEditModal");
                    }

                    else
                    {
                        var responce = await Services.PostAsync<List<M_ATTEND_DAYS_TYPE_Dto>>("/api/AttendDaysType/SaveOrUpdateAll", _Dto);
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

                    var responce = await Services.PostAsync<List<M_ATTEND_DAYS_TYPE_Dto>>("/api/AttendDaysType/SaveOrUpdateAll", _Dto);
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
            return RedirectToAction("Attend_Days_Type");
        }
    }
}