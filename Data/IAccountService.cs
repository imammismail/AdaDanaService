using AdaDanaApi.Dtos;
using AdaDanaApi.Models;

namespace AdaDanaApi.Data
{
    public interface IAccountService
    {
        bool Register(RegisterUserDto registerUserDto);
    }
}
