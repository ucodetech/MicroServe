using Microserve.Web.Models.DTOs.RequestDtos;
using Microserve.Web.Models.DTOs.ResponseDtos;
using Microserve.Web.Services.IService;
using Microserve.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Microserve.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService; 
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO loginRequestDTO = new();
            return View(loginRequestDTO);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=StaticDetails.RoleAdmin, Value=StaticDetails.RoleAdmin},
                new SelectListItem {Text=StaticDetails.RoleCustomer, Value=StaticDetails.RoleCustomer},

            };
            ViewBag.RoleList = roleList;

            RegistrationRequestDTO registrationRequestDTO = new();
            return View(registrationRequestDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDTO obj)
        {
            //if the model state is invalid, return to the view and repopulate the roles 
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=StaticDetails.RoleAdmin, Value=StaticDetails.RoleAdmin},
                new SelectListItem {Text=StaticDetails.RoleCustomer, Value=StaticDetails.RoleCustomer},

            };
            ViewBag.RoleList = roleList;

            //check if user exist
            ResponseDto userExist = await _authService.IsUserExistAsync(obj);
            if (userExist != null)
            {
                userExist.IsSuccess = false;
                userExist.Message = "User with email already exist!";
                return View(obj);
            }
           //make a call to register api endpoint
            ResponseDto result = await _authService.RegisterAsync(obj);
            //create assign role response varible
            ResponseDto assignRole;
            ResponseDto error;
            if (result != null && result.IsSuccess)
            {
                //if the form request didnt supply role, set defult to customer
                if (string.IsNullOrEmpty(obj.Role))
                {
                    // set defult to customer
                    obj.Role = StaticDetails.RoleCustomer;
                }
                //call the create role api endpoint
                assignRole = await _authService.AssignRoleAsync(obj);
                if (assignRole != null && assignRole.IsSuccess)
                {
                    //if assigned a role, then return to login page with message
                    TempData["success"] = "You haver registered successfully!";
                    return RedirectToAction(nameof(Login));
                }

            }  
            
          
            return View(obj);
            
           
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }
    }
}
