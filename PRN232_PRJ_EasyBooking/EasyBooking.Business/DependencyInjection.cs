using EasyBooking.Business.Interfaces;
using EasyBooking.Business.Services;
using EasyBooking.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EasyBooking.Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IMovieService, MovieService>();
            return services;
        }
    }
}
