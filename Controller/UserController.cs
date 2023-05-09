using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AdaDanaApi.Data;
using AdaDanaApi.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdaDanaApi.Controller
{
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService UserService, IMapper mapper)
        {
            _userService = UserService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ReadUserDto>> GetAllUser()
        {
            var userItem = _userService.GetAllUser();
            var productReadDtoList = _mapper.Map<IEnumerable<ReadUserDto>>(userItem);
            return Ok(productReadDtoList);
        }

    }
}