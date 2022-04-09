using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [ForeignKey("ProjectId")]
        public ICollection<Phase> Phases { get; set; }
        public float Budget { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
    }
}
