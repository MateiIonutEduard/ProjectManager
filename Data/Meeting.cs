using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Data
{
    public class Meeting
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public int Assignee { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProjectId { get; set; }
        [ForeignKey("MeetingId")]
        public ICollection<Invitation> Invitations { get; set; }
    }
}
