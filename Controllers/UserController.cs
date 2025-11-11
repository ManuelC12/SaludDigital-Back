using SaludDigital.Aplication;
using SaludDigital.Aplication.User;
using SaludDigital.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaludDigital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;
        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Response>> CreateUser(RegisterUser.UserRegister data)
        {
            return await mediator.Send(data);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response>> LoginUser(LoginUser.UserLogin data)
        {
            return await mediator.Send(data);
        }

        [HttpPost("getUser")]
        public async Task<ActionResult<Response>> GetUser(GetUserInfo.UserInfoDto data)
        {
            return await mediator.Send(data);
        }
    }
}
