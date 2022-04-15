namespace ProjectManager.Data
{
    public class Appeal
    {
        public int Id { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public int MeetingId { get;set; }
        public Meeting Meeting { get; set; }
        public int OwnerId { get; set; }
        public Account Account { get; set; }
    }
}
