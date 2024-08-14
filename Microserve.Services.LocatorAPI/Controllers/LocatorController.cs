using AutoMapper;
using Microserve.Services.LocatorAPI.Data;
using Microserve.Services.LocatorAPI.Models;
using Microserve.Services.LocatorAPI.Models.DTOs;
using Microserve.Web.Models.DTOs.ResponseDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microserve.Services.LocatorAPI.Controllers
{

    [Route("api/locator")] // hardcode the controller name; this is in a case where u change ur controller name it wont affect the api endpoint, u would have to change it or inform consumers
    [ApiController]
    //[Authorize]
    public class LocatorController : Controller
    {
        private readonly ApplicationDbContext _db;
        private ResponseDto _responseDto;
        private IMapper _mapper;
        public LocatorController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _responseDto = new ResponseDto();

        }


        [HttpPost("CreateLocator")]
        [Authorize(Roles = "ADMIM")]
        public ResponseDto CreateLocator([FromBody] LocatorDTO locatorDto)
        {
            try
            {
                var fac = _db.Locators.FirstOrDefault(f=>f.Name == locatorDto.Name);
                if(fac != null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = locatorDto.Name +  " Already Exist in the db";
                    return _responseDto;
                }
                Locator obj = _mapper.Map<Locator>(locatorDto);
                
                _db.Locators.Add(obj);
                _db.SaveChanges();
                _responseDto.IsSuccess = true;
                _responseDto.Message = "Location created successfully";
                _responseDto.Result = _mapper.Map<LocatorDTO>(obj);
                return _responseDto;

            }
            catch (Exception e)
            {

                _responseDto.IsSuccess = false;
                _responseDto.Message = "Error creating location : " + e.Message;
                return _responseDto;

            }
        }

        [HttpGet("GetAll")]
        public ResponseDto GetAll()
        {
           
            try
            {
                List<Locator> objectList = new();
               
                objectList = _db.Locators.ToList();

                _responseDto.Result = _mapper.Map<List<ResultDTO>>(objectList);
            }
            catch (Exception e)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = e.Message;

            }
            return _responseDto;
            
        }    
        
        
        [HttpGet("ViewMap/{locatorId}")]
        public ResponseDto ViewMap(int locatorId)
        {
           
            try
            {

                var data = _db.Locators.Where(l=>l.LocatorId == locatorId).FirstOrDefault();

                _responseDto.Result = _mapper.Map<ResultDTO>(data);
            }
            catch (Exception e)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = e.Message;

            }
            return _responseDto;
            
        }

        [HttpPost("GetAllSearch")]
        public ResponseDto GetAllSearch(Web.Models.DTOs.SearchDTO searchDto)
        {

            try
            {
                List<Locator> objectList = new();
                if (!string.IsNullOrEmpty(searchDto.Name))
                {
                    objectList = _db.Locators.Where(l => l.Name.ToLower() == searchDto.Name.ToLower()).ToList();
                    _responseDto.IsSuccess = true;
                }
                else if(!string.IsNullOrEmpty(searchDto.LGA))
                {
                    objectList = _db.Locators.Where(l => l.LGA.ToLower() == searchDto.LGA.ToLower()).ToList();
                    _responseDto.IsSuccess = true;
                }
                else if (!string.IsNullOrEmpty(searchDto.State))
                {
                    objectList = _db.Locators.Where(l => l.State.ToLower() == searchDto.State.ToLower()).ToList();
                    _responseDto.IsSuccess = true;
                }
                else if (!string.IsNullOrEmpty(searchDto.State)
                    && !string.IsNullOrEmpty(searchDto.Name) 
                    && !string.IsNullOrEmpty(searchDto.LGA))
                {
                    objectList = _db.Locators.Where(l => l.State.ToLower() == searchDto.State.ToLower() && l.Name.ToLower() == searchDto.Name.ToLower() && l.LGA.ToLower() == searchDto.LGA.ToLower()).ToList();
                    _responseDto.IsSuccess = true;
                }
                else
                {
                     _responseDto.IsSuccess = false;
                    _responseDto.Message = "Please enter any of the three fields";
                }
                if(_responseDto.IsSuccess)
                {
                    _responseDto.Result = _mapper.Map<List<ResultDTO>>(objectList);

                }
                return _responseDto;


            }
            catch (Exception e)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = e.Message;
                return _responseDto;
            }
            

        }

        [HttpGet("GetFacility/{Id}")]
        public ResponseDto GetFacility(int Id)
        {

            try
            {
                

                var objectList = _db.Locators.FirstOrDefault(l=>l.LocatorId == Id);
                if (objectList == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Not Found";

                }
                else
                {
                    _responseDto.Result = _mapper.Map<ResultDTO>(objectList);

                }

            }
            catch (Exception e)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = e.Message;

            }
            return _responseDto;

        }

        [HttpPut("UpdateLocator")]
        public ResponseDto UpdateLocator([FromBody] LocatorDTO locatorDto)
        {
            try
            {
                Locator obj = _mapper.Map<Locator>(locatorDto);
                var fac = _db.Locators.FirstOrDefault(l => l.LocatorId == obj.LocatorId);
                if (fac == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Facility Not Found";

                }
                else
                {
                    fac.Name = obj.Name;
                    fac.State = obj.State;
                    fac.LGA = obj.LGA;
                    fac.Phone_number = obj.Phone_number;
                    fac.Location = obj.Location;
                    fac.Business_status = obj.Business_status;
                    fac.GpsCordinator = obj.GpsCordinator;

                    _db.Locators.Update(fac);
                    _db.SaveChanges();
                    _responseDto.IsSuccess = true;
                    _responseDto.Message = "Facility updated successfully";
                    _responseDto.Result = _mapper.Map<LocatorDTO>(obj);
                }
            }
            catch (Exception e)
            {

                _responseDto.IsSuccess = false;
                _responseDto.Message = "Error updating facility " + e.Message;
            }
            return _responseDto;
        }

        [HttpDelete("Delete/{id}")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Locator obj = _db.Locators.FirstOrDefault(u => u.LocatorId == id);
                if (obj == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Faclility not found!";

                }
                _db.Locators.Remove(obj);
                _db.SaveChanges();
                _responseDto.IsSuccess = true;
                _responseDto.Message = "Facility deleted successfully";
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
