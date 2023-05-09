using AdaDanaService.Dtos;
using AdaDanaService.Models;

namespace AdaDanaService.Data
{
    public interface IAccountService
    {
        bool Register(RegisterUserDto registerUserDto);
        UserToken Login(LoginUserDto loginUserDto);
    }
}
