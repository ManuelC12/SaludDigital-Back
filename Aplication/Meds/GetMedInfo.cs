using SaludDigital.Data;
using SaludDigital.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SaludDigital.Aplication.Meds
{
    public class GetMedInfo
    {
        public class MedInfoDto : IRequest<Response>
        {
            public string Email { get; set; }
        }

        public class Manejador : IRequestHandler<MedInfoDto, Response>
        {
            private readonly AppDbContext context;
            public Manejador(AppDbContext context)
            {
                this.context = context;
            }

            public async Task<Response> Handle(MedInfoDto request, CancellationToken cancellationToken)
            {
                Response res = new Response("error", "Error al localizar al doctor");
                var doc = await context.Doctors.Where(x => x.Email == request.Email).FirstOrDefaultAsync();

                if (doc == null)
                {
                    return res;
                }

                res.Content = doc;
                res.Status = "ok";
                res.Info = "Doctor Localizado";

                return res;
            }
        }
    }
}
