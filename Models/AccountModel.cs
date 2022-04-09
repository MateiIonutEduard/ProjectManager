namespace ProjectManager.Models
{
    public class AccountModel
    {
        public string? username { get; set; }
        public string? address { get; set; }
        public string? password { get; set; }
        public IFormFile? profile { get; set; }
    }
}
