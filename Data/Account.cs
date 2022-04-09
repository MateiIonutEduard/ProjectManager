using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
#pragma warning disable

namespace ProjectManager.Data
{
    public class Account
    {
        public int Id { get; set; }
        [StringLength(120)]
        public string Username { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string Address { get; set; }
        [JsonIgnore]
        public string? Token { get; set; }
        [ForeignKey("AccountId"), JsonIgnore]
        public ICollection<Profile> Profiles { get; set; }
        [ForeignKey("CreatorId"), JsonIgnore]
        public ICollection<Project> Projects { get; set; }
        [ForeignKey("Assignee"), JsonIgnore]
        public ICollection<Phase> Phases { get; set; }
        [ForeignKey("Assignee"), JsonIgnore]
        public ICollection<WorkItem> WorkItems { get; set; }
        [ForeignKey("OwnerId"), JsonIgnore]
        public ICollection<Activity> Activities { get; set; }
    }
}
