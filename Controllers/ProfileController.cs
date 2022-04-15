using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Services;

namespace ProjectManager.Controllers
{
    [Route("api/account/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private IAccountService accountService;
        public ProfileController(IAccountService accountService)
        { this.accountService = accountService; }

        [HttpPost, Authorize]
        public async Task<IActionResult> CreateAccountProfile([FromForm] IFormFile img)
        {
            string header = HttpContext.Request.Headers["Authorization"];
            string token = header.Split(' ')[1];

            int res = await accountService.CreateProfileAsync(token, img);
            if (res < 0) return Unauthorized();
            if (res < 1) return NoContent();

            var client = await accountService.GetAccountAsync(token);
            var profile = await accountService.GetProfileAsync(client.Id);

            if (res == 2) return Ok();
            return Created("{profile}/?id=" + client.Id, profile);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountProfile(int id)
        {
            var img = await accountService.GetProfileAsync(id);
            return File(img.Buffer, img.ContentType, img.Filename);
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> UpdateAccountProfile([FromForm] IFormFile img)
        {
            string header = HttpContext.Request.Headers["Authorization"];
            string token = header.Split(' ')[1];

            int res = await accountService.UpdateProfileAsync(token, img);
            if (res < 0) return Unauthorized();
            if (res == 0) return NoContent();
            if (res == 1) return Ok();

            var client = await accountService.GetAccountAsync(token);
            var profile = await accountService.GetProfileAsync(client.Id);
            return Created("{profile}/?id=" + client.Id, profile);
        }

        [HttpDelete, Authorize]
        public async Task<IActionResult> DeleteAccountProfile()
        {
            string header = HttpContext.Request.Headers["Authorization"];
            string token = header.Split(' ')[1];

            int res = await accountService.DeleteProfileAsync(token);
            if (res < 0) return Unauthorized();

            if (res == 0) return NoContent();
            return Accepted();
        }
    }
}
