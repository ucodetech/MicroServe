using Microserve.Services.AuthAPI.Data;
using Microserve.Services.AuthAPI.Models;
using Microserve.Services.AuthAPI.Models.DTOs;
using Microserve.Services.AuthAPI.Models.DTOs.RequestDTO;
using Microserve.Services.AuthAPI.Models.DTOs.ResponseDTO;
using Microserve.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Microserve.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtGenerator;
        public AuthService(
            ApplicationDbContext db, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            IJwtTokenGenerator jwtGenerator
            )
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtGenerator = jwtGenerator;
            
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u=>u.Email.ToLower() == email.ToLower());
            if (user != null)
            {

                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    //create role if it does not exist
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            //check if the request user exist on the db
            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(u=>u.UserName.ToLower()
            ==loginRequestDTO.UserName.ToLower());
         //check the user password 
            bool IsValid = await _userManager.CheckPasswordAsync(user, 
                loginRequestDTO.Password);
            //if user is null and password is not correct
            if (user is null || IsValid == false)
            {
                //return response with user set to null and token empty
                return new LoginResponseDTO()
                {
                    User = null, 
                    Token = ""
                    
                };
              }
            //if user exist  and password is correct, 
            //generate token
            var roles = await _userManager.GetRolesAsync(user);
            var token =  _jwtGenerator.GenerateToken(user, roles);
            //return user dto
            AuthDTO userDTO = new()
                {
                    Email = user.Email,
                    Id = user.Id,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                };

            //return response with user data
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                User = userDTO,
                Token = token
            };

            return loginResponseDTO;
        }

        public async Task<string> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            //checke if user exist
            var userExist = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Email.ToLower() == registrationRequestDTO.Email.ToLower());
            if(userExist != null)
            {
                return "User already exist!";
            }
            else
            {
                ApplicationUser user = new()
                {
                    UserName = registrationRequestDTO.Email,
                    Name = registrationRequestDTO.Name,
                    NormalizedEmail = registrationRequestDTO.Email.ToUpper(),
                    Email = registrationRequestDTO.Email,
                    PhoneNumber = registrationRequestDTO.PhoneNumber
                };

                try
                {
                    var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);
                    if (result.Succeeded)
                    {
                        var userToReturn = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName
                        == registrationRequestDTO.Email);
                        AuthDTO userDTO = new()
                        {
                            Id = userToReturn.Id,
                            Name = userToReturn.Name,
                            Email = userToReturn.Email,
                            PhoneNumber = userToReturn.PhoneNumber,

                        };
                        return "";
                    }
                    else
                    {
                        return result.Errors.FirstOrDefault()!.Description;
                    }
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
           
          
        }

       
    
    }
}
