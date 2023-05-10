using AdaDanaService.Dtos;
using AdaDanaService.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public AccountService(IUserService userService, IRoleService roleService,
        IMapper mapper, IUserRoleService userRoleService, IConfiguration configuration)
        {
            _userService = userService;
            _roleService = roleService;
            _mapper = mapper;
            _userRoleService = userRoleService;
            _configuration = configuration;
        }

        public UserToken Login(LoginUserDto loginUserDto)
        {

            var usr = _userService.FindUserByUsername(loginUserDto.Username);
            if (usr != null)
            {
                if (BC.Verify(loginUserDto.Password, usr.Password))
                {

                    var roles = _userRoleService.GetRolesByUsername(loginUserDto.Username);

                    var roleClaims = new Dictionary<string, object>();
                    foreach (var role in roles)
                    {
                        roleClaims.Add(ClaimTypes.Role, "" + role);
                    }


                    var secret = _configuration.GetValue<string>("AppSettings:Secret");
                    var secretBytes = Encoding.ASCII.GetBytes(secret);

                    // token
                    var expired = DateTime.Now.AddDays(2); // 2 hari
                    var tokenHandler = new JwtSecurityTokenHandler();
                    // data
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        // payload
                        Subject = new System.Security.Claims.ClaimsIdentity(
                                new Claim[]
                                {
                                    new Claim(ClaimTypes.Name, loginUserDto.Username)
                                }),
                        Claims = roleClaims, // claims - roles
                        Expires = expired,
                        SigningCredentials = new SigningCredentials(
                                new SymmetricSecurityKey(secretBytes),
                                SecurityAlgorithms.HmacSha256Signature
                            )
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var userToken = new UserToken
                    {
                        Token = tokenHandler.WriteToken(token), // token as string
                        ExpiredAt = expired.ToString(),
                        Message = ""
                    };
                    return userToken;
                }
            }
            return new UserToken { Message = "Invalid username or password" };
        }


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

                    // ambil role user
                    var role = _roleService.GetRoleByName("User"); // Hardcode langsung jadi user

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
