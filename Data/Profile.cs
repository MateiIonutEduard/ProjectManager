#pragma warning disable

namespace ProjectManager.Data
{
    public class Profile
    {
        public int Id { get; set; }
        public string Filename { get; set; }
        public string ContentType { get; set; }
        public byte[] Buffer { get; set; }
        public bool Status { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}
