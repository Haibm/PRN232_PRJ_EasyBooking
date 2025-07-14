<<<<<<< HEAD
﻿using Microsoft.Extensions.DependencyInjection;

namespace EasyBooking.Business

=======
﻿using EasyBooking.Business.Interfaces;
using EasyBooking.Business.Services;
using EasyBooking.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EasyBooking.Business
>>>>>>> a3f25924311fb0eaf3be769e45dad7a45ad9c6c0
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
<<<<<<< HEAD

=======
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IMovieService, MovieService>();
>>>>>>> a3f25924311fb0eaf3be769e45dad7a45ad9c6c0
            return services;
        }
    }
}
