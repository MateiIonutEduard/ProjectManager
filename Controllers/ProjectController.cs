using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Models;
using ProjectManager.Services;

namespace ProjectManager.Controllers
{
    [Route("api/[controller]"), IgnoreAntiforgeryToken]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private IProjectService projectService;
        public ProjectController(IProjectService projectService)
        { this.projectService = projectService; }

        [HttpGet("{id?}"), Authorize]
        public async Task<IActionResult> GetProjects(int? id)
        {
            string header = HttpContext.Request.Headers["Authorization"];
            string token = header.Split(' ')[1];

            if (id != null)
            {
                var res = await projectService.GetProject(token, id.Value);
                if(res != null) return Ok(res);
                return Unauthorized();
            }

            var list = await projectService.GetProjects(token);
            if(list != null) return Ok(list);
            return Unauthorized();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> CreateProject([FromForm]ProjectModel project)
        {
            string header = HttpContext.Request.Headers["Authorization"];
            string token = header.Split(' ')[1];

            int state = await projectService.CreateProject(token, project);
            if (state == 0) return Unauthorized();
            if (state == -1) return NoContent();
            return Ok();
        }

        /*[HttpPost("{invite}"), Authorize]
        public async Task<IActionResult> SendInvite([FromForm] GroupModel group)
        {
            string header = HttpContext.Request.Headers["Authorization"];
            string token = header.Split(' ')[1];

            return Ok();
        }

        [HttpGet("{invite}/{id?}"), Authorize]
        public async Task<IActionResult> GetInvitations(int? id)
        {
            string header = HttpContext.Request.Headers["Authorization"];
            string token = header.Split(' ')[1];

            return Ok();
        }*/

        [HttpPut, Authorize]
        public async Task<IActionResult> UpdateProject([FromForm]ProjectModel project)
        {
            string header = HttpContext.Request.Headers["Authorization"];
            string token = header.Split(' ')[1];

            int state = await projectService.UpdateProject(project, token);
            if (state < 0) return Unauthorized();

            if (state == 0) return NotFound();
            return Ok();
        }

        [HttpDelete("{id?}"), Authorize]
        public async Task<IActionResult> RemoveProject(int? id)
        {
            string header = HttpContext.Request.Headers["Authorization"];
            string token = header.Split(' ')[1];

            int state = await projectService.RemoveProject(token, id);
            if (state < 0) return Unauthorized();

            if (state == 0) return NoContent();
            return Accepted();
        }
    }
}
