using System.ComponentModel.DataAnnotations;

namespace WebQuanLyhs.DTO
{
    public class Profile
    {
        public int User_id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Phone { get; set; }
        public string? Fullname { get; set; }
        public string? Detail { get; set; }
        public string? Sex_name { get; set; }
        public string? CCCD { get; set; }

    }
}
