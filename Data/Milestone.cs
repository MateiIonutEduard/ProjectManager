using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectManager.Data
{
    public class Milestone
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float TotalAmount { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        [ForeignKey("MilestoneId"), JsonIgnore]
        public ICollection<Phase> Phases { get; set; }
    }
}
