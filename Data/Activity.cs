#pragma warning disable

namespace ProjectManager.Data
{
    public class Activity
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public int OwnerId { get; set; }
        public Account Account { get; set; }
        public int TaskId { get; set; }
        public WorkItem WorkItem { get; set; }
    }
}
