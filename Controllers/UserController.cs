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
        private readonly IUserRepository _userrepository;
        private readonly IEmailService _emailService;

        public UserController(IUserRepository userrepository, IEmailService emailService)
        {
            _userrepository = userrepository;
            _emailService = emailService;
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

            var existingUser = await _userrepository.GetUserByEmailAsync(register_params.Email);
            if(existingUser != null)
                return Conflict("Email address already in use.");
            
            var newUser = new User()
            {
                Email = register_params.Email,
                HashedPassword = BCryptHelper.HashPassword(register_params.Password),
                Image = register_params.Image
            };
            try{
                await _userrepository.CreateUserAsync(newUser);
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
        
    }
}