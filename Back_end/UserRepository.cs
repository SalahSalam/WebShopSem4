using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopSem4
{
    public class UserRepository
    {
        private int _nextId = 1;
        private readonly List<User> _users = new();
        public UserRepository()
        {
            Add(new User { Username = "Frank", UserRole = Role.Customer, Password = "123" });
            Add(new User { Username = "Ned", UserRole = Role.Customer, Password = "123" });
            Add(new User { Username = "JJ", UserRole = Role.Customer, Password = "123" });
            Add(new User { Username = "Chad", UserRole = Role.Customer, Password = "123" });
            Add(new User { Username = "Toby Maguire", UserRole = Role.Customer, Password = "123" });
        }
        public List<User> GetAll()
        {
            return new List<User>(_users);
        }
        public User? GetById(int id)
        {
            User? p = _users.FirstOrDefault(p => p.Id == id);

            if (p == null)
            {
                return null;
            }
            else
                return p;
        }
        public User? Add(User user)
        {
            user.Id = _nextId++;
            user.ValidateId();
            //•	Data Integrity: Ensures all properties meet criteria (e.g., name length, race length, age range) before adding to the repository
            //Error Prevention: Catches errors early, preventing invalid data from being added.
            _users.Add(user);
            return user;
        }
        public User? Remove(int id)
        {
            User? user = GetById(id);
            if (user == null)
            {
                return null;
            }
            _users.Remove(user);
            return user;
        }
    }
}

