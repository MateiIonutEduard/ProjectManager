using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Models;
using ProjectManager.Services;

namespace ProjectManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController, IgnoreAntiforgeryToken]
    public class AccountController : ControllerBase
    {
        private IAccountService accountService;
        public AccountController(IAccountService accountService)
        { this.accountService = accountService; }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetAccount()
        {
            string header = HttpContext.Request.Headers["Authorization"];
            string token = header.Split(' ')[1];
            var data = await accountService.GetAccountAsync(token);
            return Ok(data);
        }

        [HttpGet("{profile}")]
        public async Task<IActionResult> GetAccountProfile(int id)
        {
            var img = await accountService.GetProfileAsync(id);
            return File(img.Buffer, img.ContentType, img.Filename);
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> SignIn([FromForm]AccountModel account)
        {
            if (string.IsNullOrEmpty(account.username) && account.profile == null)
            {
                var data = await accountService.SignIn(account.address, account.password);

                if (data.Item2 == 0) return NotFound();
                if (data.Item2 < 0) return BadRequest();

                return Ok(data.Item1);
            }
            else
            {
                var token = await accountService.CreateAccountAsync(account);
                if (token != null) return Ok(token);
                return BadRequest();
            }
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> Refresh()
        {
            string header = HttpContext.Request.Headers["Authorization"];
            string token = header.Split(' ')[1];

            var access = await accountService.RefreshTokenAsync(token);
            if (access == null) return Unauthorized();
            return Ok(access);
        }

        [HttpPut("{profile}"), Authorize]
        public async Task<IActionResult> UpdateAccountProfile([FromForm]IFormFile img)
        {
            string header = HttpContext.Request.Headers["Authorization"];
            string token = header.Split(' ')[1];

            int res = await accountService.UpdateProfileAsync(token, img);
            if (res < 0) return Unauthorized();
            if (res == 0) return NoContent();
            if(res == 1) return Ok();

            var client = await accountService.GetAccountAsync(token);
            var profile = await accountService.GetProfileAsync(client.Id);
            return Created("{profile}/?id=" + client.Id, profile);
        }

        [HttpPost("{profile}"), Authorize]
        public async Task<IActionResult> CreateAccountProfile([FromForm]IFormFile img)
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

        [HttpDelete("{profile}"), Authorize]
        public async Task<IActionResult> DeleteAccountProfile()
        {
            string header = HttpContext.Request.Headers["Authorization"];
            string token = header.Split(' ')[1];

            int res = await accountService.DeleteProfileAsync(token);
            if (res < 0) return Unauthorized();

            if (res == 0) return NoContent();
            return Accepted();
        }

        [HttpDelete, Authorize]
        public async Task<IActionResult> OnSignOut()
        {
            string header = HttpContext.Request.Headers["Authorization"];
            string token = header.Split(' ')[1];

            var ok = await accountService.SignOut(token);
            if (ok) return Accepted();
            return Unauthorized();
        }
    }
}
