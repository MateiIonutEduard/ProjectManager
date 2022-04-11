namespace ProjectManager.Data
{
    public class Group
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
