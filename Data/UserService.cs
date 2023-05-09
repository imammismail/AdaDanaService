﻿using AdaDanaApi.Models;

namespace AdaDanaApi.Data
{
    public class UserService : IUserService
    {
        private readonly AdaDanaContext _context;

        public UserService(AdaDanaContext context)
        {
            _context = context;
        }
        public User GetUserByUsername(string username)
        {
            var dataUser = _context.Users.FirstOrDefault(u => u.Username == username);
            if (dataUser is null)
                throw new Exception("User not match");
            return dataUser;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public IEnumerable<User> GetAllUser()
        {
            return _context.Users.ToList();
        }
    }
}
