using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
#pragma warning disable

namespace ProjectManager.Data
{
    public class Project
    {
        public int Id { get; set; }
        [StringLength(120)]
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatorId { get; set; }
        public Account Account { get; set; }
        [ForeignKey("ProjectId"), JsonIgnore]
        public ICollection<Milestone> Milestones { get; set; }
        [ForeignKey("ProjectId"), JsonIgnore]
        public ICollection<Meeting> Meetings { get; set; }
        [ForeignKey("ProjectId"), JsonIgnore]
        public ICollection<Group> Groups { get; set; }
        public float Budget { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
    }
}
