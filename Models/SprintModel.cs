namespace ProjectManager.Models
{
    public class SprintModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float TotalAmount { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
    }
}
