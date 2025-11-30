using SaludDigital.Data;
using SaludDigital.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SaludDigital.Aplication.User
{
    public class DeleteUser
    {
        public class UserDelete : IRequest<Response>
        {
            public string Email { get; set; }
        }

        public class Validador : AbstractValidator<UserDelete>
        {
            public Validador()
            {
                RuleFor(x => x.Email).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<UserDelete, Response>
        {
            private readonly AppDbContext context;
            public Manejador(AppDbContext context)
            {
                this.context = context;
            }
            public async Task<Response> Handle(UserDelete request, CancellationToken cancellationToken)
            {
                Response res = new Response("error", "Usuario no localizado");
                var user = await context.Users.Where(x => x.Email == request.Email).FirstOrDefaultAsync();
                if (user == null)
                {
                    return res;
                }

                context.Users.Remove(user);
                var response = await context.SaveChangesAsync();
                if (response > 0)
                {
                    res.Info = "Usuario eliminado correctamente";
                    res.Status = "ok";
                }
                else
                {
                    res.Info = "Error al eliminar el usuario";
                    res.Status = "error";
                }

                return res;

            }
        }
    }
}
