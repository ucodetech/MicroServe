using AutoMapper;
using Microserve.Services.CouponAPI.Data;
using Microserve.Services.CouponAPI.Models;
using Microserve.Services.CouponAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microserve.Services.CouponAPI.Controllers
{
    [Route("api/Coupon")] // hardcode the controller name; this is in a case where u change ur controller name it wont affect the api endpoint, u would have to change it or inform consumers
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ResponseDto _responseDto;
        private IMapper _mapper;
        public CouponController(ApplicationDbContext db,IMapper mapper )
        {
            _db = db;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto GetAll()
        {
            try
            {
                IEnumerable<Coupon> objectList = _db.Coupons.ToList();
               _responseDto.Result = _mapper.Map<IEnumerable<CouponDTO>>(objectList);
            }
            catch (Exception e)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = e.Message;

            }
            return _responseDto;
        }

        [HttpGet("{id}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Coupon obj = _db.Coupons.First(c=>c.CouponId==id);
                _responseDto.Result = _mapper.Map<CouponDTO>(obj);
               
            }
            catch (Exception e)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = e.Message;

            }
            return _responseDto;
        }

        [HttpGet("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                Coupon obj = _db.Coupons.First(c => c.CouponCode.ToLower() == code.ToLower());
                if (obj == null)
                {
                    _responseDto.IsSuccess=false;
                }

                _responseDto.Result = _mapper.Map<CouponDTO>(obj);

            }
            catch (Exception e)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = e.Message;

            }
            return _responseDto;
        }

        [HttpPost("CreateCoupon")]
        public ResponseDto CreateCoupon([FromBody] CouponDTO couponDto)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDto);
                //var existCoupon = _db.Coupons.Where(c => c.CouponCode == couponDto.CouponCode);
                //if (existCoupon != null)
                //{
                //    _responseDto.IsSuccess = false;
                //    _responseDto.Message = "Coupon code already exist";
                //}
                _db.Coupons.Add(obj);
                _db.SaveChanges();
                _responseDto.IsSuccess = true;
                _responseDto.Message = "Coupon created successfully";
                _responseDto.Result = _mapper.Map<CouponDTO>(obj);
            }
            catch (Exception e)
            {

                _responseDto.IsSuccess = false;
                _responseDto.Message = "Error creating coupon";
            }
            return _responseDto;
        }

        [HttpPut("UpdateCoupon/{id}")]
        public ResponseDto UpdateCoupon([FromBody] CouponDTO couponDto)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Update(obj);
                _db.SaveChanges();
                _responseDto.IsSuccess = true;
                _responseDto.Message = "Coupon updated successfully";
                _responseDto.Result = _mapper.Map<CouponDTO>(obj);
            }
            catch (Exception e)
            {

                _responseDto.IsSuccess = false;
                _responseDto.Message = "Error creating coupon";
            }
            return _responseDto;
        }

        [HttpDelete("Delete/{id}")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Coupon obj = _db.Coupons.First(u=>u.CouponId == id);
                if (obj == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Coupon not found!";

                }
                _db.Coupons.Remove(obj);
                _db.SaveChanges();
                _responseDto.IsSuccess = true;
                _responseDto.Message = "Coupon deleted successfully";
            }
            catch (Exception e)
            {

                _responseDto.IsSuccess = false;
                _responseDto.Message = e.Message;
            }
            return _responseDto;
        }

    }
}
