using AdaDanaService.Dtos;
using AdaDanaService.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Transactions;
using BC = BCrypt.Net.BCrypt;

namespace AdaDanaService.Data
{
    public class AccountService : IAccountService
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly IUserRoleService _userRoleService;
        private readonly IConfiguration _configuration;
        private readonly AdaDanaContext _context;

        public AccountService(IUserService userService, IRoleService roleService,
        IMapper mapper, IUserRoleService userRoleService, IConfiguration configuration,
        AdaDanaContext context)
        {
            _userService = userService;
            _roleService = roleService;
            _mapper = mapper;
            _userRoleService = userRoleService;
            _configuration = configuration;
            _context = context;
        }

        // Register admin dan manager
        public async Task<bool> Register(RegisterUserDto registerUserDto)
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    // ambil role user
                    var role = await _roleService.GetRoleUser(); // Hardcode langsung Admin
                    // mapping register user dto ke user
                    var user = new User
                    {
                        Username = registerUserDto.Username,
                        Password = BC.HashPassword(registerUserDto.Password)
                    };

                    if (role != null)
                    {
                        // assign role ke user
                        var ur = new UserRole();
                        ur.User = user;
                        ur.Role = role;

                        user.CreatedAt = DateTime.Now;
                        user.UpdatedAt = DateTime.Now;

                        // masukkan user ke dalam tabel user dan role ke dalam user role
                        await _userService.AddUser(user);
                        await _userRoleService.AddRoleUser(ur);
                        await _context.SaveChangesAsync();

                        trans.Commit();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    // Jika terjadi kesalahan, rollback transaksi
                    trans.Rollback();
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return false;
        }

        // Login admin & manager
        public async Task<UserToken> Login(LoginDto login)
        {
            var usr = await _userService.FindUserByUsername(login.Username);
            if (usr != null)
            {
                if (BC.Verify(login.Password, usr.Password))
                {
                    var roles = await _context.UserRoles
                    .Where(ur => ur.UserId == usr.Id)
                    .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                    .ToListAsync();

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, usr.Id.ToString()),
                        new Claim(ClaimTypes.Name, usr.Username)
                    };

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var secret = _configuration.GetValue<string>("AppSettings:Secret");
                    var secretBytes = Encoding.ASCII.GetBytes(secret);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.UtcNow.AddDays(2),
                        SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretBytes),
                    SecurityAlgorithms.HmacSha256Signature)
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);

                    return new UserToken
                    {
                        Token = tokenHandler.WriteToken(token),
                        ExpiredAt = tokenDescriptor.Expires?.ToString(),
                        Message = "Loggin success"
                    };
                }
            }
            return new UserToken { Message = "Invalid username or password" };
        }

        // Login user by goole
        public async Task<UserToken> LoginByGooleId(GooleIdDto gooleIdDto)
        {
            throw new NotImplementedException();
        }

        public async Task UpdatePasswordUser(string password)
        {
            throw new NotImplementedException();
        }
    }
}
