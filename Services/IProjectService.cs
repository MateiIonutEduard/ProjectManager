using ProjectManager.Models;

namespace ProjectManager.Services
{
    public interface IProjectService
    {
        Task<int> CreateProject(string token, ProjectModel project);
        Task<MilestoneModel[]?> GetProject(string token, int id);
        Task<ProjectModel[]?> GetProjects(string token);
        Task<int> UpdateProject(ProjectModel project, string token);
        Task<int> RemoveProject(string token, int? id);
    }
}
