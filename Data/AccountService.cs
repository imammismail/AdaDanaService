using AdaDanaService.Dtos;
using AdaDanaService.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using BC = BCrypt.Net.BCrypt;

namespace AdaDanaService.Data
{
    public class AccountService : IAccountService
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public AccountService(IUserService userService, IRoleService roleService, IMapper mapper)
        {
            _userService = userService;
            _roleService = roleService;
            _mapper = mapper;
        }
        [HttpPost]
        public bool Register(RegisterUserDto registerUserDto)
        {
            using (var trans = new TransactionScope())
            {
                try
                {
                    // tambah user
                    var user = new User
                    {
                        Username = registerUserDto.Username,
                        Password = BC.HashPassword(registerUserDto.Password)
                    };

                    // ambil role member
                    var role = _roleService.GetRoleByName("Admin"); // Hardcode langsung jadi admin

                    // assign role ke user
                    if (role != null)
                    {
                        user.RoleId = role.Id; // Set RoleId di User object sesuai dengan Id pada Role object
                        user.CreatedAt = DateTime.Now;
                        _userService.AddUser(user);
                        trans.Complete(); // commit
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return false;
        }
    }
}
