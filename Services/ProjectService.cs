using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Data;
using ProjectManager.Models;

namespace ProjectManager.Services
{
    public class ProjectService : IProjectService
    {
        readonly ProjectContext context;
        public ProjectService(ProjectContext context)
        { this.context = context; }

        public async Task<int> UpdateProject(ProjectModel project, string token)
        {
            var account = await context.Account.FirstOrDefaultAsync(u => u.Token == token);

            if(account != null)
            {
                var p = await context.Project.FirstOrDefaultAsync(e => e.Title == project.Title);

                if(p != null)
                {
                    if (p.CreatorId == account.Id)
                    {
                        p.Title = project.Title;
                        p.Description = project.Description;
                        p.BeginDate = project.BeginDate;
                        p.EndDate = project.EndDate;

                        p.Budget = project.Budget;
                        p.Status = project.Status;
                        await context.SaveChangesAsync();
                        return 1;
                    }

                    return -1;
                }

                return 0;
            }
            return -1;
        }

        public async Task<int> RemoveProject(string token, int? id)
        {
            if (id != null)
            {
                var account = await context.Account.FirstOrDefaultAsync(u => u.Token == token);
                if (account == null) return -1;

                var obj = await context.Project.FirstOrDefaultAsync(p => p.Id == id.Value && p.CreatorId == account.Id);

                if(obj != null)
                {
                    if (obj.CreatorId != account.Id) return -1;
                    context.Project.Remove(obj);
                    await context.SaveChangesAsync();
                    return 1;
                }

                return -1;
            }

            return 0;
        }

        public async Task<MilestoneModel[]?> GetProject(string token, int id)
        {
            var account = await context.Account.FirstOrDefaultAsync(u => u.Token == token);
            if (account == null) return null;

            var list = (from m in await context.Milestone.ToListAsync()
                        join p in await context.Project.ToListAsync()
                        on m.ProjectId equals p.Id
                        join g in await context.Group.ToListAsync()
                        on p.Id equals g.ProjectId
                        where g.AccountId == account.Id && p.Id == id select
                        new MilestoneModel
                        {
                            Id = m.Id,
                            Title = m.Title,
                            Description = m.Description,
                            TotalAmount = m.TotalAmount,
                            BeginDate = m.BeginDate,
                            EndDate = m.EndDate,
                            Priority = m.Priority,
                            Status = m.Status
                        }).ToArray();

            return list;
        }

        public async Task<int> CreateProject(string token, ProjectModel project)
        {
            var account = await context.Account.FirstOrDefaultAsync(u => u.Token == token);

            if(account != null)
            {
                var e = await context.Project.FirstOrDefaultAsync(p => p.Title == project.Title);
                if (e != null) return -1;

                var newProject = new Project
                {
                    Id = project.Id,
                    Title = project.Title,
                    Description = project.Description,
                    CreatorId = account.Id,
                    Budget = project.Budget,
                    BeginDate = project.BeginDate,
                    EndDate = project.EndDate,
                    Status = project.Status
                };

                context.Project.Add(newProject);
                await context.SaveChangesAsync();

                var group = new Group
                {
                    Status = 1,
                    AccountId = account.Id,
                    ProjectId = newProject.Id
                };

                context.Group.Add(group);
                await context.SaveChangesAsync();
                return 1;
            }

            return 0;
        }

        public async Task<ProjectModel[]?> GetProjects(string token)
        {
            var account = await context.Account.FirstOrDefaultAsync(u => u.Token == token);
            if (account == null) return null;

            var list = (from p in await context.Project.ToListAsync()
                        join g in await context.Group.ToListAsync() on p.Id equals g.ProjectId
                        where g.AccountId == account.Id && g.Status > 0 select new ProjectModel {
                            Id = p.Id,
                            Title = p.Title,
                            Description = p.Description,
                            Username = account.Username,
                            Budget = p.Budget,
                            BeginDate = p.BeginDate,
                            EndDate = p.EndDate,
                            Status = p.Status
                        }).ToArray();

            return list;
        }
    }
}
