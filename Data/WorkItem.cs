using System.ComponentModel.DataAnnotations;
#pragma warning disable

namespace ProjectManager.Data
{
    public class WorkItem
    {
        public int Id { get; set; }
        [StringLength(120)]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Assignee { get; set; }
        public Account Account { get; set; }
        public int PhaseId { get; set; }
        public Phase Phase { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
    }
}
