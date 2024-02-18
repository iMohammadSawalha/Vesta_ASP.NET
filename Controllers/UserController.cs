using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Vesta.DataTransferObjects.User;
using Vesta.Helpers;
using Vesta.Interfaces;
using Vesta.Models;

namespace Vesta.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IEmailService _emailService;

        public UserController(IUserRepository userRepository, IUserTokenRepository userTokenRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _userTokenRepository = userTokenRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequestDTO register_params)
        {
            if(register_params.Image != null && register_params.Image != string.Empty)
                try{await ImageUrlValidator.IsValidImageUrl(register_params.Image,1);}
                catch(Exception e){ModelState.AddModelError("Image",e.Message);}

            if(!_emailService.VerifyEmailCode(register_params.Email,register_params.EmailConfirmationCode))
                ModelState.AddModelError("EmailConfirmationCode","Invalid Confirmation Code!");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userRepository.GetUserByEmailAsync(register_params.Email);
            if(existingUser != null)
                return Conflict("Email address already in use.");
            
            var newUser = new User()
            {
                Email = register_params.Email,
                HashedPassword = BCryptHelper.HashPassword(register_params.Password),
                Image = register_params.Image
            };
            try{
                await _userRepository.CreateUserAsync(newUser);
            }catch{
                return Problem("Registration failed due to server error.");
            }

            return Ok("Registration successful.");
        }

        [HttpPost("send-confirmation-code")]
        public async Task<IActionResult> SendConfirmationEmail([FromBody] ConfirmationEmailRequestDTO confirmation_params)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try{
                await _emailService.SendRandomEmailVerificationCode(confirmation_params.Email);
                return Ok("Code has been sent.");
                
            }catch(Exception ex){

                if(ex.Message == "Unknown")
                    return Problem("An Unknown error occurred while sending a verification code to your email.");

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserRequestDTO login_params)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userRepository.GetUserByEmailAsync(login_params.Email);
            if(existingUser == null)
               return Unauthorized("Invalid email or password.");
            
            if(!BCryptHelper.VerifyPassword(existingUser.HashedPassword,login_params.Password))
                return Unauthorized("Invalid email or password.");

            var claims = new List<Claim>()
            {
                new("Email",login_params.Email)
            };
            string accessToken = JwtTokenHelper.CreateAccessToken(claims,5);

            var res = new LoginUserResponseDTO()
            {
                Email = login_params.Email,
                AccessToken = accessToken
            };
            var GeneratedUserToken = await GenerateRefreshToken(login_params.Email,claims,168); // 168hrs == 7days

            Response.Cookies.Append("refresh-token",GeneratedUserToken.RefreshToken);
            Response.Cookies.Append("uuid",GeneratedUserToken.UUID.ToString());

            return Ok(res);
        }

        private async Task<UserToken> GenerateRefreshToken(string userEmail, List<Claim> claims, int tokenLifeTimeHours)
        {
            var refreshToken = JwtTokenHelper.CreateRefreshToken(claims,tokenLifeTimeHours);

            var newUserToken = new UserToken()
            {
                UserEmail = userEmail,
                RefreshToken = refreshToken,
                ExpirationDate = DateTime.Now.AddHours(tokenLifeTimeHours)
            };

            await _userTokenRepository.CreateUserToken(newUserToken);
            
            return newUserToken;
        }
        
    }
}