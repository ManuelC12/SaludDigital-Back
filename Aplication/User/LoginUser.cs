using SaludDigital.Data;
using SaludDigital.Helpers;
using SaludDigital.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SaludDigital.Aplication.User
{
    public class LoginUser
    {
        public class UserLogin : IRequest<Response>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class Validador : AbstractValidator<UserLogin>
        {
            public Validador()
            {
                RuleFor(x => x.Email).NotEmpty().WithMessage("El email es obligatorio");
                RuleFor(x => x.Password).NotEmpty().WithMessage("La contraseña es obligatoria");
            }
        }

        // 💡 ESTA clase es la que recibe las dependencias via constructor
        public class Manejador : IRequestHandler<UserLogin, Response>
        {
            private readonly AppDbContext _context;
            private readonly GlobalFunctions _globalFunctions;

            public Manejador(AppDbContext context, GlobalFunctions globalFunctions)
            {
                _context = context;
                _globalFunctions = globalFunctions;
            }

            public async Task<Response> Handle(UserLogin request, CancellationToken cancellationToken)
            {
                var res = new Response("error", "Error al iniciar sesión");

                var hashedPassword = GlobalFunctions.ComputeSha256Hash(request.Password);

                var user = await _context.Users
                    .Where(x => x.Email == request.Email && x.Password == hashedPassword)
                    .FirstOrDefaultAsync(cancellationToken);

                if (user == null)
                {
                    res.Info = "Correo electrónico y/o contraseña incorrecta";
                    return res;
                }

                var userDto = new UserDto
                {
                    iUser = user.iUser,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Age = user.Age,
                    Gender = user.Gender
                };

                // ✅ Aquí ya puedes usar la instancia inyectada
                var token = _globalFunctions.JWT(userDto);

                res.Status = "ok";
                res.Info = "Inicio de sesión correcto";
                res.Content = token;

                return res;
            }
        }
    }
}
