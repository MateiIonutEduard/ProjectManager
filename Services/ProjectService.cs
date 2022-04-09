using ProjectManager.Data;

namespace ProjectManager.Services
{
    public class ProjectService
    {
        readonly ProjectContext context;
        public ProjectService(ProjectContext context)
        { this.context = context; }

        public async Task<List<Project>> GetProjectAsync()
        {
            return null;
        }
    }
}
