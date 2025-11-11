using SaludDigital.Data;
using SaludDigital.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SaludDigital.Aplication.User
{
    public class GetUserInfo
    {
        public class UserInfoDto : IRequest<Response>
        {
            public string Email { get; set; }
        }

        public class Manejador : IRequestHandler<UserInfoDto, Response>
        {
            private readonly AppDbContext context;
            public Manejador(AppDbContext context)
            {
                this.context = context;
            }
            public async Task<Response> Handle(UserInfoDto request, CancellationToken cancellationToken)
            {
                Response res = new Response("error", "Error al localizar al usuario");
                var user = await context.Users.Where(x => x.Email == request.Email).FirstOrDefaultAsync();

                if (user == null)
                {
                    return res;
                }

                res.Content = user;
                res.Status = "ok";
                res.Info = "Usuario Localizado";

                return res;
            }
        }
    }
}
