using LibrarieModele;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NivelAccessDate
{
    public class UsersAccessor : DataAccessorBase
    {
        public IEnumerable<User> GetAll()
        {
            return _ctx.Users.OrderBy(u => u.email).ToList();
        }

        public User GetById(Guid id)
        {
            return _ctx.Users.FirstOrDefault(u => u.user_id == id);
        }

        public bool ExistsByEmail(string email)
        {
            return _ctx.Users.Any(u => u.email == email);
        }

        public void Add(User user)
        {
            _ctx.Users.Add(user);
            _ctx.SaveChanges();
        }

        public User GetByEmail(string email)
        {
            return _ctx.Users.FirstOrDefault(u => u.email == email);
        }

    }
}
