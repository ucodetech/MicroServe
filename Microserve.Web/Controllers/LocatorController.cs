using Microserve.Services.AuthAPI.Data;
using Microserve.Web.Helpers;
using Microserve.Web.Models.DTOs;
using Microserve.Web.Models.DTOs.ResponseDtos;
using Microserve.Web.Services.IService;
using Microserve.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace Microserve.Web.Controllers
{
    public class LocatorController : Controller
    {
        private readonly ILocatorService _locatorService;
        private readonly IAuthWebService _authWebService;
        public LocatorController(ILocatorService locatorService, IAuthWebService authWebService)
        {
            _locatorService = locatorService;
            _authWebService = authWebService;
        }

        [HttpGet]
        public async Task<IActionResult> FacilityIndex()
        {

            try
            {
                List<ResultDTO> list = new();
                ResponseDto? response = await _locatorService.GetAllFacilitiesAsync();
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<ResultDTO>>(Convert.ToString(response.Result));
                }
                else
                {
                    TempData["warning"] = response.Message;
                }

                return View(list);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex;
                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ListFacilities()
        {

            try
            {
                List<ResultDTO> list = new();
                ResponseDto? response = await _locatorService.GetAllFacilitiesAsync();
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<ResultDTO>>(Convert.ToString(response.Result));
                }
                else
                {
                    TempData["warning"] = response.Message;
                }

                return View(list);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex;
                return View();
            }
        }
        public async Task<IActionResult> ViewMap(int Id)
        {

            try
            {
                ResponseDto? response = await _locatorService.ViewMapData(Id);
                if (response != null && response.IsSuccess)
                {
                   var list = JsonConvert.DeserializeObject<ResultDTO>(Convert.ToString(response.Result));
                    return View(list);

                }
                else
                {
                    TempData["warning"] = response.Message;
                    return RedirectToAction("FacilityIndex");
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = ex;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Search(string Name, string State, string LGA)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(Name) && string.IsNullOrWhiteSpace(LGA) && string.IsNullOrWhiteSpace( State))
                {
                    TempData["error"] = "Please search by state, lga or name of the facility";
                    return RedirectToAction(nameof(FacilityIndex));
                }

                List<ResultDTO> list = new();
                SearchDTO searchDTO = new SearchDTO
                {
                    Name = Name,
                    LGA = LGA,
                    State = State
                };
                ResponseDto? response = await _locatorService.SearchFacilityAsync(searchDTO);
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<ResultDTO>>(Convert.ToString(response.Result));
                }
                else
                {
                    TempData["warning"] = response.Message;
                }


                return View("FacilityIndex", list);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex;
                return View();
            }
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult FacilityCreate()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult>FacilityCreate(LocatorDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
               
                ResponseDto? response = await _locatorService.CreateLocationAsync(model);
                TempData["success"] = "Facility Created successfully";
                return RedirectToAction(nameof(FacilityCreate));
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
                return View(model);
            }
        }

 
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> FacilityDelete(int Id)
        {

            ResponseDto? response = await _locatorService.DeleteLocationAsync(Id);
            if (response != null && response.IsSuccess)
            {
                TempData["info"] = "Facility Deleted!";
                return RedirectToAction(nameof(ListFacilities));

            }
            return View("ListFacilities");

        }
        
        
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> FacilityUpdate(int Id)
        {
            
            ResponseDto? response = await _locatorService.GetFacilityByIdAsync(Id);
            if (response != null && response.IsSuccess)
            {
                LocatorDTO? model = JsonConvert.DeserializeObject<LocatorDTO>(Convert.ToString(response.Result));
                return View(model);

            }
            return NotFound();
           
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> FacilityUpdate(LocatorDTO locatorDTO)
        {

            ResponseDto? response = await _locatorService.UpdateAsync(locatorDTO);
            if (response != null && response.IsSuccess)
            {
                TempData["info"] = "Facility Updated!";
                return RedirectToAction(nameof(ListFacilities));

            }
            return View(locatorDTO);

        }

    }
}
