using Microserve.Web.Models.DTOs;
using Microserve.Web.Models.DTOs.ResponseDtos;
using Microserve.Web.Services.IService;
using Microserve.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace Microserve.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDTO> list = new();
            ResponseDto? response = await _couponService.GetAllCouponsAsync();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDTO>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["warning"] = "Error fetching coupons";
            }


            return View(list);
        }
        
        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                ResponseDto? response = await _couponService.CreateCouponAsync(model);
                TempData["success"] = "Coupon Created successfully";
                return RedirectToAction(nameof(CouponIndex));
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
                return View(model);
            }
        }


        public async Task<IActionResult> CouponDelete(int couponId)
        {
            
            ResponseDto? response = await _couponService.GetCouponByIdAsync(couponId);
            if (response != null && response.IsSuccess)
            {
                CouponDTO? model = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(response.Result));
                return View(model);

            }
            return NotFound();
           
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDTO couponDTO)
        {

            ResponseDto? response = await _couponService.DeleteCouponAsync(couponDTO.CouponId);
            if (response != null && response.IsSuccess)
            {
                TempData["info"] = "Coupon Deleted!";
                return RedirectToAction(nameof(CouponIndex));

            }
            return View(couponDTO);

        }

    }
}
