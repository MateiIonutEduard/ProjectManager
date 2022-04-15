using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Models;
using ProjectManager.Services;

namespace ProjectManager.Controllers
{
    [Route("api/project/[controller]")]
    [ApiController]
    public class InviteController : ControllerBase
    {
        private IProjectService projectService;

        public InviteController(IProjectService projectService)
        { this.projectService = projectService; }

        [HttpPost("{id}"), Authorize]
        public async Task<IActionResult> SendInvitation(int id, [FromForm] int accountId)
        {
            string header = HttpContext.Request.Headers["Authorization"];
            string token = header.Split(' ')[1];

            var state = await projectService.AddInvitation(id, accountId, token);
            if (state < -1) return Unauthorized("You have no manager role!");

            if (state < 0) return Unauthorized();
            if (state == 0) return Ok();
            return Ok();
        }

        [HttpGet("{id?}"), Authorize]
        public async Task<IActionResult> GetInvitations(int? id)
        {
            string header = HttpContext.Request.Headers["Authorization"];
            string token = header.Split(' ')[1];

            var list = await projectService.GetInvitations(id, token);
            return Ok(list);
        }

        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> UpdateInvitation(int id, [FromForm] InvitationModel inv)
        {
            string header = HttpContext.Request.Headers["Authorization"];
            string token = header.Split(' ')[1];

            int state = await projectService.UpdateInvitation(id, inv, token);
            if (state < -1) return Unauthorized("You are not owner!");
            if (state < 0) return Unauthorized();

            if (state == 0) return NotFound();
            return Ok();
        }

        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> RemoveInvitation(int id, [FromForm] int accountId)
        {
            string header = HttpContext.Request.Headers["Authorization"];
            string token = header.Split(' ')[1];
            int state = await projectService.CancelInvitation(id, accountId, token);

            if (state < -1) return Unauthorized("You have no manager role!");
            if (state < 0) return Unauthorized();

            if (state == 0) return NotFound();
            return Accepted();
        }
    }
}
