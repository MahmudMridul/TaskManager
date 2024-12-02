﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManagerApi.Db;
using TaskManagerApi.Helpers;
using TaskManagerApi.Models;
using TaskManagerApi.Models.Dtos;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace TaskManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TaskManagerContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;

        public AuthController(TaskManagerContext context, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<ApiResponse>> Signup([FromBody] SignUpDto dto)
        {
            ApiResponse res;
            try
            {
                if (!ModelState.IsValid)
                {
                    List<string> errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    res = ResponseHelper.CreateResponse(HttpStatusCode.BadRequest, msg: "Validation failed", errors: errors);
                    return BadRequest(res);
                }

                var existing = await _userManager.FindByNameAsync(dto.UserName);
                if (existing != null)
                {
                    res = ResponseHelper.CreateResponse(HttpStatusCode.Conflict, msg: "User already exists");
                    return Conflict(res);
                }
                
                User user = new User
                {
                    UserName = dto.UserName,
                    Email = dto.Email,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                };

                IdentityResult result = await _userManager.CreateAsync(user, dto.Password);

                if (!result.Succeeded)
                {
                    List<string> errors = result.Errors.Select(e => e.Description).ToList();
                    res = ResponseHelper.CreateResponse(HttpStatusCode.BadRequest, msg: "Signup failed", errors: errors);
                    return BadRequest(res);
                }

                await _userManager.AddToRoleAsync(user, "User");
                res = ResponseHelper.CreateResponse(HttpStatusCode.OK, dto, true, "Signup successfull", null);
                return Ok(res);
            }
            catch (Exception e)
            {
                List<string> errs = new List<string> { e.Message, e.StackTrace };
                res = ResponseHelper.CreateResponse(HttpStatusCode.InternalServerError, msg: "An unexpected error occurred", errors: errs);
                return StatusCode((int)HttpStatusCode.InternalServerError, res);
            }
        }

        [HttpPost("signin")]
        public async Task<ActionResult<ApiResponse>> Signin([FromBody] SignInDto dto)
        {
            ApiResponse res;
            try
            {
                if (!ModelState.IsValid)
                {
                    List<string> errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    res = ResponseHelper.CreateResponse(HttpStatusCode.BadRequest, msg: "Validation failed", errors: errors);
                    return BadRequest(res);
                }

                User? user = await _userManager.FindByNameAsync(dto.UserName);
                if (user == null)
                {
                    res = ResponseHelper.CreateResponse(HttpStatusCode.NotFound, msg: "No user found with this username");
                    return NotFound(res);
                }

                SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

                if (!result.Succeeded)
                {
                    res = ResponseHelper.CreateResponse(HttpStatusCode.Unauthorized, msg: "Invalid username or password");
                    return Unauthorized(res);
                }

                string accessToken = await TokenHelper.GenerateJwtToken(user, _userManager, _config);
                (string refreshToken, DateTime expiration) = TokenHelper.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = expiration;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                var accessTokenOps = new CookieOptions
                {
                    HttpOnly = true,       // Prevents access via JavaScript
                    Secure = true,         // Ensures the cookie is only sent over HTTPS
                    SameSite = SameSiteMode.None,  // Prevents CSRF attacks
                    Expires = DateTime.UtcNow.AddHours(1)  // Set the expiration of the token (optional)
                };

                // Append the token to the response as a cookie
                Response.Cookies.Append("AccessToken", accessToken, accessTokenOps);

                var refreshTokenOps = new CookieOptions
                {
                    HttpOnly = true,       // Prevents access via JavaScript
                    Secure = true,         // Ensures the cookie is only sent over HTTPS
                    SameSite = SameSiteMode.None,  // Prevents CSRF attacks
                    Expires = DateTime.UtcNow.AddHours(1)  // Set the expiration of the token (optional)
                };

                // Append the token to the response as a cookie
                Response.Cookies.Append("RefreshToken", refreshToken, refreshTokenOps);

                res = ResponseHelper.CreateResponse(
                    HttpStatusCode.OK, 
                    new 
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                    }, 
                    true, 
                    "Sign in successfull"
                ); 
                return Ok(res);

            }
            catch (Exception e)
            {
                List<string> errs = new List<string> { e.Message, e.StackTrace };
                res = ResponseHelper.CreateResponse(HttpStatusCode.InternalServerError, msg: "An unexpected error occurred", errors: errs);
                return StatusCode((int)HttpStatusCode.InternalServerError, res);
            }
        }

        [HttpPost("signout")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> Signout()
        {
            ApiResponse res;
            try
            {
                User? user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    res = ResponseHelper.CreateResponse(
                        HttpStatusCode.Unauthorized,
                        msg: "User not found"
                    );
                    return Unauthorized(res);
                }

                user.RefreshToken = "";
                user.RefreshTokenExpiryTime = DateTime.MinValue;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                await _signInManager.SignOutAsync();

                res = ResponseHelper.CreateResponse(
                    HttpStatusCode.OK,
                    success: true,
                    msg: "Sign out successful"
                );
                return Ok(res);
            }
            catch (Exception ex)
            {
                res = ResponseHelper.CreateResponse(
                    statusCode: HttpStatusCode.InternalServerError,
                    msg: "An error occurred during sign out",
                    errors: new List<string> { ex.Message, ex.StackTrace }
                );
                return StatusCode((int)HttpStatusCode.InternalServerError, res);
            }
        }
    }
}
