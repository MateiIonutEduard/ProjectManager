using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Models;
using ProjectManager.Services;

namespace ProjectManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
