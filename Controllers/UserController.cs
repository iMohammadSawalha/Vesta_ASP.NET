using Microsoft.AspNetCore.Mvc;
using Vesta.DataTransferObjects.User;
using Vesta.Helpers;
using Vesta.Interfaces;

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
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserRequestDTO register_params)
        {
            if(register_params.Image != null && register_params.Image != string.Empty)
                try{await ImageUrlValidator.IsValidImageUrl(register_params.Image,1);}
                catch(Exception e){ModelState.AddModelError("Image",e.Message);}

            if(!_emailService.VerifyEmailCode(register_params.Email,register_params.EmailConfirmationCode))
                ModelState.AddModelError("EmailConfirmationCode","Invalid Confirmation Code!");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userRegistrationResult = await _userrepository.CreateUserAsync(register_params);

            return userRegistrationResult switch
            {
                409 => Conflict(),
                500 => Problem(),
                _ => Ok(),
            };
        }
        [HttpPost("send-confirmation-code")]
        public async Task<IActionResult> SendConfirmationEmail([FromBody] ConfirmationEmailRequestDTO confirmation_params)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            try{
                await _emailService.SendRandomEmailVerificationCode(confirmation_params.Email);
                return Ok();
            }catch(Exception ex){
                return Problem(ex.Message);
            }
        }
        
    }
}