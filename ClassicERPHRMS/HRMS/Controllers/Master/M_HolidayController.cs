//using Microsoft.Owin;


using HRMS.Models;
using HRMS.Models.Master;
using HRMS.Models.TRN;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
//using HRMS.Models.PAYROLL.TRN;
using System.Web.UI.WebControls;
using static HRMS.Models.Model_Common;
using static HRMS_VIEWMODEL24.Payroll_Common;

namespace HRMS.Controllers.Master
{
    public class M_HolidayController : Controller
    {
        // GET: M_Holiday
        public async Task<ActionResult> M_Holiday(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            try
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var responce = await Services.GetAsync<PageModel<M_HOLIDAY>>(string.Format("/api/Holiday/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, searchString)); // Helpers.UserInfo.ModuleCode
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
                    var usersAsIPagedList = new StaticPagedList<M_HOLIDAY>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
                    var ATTENDENCECODE = await Services.GetAsync<List<M_ATTEND_DAYS_TYPE>>("/api/AttendDaysType/GetAll");
                    foreach (var item in usersAsIPagedList)
                    {


                        var ATTENDENCENAME = ATTENDENCECODE.Data.FirstOrDefault(u => u.pK_ATTEND_CODE == item.fK_ATTEND_CODE);
                        if (ATTENDENCENAME != null)
                        {
                            item.fK_ATTEND_CODE = ATTENDENCENAME.pK_ATTEND_CODE + " : " + ATTENDENCENAME.description;
                        }
                    }

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
            M_HOLIDAY model = new M_HOLIDAY();
            var attend = await Services.GetAsync<List<M_ATTEND_DAYS_TYPE>>("/api/AttendDaysType/GetAll");
            List<ModelMaster> Lst = attend.Data.Select(x => new ModelMaster
            {
                Value = x.pK_ATTEND_CODE,
                Text = x.pK_ATTEND_CODE + "-" + x.description

            }).ToList();
            ViewBag.Attend = Lst;

            if (Id > 0)
            {
                var response = await Services.GetAsync<M_HOLIDAY>("/api/Holiday/GetByID?id=" + Id);
                model = response.Data;
                return View("AddEditModal", model);
            }
            else
            {
                return View("AddEditModal", model);
            }
        }



        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(M_HOLIDAY m_HOLIDAY)
        {
            //if (TempData["HolidayList"] != null)
            //{
            //    var holidayList = (TempData["HolidayList"]) as List<M_HOLIDAY>;
            //    ViewBag.HolidayList = holidayList;
            //    TempData.Keep("HolidayList");
            //}
            //var attend = await Services.GetAsync<List<M_ATTEND_DAYS_TYPE>>("/api/AttendDaysType/GetAll");
            //List<ModelMaster> Lst = attend.Data.Select(x => new ModelMaster
            //{
            //    Value = x.pK_ATTEND_CODE,
            //    Text = x.pK_ATTEND_CODE + "-" + x.description

            //}).ToList();
            //ViewBag.Attend = Lst;
            List<M_HOLIDAY> m_HOLIDAYs = new List<M_HOLIDAY> {
                m_HOLIDAY
            };
            m_HOLIDAY.dtag = "";
            m_HOLIDAY.hoL_DATE = DateTime.Now;
            m_HOLIDAY.useR_ENTDT_NEW = DateTime.Now;
           
            m_HOLIDAY.useR_NAME_NEW = "";
            M_HOLIDAY_Dto _lDto = new M_HOLIDAY_Dto();
            _lDto.m_holiday = m_HOLIDAYs;
            var result = await Services.PostAsync<List<M_HOLIDAY_Dto>>("/api/Holiday/SaveOrUpdateAll", _lDto);
            if (result.IsSuccessStatusCode == true)
            {
                TempData["toastrSuccess"] = "Data has been successfully submitted";
            }
            else
            {
                TempData["toastrError"] = result.Message;
            }
            return RedirectToAction("M_Holiday");
        }



        //[HttpPost]
        //public ActionResult HolidayList(M_HOLIDAY m_HOLIDAY)
        //{
        //    List<M_HOLIDAY> _Dto = new List<M_HOLIDAY>();
        //    {
        //        var FrmDate = m_HOLIDAY.froM_DATE;
        //        var ToDate = m_HOLIDAY.tO_DATE;
        //        for (DateTime dt = FrmDate; dt <= ToDate; dt = dt.AddDays(1))
        //        {
        //            M_HOLIDAY _hDto = new M_HOLIDAY();
        //            _hDto.froM_DATE = m_HOLIDAY.froM_DATE;
        //            _hDto.tO_DATE = m_HOLIDAY.tO_DATE;
        //            _hDto.preV_DATE = m_HOLIDAY.preV_DATE;
        //            _hDto.nexT_DATE = m_HOLIDAY.nexT_DATE;
        //            _hDto.description = m_HOLIDAY.description;
        //            _hDto.fK_ATTEND_CODE = m_HOLIDAY.fK_ATTEND_CODE;
        //            _hDto.hoL_DATE = dt;
        //            _hDto.PTYPE = "S";
        //            _Dto.Add(_hDto);
        //        }
        //    }
        //    TempData["HolidayList"] = _Dto;
        //    //return view("AddEditModal", new { ID= m_HOLIDAY.id});
        //    return RedirectToAction("AddEditModal", m_HOLIDAY);
        //}
        //[HttpPost]
        //public async Task<ActionResult> AddOrUpdate(List<M_HOLIDAY> entity)
        //{
        //    try
        //    {

        //        entity = (TempData["HolidayList"]) as List<M_HOLIDAY>;
        //        List<M_HOLIDAY> _Dto = new List<M_HOLIDAY>();

        //        foreach (var item in entity)
        //        {
        //            item.useR_ENTDT_NEW = DateTime.Now;
        //            item.useR_NAME_NEW = "";

        //            item.dtag = "";

        //            _Dto.Add(item);
        //        }
        //        M_HOLIDAY_Dto _lDto = new M_HOLIDAY_Dto();
        //        _lDto.m_holiday = entity.Take(1).ToList().FirstOrDefault();
        //        var result = await Services.PostAsync<List<M_HOLIDAY_Dto>>("/api/Holiday/SaveOrUpdateAll", _lDto);
        //        if (result.IsSuccessStatusCode == true)
        //        {
        //            TempData["toastrSuccess"] = "Data has been successfully submitted";
        //        }
        //        else
        //        {
        //            TempData["toastrError"] = result.Message;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["toastrError"] = ex.Message;
        //    }
        //    return RedirectToAction("M_Holiday");
        //}


    }
}