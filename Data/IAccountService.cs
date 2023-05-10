using AdaDanaService.Dtos;
using AdaDanaService.Models;

namespace AdaDanaService.Data
{
    public interface IAccountService
    {
        // bool Register(RegisterUserDto registerUserDto);
        // UserToken Login(LoginUserDto loginUserDto);
        Task<bool> Register(RegisterUserDto registerUserDto);
        Task<UserToken> Login(LoginDto login);
        Task<UserToken> LoginByGooleId(GooleIdDto gooleIdDto);
        Task UpdatePasswordUser(UpdatePassword updatePassword);
    }
}
