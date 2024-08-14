using Microserve.Web.Models.DTOs.RequestDtos;
using Microserve.Web.Models.DTOs.ResponseDtos;
using Microserve.Web.Services.IService;
using Microserve.Web.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Microserve.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthWebService _authWebService;
        private readonly ITokenProvider _tokenProvider;
        public AuthenticationController(IAuthWebService authWebService, ITokenProvider tokenProvider)
        {
            _authWebService = authWebService;
            _tokenProvider = tokenProvider; 

        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
           
            return View();
        } 
        
        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO loginRequestDTO = new();
            return View(loginRequestDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {

            ResponseDto responseDto = await _authWebService.LoginAsync(loginRequestDTO);
            if(responseDto != null && responseDto.IsSuccess)
            {
                //if login was successful deserizalize the loginResponseDTO to get the result
                LoginResponseDTO loginResponseDTO = JsonConvert.DeserializeObject<LoginResponseDTO?>(Convert.ToString(responseDto.Result));
                //sign in user
                await SignInUserAsync(loginResponseDTO);
                //set the cookie with token 
                _tokenProvider.setToken(loginResponseDTO.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //return error if there is any
                ModelState.AddModelError("CustomError", responseDto.Message);
                //return back with view with login request data and the error
                return View(loginRequestDTO);
            }

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

           //make a call to register api endpoint
            ResponseDto result = await _authWebService.RegisterAsync(obj);
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
                assignRole = await _authWebService.AssignRoleAsync(obj);
                if (assignRole != null && assignRole.IsSuccess)
                {
                    //if assigned a role, then return to login page with message
                    TempData["success"] = "You haver registered successfully!";
                    return RedirectToAction(nameof(Login));
                }

            }
            else
            {
                TempData["error"] = result!.Message;
            }
           
            return View(obj);
            
           
        }

   
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.clearToken();
            TempData["success"] = "You have successfully logged out of the system!";
            return RedirectToAction("Index", "Home");
        }


        private async Task SignInUserAsync(LoginResponseDTO model)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            //retrive role

            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));


            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
