using ProjectManager.Models;

namespace ProjectManager.Services
{
    public interface IProjectService
    {
        Task<int> AddInvitation(int id, int accountId, string token);
        Task<InvitationModel[]?> GetInvitations(int? id, string token);
        Task<int> UpdateInvitation(int id, InvitationModel inv, string token);
        Task<int> CancelInvitation(int id, int accountId, string token);
        Task<int> CreateProject(string token, ProjectModel project);
        Task<SprintModel[]?> GetProject(string token, int id);
        Task<ProjectModel[]?> GetProjects(string token);
        Task<int> UpdateProject(ProjectModel project, string token);
        Task<int> RemoveProject(string token, int? id);
    }
}
