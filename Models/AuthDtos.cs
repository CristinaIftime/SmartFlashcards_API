using System;

namespace SmartFlashcards_API.Models
{
    public class LoginDto
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    public class LoginResultDto
    {
        public Guid user_id { get; set; }
        public string email { get; set; }
    }
}
