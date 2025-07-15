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
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IShowtimeRepository, ShowtimeRepository>();
            services.AddScoped<IShowtimeService, ShowtimeService>();
            services.AddScoped<ICinemaRepository, CinemaRepository>();
            services.AddScoped<ICinemaService, CinemaService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
