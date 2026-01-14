using System;

namespace SmartFlashcards_API.Models
{
    public class UserDto
    {
        public Guid user_id { get; set; }
        public string email { get; set; }
    }

    public class CreateUserDto
    {
        public string email { get; set; }
        public string password { get; set; } 
    }
}
