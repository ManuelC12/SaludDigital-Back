using SaludDigital.Models;
using SaludDigital.Data;
using SaludDigital.Helpers;
using SaludDigital.Controllers;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SaludDigital.Aplication.User
{
    public class RegisterUser : IRequest<Response>
    {
        public class UserRegister : IRequest<Response>
        {
            public string? Nombre { get; set; }
            public string? Email { get; set; }
            public string? Celular { get; set; }
            public string? Password { get; set; }
            public int Edad { get; set; }
            public string? Genre { get; set; }
        }

        public class Validador : AbstractValidator<UserRegister>
        {
            public Validador()
            {
                RuleFor(x => x.Nombre).NotEmpty().WithMessage("El campo Nombre es obligatorio");
                RuleFor(x => x.Email).NotEmpty().WithMessage("El campo Email es obligatorio");
                RuleFor(x => x.Password).NotEmpty().WithMessage("El campo Password es obligatorio");
            }
        }

        public class Manejador : IRequestHandler<UserRegister, Response>
        {
            private readonly AppDbContext context;
            public Manejador(AppDbContext context)
            {
                this.context = context;
            }

            public async Task<Response> Handle(UserRegister request, CancellationToken cancellationToken)
            {
                Response res = new Response("error", "Error al registrar un nuevo usuario");
                var userExists = await context.Users.Where(x => x.Email == request.Email).FirstOrDefaultAsync();
                if (userExists != null)
                {
                    res.Status = "error";
                    res.Info = "Usuario registrado";
                    res.Content = userExists;
                    return res;
                }

                var newUser = new Usuario()
                {
                    Name = request.Nombre!,
                    Email = request.Email!,
                    PhoneNumber = request.Celular!,
                    Password = GlobalFunctions.ComputeSha256Hash(request.Password!),
                    Gender = request.Genre!,
                    Age = request.Edad
                };

                context.Users.Add(newUser);
                var createResponse = await context.SaveChangesAsync();
                if (createResponse > 0)
                {
                    res.Status = "ok";
                    res.Info = "Usuario registrado correctamente";
                    res.Content = newUser;
                }

                return res;
            }
        }
    }
}
